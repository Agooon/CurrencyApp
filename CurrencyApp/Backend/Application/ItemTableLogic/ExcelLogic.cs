using CurrencyApp.Models.Application.ItemTable;
using CurrencyAppDatabase.DataAccess;
using CurrencyAppDatabase.DatabaseOperations;
using CurrencyAppDatabase.Models.CurrencyApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyApp.Backend.Application.ItemTableLogic
{
    public static class ExcelLogic
    {

        // On Post add items to database
        public static async Task<Tuple<string, string>> ImportTable(CurrencyContext _context, HttpClient client, ItemTable TableI, int maxNumberOfCalls, string FromExcel)
        {
            // Getting Data from Database
            string errorString = "";

            string[] rows = FromExcel.Split("\r\n");
            List<string> addedItems = new List<string>();
            //var client = _clientFactory.CreateClient("nbp");
            //var maxNumberOfCalls = _configuration.GetValue<int>("MaxNumberOfCalls");
            foreach (var row in rows.Skip(1))
            {

                string[] words = row.Split("\t");
                // call to API http://api.nbp.pl/api/exchangerates/rates/{table}/code}/{date}/

                // date = RRRR-MM-DD

                // We need the date from day before the transaction
                var date = DateTime.Parse(words[1]).ToString("yyyy-MM-dd");
                StaticFunctions.PreviousDay(ref date, "yyyy-MM-dd");

                var call = await GetCorrectCallNOERROR(client, words[3], date);
                string errString;
                int counter = 0;
                while (call == null && counter < maxNumberOfCalls)
                {
                    StaticFunctions.PreviousDay(ref date, "yyyy-MM-dd");
                    call = await GetCorrectCallNOERROR(client, words[3], date);
                    counter++;
                }
                if (call == null)
                    // If the call excited the maximum call amount
                    errString = "Nie udało się dodać przedmiotu <b>" + words[0] + "</b>. Błąd przy wysyłaniu zapytania </br>";
                else
                    errString = null;
                if (!string.IsNullOrWhiteSpace(errString))
                    errorString += errString;
                else
                {
                    // Creating new Item
                    Item newItem = new Item()
                    {
                        Name = words[0],
                        Date = DateTime.Parse(words[1]),
                        Price = decimal.Parse(words[2]),
                        CurrencyFrom = words[3],
                        Rate = (decimal)call.Rates[0].Mid,
                        DateTable = DateTime.Parse(call.Rates[0].EffectiveDate),
                        ConvertedPrice = (decimal)call.Rates[0].Mid * decimal.Parse(words[2]),
                        CurrencyTo = "PLN" // For now all of currencies will be converted into
                    };
                    // Adding item to database
                    await TableOperations.AddItemToItemTable(_context, TableI, newItem);
                    addedItems.Add(words[0]);
                }
                await _context.SaveChangesAsync();
            }

            // Adding message for each added item
            string messages = "Koniec dodawania przedmiotów, dodano:";
            foreach (var itemS in addedItems)
            {
                messages += "</br>" + itemS;
            }

            return new Tuple<string, string>(errorString, messages);
        }

        //To get list to outuput excel - \t as a separator of columns \n as separator of rows
        //  colname1    colname2    colname3    ...
        //  val11       val12       val13       ...
        //  val21       val22       val23       ...
        //  ...         ...         ...
        public static string ExportTable(HttpResponse Response, CheckBoxesViewModel Checkboxes, ItemTable TableI)
        {
            //await ChangePosAsync(Ids);
            CheckBoxesLogic.SaveCheckBoxes(Response, Checkboxes);
            string toExcel = "";

            if (!Checkboxes.NameCheck)
                toExcel += "Nazwa\t";
            if (!Checkboxes.DateCheck)
                toExcel += "Data\t";
            if (!Checkboxes.PriceCheck)
                toExcel += "Cena\t";
            if (!Checkboxes.CurrencyFCheck)
                toExcel += "Waluta\t";
            if (!Checkboxes.RateCheck)
                toExcel += "Kurs\t";
            if (!Checkboxes.PriceConCheck)
                toExcel += "Cena po przeliczeniu\t";
            if (!Checkboxes.DateTableCheck)
                toExcel += "Data tabeli\t";
            if (!Checkboxes.CurrencyTCheck)
                toExcel += "Waluta docelowa\t";

            toExcel += "\n";

            foreach (var item in TableI.Items)
            {
                if (!Checkboxes.NameCheck)
                    toExcel += item.Name + "\t";
                if (!Checkboxes.DateCheck)
                    toExcel += item.Date.ToString("yyyy-MM-dd") + "\t";
                if (!Checkboxes.PriceCheck)
                    toExcel += item.Price + "\t";
                if (!Checkboxes.CurrencyFCheck)
                    toExcel += item.CurrencyFrom + "\t";
                if (!Checkboxes.RateCheck)
                    toExcel += item.Rate.ToString().Replace(".", ",") + "\t";
                if (!Checkboxes.PriceConCheck)
                    toExcel += item.ConvertedPrice + "\t";
                if (!Checkboxes.DateTableCheck)
                    toExcel += item.DateTable.ToString("yyyy-MM-dd") + "\t";
                if (!Checkboxes.CurrencyTCheck)
                    toExcel += item.CurrencyTo + "\t";
                toExcel += "\n";
            }
            return toExcel;
        }

        private static async Task<SingleRateModel> GetCorrectCallNOERROR(HttpClient client, string code, string date)
        {
            try
            {
                return await client.GetFromJsonAsync<SingleRateModel>($"rates/A/{code}/{date}?format=json");
            }
            catch
            {
                return null;
            }
        }
    }
}

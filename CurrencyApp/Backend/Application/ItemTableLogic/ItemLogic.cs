using CurrencyApp.Models.Application.ItemTable;
using CurrencyAppDatabase.DataAccess;
using CurrencyAppDatabase.DatabaseOperations;
using CurrencyAppDatabase.Models.CurrencyApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyApp.Backend.Application.ItemTableLogic
{
    public static class ItemLogic
    {
        public static async Task<Tuple<string, string>> AddItemAsync(CurrencyContext _context, HttpClient client, ItemTable TableI, int maxNumberOfCalls, ItemAddModel AddItemModel)
        {
            string messages = "";
            string errorString;

            // For l
            var date = AddItemModel.Date.ToString("yyyy-MM-dd");

            if (date != "" && !UtilFunctions.IsValidDate(date, "yyyy-MM-dd"))
            {
                errorString = "Nieprawidłowy format daty, należy podać format: RRRR-MM-DD (np. 2020-11-20)";
                return new Tuple<string, string>(errorString, messages);
            }

            // You cannot "plan" an transaction
            if (DateTime.Parse(date) > DateTime.Now)
            {
                errorString = "Nie udało się dodać przedmiotu <b>" + "</b>. Data transakcji jest później niż dzisiaj </br>";
            }
            else
            {
                UtilFunctions.PreviousDay(ref date, "yyyy-MM-dd");

                // call to API http://api.nbp.pl/api/exchangerates/rates/{table}/code}/{date}/
                var call = await GetCorrectCall(client, AddItemModel.CurrencyFrom, date);
                int counter = 0;
                while (call == null && counter < maxNumberOfCalls)
                {
                    UtilFunctions.PreviousDay(ref date, "yyyy-MM-dd");
                    call = await GetCorrectCall(client, AddItemModel.CurrencyFrom, date);
                    counter++;
                }
                if (call == null)
                {
                    errorString = "</br>Nie odnaleziono strony z tabelami lub przekroczono ilość zapytań. Sprawdź działanie strony NBP.";
                    return new Tuple<string, string>(errorString, messages);
                }
                else
                {
                    errorString = null;
                }
                // Creating new Item
                Item newItem = new Item()
                {
                    Name = AddItemModel.Name,
                    Date = AddItemModel.Date,
                    Price = AddItemModel.Price,
                    CurrencyFrom = AddItemModel.CurrencyFrom,
                    Rate = (decimal)call.Rates[0].Mid,
                    DateTable = DateTime.Parse(call.Rates[0].EffectiveDate),
                    ConvertedPrice = (decimal)call.Rates[0].Mid * AddItemModel.Price,
                    CurrencyTo = AddItemModel.CurrencyTo
                };
                // Adding item to database
                await TableOperations.AddItemToItemTable(_context, TableI, newItem);
                messages = "Dodano nowy przedmiot!";
                if (newItem.Date != newItem.DateTable)
                    messages += "</br>Data transakcji <b>" + newItem.Date.ToString("yyyy-MM-dd") + "</b> jest inna od daty tabeli <b>" + newItem.DateTable.ToString("yyyy-MM-dd") + "</b>";
            }
            return new Tuple<string, string>(errorString, messages);
        }

        public static async Task DeleteItemAsync(CurrencyContext _context, ItemTable TableI, int number, int[] Ids)
        {
            Item item = TableI.Items.FirstOrDefault(x => x.Position == number);
            ChangePosWithout(TableI, Ids, number);
            _context.Items.Remove(item);

            await _context.SaveChangesAsync();
        }

        public static async Task DeleteAllItemsAsync(CurrencyContext _context, ItemTable TableI)
        {
            _context.Items.RemoveRange(TableI.Items);
            await _context.SaveChangesAsync();
        }


        private static void ChangePosWithout(ItemTable TableI, int[] ids, int position)
        {
            int counter = 0;

            if (UtilFunctions.IsSorted(ids))
            {
                foreach (var item in TableI.Items)
                {
                    if (item.Position != position)
                        item.Position = counter++;
                }
                return;
            }

            List<Item> items = TableI.Items.ToList(); // To use indexes I have to cast ICollection to List
            foreach (var id in ids)
            {
                if (items[id].Position != position)
                    items[id].Position = counter++;
            }
            TableI.Items = items.OrderBy(x => x.Position).ToList();
        }

        private static async Task<SingleRateModel> GetCorrectCall(HttpClient client, string code, string date)
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

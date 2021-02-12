using CurrencyApp.Backend;
using CurrencyApp.Backend.Application.ItemTableLogic;
using CurrencyApp.Models.Application.ItemTable;
using CurrencyAppDatabase.DataAccess;
using CurrencyAppDatabase.DatabaseOperations;
using CurrencyAppDatabase.Models.CurrencyApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CurrencyApp.Pages
{
    public class ItemsModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly CurrencyContext _context;
        // To Show api error messages or just messages
        [TempData]
        public string ErrorString { get; set; }
        [TempData]
        public string Messages { get; set; }

        // Model for Checkboxes

        [BindProperty]
        public CheckBoxesViewModel Checkboxes { get; set; } = new CheckBoxesViewModel();

        // Sorting properties
        public string SortName { get; set; }
        public string SortDate { get; set; }
        public string SortPrice { get; set; }
        public string SortCurrency { get; set; }
        public string SortRate { get; set; }
        // To check if there was a position change
        [BindProperty]
        public int[] Ids { get; set; } // To see the current order of items
        public ItemTable TableI { get; set; }
        // AddItem Model
        [BindProperty]
        public ItemAddModel AddItemModel { get; set; } = new ItemAddModel();
        // From and To excel strings
        [BindProperty]
        public string FromExcel { get; set; }
        [TempData]
        public string ToExcel { get; set; }
        public ItemsModel(IHttpClientFactory clientFactory, IConfiguration iConfig, CurrencyContext context)
        {
            _clientFactory = clientFactory;
            _configuration = iConfig;
            _context = context;
            // Setting up default sorts
            SortName = "name";
            SortDate = "date_desc";
            SortPrice = "price";
            SortCurrency = "currency";
            SortRate = "rate";
        }
        // To get list of items
        public async Task OnGetAsync()
        {
            // Setting currencies
            SetCurrencies();
            if (User.IsInRole("Admin"))
            {
                TempData["BigErrorString"] = "Jako admin nie mo¿esz posiadaæ w³asnej tabeli";
            }
            else
            {
                TableI = await SetItemsAsync();
                // Getting Data from Cookies 
                CheckBoxesLogic.SetCheckboxes(Request, Checkboxes);
            }
        }

        // To sort list of items
        public async Task OnGetSortAsync(string sortOption = "")
        {
            // Getting Data from Database
            TableI = await SetItemsAsync();
            if (TableI == null)
            {
                TempData["BigErrorString"] = "Nie odnaleziono poszukiwanej tabeli";
                return;
            }
            // Setting sort values
            SortName = sortOption == "name" ? "name_desc" : "name";
            SortDate = sortOption == "date_desc" ? "date" : "date_desc";
            SortPrice = sortOption == "price_desc" ? "price" : "price_desc";
            SortCurrency = sortOption == "currency" ? "currency_desc" : "currency";
            SortRate = sortOption == "rate_desc" ? "rate" : "rate_desc";

            // Getting Data from cookies
            CheckBoxesLogic.SetCheckboxes(Request, Checkboxes);
            // Setting currencies
            SetCurrencies();

            if (TableI.Items == null || TableI.Items.Count == 0)
                return;
            if (!string.IsNullOrWhiteSpace(sortOption))
                TableI.Items = TableOperations.ItemSort(TableI.Items, sortOption);
        }
        public async Task<ItemTable> SetItemsAsync()
        {
            // For now the user can only have 1 Table, although Database is prepared for N-N relationship
            string username = User.Identity.Name;
            int userId = (await _context.Users.FirstOrDefaultAsync(x => x.UserName == username)).Id;
            // Getting Data from Database 
            int tableId = (await _context.UserTables.FirstOrDefaultAsync(x => x.UserId == userId)).TableId;

            var table = await _context.ItemTables.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == tableId);
            table.Items = table.Items.OrderBy(x => x.Position).ToList();

            return table;
        }

        // To change places list of items
        public async Task ChangePosAsync(int[] ids)
        {
            if (StaticFunctions.IsSorted(ids))
            {
                TableI = await SetItemsAsync();
                return;
            }
            var table = await SetItemsAsync();
            int counter = 0;
            List<Item> items = table.Items.ToList(); // To use indexes I have to cast ICollection to List
            foreach (var id in ids)
            {
                items[id].Position = counter++;
            }
            table.Items = items;
            table.Items = table.Items.OrderBy(x => x.Position).ToList();
            await _context.SaveChangesAsync();
            TableI = table;
        }
        // To change places list of items
        public async Task ChangePosWithoutAsync(int[] ids, int position)
        {
            int counter = 0;
            if (StaticFunctions.IsSorted(ids))
            {
                TableI = await SetItemsAsync();
                foreach (var item in TableI.Items)
                {
                    if (item.Position != position)
                        item.Position = counter++;
                }
                return;
            }
            var table = await SetItemsAsync();
            List<Item> items = table.Items.ToList(); // To use indexes I have to cast ICollection to List
            foreach (var id in ids)
            {
                if (items[id].Position != position)
                    items[id].Position = counter++;
            }
            table.Items = items;
            table.Items = table.Items.OrderBy(x => x.Position).ToList();
            TableI = table;
        }

        // To add an item
        public async Task<IActionResult> OnPostAddItemAsync()
        {
            await ChangePosAsync(Ids);
            if (!ModelState.IsValid)
            {
                // Setting currencies
                SetCurrencies();
                return Page();
            }

            // call to API http://api.nbp.pl/api/exchangerates/rates/{table}/code}/{date}/

            // date = RRRR-MM-DD
            var maxNumberOfCalls = _configuration.GetValue<int>("MaxNumberOfCalls");
            var client = _clientFactory.CreateClient("nbp");
            var date = AddItemModel.Date.ToString("yyyy-MM-dd");

            if (date != "" && !StaticFunctions.IsValidDate(date, "yyyy-MM-dd"))
            {
                ErrorString = "Nieprawid³owy format daty, nale¿y podaæ format: RRRR-MM-DD (np. 2020-11-20)";
                return RedirectToPage();
            }

            var call = await GetCorrectCall(client, AddItemModel.CurrencyFrom, date);
            int counter = 0;
            while (call == null && counter < maxNumberOfCalls)
            {
                StaticFunctions.PreviousDay(ref date, "yyyy-MM-dd");
                call = await GetCorrectCall(client, AddItemModel.CurrencyFrom, date);
                counter++;
            }
            if (call == null)
            {
                ErrorString = "</br>Nie odnaleziono strony z tabelami lub przekroczono iloœæ zapytañ. SprawdŸ dzia³anie strony NBP.";
                return RedirectToPage();
            }
            else
            {
                ErrorString = null;
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
            Messages = "Dodano nowy przedmiot!";
            if (newItem.Date != newItem.DateTable)
                Messages += "</br>Data transakcji <b>" + newItem.Date.ToString("yyyy-MM-dd") + "</b> jest inna od daty tabeli <b>" + newItem.DateTable.ToString("yyyy-MM-dd") + "</b>";
            return RedirectToPage();
        }

        // To delete an item
        public async Task<IActionResult> OnPostDeleteItemAsync(int number)
        {
            TableI = await SetItemsAsync();
            Item item = TableI.Items.FirstOrDefault(x => x.Position == number);
            await ChangePosWithoutAsync(Ids, number);
            _context.Items.Remove(item);

            await _context.SaveChangesAsync();
            return RedirectToPage();
            // Adding item to database
        }

        // To delete all items
        public async Task<IActionResult> OnPostDeleteAllItemsAsync()
        {
            TableI = await SetItemsAsync();
            _context.Items.RemoveRange(TableI.Items);

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
        // To save a changes in position of list to database
        public async Task<IActionResult> OnPostSaveChangesAsync()
        {
            await ChangePosAsync(Ids);
            CheckBoxesLogic.SaveCheckBoxes(Response, Checkboxes);
            return RedirectToPage();
        }

        // On Post add items to database
        public async Task<IActionResult> OnPostImportTable()
        {
            // Getting Data from Database
            TableI = await SetItemsAsync();
            ErrorString = "";
            string correctness = StaticFunctions.IsStringOfItemTable(FromExcel);
            if (correctness != "SUCCESS")
            {
                ErrorString = correctness;
                return RedirectToPage();
            }

            var client = _clientFactory.CreateClient("nbp");
            int maxNumberOfCalls = _configuration.GetValue<int>("MaxNumberOfCalls");

            var errMsg = await ExcelLogic.ImportTable(_context, client, TableI, maxNumberOfCalls, FromExcel);

            ErrorString = errMsg.Item1;
            Messages = errMsg.Item2;

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostExportTable()
        {
            await ChangePosAsync(Ids);
            CheckBoxesLogic.SaveCheckBoxes(Response, Checkboxes);
            ToExcel = ExcelLogic.ExportTable(Response, Checkboxes, TableI);
            return RedirectToPage();
        }



        // Private methods to clarify code
        private void SetCurrencies()
        {
            foreach (string currency in _configuration.GetSection("Currencies").Get<string[]>().Where(x => x != "PLN"))
            {
                AddItemModel.CurrenciesFrom.Add(new SelectListItem { Value = currency, Text = currency });
            }
            AddItemModel.CurrenciesTo.Add(new SelectListItem { Value = "PLN", Text = "PLN" });
        }

        private async Task<SingleRateModel> GetCorrectCall(HttpClient client, string code, string date)
        {
            try
            {
                return await client.GetFromJsonAsync<SingleRateModel>($"rates/A/{code}/{date}?format=json");
            }
            catch (Exception ex)
            {
                ErrorString = $"Error has accured:</br>{ex.Message}</br>";
                return null;
            }
        }

        private async Task<SingleRateModel> GetCorrectCallNOERROR(HttpClient client, string code, string date)
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

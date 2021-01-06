using CurrencyApp.Backend;
using CurrencyApp.Models.Application.ItemTable;
using CurrencyAppDatabase.DataAccess;
using CurrencyAppDatabase.DatabaseOperations;
using CurrencyAppDatabase.Models.CurrencyApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        // Checkboxes
        [BindProperty]
        public bool NameCheck { get; set; } // 1
        [BindProperty]
        public bool DateCheck { get; set; } // 2
        [BindProperty]
        public bool PriceCheck { get; set; } // 3
        [BindProperty]
        public bool CurrencyFCheck { get; set; } // 4
        [BindProperty]
        public bool RateCheck { get; set; }// 5
        [BindProperty]
        public bool PriceConCheck { get; set; } // 6
        [BindProperty]
        public bool DateTableCheck { get; set; } // 7
        [BindProperty]
        public bool CurrencyTCheck { get; set; } // 8
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
            TableI = await SetItemsAsync();
            // Getting Data from Cookies 
            SetCheckboxes();
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

            // Getting Data from Database !!! TO-DO !!!
            SetCheckboxes();
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
            // Getting Data from Database !!! TO-DO !!!
            int tableId = (await _context.UserTables.FirstOrDefaultAsync(x => x.UserId == userId)).TableId;

            var table = await _context.ItemTables.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == tableId);
            table.Items = table.Items.OrderBy(x => x.Position).ToList();

            return table;
        }
        public void SetCheckboxes()
        {
            // Geting checkboxes from cookies
            if (Request.Cookies["CheckboxesCookie"] == null)
            {
                NameCheck = false;
                DateCheck = false;
                PriceCheck = false;
                CurrencyFCheck = false;
                RateCheck = false;
                PriceConCheck = false;
                DateTableCheck = false;
                CurrencyTCheck = false;
                return;
            }
            var checkBoxes = JsonConvert.DeserializeObject<bool[]>(Request.Cookies["CheckboxesCookie"]);
            NameCheck = checkBoxes[0];
            DateCheck = checkBoxes[1];
            PriceCheck = checkBoxes[2];
            CurrencyFCheck = checkBoxes[3];
            RateCheck = checkBoxes[4];
            PriceConCheck = checkBoxes[5];
            DateTableCheck = checkBoxes[6];
            CurrencyTCheck = checkBoxes[7];
        }

        // To save selected checkboxes
        public void SaveCheckBoxes()
        {
            bool[] checkBoxes = new bool[8];
            checkBoxes[0] = NameCheck;
            checkBoxes[1] = DateCheck;
            checkBoxes[2] = PriceCheck;
            checkBoxes[3] = CurrencyFCheck;
            checkBoxes[4] = RateCheck;
            checkBoxes[5] = PriceConCheck;
            checkBoxes[6] = DateTableCheck;
            checkBoxes[7] = CurrencyTCheck;
            // Setting the values of checkboxes, async
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTimeOffset.MaxValue
            };
            string itemsString = JsonConvert.SerializeObject(checkBoxes);
            Response.Cookies.Append("CheckboxesCookie", itemsString, options);
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
                ErrorString += "</br>Nie odnaleziono strony z tabelami lub przekroczono iloœæ zapytañ. SprawdŸ dzia³anie strony NBP.";
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
            // Adding item to database
        }
        // To save a changes in position of list to database
        public async Task<IActionResult> OnPostSaveChangesAsync()
        {

            // Jest to wywo³ywane przy 
            await ChangePosAsync(Ids);
            SaveCheckBoxes();
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
            List<Item> newItems = new List<Item>();

            string[] rows = FromExcel.Split("\r\n");
            List<string> addedItems = new List<string>();
            var client = _clientFactory.CreateClient("nbp");
            var maxNumberOfCalls = _configuration.GetValue<int>("MaxNumberOfCalls");
            foreach (var row in rows.Skip(1))
            {

                string[] words = row.Split("\t");
                // call to API http://api.nbp.pl/api/exchangerates/rates/{table}/code}/{date}/

                // date = RRRR-MM-DD

                var date = DateTime.Parse(words[1]).ToString("yyyy-MM-dd");

                var call = await GetCorrectCallNOERROR(client, words[3], date);
                string errString = "";
                int counter = 0;
                while (call == null && counter < maxNumberOfCalls)
                {
                    errString += "</br>" + date;
                    StaticFunctions.PreviousDay(ref date, "yyyy-MM-dd");
                    errString += "</br>" + date + "</br>";
                    call = await GetCorrectCallNOERROR(client, words[3], date);
                    counter++;
                }
                if (call == null)
                {
                    errString += "Nie uda³o siê dodaæ przedmiotu <b>" + words[0] + "</b>. B³¹d przy wysy³aniu zapytania </br>";
                }
                else
                {
                    errString = null;
                }
                if (!string.IsNullOrWhiteSpace(errString))
                    ErrorString += errString;
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
            Messages = "Koniec dodawania przedmiotów, dodano:";
            foreach (var itemS in addedItems)
            {
                Messages += "</br>" + itemS;
            }
            return RedirectToPage();
        }
        //To get list to outuput excel - \t as a separator of columns \n as separator of rows
        //  colname1    colname2    colname3    ...
        //  val11       val12       val13       ...
        //  val21       val22       val23       ...
        //  ...         ...         ...
        public async Task<IActionResult> OnPostExportTable()
        {
            await ChangePosAsync(Ids);
            SaveCheckBoxes();
            ToExcel = "";

            if (!NameCheck)
                ToExcel += "Nazwa\t";
            if (!DateCheck)
                ToExcel += "Data\t";
            if (!PriceCheck)
                ToExcel += "Cena\t";
            if (!CurrencyFCheck)
                ToExcel += "Waluta\t";
            if (!RateCheck)
                ToExcel += "Kurs\t";
            if (!PriceConCheck)
                ToExcel += "Cena po przeliczeniu\t";
            if (!DateTableCheck)
                ToExcel += "Data tabeli\t";
            if (!CurrencyTCheck)
                ToExcel += "Waluta docelowa\t";

            ToExcel += "\n";

            foreach (var item in TableI.Items)
            {
                if (!NameCheck)
                    ToExcel += item.Name + "\t";
                if (!DateCheck)
                    ToExcel += item.Date.ToString("yyyy-MM-dd") + "\t";
                if (!PriceCheck)
                    ToExcel += item.Price + "\t";
                if (!CurrencyFCheck)
                    ToExcel += item.CurrencyFrom + "\t";
                if (!RateCheck)
                    ToExcel += item.Rate.ToString().Replace(".", ",") + "\t";
                if (!PriceConCheck)
                    ToExcel += item.ConvertedPrice + "\t";
                if (!DateTableCheck)
                    ToExcel += item.DateTable.ToString("yyyy-MM-dd") + "\t";
                if (!CurrencyTCheck)
                    ToExcel += item.CurrencyTo + "\t";
                ToExcel += "\n";
            }
            ModelState.Clear();
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
            catch (Exception ex)
            {
                ErrorString += $"rates/A/{code}/{date}?format=json";
                return null;
            }
        }
    }
}

using CurrencyAppDatabase.DataAccess;
using CurrencyAppDatabase.DatabaseOperations;
using CurrencyAppDatabase.Models.CurrencyApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyApp.Pages
{
    public class ItemsModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly CurrencyContext _context;
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
        public ItemTable TableI { get; set; }
        public ItemsModel(IHttpClientFactory clientFactory, IConfiguration iConfig, CurrencyContext context, IMemoryCache cache)
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
            _cache = cache;
        }
        // To get list of items
        public async Task OnGetAsync()
        {
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
            // Getting Data from Database !!! TO-DO !!!
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
            if (Request.Cookies["CheboxesCookie"] == null){
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
            var checkBoxes = JsonConvert.DeserializeObject<bool[]>(Request.Cookies["CheboxesCookie"]);
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
        public void OnPostChecked(bool[] checkBoxes)
        {
            // Setting the values of checkboxes, async
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.MaxValue
            };
            string itemsString = JsonConvert.SerializeObject(checkBoxes);
            Response.Cookies.Append("CheboxesCookie", itemsString, options);
        }
        // To change places list of items
        public async Task ChangePosAsync(int[] ids)
        {
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

        // To add an item
        public void OnPostAddItem()
        {
            var x = 2;
            // Adding item to database
        }
        // To delete an item
        public async Task<IActionResult> OnPostDeleteItemAsync(int id)
        {
            TableI = await SetItemsAsync();
            Item item = TableI.Items.FirstOrDefault(x => x.Position == id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToPage();
            // Adding item to database
        }
        // To save a changes in position of list to database
        public async Task<IActionResult> OnPostSaveChangesAsync(int[] ids)
        {
            // Jest to wywo³ywane przy 
            await ChangePosAsync(ids);
            await _context.SaveChangesAsync();
            return Page();
        }



        // On Post add items to database
        public void OnPostImportTable(string tableString)
        {
            throw new NotImplementedException();
        }
        //To get list to outuput excel - \t as a separator of columns \n as separator of rows
        //  colname1    colname2    colname3    ...
        //  val11       val12       val13       ...
        //  val21       val22       val23       ...
        //  ...         ...         ...
        public void OnGetExportTable()
        {
            throw new NotImplementedException();
        }
    }
}

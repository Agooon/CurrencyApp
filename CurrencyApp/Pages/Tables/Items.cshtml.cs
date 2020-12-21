using CurrencyApp.Backend;
using CurrencyAppDatabase.DataAccess;
using CurrencyAppDatabase.Models.CurrencyApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyApp.Pages
{
    public class ItemsModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly CurrencyContext _context;
        // Checkboxes
        public bool NameCheck { get; set; }
        public bool DateCheck { get; set; }
        public bool PriceCheck { get; set; }
        public bool CurrencyFCheck { get; set; }
        public bool RateCheck { get; set; }
        public bool PriceConCheck { get; set; }
        public bool CurrencyTCheck { get; set; }
        // Sorting properties
        public string SortName { get; set; }
        public string SortDate { get; set; }
        public string SortPrice { get; set; }
        public string SortCurrency { get; set; }
        public string SortRate { get; set; }

        public ItemTable TableI { get; set; }
        public ItemsModel(IHttpClientFactory clientFactory, IConfiguration iConfig, CurrencyContext context)
        {
            _clientFactory = clientFactory;
            _configuration = iConfig;
            _context = context;
            SortName = "name";
            SortDate = "date_desc";
            SortPrice = "price";
            SortCurrency = "currency";
            SortRate = "rate";
        }
        // To get list of items
        public async Task OnGetAsync(int id,string sortOrder)
        {
            // Getting Data from Database !!! TO-DO !!!
            await SetItemsAsync(id);
            // Getting Data from Database !!! TO-DO !!!
            SetCheckboxes();
        }

        // To sort list of items
        public async Task OnGetSortAsync(int id,string sortOption = "")
        {

            SortName = sortOption == "name" ? "name_desc" : "name";
            SortDate = sortOption == "date_desc" ? "date" : "date_desc";
            SortPrice = sortOption == "price_desc" ? "price" : "price_desc";
            SortCurrency = sortOption == "currency" ? "currency_desc" : "currency";
            SortRate = sortOption == "rate_desc" ? "rate" : "rate_desc";

            // Getting Data from Database !!! TO-DO !!!
            await SetItemsAsync(id);
            if(TableI == null)
            {
                TempData["ErrorString"] = "Nie odnaleziono poszukiwanej tabeli";
            }
            // Getting Data from Database !!! TO-DO !!!
            SetCheckboxes();

            //if (items == null || items.Count == 0)
            //    return;
            //if (!string.IsNullOrWhiteSpace(sortOption))
            //    TableOperations.ItemSort(TableI.Items, sortOption);
        }
        public async Task SetItemsAsync(int Id)
        {
            TableI = await _context.ItemTables.FirstOrDefaultAsync(x => x.Id == Id);
        }
        public void SetCheckboxes()
        {
            // Geting checkboxes from database
            //var checkBoxes = ???
            //NameCheck = checkBoxes[0];
            //DateCheck = checkBoxes[1];
            //PriceCheck = checkBoxes[2];
            //CurrencyFCheck = checkBoxes[3];
            //RateCheck = checkBoxes[4];
            //PriceConCheck = checkBoxes[5];
            //CurrencyTCheck = checkBoxes[6];
        }
        // To save selected checkboxes
        public void OnPostChecked(bool[] checkBoxes)
        {
            // Setting the values of checkboxes, async
            //CookieOptions options = new CookieOptions()
            //{
            //    Expires = DateTime.MaxValue
            //};
            //string itemsString = JsonConvert.SerializeObject(checkBoxes);
            //Response.Cookies.Append("CheboxesCookie", itemsString, options);
        }
        // To change places list of items
        public void OnPostChangePos(int[] ids)
        {
            //SetItems();
            //int counter = 0;
            //foreach (var itemId in ids)
            //{
            //    items[itemId].Position = counter++;
            //}
            //items = items.OrderBy(x => x.Position).ToList();
            //SaveChanges();
        }

        // To add an item
        public void OnPostAddItem()
        {
            // Adding item to database
        }
        // To save a list to cookies
        public void SaveChanges()
        {
            // Saving changes to database
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

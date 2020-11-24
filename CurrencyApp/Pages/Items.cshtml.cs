using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CurrencyApp.Backend;
using CurrencyApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CurrencyApp.Pages
{
    public class ItemsModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
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

        public List<ItemViewModel> items;
        public ItemsModel(IHttpClientFactory clientFactory, IConfiguration iConfig)
        {
            _clientFactory = clientFactory;
            _configuration = iConfig;
            SortName = "name";
            SortDate = "date_desc";
            SortPrice = "price";
            SortCurrency = "currency";
            SortRate = "rate";
        }
        public void SetItems()
        {
            // getting items from database
            // items = ???
        }
        public void InitializeDatabase()
        {

            items = TableOperations.GenerateTestList();
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.MaxValue
            };
            string itemsString = JsonConvert.SerializeObject(items);
            Response.Cookies.Append("ItemsCookie", itemsString, options);
        }

        // To get list of items
        public void OnGet(string sortOrder)
        {
            SetItems();
            SetCheckboxes();
        }

        // To sort list of items
        public void OnGetSort(string sortOption = "")
        {

            SortName = sortOption == "name" ? "name_desc" : "name";
            SortDate = sortOption == "date_desc" ? "date" : "date_desc";
            SortPrice = sortOption == "price_desc" ? "price" : "price_desc";
            SortCurrency = sortOption == "currency" ? "currency_desc" : "currency";
            SortRate = sortOption == "rate_desc" ? "rate" : "rate_desc";

            SetItems();
            SetCheckboxes();

            if (items == null || items.Count == 0)
                return;
            if (!string.IsNullOrWhiteSpace(sortOption))
                TableOperations.ItemSort(ref items, sortOption);
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
            SetItems();
            int counter = 0;
            foreach (var itemId in ids)
            {
                items[itemId].Position = counter++;
            }
            items = items.OrderBy(x => x.Position).ToList();
            SaveChanges();
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
        // On Post return an json item list
        public void OnPostImportTable(string tableString)
        {
            throw new NotImplementedException();
        }
        //To get list to outuput excel
        public void OnGetExportTable()
        {
            throw new NotImplementedException();
        }
    }
}

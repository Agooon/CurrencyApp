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

        public string SortName { get; set; }
        public string SortDate { get; set; }
        public string SortPrice { get; set; }
        public string SortCurrency { get; set; }
        public string SortRate { get; set; }

        public List<ItemModel> items;
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
            items = JsonConvert.DeserializeObject<List<ItemModel>>(Request.Cookies["ItemsCookie"]);
        }
        public void InitializeCookies()
        {

            List<ItemModel> items = new List<ItemModel>()
            {
                new ItemModel("xp  69", "2020-11-19", "USD", (decimal)251.99, (decimal)3.7,0),
                new ItemModel(":O   1", "2020-11-19", "USD", (decimal)251.99, (decimal)3.7,1),
                new ItemModel("xD   2", "2020-11-19", "EUR", (decimal)22.99, (decimal)4.1,2),
                new ItemModel("xD   3", "2020-11-18", "EUR", (decimal)22.99, (decimal)4.09,3),
                new ItemModel("Item 4", "2020-11-15", "AUD", (decimal)100.0, (decimal)2.75,4),
                new ItemModel("Item 5", "2020-11-14", "AUD", (decimal)100.0, (decimal)2.75,5),
                new ItemModel("Item 6", "2020-11-13", "AUD", (decimal)100.0, (decimal)2.75,6),
                new ItemModel("Item 7", "2020-10-01", "AUD", (decimal)100.0, (decimal)2.83,7),
                new ItemModel(":O   7", "2020-11-19", "USD", (decimal)251.99, (decimal)3.7,8),
                new ItemModel(":C   8", "2019-11-19", "USD", (decimal)300.25, (decimal)3.7,9),
                new ItemModel(":C   9", "2020-11-19", "USD", (decimal)300.25, (decimal)3.7,10)
            };
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.MaxValue
            };
            string itemsString = JsonConvert.SerializeObject(items);
            this.items = items;
            Response.Cookies.Append("ItemsCookie", itemsString, options);
        }

        // To get list of items
        public void OnGet(string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(Request.Cookies["ItemsCookie"]))
            {
                InitializeCookies();
                return;
            }
            SetItems();
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

            if (items == null || items.Count == 0)
                return;
            if (!string.IsNullOrWhiteSpace(sortOption))
                StaticFunctions.ItemSort(ref items, sortOption);
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
        public void OnPostAddItem(string name, string date, string xd)
        {
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.MaxValue
            };
            string itemsString = JsonConvert.SerializeObject(items);
            Response.Cookies.Append("ItemsCookie", itemsString, options);
        }
        // To save a list to cookies
        public void SaveChanges()
        {
            CookieOptions options = new CookieOptions()
            {
                Expires = DateTime.MaxValue
            };
            string itemsString = JsonConvert.SerializeObject(items);
            Response.Cookies.Append("ItemsCookie", itemsString, options);
        }
    }
}

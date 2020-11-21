using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
        public List<ItemModel> Items { get; set; }
        public ItemsModel(IHttpClientFactory clientFactory, IConfiguration iConfig)
        {
            _clientFactory = clientFactory;
            _configuration = iConfig;
        }

        public void InitializeCookies()
        {

            List<ItemModel> items = new List<ItemModel>()
            {
                new ItemModel(":O   1", "2020-11-19", "USD", (decimal)251.99),
                new ItemModel("xD   2", "2020-11-19", "EUR", (decimal)22.99),
                new ItemModel("xD   3", "2020-11-18", "EUR", (decimal)22.99),
                new ItemModel("Item 4", "2020-11-15", "AUD", (decimal)100.0),
                new ItemModel("Item 5", "2020-11-14", "AUD", (decimal)100.0),
                new ItemModel("Item 6", "2020-10-01", "AUD", (decimal)100.0),
                new ItemModel(":O   7", "2020-11-19", "USD", (decimal)251.99),
                new ItemModel(":C   8", "2019-11-19", "USD", (decimal)300.25),
                new ItemModel(":C   9", "2020-11-19", "USD", (decimal)300.25)
            };
            CookieOptions options = new CookieOptions() { 
                Expires = DateTime.MaxValue
            };
            string itemsString = JsonConvert.SerializeObject(items);
            Response.Cookies.Append("ItemsCookie", itemsString, options);
        }

        // To get list of items
        public void OnGet()
        {
            if (string.IsNullOrWhiteSpace(Request.Cookies["ItemsCookie"]))
                InitializeCookies();
            Items = JsonConvert.DeserializeObject<List<ItemModel>>(Request.Cookies["ItemsCookie"]);
        }

        // To add an item to list
        public void OnPost()
        {

        }
    }
}

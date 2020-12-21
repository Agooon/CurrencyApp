using CurrencyApp.Models;
using CurrencyAppDatabase.Models.CurrencyApp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CurrencyApp.Backend
{
    public static class TableOperations
    {
        public static void ItemSort(ICollection<Item> items, string sortOption)
        {
            switch (sortOption)
            {
                case "name":
                    items = items.OrderBy(x => x.Name).ToList();
                    break;
                case "name_desc":
                    items = items.OrderByDescending(x => x.Name).ToList();
                    break;
                case "date":
                    items = items.OrderBy(x => x.Date).ToList();
                    break;
                case "date_desc":
                    items = items.OrderByDescending(x => x.Date).ToList();
                    break;
                case "price":
                    items = items.OrderBy(x => x.Price).ToList();
                    break;
                case "price_desc":
                    items = items.OrderByDescending(x => x.Price).ToList();
                    break;
                case "currency":
                    items = items.OrderBy(x => x.CurrencyFrom).ToList();
                    break;
                case "currency_desc":
                    items = items.OrderByDescending(x => x.CurrencyFrom).ToList();
                    break;
                case "rate":
                    items = items.OrderBy(x => x.Rate).ToList();
                    break;
                case "rate_desc":
                    items = items.OrderByDescending(x => x.Rate).ToList();
                    break;
                default:
                    break;
            }
        }
    }
}

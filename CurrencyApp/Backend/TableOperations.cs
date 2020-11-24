using CurrencyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyApp.Backend
{
    public static class TableOperations
    {
        public static List<ItemViewModel> GenerateTestList()
        {
            return new List<ItemViewModel>()
            {
                new ItemViewModel("xp  69", "2020-11-19", "USD", (decimal)251.99, (decimal)3.7,"2020-11-19",0),
                new ItemViewModel(":O   1", "2020-11-19", "USD", (decimal)251.99, (decimal)3.7,"2020-11-19",1),
                new ItemViewModel("xD   2", "2020-11-19", "EUR", (decimal)22.99, (decimal)4.1,"2020-11-19",2),
                new ItemViewModel("xD   3", "2020-11-18", "EUR", (decimal)22.99, (decimal)4.09,"2020-11-18",3),
                new ItemViewModel("Item 4", "2020-11-15", "AUD", (decimal)100.0, (decimal)2.75,"2020-11-13",4),
                new ItemViewModel("Item 5", "2020-11-14", "AUD", (decimal)100.0, (decimal)2.75,"2020-11-13",5),
                new ItemViewModel("Item 6", "2020-11-13", "AUD", (decimal)100.0, (decimal)2.75,"2020-11-13",6),
                new ItemViewModel("Item 7", "2020-10-01", "AUD", (decimal)100.0, (decimal)2.83,"2020-10-01",7),
                new ItemViewModel(":O   7", "2020-11-19", "USD", (decimal)251.99, (decimal)3.7,"2020-11-19",8),
                new ItemViewModel(":C   8", "2019-11-19", "USD", (decimal)300.25, (decimal)3.7,"2020-11-19",9),
                new ItemViewModel(":C   9", "2020-11-19", "USD", (decimal)300.25, (decimal)3.7,"2020-11-19",10)
            };
        }
        public static void ItemSort(ref List<ItemViewModel> items, string sortOption)
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
                    items = items.OrderBy(x => DateTime.Parse(x.Date)).ToList();
                    break;
                case "date_desc":
                    items = items.OrderByDescending(x => DateTime.Parse(x.Date)).ToList();
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

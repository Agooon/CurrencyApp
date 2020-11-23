using CurrencyApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyApp.Backend
{
    public static class StaticFunctions
    {
        public static bool IsValidDate(string value, string dateFormat)
        {
            return DateTime.TryParseExact(value, dateFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out _);
        }
        public static void PreviousDay(ref string value, string dateFormat)
        {
            DateTime newDate;
            DateTime.TryParseExact(value, dateFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out newDate);
            value = newDate.AddDays(-1).ToString(dateFormat);
        }
        public static void ItemSort(ref List<ItemModel> items, string sortOption)
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

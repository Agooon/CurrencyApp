using System;
using System.Globalization;

namespace CurrencyApp.Models
{
    public class ItemModel
    {
        public ItemModel(string name, string date, string currency, decimal price)
        {
            Name = name;
            Date = date;
            Price = price;
            Currency = currency;
        }
        public string Name { get; set; }
        public string Date { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }
}

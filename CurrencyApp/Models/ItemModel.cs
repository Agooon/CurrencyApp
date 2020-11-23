using System;
using System.Globalization;

namespace CurrencyApp.Models
{
    public class ItemModel
    {
        public ItemModel(string name, string date, string currencyFrom, decimal price, decimal rate, int position, string currencyTo = "PLN")
        {
            Name = name;
            Date = date;
            Price = price;
            CurrencyFrom = currencyFrom;
            Rate = rate;
            CurrencyTo = currencyTo;
            Position = position;
            ConvertedPrice = decimal.Round(price / rate, 3);
        }
        public string Name { get; set; }
        public string Date { get; set; }
        public int Position { get; set; }
        public decimal Price { get; set; }
        public string CurrencyFrom { get; set; }
        public decimal Rate { get; set; }
        public string CurrencyTo { get; set; }
        public decimal ConvertedPrice { get; set; }
    }
}

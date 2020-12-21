using CurrencyAppDatabase.Models.CurrencyApp;
using System;
using System.Globalization;

namespace CurrencyApp.Models
{
    public class ItemViewModel
    {
        public ItemViewModel(Item dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Price = dto.Price;

        }
        public ItemViewModel(string name, string date, string currencyFrom, decimal price, decimal rate,string dateTable, int position, string currencyTo = "PLN")
        {
            Name = name;
            Date = date;
            Price = price;
            CurrencyFrom = currencyFrom;
            Rate = rate;
            CurrencyTo = currencyTo;
            Position = position;
            DateTable = dateTable;
            ConvertedPrice = decimal.Round(price * rate, 3);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public decimal Price { get; set; }
        public string CurrencyFrom { get; set; }
        public int Position { get; set; }
        public decimal Rate { get; set; }
        public string DateTable { get; set; }
        public decimal ConvertedPrice { get; set; }
        public string CurrencyTo { get; set; }
    }
}

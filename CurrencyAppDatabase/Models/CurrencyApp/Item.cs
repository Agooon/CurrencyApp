using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyAppDatabase.Models.CurrencyApp
{
    public class Item
    {
        public Item(string name, DateTime date, string currencyFrom, decimal price, decimal rate, DateTime dateTable, int position, string currencyTo = "PLN")
        {
            Name = name;
            Date = date;
            CurrencyFrom = currencyFrom;
            Price = price;
            Rate = rate;
            DateTable = dateTable;
            Position = position;
            ConvertedPrice = decimal.Round(price * rate, 3);
            CurrencyTo = currencyTo;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [Column(TypeName = "decimal(12,4)")]
        public decimal Price { get; set; }
        [Required]
        public int Position { get; set; }
        [Column(TypeName = "VARCHAR(8)")]
        public string CurrencyFrom { get; set; }
        [Column(TypeName = "decimal(12,4)")]
        public decimal Rate { get; set; }
        public DateTime DateTable { get; set; }
        [Column(TypeName = "decimal(12,4)")]
        public decimal ConvertedPrice { get; set; }
        [Column(TypeName = "VARCHAR(8)")]
        public string CurrencyTo { get; set; }

        public int ItemTableId { get; set; }
        [ForeignKey("ItemTableId")]
        public ItemTable ItemTable { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace CurrencyAppDatabase.Models.CurrencyApp
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Position { get; set; }
        public string CurrencyFrom { get; set; }
        public decimal Rate { get; set; }
        public DateTime DateTable { get; set; }
        public decimal ConvertedPrice { get; set; }
        public string CurrencyTo { get; set; }
    }
}

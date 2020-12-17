using System;
using System.ComponentModel.DataAnnotations;

namespace CurrenycAppDatabase.Models.CurrencyApp
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
        public string CurrencyFrom { get; set; }
        public decimal Rate { get; set; }
        public DateTime DateTable { get; set; }
        public decimal ConvertedPrice { get; set; }
        public string CurrencyTo { get; set; }
    }
}

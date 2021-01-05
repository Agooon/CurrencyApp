using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyApp.Models.Application.ItemTable
{
    public class ItemAddModel
    {
        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [MaxLength(3)]
        public string CurrencyFrom { get; set; }
        [Required]
        [MaxLength(3)] // For now it's always PLN
        public string CurrencyTo { get; set; }

        public List<SelectListItem> CurrenciesFrom { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> CurrenciesTo { get; set; } = new List<SelectListItem>();
    }
}

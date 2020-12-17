using System;
using System.ComponentModel.DataAnnotations;

namespace CurrencyAppDatabase.Models.AdminPanel
{
    public class RegistrationCode
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CurrencyAppDatabase.Models.AdminPanel
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        // Nvarcha(max) is here, because of the change in length of messages, 
        // it could have information about what exacly items were deleted, 
        // when the whole Table is being removed.
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}

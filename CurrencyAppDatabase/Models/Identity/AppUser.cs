using CurrencyAppDatabase.Models.CurrencyApp.Connections;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyAppDatabase.Models.CurrencyApp
{
    public class AppUser : IdentityUser<int>
    {
        public AppUser() : base()
        {
        }
        [Required]
        public DateTime CreatedAt { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-yyyy-MM}", ApplyFormatInEditMode = true)]
        public DateTime SubscriptionUntil { get; set; }
        public int LimitOfTables { get; set; }

        public virtual ICollection<UserTable> ItemTables { get; set; }
    }
}

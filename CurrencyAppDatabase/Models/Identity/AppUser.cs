using CurrencyAppDatabase.Models.CurrencyApp.Connections;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrenycAppDatabase.Models.CurrencyApp
{
    public class AppUser : IdentityUser<int>
    {
        public AppUser() : base()
        {
        }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime SubscriptionUntil { get; set; }
        public int LimitOfTables { get; set; }

        public virtual ICollection<UserTable> ItemTables { get; set; }
    }
}

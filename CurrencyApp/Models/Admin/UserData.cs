using CurrencyApp.Models.Application;
using CurrencyAppDatabase.Models.Identity;
using System;
using System.Collections.Generic;

namespace CurrencyApp.Models.Admin
{
    public class UserData
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime SubscriptionUntil { get; set; }
        public int LimitOfTables { get; set; }
        public List<AppRole> Roles { get; set; }
        public List<ItemTableViewModel> Tables { get; set; }
    }
}

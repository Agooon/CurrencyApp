using CurrencyAppDatabase.Models.AdminPanel;
using CurrencyAppDatabase.Models.CurrencyApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyApp.Models.Admin
{
    public class DashboardData
    {
        public List<AppUser> Users { get; set; }
        public List<RegistrationCode> RegistrationCodes { get; set; }
        public int MyProperty { get; set; }
    }
}

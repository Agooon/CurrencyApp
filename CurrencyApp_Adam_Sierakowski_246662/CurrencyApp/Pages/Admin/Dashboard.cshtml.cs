using CurrencyApp.Models.Admin;
using CurrencyAppDatabase.DataAccess;
using CurrencyAppDatabase.Models.AdminPanel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace CurrencyApp.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly CurrencyContext _context;

        public List<UserData> Users { get; set; }
        public List<RegistrationCode> RegistrationCodes { get; set; }

        public DashboardModel(CurrencyContext context,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }
        public void OnGet()
        {
        }
    }
}

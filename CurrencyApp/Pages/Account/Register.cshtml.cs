using CurrencyApp.Models.Account;
using CurrencyAppDatabase.DataAccess;
using CurrencyAppDatabase.Models.AdminPanel;
using CurrencyAppDatabase.Models.CurrencyApp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyApp.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly CurrencyContext _context;
        private readonly IConfiguration _configuration;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            CurrencyContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        [BindProperty]
        public RegisterViewModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public string RegistrationCode { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task OnGetAsync(string returnUrl = null, string registrationCode = "")
        {
            Guid guidCode;
            if (!Guid.TryParse(registrationCode, out guidCode))
            {
                TempData["ErrorString"] = "Podano nieprawidłowy format linku rejestracyjny";
                return;
            }
            else
            {
                RegistrationCode regCode = await _context.RegistrationCodes.FindAsync(guidCode);
                if (regCode == null)
                {
                    TempData["ErrorString"] = "Podano nieprawidłowy/zużyty link rejestracyjny";
                    return;
                }
                else
                {
                    if (regCode.ExpirationDate < DateTime.Now)
                    {
                        // Deleting of outudated registration link
                        TempData["ErrorString"] = "Kod uległ przedawnieniu.";
                        _context.RegistrationCodes.Remove(regCode);
                        await _context.SaveChangesAsync();
                        return;
                    }
                }
                ReturnUrl = returnUrl;
                RegistrationCode = registrationCode;
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null, string registrationCode = "")
        {
            Guid guidCode;
            if (!Guid.TryParse(registrationCode, out guidCode))
            {
                TempData["ErrorString"] = "Podano nieprawidłowy format linku rejestracyjny";
                return RedirectToPage("Register", new { returnUrl, registrationCode });
            }
            RegistrationCode regCode = await _context.RegistrationCodes.FindAsync(guidCode);
            if (regCode == null)
            {
                TempData["ErrorString"] = "Podano nieprawidłowy/zużyty kod rejestracyjny";
                return RedirectToPage("Register", new { returnUrl, registrationCode });
            }

            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = Input.Username, Email = Input.Email, CreatedAt = DateTime.Now, SubscriptionUntil = DateTime.Now.AddDays(1), LimitOfTables = 10 };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _context.RegistrationCodes.Remove(regCode);
                    await _context.SaveChangesAsync();
                    await _userManager.AddToRoleAsync(user, _configuration.GetSection("RoleList").Get<string[]>()[1]);
                    await _signInManager.SignInAsync(user, isPersistent: false);                    

                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            RegistrationCode = registrationCode;
            ReturnUrl = returnUrl;
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

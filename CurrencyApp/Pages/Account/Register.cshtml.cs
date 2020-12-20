using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using CurrenycAppDatabase.Models.CurrencyApp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using CurrenycAppDatabase.DataAccess;
using CurrencyAppDatabase.Models.AdminPanel;

namespace CurrencyApp.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly CurrencyContext _context;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            CurrencyContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

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
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null, string registrationCode = "")
        {
            Guid guidCode;
            if (!Guid.TryParse(registrationCode, out guidCode))
            {
                TempData["ErrorString"] = "Podano nieprawidłowy format linku rejestracyjny";
                return Page();
            }
            RegistrationCode regCode = await _context.RegistrationCodes.FindAsync(guidCode);
            if (regCode == null)
            {
                TempData["ErrorString"] = "Podano nieprawidłowy/zużyty kod rejestracyjny";
                return Page();
            }
                

            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = Input.Username, Email = Input.Email, CreatedAt = DateTime.Now, SubscriptionUntil = DateTime.Now.AddDays(1), LimitOfTables = 10 };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _context.RegistrationCodes.Remove(regCode);
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

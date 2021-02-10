using System.ComponentModel.DataAnnotations;

namespace CurrencyApp.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} musi posiadać między {2} a {1} ilośc znaków.", MinimumLength = 6)]
        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "{0} musi posiadać między {2} a {1} ilośc znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasła do siebie nie pasują.")]
        public string ConfirmPassword { get; set; }
    }
}

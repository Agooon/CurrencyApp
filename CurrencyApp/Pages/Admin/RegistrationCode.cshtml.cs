﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CurrencyAppDatabase.DataAccess;
using CurrencyAppDatabase.Models.AdminPanel;

namespace CurrencyApp.Pages.Admin
{
    public class RegistrationCodeModel : PageModel
    {
        private readonly CurrencyAppDatabase.DataAccess.CurrencyContext _context;

        public RegistrationCodeModel(CurrencyAppDatabase.DataAccess.CurrencyContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public RegistrationCode RegistrationCode { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.RegistrationCodes.Add(RegistrationCode);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

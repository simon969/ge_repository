using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

using ge_repository.Models;

namespace ge_repository.Pages.User
{
    public class CreateModel : PageModel
    {
        private readonly ge_repository.Models.ge_DbContext _context;

        public CreateModel(ge_repository.Models.ge_DbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ge_user ge_user { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
             _context.ge_user.Add(ge_user);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ge_repository.Models;

namespace ge_repository.Pages.User
{
    public class DetailsModel : PageModel
    {
        private readonly ge_repository.Models.ge_DbContext _context;

        public DetailsModel(ge_repository.Models.ge_DbContext context)
        {
            _context = context;
        }

        public ge_user ge_user { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ge_user = await _context.ge_user.FirstOrDefaultAsync(m => m.Id == id);

            if (ge_user == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

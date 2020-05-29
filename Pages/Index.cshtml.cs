using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace ge_repository.Pages
{
    
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        String constAGSDictionary = "3fc7648f-fd41-4732-75af-08d6ffe6183c";
    
        public void OnGet()
        {
       ViewData["article1"] = constAGSDictionary;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NourishingHands.Pages.Parent
{
    public class ParentLetterModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (!string.IsNullOrWhiteSpace(HttpContext.Request.Query["Id"]))
                return Page();
            else
                return RedirectToPage("/Index");          
        }

        public IActionResult OnPost()
        {
            var menteeId = Convert.ToInt32(HttpContext.Request.Query["Id"]);
            var callbackUrl = Url.Page(
                        "/Parent/Application",
                        pageHandler: null,
                        values: new { Id = menteeId },
                        protocol: Request.Scheme);

            return Redirect(callbackUrl);
        }
    }
}

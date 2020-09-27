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
            var menteeId = 0;
            if (!string.IsNullOrWhiteSpace(HttpContext.Request.Query["Id"]))
                menteeId = Convert.ToInt32(HttpContext.Request.Query["Id"]);
            else
                return RedirectToPage("/Index");

            return Page();
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

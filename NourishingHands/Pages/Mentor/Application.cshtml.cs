using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Mentor
{
    public class ApplicationModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;

        public ApplicationModel(NourishingHandsContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public Person Person { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Person.CreatedOn = DateTime.Now;
            _dbContext.Persons.Add(Person);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("./");
        }

    }
}

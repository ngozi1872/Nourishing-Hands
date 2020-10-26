using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Admin.EventsVlt
{
    public class CreateModel : PageModel
    {
        private readonly NourishingHands.Areas.Identity.NourishingHands.Data.NourishingHandsContext _context;

        public CreateModel(NourishingHands.Areas.Identity.NourishingHands.Data.NourishingHandsContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id");
        ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public EventVolunteer EventVolunteer { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.EventVolunteers.Add(EventVolunteer);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

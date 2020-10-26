using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Admin.EventsVlt
{
    public class EditModel : PageModel
    {
        private readonly NourishingHands.Areas.Identity.NourishingHands.Data.NourishingHandsContext _context;

        public EditModel(NourishingHands.Areas.Identity.NourishingHands.Data.NourishingHandsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EventVolunteer EventVolunteer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EventVolunteer = await _context.EventVolunteers
                .Include(e => e.Events)
                .Include(e => e.Person).FirstOrDefaultAsync(m => m.EventId == id);

            if (EventVolunteer == null)
            {
                return NotFound();
            }
           ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id");
           ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(EventVolunteer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventVolunteerExists(EventVolunteer.EventId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool EventVolunteerExists(int id)
        {
            return _context.EventVolunteers.Any(e => e.EventId == id);
        }
    }
}

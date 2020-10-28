using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Admin.NHEvents
{
    public class IndexModel : PageModel
    {
        private readonly NourishingHands.Areas.Identity.NourishingHands.Data.NourishingHandsContext _context;

        public IndexModel(NourishingHands.Areas.Identity.NourishingHands.Data.NourishingHandsContext context)
        {
            _context = context;
        }

        public IList<Events> Events { get;set; }
        public IList<EventVolunteer> EventVolunteers { get; set; }

        public async Task OnGetAsync()
        {
            Events = await _context.Events
                .ToListAsync();

            EventVolunteers = await _context.EventVolunteers
                .Include(p => p.Person)
                .ToListAsync();
        }
    }
}

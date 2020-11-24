﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Admin.EventsVlt
{
    public class IndexModel : PageModel
    {
        private readonly NourishingHands.Areas.Identity.NourishingHands.Data.NourishingHandsContext _context;

        public IndexModel(NourishingHands.Areas.Identity.NourishingHands.Data.NourishingHandsContext context)
        {
            _context = context;
        }

        public IList<EventVolunteer> EventVolunteer { get;set; }

        public async Task OnGetAsync()
        {
            EventVolunteer = await _context.EventVolunteers
                .Include(e => e.Events)
                .Include(e => e.Person).ToListAsync();
        }
    }
}
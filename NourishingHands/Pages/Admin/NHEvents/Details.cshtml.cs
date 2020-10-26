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
    public class DetailsModel : PageModel
    {
        private readonly NourishingHands.Areas.Identity.NourishingHands.Data.NourishingHandsContext _context;

        public DetailsModel(NourishingHands.Areas.Identity.NourishingHands.Data.NourishingHandsContext context)
        {
            _context = context;
        }

        public Events Events { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Events = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);

            if (Events == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

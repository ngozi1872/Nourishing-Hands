using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Event
{
    public class EventsModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;

        public EventsModel(NourishingHandsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Areas.Identity.Data.Events> AllEvents  { get; set; }
        public void OnGet()
        {
            AllEvents = _dbContext.Events.Where(e => e.EventStartDate >= DateTime.Now).OrderBy(d => d.EventStartDate).ToList();
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Volunteer
{
    [Authorize]
    public class HomeModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        public HomeModel(NourishingHandsContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public Person Person { get; set; }
        public bool HasPersonRecord { get; set; }
        public List<Events> Events { get; set; }
        public List<EventVolunteer> EventVolunteers { get; set; }

        [BindProperty]
        public Events GetEvent { get; set; }

        public IActionResult OnGet()
        {
            var userId = _userManager.GetUserId(User);
            Person = _dbContext.Persons.FirstOrDefault(p => p.UserId == userId && p.Role.Trim() == "Volunteer");

            if (Person == null || Person.Id <= 0)
                return RedirectToPage("/Volunteer/Application");

            HasPersonRecord = true;
            Events = _dbContext.Events.Where(e => e.EventStartDate > DateTime.Now).ToList();
            EventVolunteers = _dbContext.EventVolunteers.Where(v => v.PersonId == Person.Id).ToList();

            return Page();
        }

        public IActionResult OnPostAddVoluntaryForEvent(int eventID)
        {
            if (eventID == 0)
            {
                return Page();
            }

            var eventVolunteer = new EventVolunteer
            {
                EventId = eventID,
                PersonId = PersonId()
            };

            _dbContext.Add(eventVolunteer);
            _dbContext.SaveChanges();

            Events = _dbContext.Events.Where(e => e.EventStartDate > DateTime.Now).ToList();
            EventVolunteers = _dbContext.EventVolunteers.Where(v => v.PersonId == Person.Id).ToList();

            return Page();
        }

        private int PersonId()
        {
            var userId = _userManager.GetUserId(User);
            Person person = _dbContext.Persons.FirstOrDefault(p => p.UserId == userId);

            if (person != null)
                return person.Id;
            else
                return 0;
        }
    }
}

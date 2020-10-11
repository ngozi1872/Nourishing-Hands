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
        public List<AllEvents> AllEvents { get; set; }

        [BindProperty]
        public Events GetEvent { get; set; }

        public IActionResult OnGet()
        {
            var userId = _userManager.GetUserId(User);
            Person = _dbContext.Persons.FirstOrDefault(p => p.UserId == userId && p.Role.Trim() == "Volunteer");

            if (Person == null || Person.Id <= 0)
                return RedirectToPage("/Volunteer/Application");

            HasPersonRecord = true;
            EventsVolunteer(Person.Id);

            return Page();
        }

        public IActionResult OnPostAddVoluntaryForEvent(int eventID)
        {
            var personId = PersonId();
            if (eventID == 0)
            {
                return Page();
            }

            var eventVolunteer = new EventVolunteer
            {
                EventId = eventID,
                PersonId = personId
            };

            _dbContext.Add(eventVolunteer);
            _dbContext.SaveChanges();

            EventsVolunteer(personId);

            return Page();
        }

        public IActionResult OnPostDeleteVoluntaryForEvent(int eventID)
        {
            var personId = PersonId();
            if (eventID == 0)
            {
                return Page();
            }

            var eventVolunteer = new EventVolunteer
            {
                EventId = eventID,
                PersonId = personId
            };

            _dbContext.Remove(eventVolunteer);
            _dbContext.SaveChanges();

            EventsVolunteer(personId);

            return Page();
        }

        private void EventsVolunteer(int personId)
        {
            //Events = _dbContext.Events.Where(e => e.EventStartDate > DateTime.Now).ToList();
            EventVolunteers = _dbContext.EventVolunteers.Where(v => v.PersonId == personId).ToList();

            AllEvents = _dbContext.Events
                .Where(e => e.EventStartDate > DateTime.Now)
                .Select(s => new AllEvents { EventId = s.Id, Name = s.Name,  Description = s.Description, EventStartDate = s.EventStartDate, EventEndDate = s.EventEndDate, StartTime = s.StartTime, EndTime = s.EndTime})
                .ToList();

            foreach(var ev in EventVolunteers)
            {
                foreach(var ee in AllEvents)
                {
                    if (ev.PersonId == personId && ev.EventId == ee.EventId)
                        ee.PersonId = personId;
                }
            }

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

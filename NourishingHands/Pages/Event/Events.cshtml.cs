using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;
using NourishingHands.Utilities;

namespace NourishingHands.Pages.Event
{
    public class EventsModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [TempData]
        public string Message { get; set; }
        
        public EventsModel(NourishingHandsContext dbContext, UserManager<IdentityUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _hostingEnvironment = hostEnvironment;
        }

        public List<Events> AllEvents  { get; set; }
        public void OnGet()
        {
            AllEvents = _dbContext.Events.Where(e => e.EventStartDate >= DateTime.Now).OrderBy(d => d.EventStartDate).ToList();
        }

        public IActionResult OnGetFindAllEvents()
        {
            var events = _dbContext.Events.Select(e => new
            {
                id = e.Id,
                title = e.Name,
                description = e.Description + "<br/><br/>" + e.StartTime +" - " + e.EndTime + "<br/>Location: Virtual",
                start = e.EventStartDate.HasValue ? e.EventStartDate.Value.ToString("MM/dd/yyyy") : "",
                end = e.EventEndDate.HasValue ? e.EventEndDate.Value.ToString("MM/dd/yyyy") : ""
            }).ToList();
            return new JsonResult(events);
        }
        public IActionResult OnPostAddVoluntaryForEvent(int eventID)
        {
            var person = PersonData();
            if (User.Identity.IsAuthenticated && person != null)
            {
                var personId = person.Id;
                if (eventID == 0)
                {
                    return Page();
                }
                var eventVolun = _dbContext.EventVolunteers.FirstOrDefault(v => v.PersonId == personId && v.EventId == eventID);

                if (eventVolun == null)
                {
                    var eventVolunteer = new EventVolunteer
                    {
                        EventId = eventID,
                        PersonId = personId
                    };

                    _dbContext.Add(eventVolunteer);
                    _dbContext.SaveChanges();
                }

                AllEvents = _dbContext.Events.Where(e => e.EventStartDate >= DateTime.Now).OrderBy(d => d.EventStartDate).ToList();

                var myEvent = AllEvents.FirstOrDefault(e => e.Id == eventID);
                var date = myEvent.EventStartDate.HasValue ? myEvent.EventStartDate.Value.ToString("MM/dd/yyyy"): "";

                Message = ($"You've signed up for {myEvent.Name}, on {date}. Please check your email for confirmation!");

                var logoPath = Path.Combine(_hostingEnvironment.WebRootPath, $"assets/images/NH-Logo.png");
               
                SendEmailFromGmail sfgmail = new SendEmailFromGmail();
                sfgmail.SendEmail(person.Email, person.FirstName+" " +person.LastName, "Nourishing Hands Event Volunteering Confirmation",
                        string.Format("Dear "+ person.FirstName + ", <br/> Thanks for volunteering to for our "+ myEvent.Name +" on "+ date +" from " + myEvent.StartTime +" to "+ myEvent.EndTime +". <br/> We will follow up with you soon. <br/><br/> Nourishing Hands, Inc.<br/><br/>"), logoPath);

                return Page();
            }
            else
            {
                Message = "Error: Could not sign you up for this event. Please login or register as a volunteer!";
                AllEvents = _dbContext.Events.Where(e => e.EventStartDate >= DateTime.Now).OrderBy(d => d.EventStartDate).ToList();
                return Page();
            }
        }

        private Person PersonData()
        {
            Person person = new Person();
            var userId = _userManager.GetUserId(User);
            person = _dbContext.Persons.FirstOrDefault(p => p.UserId == userId && p.Role == "Volunteer");

            return person;
        }

    }
}

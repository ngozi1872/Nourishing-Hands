using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;
using NourishingHands.Pages.Mentee;

namespace NourishingHands.Pages.Mentor.MentorSchedule
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly NourishingHandsContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(NourishingHandsContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Areas.Identity.Data.MentorSchedule MentorSchedule { get; set; }
        public Person Person { get; set; }
        public List<Areas.Identity.Data.MentorSchedule> MentorSchedules { get; set; }

        [TempData]
        public string Message { get; set; }
        public string FullName { get; set; }

        public IActionResult OnGet()
        {
            var person = GetPerson();

            if (person == null && person.Id < 0)
                return RedirectToPage("/Mentor/Application");

            FullName = $"{Person.FirstName} {Person.LastName}";

            ViewData["MentorId"] = new SelectList(_context.Persons, "Id", "Id");
            ViewData["MenteeData"] = new SelectList(GetParticipants(), "Id", "Title");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var person = GetPerson();

            if (person != null && person.Id > 0)
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var menteeId = Request.Form["menteeDdl"].ToString();
                //var mentorSchedule = _context.MentorSchedules.Where(s => s.MentorId == Person.Id && s.MenteeId == Convert.ToInt32(menteeId));

                //if(mentorSchedule == null )

                MentorSchedule.MentorId = Person.Id;
                MentorSchedule.MenteeId = Convert.ToInt32(menteeId);

                _context.MentorSchedules.Add(MentorSchedule);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Mentor/Home");
        }

        private Person GetPerson()
        {
            var userId = _userManager.GetUserId(User);
            var email = _userManager.GetUserName(User);
            Person = _context.Persons.FirstOrDefault(p => p.UserId.Trim() == userId.Trim() && p.Role.Trim() == "Mentor");

            return Person;
        }

        private List<Participants> GetParticipants()
        {
            var person = GetPerson();
            var mmt = _context.MentorSchedules.Where(m => m.MentorId == person.Id).ToList();

            List<Participants> participants = new List<Participants>();
            foreach (var a in mmt)
            {
                if (a.MenteeId > 0)
                {
                    var parti = _context.Persons
                        .Where(m => m.Id == a.MenteeId)
                        .Select(p => new Participants
                        {
                            Id = p.Id,
                            Title = $"{p.FirstName} {p.LastName} - School: {p.StudSchoolId}, Grade: {p.Grade}"
                        })
                        .FirstOrDefault();

                    if (!participants.Any(m => m.Id == a.MenteeId))
                        participants.Add(parti);
                }
            }

            return participants
                .OrderBy(s => s.Title).ToList();
        }
    }

    public class Participants
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}

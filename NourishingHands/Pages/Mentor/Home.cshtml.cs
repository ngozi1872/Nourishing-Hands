using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Mentor
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

        public List<EmploymentHistory> EmploymentHistories { get; set; }
        public Person Person { get; set; }
        public Answer Answer { get; set; }
        public List<Answer> Answers { get; set; }
        public List<Areas.Identity.Data.MentorSchedule> MentorSchedules { get; set; }
        public List<AllParticipants> Participants { get; set; }
        public bool HasPersonRecord { get; set; }
        public string ApplicationStatus { get; set; }
        public bool HasEmployment { get; set; }
        public bool HasAnswer { get; set; }
        public Person Participant { get; set; }
        public int ParticipantId { get; set; }
        public DateTime UpComingEvent { get; set; }

        public IActionResult OnGet()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                Person = _dbContext.Persons.FirstOrDefault(p => p.UserId == userId && p.Role.Trim() == "Mentor");

                if (Person == null || Person.Id <= 0)
                    return RedirectToPage("/Mentor/Application");

                EmploymentHistories = _dbContext.EmploymentHistories
                    .Where(e => e.PersonId == Person.Id)
                    .ToList();

                Answers = _dbContext.Answers
                    .Where(a => a.PersonId == Person.Id)
                    .ToList();

                MentorSchedules = _dbContext.MentorSchedules
                    .Where(s => s.MentorId == Person.Id && s.StartDate >= DateTime.Now)
                    .OrderBy(s => s.StartDate)
                    .ToList();


                Participants = new List<AllParticipants>();


                foreach (var p in MentorSchedules)
                {
                    if (p.MenteeId > 0)
                    {
                        var person = _dbContext.Persons
                           .Where(s => s.Role == "Mentee" && s.Id == p.MenteeId)
                           .Select(s => new AllParticipants
                           {
                               Id = s.Id,
                               Name = $"{s.FirstName} {s.LastName} ",
                               Gender = s.Gender,
                               Ethnicity = s.Ethnicity,
                               Grade = s.Grade,
                               School = s.School
                           })
                           .FirstOrDefault();


                        if(!Participants.Any(m => m.Id == p.MenteeId))
                            Participants.Add(person);
                    }
                }

                if (EmploymentHistories.Count > 0)
                    HasEmployment = true;
                else
                    return RedirectToPage("/Mentor/AppStatus");

                if (Answers.Count > 0)
                    HasAnswer = true;
                else
                    return RedirectToPage("/Mentor/AppStatus");

                if (Person.Approved == null)
                    return RedirectToPage("/Mentor/AppStatus");
                else if (Person.Approved != null && Person.Approved == true)
                    ApplicationStatus = "Approved";
                else
                    return RedirectToPage("/Mentor/AppStatus");

                HasPersonRecord = true;

                var eventn = _dbContext.MentorSchedules
                    .Where(s => s.StartDate >= DateTime.Now && s.MentorId == Person.Id)
                    .OrderBy(t => t.StartDate).FirstOrDefault();
                UpComingEvent = eventn.StartDate.HasValue ? (DateTime)eventn.StartDate.Value.Date : DateTime.Now.Date;
            }
            catch (Exception ex)
            {
                var ts = ex.Message;
            }

            return Page();
        }

        public IActionResult OnGetAjaxEmploymentDetail(int employementID)
        {
            if (employementID == 0)
            {
                return Page();
            }

            Participant = _dbContext.Persons.FirstOrDefault(p => p.Id == employementID);

            if (Participant == null)
                return Page();

            return new JsonResult(Participant);
        }

        public IActionResult OnGetFindAllEvents()
        {
            var events = _dbContext.MentorSchedules
                .Where(s => s.MentorId == Person.Id)
                .Select(e => new
                    {
                        id = e.Id,
                        title = e.Title,
                        description = e.Notes,
                        start = e.StartDate,
                        end = e.EndDate
                    })
                .ToList();

            return new JsonResult(events);
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

    public class AllParticipants
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Ethnicity { get; set; }
        public string Grade { get; set; }
        public string School { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;
using NourishingHands.Utilities;

namespace NourishingHands.Pages.Mentor
{
    [Authorize]
    public class QuestionsModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public QuestionsModel(NourishingHandsContext dbContext, UserManager<IdentityUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _hostingEnvironment = hostEnvironment;
        }

        [BindProperty]
        public List<Answer> Answers { get; set; }
        public List<Question> Questions { get; set; }
        public IActionResult OnGet()
        {
            var person = GetPerson();
            if (person != null && person.Id > 0)
            {
                var employmentHistories = _dbContext.EmploymentHistories.FirstOrDefault(e => e.PersonId == person.Id);
                if(employmentHistories == null)
                    return RedirectToPage("/Mentor/EmploymentHistory");

                Questions = _dbContext.Questions.Where(q => q.QuestionFor.Trim() == "Mentor").ToList();
                var answers = _dbContext.Answers.Where(a => a.PersonId == person.Id).ToList();
                if (answers.Count > 0)
                    Answers = answers;

                return Page();
            }
            else
                return RedirectToPage("/Mentor/Application");
        }
        public IActionResult OnPostAddAnswer()
        {
            if (ModelState.IsValid)
            {
                var person = GetPerson();
                Questions = _dbContext.Questions.Where(q => q.QuestionFor.Trim() == "Mentor").ToList();
                Answers = _dbContext.Answers.Where(a => a.PersonId == person.Id).ToList();
                if (Answers.Count > 0)
                {
                    foreach(var answer in Answers)
                    {
                        answer.QuestionId = answer.QuestionId;
                        answer.PersonId = person.Id;
                        answer.Description = Request.Form[answer.QuestionId.ToString()].ToString();
                        answer.UpdatedOn = DateTime.Now;
                        _dbContext.Answers.Attach(answer).State = EntityState.Modified;
                        _dbContext.SaveChanges();
                    }
                }
                else
                {
                    foreach (var question in Questions)
                    {
                        var answer = new Answer
                        {
                            QuestionId = question.Id,
                            PersonId = person.Id,
                            Description = Request.Form[question.Id.ToString()].ToString(),
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now
                        };
                        _dbContext.Answers.Add(answer);
                        _dbContext.SaveChanges();
                    }

                    var logoPath = Path.Combine(_hostingEnvironment.WebRootPath, $"assets/images/NH-Logo.png");

                    SendEmailFromGmail sfgmail = new SendEmailFromGmail();
                    sfgmail.SendEmail(person.Email, person.FirstName + " " + person.LastName, "Nourishing Hands Mentor Application",
                            string.Format("Dear " + person.FirstName + ", <br/> Thanks for applying to the Nourishing Hands Mentoring program. Someone from our team will get in touch with you shortly. <br/><br/> Nourishing Hands, Inc.<br/><br/>"), logoPath);
                }
               
                return RedirectToPage("/Mentor/Home");
            }

            return Page();
        }

        private Person GetPerson()
        {
            var userId = _userManager.GetUserId(User);
            Person person = _dbContext.Persons.FirstOrDefault(p => p.UserId == userId);

            return person;
        }

    }

}

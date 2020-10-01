using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Parent
{
    public class ParentQuestionnaireModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public ParentQuestionnaireModel(NourishingHandsContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [BindProperty]
        public List<Answer> Answers { get; set; }
        public List<Question> Questions { get; set; }
        public IActionResult OnGet()
        {
            if (!string.IsNullOrWhiteSpace(HttpContext.Request.Query["Id"]))
            {
                var personId = PersonId();
                Questions = _dbContext.Questions.Where(q => q.QuestionFor.Trim() == "Parent").ToList();
                var answers = _dbContext.Answers.Where(a => a.PersonId == personId).ToList();
                if (answers.Count > 0)
                    Answers = answers;
                return Page();
            }
            else
                return RedirectToPage("/Index");

           
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var personId = PersonId();
                Questions = _dbContext.Questions.Where(q => q.QuestionFor.Trim() == "Parent").ToList();
                Answers = _dbContext.Answers.Where(a => a.PersonId == personId).ToList();
                if (Answers.Count > 0)
                {
                    foreach (var answer in Answers)
                    {
                        answer.QuestionId = answer.QuestionId;
                        answer.PersonId = personId;
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
                            PersonId = personId,
                            Description = Request.Form[question.Id.ToString()].ToString(),
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now
                        };
                        _dbContext.Answers.Add(answer);
                        _dbContext.SaveChanges();
                    }
                }


                return RedirectToPage("/Parent/Home");
            }

            return Page();
        }

        private int PersonId()
        {
            var pId = Convert.ToInt32(HttpContext.Request.Query["Id"]);
            Person person = _dbContext.Persons.FirstOrDefault(p => p.Id == pId);

            if (person != null)
                return person.Id;
            else
                return 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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
        public bool HasPersonRecord { get; set; }
        public bool HasEmployment { get; set; }
        public bool HasAnswer { get; set; }


        public IActionResult OnGet()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                Person = _dbContext.Persons.FirstOrDefault(p => p.UserId == userId && p.Role.Trim() == "Mentor");

                if (Person == null || Person.Id <= 0)
                    return RedirectToPage("/Mentor/Application");

                EmploymentHistories = _dbContext.EmploymentHistories.Where(e => e.PersonId == Person.Id).ToList();
                Answers = _dbContext.Answers.Where(a => a.PersonId == Person.Id).ToList();

                if (EmploymentHistories.Count > 0)
                    HasEmployment = true;
                else
                    HasEmployment = false;

                if (Answers.Count > 0)
                    HasAnswer = true;
                else
                    HasAnswer = false;

                HasPersonRecord = true;
            }
            catch (Exception ex)
            {
                var ts = ex.Message;
            }

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

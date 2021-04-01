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

namespace NourishingHands.Pages.Mentee
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

        public EmploymentHistory EmploymentHistory { get; set; }
        public List<EmploymentHistory> EmploymentHistories { get; set; }
        public Person Person { get; set; }
        public Answer Answer { get; set; }
        public List<Answer> Answers { get; set; }
        public bool HasPersonRecord { get; set; }
        public bool HasEmployment { get; set; }
        public bool HasAnswer { get; set; }
        public bool HasParentSigned { get; set; }
        public IActionResult OnGet()
        {
            var userId = _userManager.GetUserId(User);
            Person = _dbContext.Persons.FirstOrDefault(p => p.UserId == userId && p.Role.Trim() == "Mentee");

            

            if (Person == null || Person.Id <= 0)
                return RedirectToPage("/Mentee/Application");

            var parent = _dbContext.Persons.FirstOrDefault(p => p.MenteeId == Person.Id);
            Answers = _dbContext.Answers.Where(a => a.PersonId == Person.Id).ToList();

            if(parent != null && parent.IsSigned && Answers.Count > 0 && Person.Id > 0)
                return RedirectToPage("/Mentee/PaticipantDashboard");

            if (parent != null && parent.IsSigned)
                HasParentSigned = true;
            else
                HasParentSigned = false;

            if (Answers.Count > 0)
                HasAnswer = true;
            else
                HasAnswer = false;

            HasPersonRecord = true;

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

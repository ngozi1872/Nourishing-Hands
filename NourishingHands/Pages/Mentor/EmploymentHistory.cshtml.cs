using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Mentor
{
    [Authorize]
    public class EmploymentHistoryModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        public EmploymentHistoryModel(NourishingHandsContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [BindProperty]
        public EmploymentHistory EmploymentHistory { get; set; }
        [TempData]
        public string Message { get; set; }
        public List<EmploymentHistory> EmploymentHistories { get; set; }
        public IActionResult OnGet()
        {
            if (PersonId() == 0)
            {
                return RedirectToPage("/Mentor/Application");
            }

            GetEmploymentHistoryById(PersonId());
            return Page();
        }


        public IActionResult OnPostAddEmployment()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            AddEmployment();
            EmploymentHistory = new EmploymentHistory();

            return Page();
        }

        public IActionResult OnGetAjaxEmploymentDetail(int employementID)
        {
            if(employementID == 0)
            {
                return Page();
            }

            EmploymentHistory = _dbContext.EmploymentHistories.FirstOrDefault(e => e.Id == employementID);

            if (EmploymentHistory == null)
                return Page();

            return new JsonResult(EmploymentHistory);
        }

        public IActionResult OnPostUpdateEmployment()
        {
            var personId = PersonId();
            if (!ModelState.IsValid && personId <= 0)
            {
                return Page();
            }

            EmploymentHistory.PersonId = personId;
            _dbContext.EmploymentHistories.Attach(EmploymentHistory).State = EntityState.Modified;
            _dbContext.SaveChanges();

            GetEmploymentHistoryById(EmploymentHistory.PersonId);

            return Page();
        }

        public IActionResult OnPostContinueToQuestionairs()
        {
            return RedirectToPage("/Mentor/Questions");
        }

        public IActionResult OnPostDeleteEmployment(int employementID)
        {
            if (employementID == 0)
            {
                return Page();
            }

            EmploymentHistory = _dbContext.EmploymentHistories.FirstOrDefault(e => e.Id == employementID);
            _dbContext.EmploymentHistories.Remove(EmploymentHistory);
            _dbContext.SaveChanges();

            if (EmploymentHistory == null)
            {
                return RedirectToPage("Error");
            }
            //Message = "Employment History " + EmploymentHistory.Employer + " deleted successfully";
            GetEmploymentHistoryById(PersonId());

            return Page();
        }

        private void AddEmployment()
        {
            EmploymentHistory.PersonId = PersonId();
            _dbContext.EmploymentHistories.Add(EmploymentHistory);

            _dbContext.SaveChanges();

            GetEmploymentHistoryById(EmploymentHistory.PersonId);
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
        private void GetEmploymentHistoryById(int personId)
        {
            EmploymentHistories = _dbContext.EmploymentHistories.Where(p => p.PersonId == personId).ToList();
        }
    }
}

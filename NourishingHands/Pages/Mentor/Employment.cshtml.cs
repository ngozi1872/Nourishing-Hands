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

namespace NourishingHands.Pages.Mentor
{
    [Authorize]
    public class EmploymentModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public EmploymentModel(NourishingHandsContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [BindProperty]
        public EmploymentHistory EmploymentHistory { get; set; }
        public List<EmploymentHistory> EmploymentHistories { get; set; }
        public IActionResult OnGet()
        {
            if (PersonId() == 0)
            {
                return RedirectToPage("/Mentor/Application");
            }

            EmploymentHistories = _dbContext.EmploymentHistories.Where(p => p.PersonId == PersonId()).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            EmploymentHistory.PersonId = PersonId();
            _dbContext.EmploymentHistories.Add(EmploymentHistory);

            await _dbContext.SaveChangesAsync();

            EmploymentHistories = _dbContext.EmploymentHistories.Where(e => e.PersonId == EmploymentHistory.PersonId).ToList();
            
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

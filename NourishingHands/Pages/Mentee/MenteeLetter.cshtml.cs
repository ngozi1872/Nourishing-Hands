using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Mentee
{
    public class MenteeLetterModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        public MenteeLetterModel(NourishingHandsContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public Person Person { get; set; }

        public IActionResult OnGet()
        {
            var userId = _userManager.GetUserId(User);
            Person = _dbContext.Persons.FirstOrDefault(p => p.UserId == userId);

            if (Person == null || Person.Id <= 0)
                return RedirectToPage("/Mentee/Application");
            else
                return Page();

        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/Mentee/MenteeQuestionnaire");
        }
    }
}

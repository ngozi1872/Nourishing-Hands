using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Event
{
    [Authorize]
    public class VolunteerModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public VolunteerModel(NourishingHandsContext dbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public Person Person { get; set; }
        public void OnGet()
        {
            var userId = _userManager.GetUserId(User);
            var email = _userManager.GetUserName(User);
            Person = _dbContext.Persons.FirstOrDefault(p => p.UserId.Trim() == userId.Trim());
        }

        public async Task<IActionResult> OnPostAddPersonRecord()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Person.UserId = _userManager.GetUserId(User);
            Person.Email = _userManager.GetUserName(User);

            if (Person.Id > 0)
            {
                Person.UpdatedOn = DateTime.Now;
                _dbContext.Persons.Attach(Person).State = EntityState.Modified;
                _dbContext.SaveChanges();
                Person = _dbContext.Persons.FirstOrDefault(p => p.UserId.Trim() == Person.UserId.Trim());
                return Page();

            }
            else
            {
                Person.CreatedOn = DateTime.Now;
                Person.Role = "Mentor";
                _dbContext.Persons.Add(Person);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToPage("/Mentor/EmploymentHistory");
        }

    }
}

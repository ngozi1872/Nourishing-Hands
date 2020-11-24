using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class ApplicationModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationModel(NourishingHandsContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [BindProperty]
        public Person Person { get; set; }

        [TempData]
        public string Message { get; set; }


        public async Task<IActionResult> OnGet()
        {
            Message = string.Empty;
            var userId = _userManager.GetUserId(User);
            var email = _userManager.GetUserName(User);
            var user = await _userManager.GetUserAsync(User);
            Person = _dbContext.Persons.FirstOrDefault(p => p.UserId.Trim() == userId.Trim());

            if (Person != null)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                if (userRole.FirstOrDefault() != AllRoles.MentorEndUser)
                    return RedirectToPage("/");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddPersonRecord()
        {
            var person = PersonRole();

            if (person == null || person.Role.Trim() == "Mentor")
            {
                if (!ModelState.IsValid)
                {
                    Message = $"Error: Processing request!";
                    return Page();
                }

                if(Person.BirthDay > DateTime.Now.AddYears(-18))
                {
                    Message = $"Error: Action cancelled. You must be 18 or older to be a mentor. Please re-enter a valid birthdate.";
                    return Page();
                }

                Person.UserId = _userManager.GetUserId(User);
                Person.Email = _userManager.GetUserName(User);
                Person.Role = "Mentor";
                var user = await _userManager.GetUserAsync(User);

                if (Person.Id > 0)
                {
                    Person.UpdatedOn = DateTime.Now;
                    //_dbContext.Persons.Attach(Person).State = EntityState.Modified;
                    _dbContext.Attach(Person).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();

                    Person = _dbContext.Persons.FirstOrDefault(p => p.UserId.Trim() == Person.UserId.Trim() && p.Role.Trim() == "Mentor");
                    return RedirectToPage("/Mentor/Home");

                }
                else
                {
                    Person.CreatedOn = DateTime.Now;
                    _dbContext.Persons.Add(Person);
                    await _dbContext.SaveChangesAsync();
                    await _userManager.AddToRoleAsync(user, AllRoles.MentorEndUser);
                }

                Message = string.Empty;
            }
            else
            {
                Message = $"Error: Action cancelled. Email: {person.Email} is in use as a {person.Role}.";
                return Page();
            }

            return RedirectToPage("/Mentor/EmploymentHistory");
        }

        private Person PersonRole()
        {
            var userId = _userManager.GetUserId(User);
            var person = _dbContext.Persons.FirstOrDefault(p => p.UserId.Trim() == userId.Trim());

            return person;
        }
    }
}

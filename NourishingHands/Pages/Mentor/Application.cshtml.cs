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

namespace NourishingHands.Pages.Mentor
{
    [Authorize]
    public class ApplicationModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public ApplicationModel(NourishingHandsContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }


        [BindProperty]
        public Person Person { get; set; }

        [TempData]
        public string Message { get; set; }


        public void OnGet()
        {
            Message = string.Empty;
            var userId = _userManager.GetUserId(User);
            var email = _userManager.GetUserName(User);
            Person = _dbContext.Persons.FirstOrDefault(p => p.UserId.Trim() == userId.Trim() && p.Role.Trim() == "Mentor");
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

                if (Person.Id > 0)
                {
                    Person.UpdatedOn = DateTime.Now;
                    _dbContext.Persons.Attach(Person).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    Person = _dbContext.Persons.FirstOrDefault(p => p.UserId.Trim() == Person.UserId.Trim() && p.Role.Trim() == "Mentor");
                    return RedirectToPage("/Mentor/Home");

                }
                else
                {
                    Person.CreatedOn = DateTime.Now;
                    _dbContext.Persons.Add(Person);
                    await _dbContext.SaveChangesAsync();
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

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

namespace NourishingHands.Pages.Parent
{
    [Authorize]
    public class ApplicationModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ApplicationModel(NourishingHandsContext dbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public Person Person { get; set; }
        public IActionResult OnGet()
        {
            //var userId = _userManager.GetUserId(User);
            //var email = _userManager.GetUserName(User);
            //Person = _dbContext.Persons.FirstOrDefault(p => p.UserId.Trim() == userId.Trim());

            if (!string.IsNullOrWhiteSpace(HttpContext.Request.Query["Id"]))
                return Page();
            else
                return RedirectToPage("/Index");

            
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Person.UserId = _userManager.GetUserId(User);
            //Person.Email = _userManager.GetUserName(User);


            Person.CreatedOn = DateTime.Now;
            Person.Role = "Parent";
            Person.MenteeId = Convert.ToInt32(HttpContext.Request.Query["Id"]);
            Person.IsSigned = true;
            _dbContext.Persons.Add(Person);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("/Parent/ParentQuestionnaire?Id="+Person.Id);
        }

    }
}

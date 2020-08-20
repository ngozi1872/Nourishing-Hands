using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Mentor
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
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Person.CreatedOn = DateTime.Now;
            Person.Role = "Mentor";
            Person.UserId = _userManager.GetUserId(User);
            Person.Email = _userManager.GetUserName(User);
            _dbContext.Persons.Add(Person);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("./");
        }

    }
}

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
        public string MenteeFullName { get; set; }
        public string MenteeName { get; set; }
        public string ParentName { get; set; }
        public DateTime CurrentDate { get; set; }

        [TempData]
        public string Message { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mentee = _dbContext.Persons.FirstOrDefault(p => p.Id == id);
            if(mentee != null)
                MenteeFullName = $"{mentee.FirstName} {mentee.LastName}";

            if (!string.IsNullOrWhiteSpace(HttpContext.Request.Query["Id"]))
                return Page();
            else
                return RedirectToPage("/Index");

            
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var mentee = _dbContext.Persons.FirstOrDefault(p => p.Id == Convert.ToInt32(HttpContext.Request.Query["Id"]));
            if (mentee != null)
                MenteeFullName = $"{mentee.FirstName} {mentee.LastName}";

            //if(MenteeName != MenteeFullName)
            //{
            //    Message = "Error: Participant name on file is different from what you've entered in the parent consent form";
            //    return Page();
            //}

            Person.CreatedOn = DateTime.Now;
            Person.Role = "Parent";
            Person.MenteeId = Convert.ToInt32(HttpContext.Request.Query["Id"]);
            Person.IsSigned = true;
            _dbContext.Persons.Add(Person);
            await _dbContext.SaveChangesAsync();

            var pId = Person.Id;

            returnUrl = returnUrl ?? Url.Content("/Parent/ParentQuestionnaire?Id=" + pId);

            return LocalRedirect(returnUrl);
        }

    }
}

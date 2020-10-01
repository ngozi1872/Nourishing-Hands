using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;
using NourishingHands.Utilities;

namespace NourishingHands.Pages.Mentee
{
    [Authorize]
    public class ApplicationModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ApplicationModel(NourishingHandsContext dbContext, UserManager<IdentityUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _hostingEnvironment = hostEnvironment;
        }

        [BindProperty]
        public Person Person { get; set; }
        public IActionResult OnGet()
        {
            var userId = _userManager.GetUserId(User);
            var email = _userManager.GetUserName(User);
            Person = _dbContext.Persons.FirstOrDefault(p => p.UserId.Trim() == userId.Trim() );

            if (Person != null && Person.Role != "Mentee")
                return RedirectToPage("/Index");
            else
                return Page();
        }

        public async Task<IActionResult> OnPostAddPersonRecord()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Person.UserId = _userManager.GetUserId(User);
            Person.Email = _userManager.GetUserName(User);
            Person.Role = "Mentee";

            if (Person.Id > 0)
            {
                Person.UpdatedOn = DateTime.Now;
                Person.CreatedOn = Person.CreatedOn;
                _dbContext.Persons.Attach(Person).State = EntityState.Modified;
                _dbContext.SaveChanges();
                Person = _dbContext.Persons.FirstOrDefault(p => p.UserId.Trim() == Person.UserId.Trim());
                //return Page();

            }
            else
            {
                Person.CreatedOn = DateTime.Now;
                _dbContext.Persons.Add(Person);
                await _dbContext.SaveChangesAsync();

                ContactParent(Person.Id);
            }

            return RedirectToPage("/Mentee/Home");
        }
        private void ContactParent(int recordId)
        {
            var logoPath = Path.Combine(_hostingEnvironment.WebRootPath, $"assets/images/NH-Logo.png");
            var pagePath = Path.Combine(_hostingEnvironment.WebRootPath, $"/Mentee/EmploymentHistory");

            var callbackUrl = Url.Page(
                        "/Parent/ParentLetter",
                        pageHandler: null,
                        values: new { Id = recordId },
                        protocol: Request.Scheme);

            SendEmailFromGmail sfgmail = new SendEmailFromGmail();
            sfgmail.SendEmail(Person.ParentEmail, "Registrant", "Confirm your email",
                    string.Format("Dear Parent, <br/> Your child has applied to participate in Nourishing Hands Inc., teen mentoring Program this year. Please <a href=" + HtmlEncoder.Default.Encode(callbackUrl) + ">clicking here</a> to consent. Thanks, <br/> Nourishing Hands, Inc.<br/><br/>"), logoPath);
        }

    }
}

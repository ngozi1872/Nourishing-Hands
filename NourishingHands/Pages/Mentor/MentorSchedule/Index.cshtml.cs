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

namespace NourishingHands.Pages.Mentor.MentorSchedule
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly NourishingHandsContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public IndexModel(NourishingHandsContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public string FullName { get; set; }
        public IList<Areas.Identity.Data.MentorSchedule> MentorSchedule { get;set; }

        public IList<Person> Persons { get; set; }
        public Person Person { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var person = GetPerson();

            if (person == null && person.Id < 0)
                return RedirectToPage("/Mentor/Application");

            Persons = await _context.Persons.Where(p => p.Role == "Mentee").ToListAsync();
            FullName = $"{person.FirstName} {person.LastName}";

            MentorSchedule = await _context.MentorSchedules
                .Include(m => m.Person)
                .Where(m => m.MentorId == person.Id)
                .ToListAsync();

            return Page();
        }

        private Person GetPerson()
        {
            var userId = _userManager.GetUserId(User);
            var email = _userManager.GetUserName(User);
            Person = _context.Persons.FirstOrDefault(p => p.UserId.Trim() == userId.Trim() && p.Role.Trim() == "Mentor");

            return Person;
        }
    }
}

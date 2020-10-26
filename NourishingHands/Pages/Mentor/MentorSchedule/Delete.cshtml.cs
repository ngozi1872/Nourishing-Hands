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
    public class DeleteModel : PageModel
    {
        private readonly NourishingHands.Areas.Identity.NourishingHands.Data.NourishingHandsContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DeleteModel(NourishingHands.Areas.Identity.NourishingHands.Data.NourishingHandsContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Areas.Identity.Data.MentorSchedule MentorSchedule { get; set; }
        public Person Person { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MentorSchedule = await _context.MentorSchedules
                .Include(m => m.Person).FirstOrDefaultAsync(m => m.Id == id);

            var userId = _userManager.GetUserId(User);
            Person = _context.Persons.FirstOrDefault(p => p.UserId == userId && p.Role.Trim() == "Mentor");

            if (Person == null || Person.Id <= 0)
                return RedirectToPage("/Mentor/Application");


            if (MentorSchedule == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MentorSchedule = await _context.MentorSchedules.FindAsync(id);

            if (MentorSchedule != null)
            {
                _context.MentorSchedules.Remove(MentorSchedule);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Mentor/Home");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Mentor.MentorSchedule
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly NourishingHandsContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EditModel(NourishingHandsContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Areas.Identity.Data.MentorSchedule MentorSchedule { get; set; }

        public Person Person { get; set; }

        [TempData]
        public string Message { get; set; }
        public string FullName { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MentorSchedule = await _context.MentorSchedules
                .Include(m => m.Person).FirstOrDefaultAsync(m => m.Id == id);

            if (MentorSchedule == null)
            {
                return NotFound();
            }

            var person = GetPerson();

            if (person == null && person.Id < 0)
                return RedirectToPage("/Mentor/Application");

            FullName = $"{Person.FirstName} {Person.LastName}";

            ViewData["MentorId"] = new SelectList(_context.Persons, "Id", "Id");
            ViewData["MenteeData"] = new SelectList(GetParticipants(), "Id", "Title");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(MentorSchedule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MentorScheduleExists(MentorSchedule.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MentorScheduleExists(int id)
        {
            return _context.MentorSchedules.Any(e => e.Id == id);
        }
        private Person GetPerson()
        {
            var userId = _userManager.GetUserId(User);
            var email = _userManager.GetUserName(User);
            Person = _context.Persons.FirstOrDefault(p => p.UserId.Trim() == userId.Trim() && p.Role.Trim() == "Mentor");

            return Person;
        }

        private IList<Mentee> GetParticipants()
        {
            var person = GetPerson();
            var mmt = _context.MentorSchedules.Where(m => m.MentorId == person.Id).ToList();

            List<Mentee> participants = new List<Mentee>();
            foreach (var a in mmt)
            {
                if (a.MenteeId > 0)
                {
                    var parti = _context.Persons
                        .Where(m => m.Id == a.MenteeId)
                        .Select(p => new Mentee
                        {
                            Id = p.Id,
                            Title = $"{p.FirstName} {p.LastName} - School: {p.StudSchoolId}, Grade: {p.Grade}"
                        })
                        .FirstOrDefault();

                    if (!participants.Any(m => m.Id == a.MenteeId))
                        participants.Add(parti);
                }
            }

            return participants
                .OrderBy(s => s.Title).ToList();
        }
    }
    public class Mentee
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}

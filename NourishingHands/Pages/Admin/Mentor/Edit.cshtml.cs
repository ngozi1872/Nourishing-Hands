using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Admin.Mentor
{
    public class EditModel : PageModel
    {
        private readonly NourishingHands.Areas.Identity.NourishingHands.Data.NourishingHandsContext _context;

        public EditModel(NourishingHands.Areas.Identity.NourishingHands.Data.NourishingHandsContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Person Person { get; set; }
        public IList<Answer> Answer { get; set; }
        public IList<EmploymentHistory> EmploymentHistory { get; set; }
        [BindProperty]
        public bool IsApproved { get; set; }
        public MentorSchedule MentorSchedule { get; set; }
        public IList<MentorSchedule> MentorSchedules { get; set; }
        public IList<Person> Persons { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            Person = await _context.Persons
                .Include(p => p.IdentityUser).FirstOrDefaultAsync(m => m.Id == id);

            if (Person == null)
            {
                return NotFound();
            }

            if (Person.Approved == null)
                IsApproved = false;
            else
                IsApproved = true;

            Answer = await _context.Answers
                .Where(a => a.PersonId == Person.Id)
               .Include(a => a.Persons)
               .Include(a => a.Question).ToListAsync();

            EmploymentHistory = await _context.EmploymentHistories
                .Where(e => e.PersonId == Person.Id)
               .Include(e => e.Mentor).ToListAsync();

            Persons = await _context.Persons
                .Where(p => p.Role == "Mentee")
                .ToListAsync();

            MentorSchedules = await _context.MentorSchedules.Where(s => s.MentorId == Person.Id)
                .Include(s => s.Person)
                .ToListAsync();

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");

            ViewData["MenteeData"] = new SelectList(GetParticipants(), "Id", "Title");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var menteeId = Person.MenteeId;

            Person = await _context.Persons
                .Include(p => p.IdentityUser).FirstOrDefaultAsync(m => m.Id == Person.Id);

            Person.Approved = IsApproved;
            Person.MenteeId = menteeId;
            Person.UpdatedOn = DateTime.Now;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Person).State = EntityState.Modified;

            if (Person != null && Person.MenteeId > 0)
            {
                MentorSchedule = new MentorSchedule();

                MentorSchedule.MentorId = Person.Id;
                MentorSchedule.MenteeId = Person.MenteeId;
                var mExist = MenteeExists(Person.Id, Person.MenteeId);

                if(!mExist)
                    _context.MentorSchedules.Add(MentorSchedule);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(Person.Id))
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

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }

        private bool MenteeExists(int mtId, int mmId)
        {
            return _context.MentorSchedules.Any(e => e.MentorId == mtId && e.MenteeId == mmId);
        }

        private IList<Participants> GetParticipants()
        {
            IList<Participants> participants = _context.Persons
                .Where(m => m.Role == "Mentee")
                .Select(p => new Participants
                    {
                        Id = p.Id,
                        Title = $"{p.FirstName} {p.LastName} - School: {p.School}, Grade: {p.Grade}"
                    })
                .ToList();

            return participants.OrderBy(s => s.Title).ToList();
        }
    }

    public class Participants
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}

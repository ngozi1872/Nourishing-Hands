using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NourishingHands.Areas.Identity.Data;

namespace NourishingHands.Areas.Identity.NourishingHands.Data
{
    public class NourishingHandsContext : IdentityDbContext<IdentityUser>
    {
        public NourishingHandsContext(DbContextOptions<NourishingHandsContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Answer>().HasKey(a => new { a.QuestionId, a.PersonId });
            builder.Entity<EventVolunteer>().HasKey(e => new { e.EventId, e.PersonId });

        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<EducationHistory> EducationHistories { get; set; }
        public DbSet<EmploymentHistory> EmploymentHistories { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<EventVolunteer> EventVolunteers { get; set; }
        public DbSet<Donation> Donations { get; set; }

    }
}

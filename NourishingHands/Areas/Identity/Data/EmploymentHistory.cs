using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NourishingHands.Areas.Identity.Data
{
    public class EmploymentHistory
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Employer { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public DateTime StartDate  { get; set; }
        public DateTime EndDate { get; set; }

        [ForeignKey(nameof(PersonId))]
        public virtual Person Mentor { get; set; }

    }
}

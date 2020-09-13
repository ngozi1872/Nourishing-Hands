using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NourishingHands.Areas.Identity.Data
{
    public class EmploymentHistory
    {
        public int Id { get; set; }
        public int PersonId { get; set; }

        [Display(Name = "Employer")]
        public string Employer { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }

        [Display(Name = "Start Date")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Date)]
        public DateTime StartDate  { get; set; }
        public bool? ContactEmployer { get; set; }

        //[RegularExpression(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")]
        [Display(Name = "Employer Phone")]
        public string EmployerPhone { get; set; }

        [Display(Name = "Employer Email")]
        public string EmployerEmail { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [ForeignKey(nameof(PersonId))]
        public virtual Person Mentor { get; set; }

    }
}

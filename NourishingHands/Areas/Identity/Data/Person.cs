using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NourishingHands.Areas.Identity.Data
{
    public class Person
    {
        public Person()
        {
            EmploymentHistories = new HashSet<EmploymentHistory>();
            Answers = new HashSet<Answer>();
        }   

        public int Id { get; set; }
        public string UserId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Role { get; set; }
        [Display(Name = "Birth Date")]
        public DateTime? BirthDay { get; set; }
        [Display(Name = "Social Security #")]
        public string SSNumber { get; set; }
        public string Gender { get; set; }
        public string Ethnicity { get; set; }
        [Display(Name = "Grade")]
        public string Grade { get; set; }
        public int MenteeId { get; set; } 
        public string School { get; set; }

        [Display(Name = "School ID")]
        public string StudSchoolId { get; set; }
        [Display(Name = "Parent's Email")]
        public string ParentEmail { get; set; }
        [Display(Name = "Relationship to Mentee")]
        public string RelationshipToMentee { get; set; }

        [Display(Name = "Cordinator Name")]
        public string CordinatorName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        public string Country { get; set; }
        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }
        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }
        public string Email { get; set; }
        [Display (Name = "Language")]
        public string Language { get; set; }
        [Display(Name = "Highest Education")]
        public string HighestLevelOfEducation { get; set; }
        public string AvatarPath { get; set; }
        public string BarcodePath { get; set; }
        public bool IsSigned { get; set; }
        public DateTime CreatedOn { get; set; }
        public  DateTime? UpdatedOn { get; set; }

        [Display(Name = "Volunteer Frequency")]
        public string VolunteerFrequency { get; set; }

        [Display(Name = "Volunteer Role")]
        public string VolunteerRole { get; set; }

        public ICollection<EmploymentHistory> EmploymentHistories { get; set; }
        public ICollection<Answer> Answers { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual IdentityUser IdentityUser { get; set; }
    }
}

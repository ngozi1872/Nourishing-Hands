using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NourishingHands.Areas.Identity.Data
{
    public class Donation
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [Display(Name = "Donation Amount")]
        public decimal DonationAmount { get; set; }
        public bool Recurring { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

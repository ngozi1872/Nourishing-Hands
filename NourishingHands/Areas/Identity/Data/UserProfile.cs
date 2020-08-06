using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NourishingHands.Areas.Identity.Data
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string AvatarPath { get; set; }
        public string BarcodePath { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual Person Person { get; set; }
    }
}

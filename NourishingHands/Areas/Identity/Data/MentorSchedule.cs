using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NourishingHands.Areas.Identity.Data
{
    public class MentorSchedule
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }
        public int MentorId { get; set; }
        public int? MenteeId { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Notes { get; set; }


        [ForeignKey(nameof(MentorId))]
        public virtual Person Person { get; set; }

        //[ForeignKey(nameof(MenteeId))]
        //public virtual Person Person1 { get; set; }

    }
}

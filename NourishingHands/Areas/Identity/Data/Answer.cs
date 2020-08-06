using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NourishingHands.Areas.Identity.Data
{
    public class Answer
    {
        public int QuestionId { get; set; }
        public int PersonId { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }


        [ForeignKey(nameof(QuestionId))]
        public virtual Question Question { get; set; }


        [ForeignKey(nameof(PersonId))]
        public virtual Person Persons { get; set; }


    }
}

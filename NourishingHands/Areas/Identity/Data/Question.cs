using System;
using System.Collections.Generic;

namespace NourishingHands.Areas.Identity.Data
{
    public class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public string QuestionFor { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public ICollection<Answer> Answers { get; set; }

    }
}

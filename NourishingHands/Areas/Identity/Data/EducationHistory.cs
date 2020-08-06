using System.ComponentModel.DataAnnotations.Schema;

namespace NourishingHands.Areas.Identity.Data
{
    public class EducationHistory
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string HighSchool { get; set; }
        public string College { get; set; }
        public string OtherEducation { get; set; }

        [ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; }
    }
}

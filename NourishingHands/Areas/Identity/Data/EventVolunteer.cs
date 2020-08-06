using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NourishingHands.Areas.Identity.Data
{
    public class EventVolunteer
    {
        public int EventId { get; set; }
        public int PersonId { get; set; }


        [ForeignKey(nameof(EventId))]
        public virtual Events Events { get; set; }

        [ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; }


    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NourishingHands.Areas.Identity.Data
{
    public class AllEvents
    {
        public int EventId { get; set; }
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? EventStartDate { get; set; }
        public DateTimeOffset? EventEndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
    }
}
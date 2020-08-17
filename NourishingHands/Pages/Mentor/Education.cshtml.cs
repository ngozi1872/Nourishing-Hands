using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Mentor
{
    public class EducationModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;

        public EducationModel(NourishingHandsContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public EducationHistory EducationHistory { get; set; }
        public void OnGet()
        {
        }
    }
}

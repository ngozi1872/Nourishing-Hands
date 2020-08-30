using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;

namespace NourishingHands.Pages.Mentor
{
    public class _UpdateEmploymentModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public _UpdateEmploymentModel(NourishingHandsContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [BindProperty]
        public EmploymentHistory EmploymentHistory { get; set; }
        public async Task<PartialViewResult> OnGetAsync(int id)
        {
            if (id == 0)
            {
                return Partial("/");
            }

            EmploymentHistory = await _dbContext.EmploymentHistories.SingleOrDefaultAsync(p => p.Id == id);
            ViewData["Employer"] = EmploymentHistory.Employer;

            return Partial("_UpdateEmployment", EmploymentHistory);

            // return Partial("_ProductDetails", await productService.GetProductAsync(id));
        }
    }
}

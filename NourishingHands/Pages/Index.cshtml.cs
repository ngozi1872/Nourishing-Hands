using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NourishingHands.Utilities;

namespace NourishingHands.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Phone")]
            public string Phone { get; set; }

            [Required]
            [Display(Name = "Message")]
            public string Message { get; set; }

        }
        public void OnGet()
        {

        }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                SendEmailFromGmail contactFromHome = new SendEmailFromGmail();

                var subj = "Message From contact page - From " + Input.FullName;
                var body = "Email: " + Input.Email + " <br/>Phone: " + Input.Phone + "< br />< br />"+ Input.Message + " <br/><br/>";
                contactFromHome.SendEmail("ngozi1872@gmail.com", "COP", subj, body);
            }

        }
    }
}

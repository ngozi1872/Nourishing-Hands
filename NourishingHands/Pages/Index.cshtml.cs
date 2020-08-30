using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NourishingHands.Utilities;

namespace NourishingHands.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostEnvironment;

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
                var path = Path.Combine(_hostingEnvironment.WebRootPath, $"assets/images/NourishingHandsLogo.png");

                SendEmailFromGmail contactFromHome = new SendEmailFromGmail();

                var subj = string.Format ($"Message From contact page - From {Input.FullName}");
                string body = string.Format($"From: {Input.FullName} <br/> Email: {Input.Email} <br/>Phone: {Input.Phone} <br/><br/>{Input.Message} <br/><br/>");
                contactFromHome.SendEmail("ngozi1872@gmail.com", "COP", subj, body, path);

                ModelState.Clear();
                
            }

        }
    }
}

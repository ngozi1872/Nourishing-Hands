using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;
using NourishingHands.Utilities;

namespace NourishingHands.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly NourishingHandsContext _dbContext;

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment hostEnvironment, NourishingHandsContext dbContext)
        {
            _logger = logger;
            _hostingEnvironment = hostEnvironment;
            _dbContext = dbContext;

        }

        [BindProperty]
        public InputModel Input { get; set; }
        public IList<Events> Events { get; set; }
        public DateTime UpComingEvent { get; set; }
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
            Events = _dbContext.Events
                .Where(e => e.EventStartDate >= DateTime.Now)
                .OrderBy(e => e.EventStartDate)
                .Take(3)
                .ToList();

            var eventn = _dbContext.Events.OrderBy(t => t.EventStartDate).FirstOrDefault();

            UpComingEvent = eventn.EventStartDate.HasValue ? (DateTime)eventn.EventStartDate.Value.Date : DateTime.Now.Date;

        }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                var path = Path.Combine(_hostingEnvironment.WebRootPath, $"assets/images/NH-Logo.png");

                SendEmailFromGmail contactFromHome = new SendEmailFromGmail();

                var subj = string.Format ($"Message From contact page - From {Input.FullName}");
                string body = string.Format($"From: {Input.FullName} <br/> Email: {Input.Email} <br/>Phone: {Input.Phone} <br/><br/>{Input.Message} <br/><br/>");
                contactFromHome.SendEmail("ngozi1872@gmail.com", "COP", subj, body, path);

                ModelState.Clear();
                
            }
        }

        public IActionResult OnGetFindAllEvents()
        {
            var events = _dbContext.Events.Select(e => new
            {
                id = e.Id,
                title = e.Name,
                description = e.Description,
                start = e.EventStartDate,
                end = e.EventEndDate
            }).ToList();
            return new JsonResult(events);
        }
    }
}

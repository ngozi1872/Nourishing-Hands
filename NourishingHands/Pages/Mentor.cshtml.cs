using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NourishingHands.Pages
{
    public class MentorModel : PageModel
    {
        public class Input
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Role { get; set; }
            public DateTime? BirthDay { get; set; }
            public string SSNumber { get; set; }
            public string Email { get; set; }
        }
        public void OnGet()
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using NourishingHands.Areas.Identity.Data;
using NourishingHands.Areas.Identity.NourishingHands.Data;
using NourishingHands.Utilities;
using Stripe;

namespace NourishingHands.Pages
{
    public class DonationModel : PageModel
    {
        private readonly NourishingHandsContext _dbContext;
        private readonly StripeApiFactory _apiFactory;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DonationModel(NourishingHandsContext dbContext, StripeApiFactory apiFactory, IConfiguration config, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _apiFactory = apiFactory;
            _config = config;
            _hostingEnvironment = hostingEnvironment;
        }

        [BindProperty]
        public Donation Donation { get; set; }
        [BindProperty]
        public string DonationAmountStr { get; set; }
        public string PublishableKey { get; set; }
        public void OnGet()
        {
            PublishableKey = _apiFactory.GetPublishableKey();
        }
        public async Task<IActionResult> OnPost(string stripeToken)
        {
            Donation.DonationAmount = Convert.ToDecimal(DonationAmountStr.Replace("$", ""));
            long donationAmt = Convert.ToInt64(Donation.DonationAmount) * 100;
            var recurring = Convert.ToBoolean(Request.Form["recurringCheckbox"]);

            if (ModelState.IsValid && Donation != null)
            {
                var customerOptions = new CustomerCreateOptions
                {
                    Email = Donation.Email,
                    Source = stripeToken,
                };

                var customerService = new CustomerService();
                Customer customer = customerService.Create(customerOptions);

                if(recurring != true)
                {
                    var chargeOptions = new ChargeCreateOptions
                    {
                        Customer = customer.Id,
                        Description = "One-Time Donation to Nourishing Hands.",
                        Amount = donationAmt,
                        Currency = "usd",
                    };
                    var chargeService = new ChargeService();
                    Charge charge = chargeService.Create(chargeOptions);

                    if (charge.Status == "succeeded")
                    {
                        await AddDonation();
                        return RedirectToPage("DonationConfirmed");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, charge.Status);
                        //StatusMessage = charge.Status;
                    }
                }
                else
                {
                    var options = new PriceCreateOptions
                    {
                        Nickname = "Monthly Donation to Nourishing Hands.",
                        Product = _config.GetValue<string>("Stripe:RecurringDonationProduct"),
                        Active = true,
                        UnitAmount = donationAmt,
                        Currency = "usd",
                        Recurring = new PriceRecurringOptions
                        {
                            Interval = "month",
                            UsageType = "licensed",
                        },
                    };
                    var donation = new PriceService();
                    var price = donation.Create(options);

                    var subscriptionOptions = new SubscriptionCreateOptions
                    {
                        Customer = customer.Id,
                        Items = new List<SubscriptionItemOptions> {
                            new SubscriptionItemOptions {
                                Price = price.Id,
                            },
                        },
                    };

                    var subscriptionService = new SubscriptionService();
                    var subscription = subscriptionService.Create(subscriptionOptions);

                    if (subscription.Status == "active")
                    {
                        Donation.Recurring = true;
                        await AddDonation();
                        return RedirectToPage("DonationConfirmed");
                    }
                }
            }

            return Page();
        }

        private async Task AddDonation()
        {
            Donation.CreatedOn = DateTime.Now;

            await _dbContext.Donations.AddAsync(Donation);

            _dbContext.SaveChanges();

            var path = Path.Combine(_hostingEnvironment.WebRootPath, $"assets/images/NH-Logo.png");

            SendEmailFromGmail sfgmail = new SendEmailFromGmail();

            string body = string.Format("Nourishing Hands Inc.<br/> 1445 Woodmount Lane NW, <br/> Atlanta GA, 30318 <br/><br/>EIN#20-1510373 <br/><br/>Thank You for your generous donation of $" + Donation.DonationAmount
            + " to the Nourishing Hands youth mentoring program. <br/><br/>Your donation goes directly to helping us in our work with developing youth into productive and sustainable citizens, to preserve and lead our future for generations to come.<br/><br/>Payment methods<br/>Online: https://www.nourishinghandsinc.org<br/>"
            + "Paypal: PayPal.Me<br/>Via Check: Payable Nourishing Hands Inc.<br/>1445 Woodmount Lane NW,<br/>Atlanta GA, 30318<br/><br/>Sincerely,<br/>Cynthia Logan<br/>Executive Director<br/><br/>"
            + "<br/><br/>Our mission: To deliver relevant resources, experiences, and opportunities to youth through mentoring; empowering them for future success.");
            sfgmail.SendEmail(Donation.Email, Donation.FirstName, "Thank you!", body, path);

        }
    }
}
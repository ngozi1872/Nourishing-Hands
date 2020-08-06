using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NourishingHands.Areas.Identity.NourishingHands.Data;

[assembly: HostingStartup(typeof(NourishingHands.Areas.Identity.IdentityHostingStartup))]
namespace NourishingHands.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<NourishingHandsContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("NourishingHandsContextConnection")));

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<NourishingHandsContext>();
            });
        }
    }
}
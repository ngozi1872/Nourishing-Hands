using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NourishingHands.Utilities;
using Stripe;

namespace NourishingHands
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddAuthentication()
               .AddGoogle(options =>
               {
                   //IConfigurationSection googleAuthNSection =
                   //    Configuration.GetSection("Authentication:Google");

                   options.ClientId = Configuration["Google:ClientId"];
                   options.ClientSecret = Configuration["Google:Secret"];
               })
               .AddFacebook(facebookOptions =>
               {
                   facebookOptions.AppId = Configuration["FaceBook:ClientId"];
                   facebookOptions.AppSecret = Configuration["FaceBook:Secret"];
               });

            services.AddSingleton(factory => new StripeApiFactory(
                Configuration["Stripe:SecretKey"],
                Configuration["Stripe:PublishableKey"]));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}

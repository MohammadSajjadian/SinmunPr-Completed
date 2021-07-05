using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SinmunPr.Areas.Identity.Data;
using SinmunPr.Data;

[assembly: HostingStartup(typeof(SinmunPr.Areas.Identity.IdentityHostingStartup))]
namespace SinmunPr.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<DBsinmun>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DBsinmunConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<DBsinmun>();

                services.Configure<IdentityOptions>(x =>
                {
                    x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                    x.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMOPQRSTUVWXYZ0123456789-_.@$ ";
                });

                services.ConfigureApplicationCookie(x =>
                {
                    x.LoginPath = "/account/login";
                    x.AccessDeniedPath = "/home/error";
                });

                services.AddAuthorization(x =>
                {
                    x.AddPolicy("adminPolicy", y => y.RequireRole("admin"));
                });
            });
        }
    }
}
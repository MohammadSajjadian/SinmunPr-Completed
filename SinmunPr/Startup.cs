using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReflectionIT.Mvc.Paging;
using SinmunPr.Areas.Identity.Data;
using SinmunPr.Models;
using SinmunPr.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr
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
            services.AddControllersWithViews();
            services.AddSingleton<IResize, ResizeProssece>();
            services.AddSingleton<IMail, EmailProcess>();
            services.AddAuthentication();
            services.AddAuthorization();
            services.AddSession();
            services.AddPaging();
            services.AddCloudscribePagination();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Home/Error";
                    await next();
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            MohammadInit(userManager, roleManager).Wait();
        }

        private async Task MohammadInit(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            List<string> roles = new List<string>() { "admin", "user" };

            foreach (var item in roles)
            {
                if (await roleManager.RoleExistsAsync(item) == false)
                {
                    var role = new IdentityRole(item);

                    await roleManager.CreateAsync(role);
                }
            }

            var user = await userManager.FindByIdAsync("ddc81bf5-d1c2-4c5f-a936-b8f52b53af97");
            if (user == null)
            {
                ApplicationUser admin = new ApplicationUser()
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "pP_0987");
            }

            if (await userManager.IsInRoleAsync(user, "admin") == false)
            {
                await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}

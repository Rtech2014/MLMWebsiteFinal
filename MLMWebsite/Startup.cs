using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MLMWebsite.Data;
using MLMWebsite.Services;
using System;
using System.Threading.Tasks;

namespace MLMWebsite
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IEmailSender, EmailSender>(i =>
                new EmailSender(
                    Configuration["EmailSender:Host"],
                    Configuration.GetValue<int>("EmailSender:Port"),
                    Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    Configuration["EmailSender:UserName"],
                    Configuration["EmailSender:Password"]
                ));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders(); ;

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

          CreateRoles(serviceProvider).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "SuperAdmin", "InitAdmin", "Admin" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            ApplicationUser superuser = await UserManager.FindByEmailAsync("superadmin@gmail.com");

            if (superuser == null)
            {
                superuser = new ApplicationUser()
                {
                    UserName = "superadmin",
                    Email = "superadmin@gmail.com",
                };
                await UserManager.CreateAsync(superuser, "Test@123");
            }
            await UserManager.AddToRoleAsync(superuser, "SuperAdmin");

            //ApplicationUser user = await UserManager.FindByEmailAsync("initadmin@gmail.com");

            //if (user == null)
            //{
            //    user = new ApplicationUser()
            //    {
            //        UserName = "InitAdmin",
            //        Email = "InitAdmin@gmail.com",
            //    };
            //    await UserManager.CreateAsync(user, "Test@123");
            //}
            //await UserManager.AddToRoleAsync(user, "InitAdmin");

            //ApplicationUser Admin1 = await UserManager.FindByEmailAsync("admin1@gmail.com");

            //if (Admin1 == null)
            //{
            //    user = new ApplicationUser()
            //    {
            //        UserName = "Admin1",
            //        Email = "Admin1@gmail.com",
            //    };
            //    await UserManager.CreateAsync(user, "Test@123");
            //}
            //await UserManager.AddToRoleAsync(user, "Admin");
            //ApplicationUser Admin2 = await UserManager.FindByEmailAsync("admin2@gmail.com");

            //if (Admin2 == null)
            //{
            //    user = new ApplicationUser()
            //    {
            //        UserName = "Admin2",
            //        Email = "Admin2@gmail.com",
            //    };
            //    await UserManager.CreateAsync(user, "Test@123");
            //}
            //await UserManager.AddToRoleAsync(user, "Admin");
            //ApplicationUser Admin3 = await UserManager.FindByEmailAsync("admin3@gmail.com");

            //if (Admin3 == null)
            //{
            //    user = new ApplicationUser()
            //    {
            //        UserName = "Admin3",
            //        Email = "Admin3@gmail.com",
            //    };
            //    await UserManager.CreateAsync(user, "Test@123");
            //}
            //await UserManager.AddToRoleAsync(user, "Admin");

            //ApplicationUser Admin4 = await UserManager.FindByEmailAsync("admin4@gmail.com");

            //if (Admin4 == null)
            //{
            //    user = new ApplicationUser()
            //    {
            //        UserName = "Admin4",
            //        Email = "Admin4@gmail.com",
            //    };
            //    await UserManager.CreateAsync(user, "Test@123");
            //}
            //await UserManager.AddToRoleAsync(user, "Admin");
            //ApplicationUser Admin5 = await UserManager.FindByEmailAsync("admin5@gmail.com");

            //if (user == null)
            //{
            //    user = new ApplicationUser()
            //    {
            //        UserName = "Admin5",
            //        Email = "Admin5@gmail.com",
            //    };
            //    await UserManager.CreateAsync(user, "Test@123");
            //}
            //await UserManager.AddToRoleAsync(user, "Admin");

            //ApplicationUser Admin6 = await UserManager.FindByEmailAsync("admin6@gmail.com");

            //if (Admin6 == null)
            //{
            //    user = new ApplicationUser()
            //    {
            //        UserName = "Admin6",
            //        Email = "Admin6@gmail.com",
            //    };
            //    await UserManager.CreateAsync(user, "Test@123");
            //}
            //await UserManager.AddToRoleAsync(user, "Admin");
            //ApplicationUser Admin7 = await UserManager.FindByEmailAsync("admin7@gmail.com");

            //if (Admin7 == null)
            //{
            //    user = new ApplicationUser()
            //    {
            //        UserName = "Admin7",
            //        Email = "Admin7@gmail.com",
            //    };
            //    await UserManager.CreateAsync(user, "Test@123");
            //}
            //await UserManager.AddToRoleAsync(user, "Admin");


            //ApplicationUser Admin8 = await UserManager.FindByEmailAsync("admin8@gmail.com");

            //if (Admin8 == null)
            //{
            //    user = new ApplicationUser()
            //    {
            //        UserName = "Admin8",
            //        Email = "Admin8@gmail.com",
            //    };
            //    await UserManager.CreateAsync(user, "Test@123");
            //}
            //await UserManager.AddToRoleAsync(user, "Admin");

            //ApplicationUser Admin9 = await UserManager.FindByEmailAsync("admin9@gmail.com");

            //if (Admin9 == null)
            //{
            //    user = new ApplicationUser()
            //    {
            //        UserName = "Admin9",
            //        Email = "Admin9@gmail.com",
            //    };
            //    await UserManager.CreateAsync(user, "Test@123");
            //}
            //await UserManager.AddToRoleAsync(user, "Admin");


        }
    }
}

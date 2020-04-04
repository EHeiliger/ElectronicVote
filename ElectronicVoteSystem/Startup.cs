using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using ElectronicVoteSystem.Models;
using ElectronicVoteSystem.Models.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ElectronicVoteSystem
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //This method gets called by the runtime.Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var emailConfig = Configuration.GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSenderGmail>();
            services.AddControllersWithViews();
            services.AddDbContext<ElectronicVotingContext>(options => options.UseSqlServer(Configuration.GetConnectionString("VoteSystemDb")));
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddAutoMapper(typeof(AutoMapping).GetTypeInfo().Assembly);
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ElectronicVotingContext>();
            services.ConfigureApplicationCookie(options => options.LoginPath = "/home/login");
        }

       // This method gets called by the runtime.Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

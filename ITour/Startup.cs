using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rotativa.AspNetCore;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;
using ITour.Services.Options;
using Microsoft.AspNetCore.Identity.UI.Services;
using WebPWrecover.Services;
using ITour.Services.Email;

namespace ITour
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

            // UserRoles in DefaultIdentity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            services.AddAuthorization(options =>
            {
                // Role based Policy
                options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("Director", policy => policy.RequireRole("Director"));
                options.AddPolicy("Accountant", policy => policy.RequireRole("Accountant"));
                options.AddPolicy("Manager", policy => policy.RequireRole("Manager"));
                options.AddPolicy("Customer", policy => policy.RequireRole("Customer"));
                // Object based Policy
                options.AddPolicy("Profits", policy => policy.RequireRole("Manager", "Accountant"));
            });

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    // Role based Authorize
                    options.Conventions.AuthorizeFolder("/Admin", "Administrator");
                    options.Conventions.AuthorizeFolder("/RegistryTO", "Administrator");
                    options.Conventions.AuthorizeFolder("/AppUsers", "Manager");
                    options.Conventions.AuthorizeFolder("/AppCompanies", "Manager");
                    options.Conventions.AuthorizeFolder("/Orders", "Manager");
                    options.Conventions.AuthorizeFolder("/Payments", "Manager");
                    options.Conventions.AuthorizeFolder("/Prints", "Manager");
                    options.Conventions.AuthorizeFolder("/Prints/Templates", "Director");
                    options.Conventions.AuthorizeFolder("/Reports", "Manager");
                    options.Conventions.AuthorizeFolder("/Services", "Manager");
                    options.Conventions.AuthorizeFolder("/Profits", "Manager");
                    // Object based Authorize
                    //options.Conventions.AuthorizeFolder("/Profits", "Profits");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            services.AddOptions();
            services.Configure<List<Tenant>>("Tenants", Configuration.GetSection("Tenants"));
            services.Configure<AppFileOptions>("AppFiles", Configuration.GetSection("AppFiles"));
            services.Configure<EmailSenderOptions>("EmailSender", Configuration.GetSection("EmailSender"));


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ITenantProvider, OptionsTenantProvider>();
            services.AddScoped<IAppFileHandler, AppFileHandler>();
            services.AddScoped<IEmailSender, SendGridEmailSender>();
            services.AddScoped<IAppOptionsProvider, AppOptionsProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();

            RotativaConfiguration.Setup(env);   // To use Rotativa - must be
        }
    }
}

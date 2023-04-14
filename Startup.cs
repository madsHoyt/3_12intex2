using intex2.Data;
using intex2.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace intex2
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
            //Connectionstrings
            string defaultconnectionString = System.Environment.GetEnvironmentVariable("DefaultConnection");
            string mysqlconnectionString = System.Environment.GetEnvironmentVariable("MySqlConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<intexdataContext>(options =>
                options.UseMySql(
                    Configuration.GetConnectionString("MySqlConnection")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
               .AddDefaultUI()
               .AddDefaultTokenProviders()
               .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration configuration = builder.Build();

            //Cookies
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //Improve passwords
            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddScoped<PredictSexService>();
            services.AddScoped<PredictWrappingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            ////CSP header
            app.Use(async (context, next) =>
           {
               context.Response.Headers.Add("Content-Security-Policy",
                   "default-src 'self';" +
                   "script-src 'self'  https://use.fontawesome.com https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js https://cdn.startbootstrap.com/sb-forms-latest.js https://cdn.plot.ly/plotly-latest.min.js https://code.jquery.com/jquery-3.6.0.min.js https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js ;" +
                   "script-src-elem 'self' 'sha256-m1igTNlg9PL5o60ru2HIIK6OPQet2z9UgiEAhCyg/RU=' https://cdn.startbootstrap.com/sb-forms-latest.js https://code.jquery.com/jquery-3.6.0.min.js https://cdn.plot.ly/plotly-latest.min.js https://use.fontawesome.com/releases/v6.3.0/js/all.js https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js https://fonts.googleapis.com/css?family=Montserrat:400,700  https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js ;" +
                   "style-src 'self' https://cdn.startbootstrap.com/sb-forms-latest.js 'sha256-vJP66++gXP+5BHaPBuciXrdQiwJkDpcXCbxM+tt2D0g=' 'sha256-vJP66++gXP+5BHaPBuciXrdQiwJkDpcXCbxM+tt2D0g=' 'sha256-47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU=' 'sha256-Jc7XaRBVYMy6h6FvjL32miHrOGOxYV+OP4swZ/9Gysw=' 'sha256-Jc7XaRBVYMy6h6FvjL32miHrOGOxYV+OP4swZ/9Gysw=' 'sha256-47DEQpj8HBSa+/TImW+5JCeuQeRkm5NMpJWZG3hSuFU=' https://fonts.googleapis.com/css?family=Montserrat:400,700 https://fonts.googleapis.com/css?family=Roboto+Slab:400,100,300,700 ; " +
                   "style-src-elem 'self' https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js https://fonts.googleapis.com/css?family=Montserrat:400,700 https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.bundle.min.js ;" +
                   "font-src 'self' https://fonts.googleapis.com/css?family=Montserrat:400,700; " +
                   "img-src 'self';" +
                   " frame-src 'self'");
               await next();
           });

            //cookies
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "burial",
                pattern: "{ controller = Home}/{ action = Details}/{ id?}");

                endpoints.MapControllerRoute(
                name: "Paging",
                pattern: "Page{pageNum}",
                defaults: new { Controller = "Home", action = "Index" }
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}

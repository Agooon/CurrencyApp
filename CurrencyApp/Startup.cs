using CurrencyAppDatabase.Initialization;
using CurrencyAppDatabase.DataAccess;
using CurrencyAppDatabase.Models.CurrencyApp;
using CurrencyAppDatabase.Models.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using System;

namespace CurrencyApp
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
            // For making less usage of Database
            services.AddMemoryCache();
            // Identity and Db context //
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<CurrencyContext>();

            services.AddDbContext<CurrencyContext>(config => {
                config.UseSqlServer(Configuration.GetConnectionString("DbConnection"));
            });
            /////////////////////////////

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            // Security //
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Admin"));
            });

            services.AddRazorPages().AddRazorRuntimeCompilation()
                .AddRazorPagesOptions(config => { 
                    config.Conventions.AuthorizeFolder("/");
                    config.Conventions.AuthorizeFolder("/Admin", "RequireAdministratorRole");
                    //config.Conventions.AuthorizeFolder("/AdminPanel","RequireAdministratorRole");
                });

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            /////////////////////////////           

            // Api connections //
            services.AddHttpClient(); 
            services.AddHttpClient("nbp", config =>
            {
                config.BaseAddress = new Uri(Configuration.GetValue<string>("NbpAPI"));
            });
            /////////////////////////////
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
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

            // Setting up initialization of  database
            SetupDatabase.InitializeDatabase(serviceProvider,
                                Configuration.GetSection("AdminSettings").Get<string[]>(),
                                Configuration.GetSection("RoleList").Get<string[]>()
                                ).Wait();
        }
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Trainingsplanner.Postgres.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using NSwag.AspNetCore;
using Trainingsplanner.Postgres.BuisnessLogic.Mapping;
using Trainingsplanner.Postgres.Data.Models;
using AutoMapper;
using NSwag;
using System.Collections.Generic;
using NSwag.Generation.Processors.Security;
using Trainingsplanner.Postgres.BuisnessLogic;
using Duende.IdentityServer.Services;
using Trainingsplanner.Postgres.AuthorizationHandler;
using EmailService;
using Infrastructure;

namespace Trainingsplanner.Postgres
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddScoped<IProfileService, AdditionalRoleProfileService>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppRoles.Admin, policy =>
                    policy.Requirements.Add(new HasRoleRequirement(AppRoles.Admin)));
                options.AddPolicy(AppRoles.Trainer, policy =>
                    policy.Requirements.Add(new HasRoleRequirement(AppRoles.Trainer)));
                options.AddPolicy(AppRoles.Athlet, policy =>
                    policy.Requirements.Add(new HasRoleRequirement(AppRoles.Athlet)));
                options.AddPolicy(AppPolicies.CanEditTrainingsGroup, policy =>
                   policy.Requirements.Add(new TrainingsGroupRequirement(AppClaims.EditTrainingsGroup)));
                options.AddPolicy(AppPolicies.CanReadTrainingsGroup, policy =>
                    policy.Requirements.Add(new TrainingsGroupRequirement(AppClaims.ReadTrainingsGroup)));
                options.AddPolicy(AppPolicies.CanEditTrainingsAppointment, policy =>
                    policy.Requirements.Add(new TrainingsAppointmentRequirement()));
                options.AddPolicy(AppPolicies.CanEditTrainingsModule, policy =>
                    policy.Requirements.Add(new TrainingsModuleRequirement()));
            });

            services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddRazorPages();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });

            //Swagger
            services.AddOpenApiDocument(s =>
            {
                s.Title = "laplanner";
                s.Version = "v1";
                s.DocumentName = "laplanner";
            });

            //EmailService
            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();

            //Automapper
            services.AddAutoMapper(typeof(MapperExtensions));

            services.AddAppServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapGet("/Identity/Account/Register", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login", true)));
                endpoints.MapPost("/Identity/Account/Register", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login", true)));

            });

            //Swagger
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            //Automapper
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            UpdateDatabase(serviceScopeFactory);
        }

        private void UpdateDatabase(IServiceScopeFactory serviceScopeFactory)
        {
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider
                    .GetService<ApplicationDbContext>())
                {
                    if (context.Database == null)
                        context.Database.Migrate();


                    using (var roleMgr = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>())
                    {
                        IdentityRole athlete = roleMgr.FindByNameAsync(AppRoles.Athlet).Result;
                        if (athlete == null)
                        {
                            var roleresult = roleMgr.CreateAsync(new IdentityRole()
                            {
                                Name = AppRoles.Athlet,
                            }).Result;
                        }
                        IdentityRole trainer = roleMgr.FindByNameAsync(AppRoles.Trainer).Result;
                        if (trainer == null)
                        {
                            var roleresult = roleMgr.CreateAsync(new IdentityRole()
                            {
                                Name = AppRoles.Trainer,
                            }).Result;
                        }
                        IdentityRole admin = roleMgr.FindByNameAsync(AppRoles.Admin).Result;
                        if (admin == null)
                        {
                            var roleresult = roleMgr.CreateAsync(new IdentityRole()
                            {
                                Name = AppRoles.Admin,
                            }).Result;
                        }

                    }

                    using (var userMgr = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>())
                    {
                        var admin = userMgr.FindByEmailAsync("weidnerflo@gmail.com").Result;
                        if (admin == null)
                        {
                            admin = new ApplicationUser
                            {
                                UserName = "weidnerflo@gmail.com",
                                Email = "weidnerflo@gmail.com",
                                EmailConfirmed = true,
                                FirstName = "Florian",
                                LastName = "Weidner",
                            };
                            var result = userMgr.CreateAsync(admin, Configuration.GetValue<string>("AdminPwd")).Result;
                            admin = userMgr.FindByEmailAsync("weidnerflo@gmail.com").Result;

                            var roleresult = userMgr.AddToRoleAsync(admin, AppRoles.Admin).Result;
                        }
                    }
                }
            }
        }
    }
}
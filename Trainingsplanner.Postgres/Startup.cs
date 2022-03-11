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
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection"),options => options.SetPostgresVersion(new Version(9, 6))));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            


            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddControllersWithViews();
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
            app.UseMiddleware<ApiKeyMiddleware>();

            //Automapper
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            UpdateDatabase(serviceScopeFactory);
        }
        
        private static void UpdateDatabase(IServiceScopeFactory serviceScopeFactory)
        {
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider
                    .GetService<ApplicationDbContext>())
                {
                   if(context.Database == null)
                        context.Database.Migrate();
                }
            }
        }
    }
}
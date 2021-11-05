using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using TrainingsPlanner.AuthorizationHandler;
using TrainingsPlanner.BuisnessLogic.Mapping;
using TrainingsPlanner.Infrastructure;

namespace TrainingsPlanner
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
            services.AddDbContext<TrainingDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TrainingsDbConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5001";
                options.Audience = "api1";

                options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppPolicies.CanAdministrate, policy => policy.RequireClaim(AppClaims.CanAdminsitrate));
                options.AddPolicy(AppPolicies.CanCreateContent, policy => policy.RequireClaim(AppClaims.CanCreateContent));
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
            
            //Swagger
            services.AddOpenApiDocument(s =>
            {
                s.Title = "TrainingsplanAPI";
                s.Version = "v1";
                s.DocumentName = "v1";
                s.AddSecurity("oauth2", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = Configuration["TrainingsIdentityApiBaseUrl"] + "/connect/authorize",
                            TokenUrl = Configuration["TrainingsIdentityApiBaseUrl"] + "/connect/token",
                            Scopes = new Dictionary<string, string> { { "api1", "TrainingsPlannerApi 1.0" } }
                        }
                    }
                });
                s.OperationProcessors.Add(new OperationSecurityScopeProcessor("oauth2"));
            });

            //Automapper
            services.AddAutoMapper(typeof(MapperExtensions));

            
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });

            services.AddAppServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceScopeFactory serviceScopeFactory, IMapper mapper)
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
            app.UseSwaggerUi3(options =>
            {
                options.OAuth2Client = new OAuth2ClientSettings();
                options.OAuth2Client.ClientId = "demo_api_swagger";
                options.OAuth2Client.AppName = "Demo API - Swagger";
                options.OAuth2Client.UsePkceWithAuthorizationCodeGrant = true;
            });
            
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

            await UpdateDatabase(serviceScopeFactory);
        }
        private static async Task UpdateDatabase(IServiceScopeFactory serviceScopeFactory)
        {
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider
                                                .GetService<TrainingDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
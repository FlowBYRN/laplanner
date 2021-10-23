// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using EmailService;
using IdentityServer4;
using Infrastructure;
using StsServerIdentity.Data;
using StsServerIdentity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using StsServerIdentity.Infrastructure;

namespace StsServerIdentity
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });
            services.AddControllersWithViews();

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
                {
                    options.IssuerUri = Configuration["TrainingsIdentityApiBaseUrl"];
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                    options.EmitStaticAudienceClaim = true;
                })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients(Configuration))
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<ProfileService>();

            services.AddLocalApiAuthentication();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppPolicies.CanCreateContent, p => p.RequireClaim(AppClaims.CanCreateContent));
                options.AddPolicy(AppPolicies.CanAdministrate, p => p.RequireClaim(AppClaims.CanAdminsitrate));
            });

            //swagger
            services.AddOpenApiDocument(s =>
            {
                s.Title = "TrainingsIdentityApi";
                s.Version = "v1";
                s.DocumentName = "is4";
                s.AddSecurity("oauth2", new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = Configuration["TrainingsIdentityApiBaseUrl"] + "/connect/authorize",
                            TokenUrl = Configuration["TrainingsIdentityApiBaseUrl"] + "/connect/token",
                            Scopes = new Dictionary<string, string>
                                {{IdentityServerConstants.LocalApi.ScopeName, "TrainingsPlannerApi 1.0"}}
                        }
                    }
                });
                s.OperationProcessors.Add(new OperationSecurityScopeProcessor("oauth2"));
            });

            //EmailService
            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();

            // not recommended for production - you need to store your key material somewhere secure
            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                var rsaCertificate = new X509Certificate2(
                    "identityCertificate.pfx", "Thisisapassword_123");

                builder.AddSigningCredential(rsaCertificate);
            }
        }

        public void Configure(IApplicationBuilder app, IServiceScopeFactory serviceScopeFactory)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });

            //Swagger
            app.UseOpenApi();
            app.UseSwaggerUi3(options =>
            {
                options.OAuth2Client = new OAuth2ClientSettings();
                options.OAuth2Client.ClientId = "TrainingsIdentiySwagger";
                options.OAuth2Client.AppName = "TrainingsIdentityApi - Swagger";
                options.OAuth2Client.UsePkceWithAuthorizationCodeGrant = true;
            });
            UpdateAndSeedDatabase(serviceScopeFactory);
        }

        private async void UpdateAndSeedDatabase(IServiceScopeFactory serviceScopeFactory)
        {
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var context = serviceScope.ServiceProvider
                    .GetService<IdentityDbContext>();
                context.Database.Migrate();

                using (var userMgr = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>())
                {
                    var admin = await userMgr.FindByNameAsync("Admin");
                    if (admin == null)
                    {
                        admin = new ApplicationUser
                        {
                            UserName = "Admin",
                            Email = "admin@th.de",
                            EmailConfirmed = true,
                            FirstName = "Admin",
                            LastName = "Admin",
                            PasswordReseted = true
                        };
                        var result = userMgr.CreateAsync(admin, Configuration.GetValue<string>("AdminPwd")).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        var claims = new List<Claim>()
                        {
                            new Claim(AppClaims.CanAdminsitrate, AppClaims.CanAdminsitrate),
                            new Claim(AppClaims.CanCreateContent, AppClaims.CanCreateContent)
                        };
                        var claimsResult = userMgr.AddClaimsAsync(admin, claims).Result;
                        if (!claimsResult.Succeeded)
                        {
                            throw new Exception(claimsResult.Errors.First().Description);
                        }
                    }
                }
            }
        }
    }
}
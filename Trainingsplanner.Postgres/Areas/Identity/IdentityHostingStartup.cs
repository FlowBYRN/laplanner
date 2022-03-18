using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trainingsplanner.Postgres.Data;
using Trainingsplanner.Postgres.Data.Models;

[assembly: HostingStartup(typeof(Trainingsplanner.Postgres.Areas.Identity.IdentityHostingStartup))]
namespace Trainingsplanner.Postgres.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
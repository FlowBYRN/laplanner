using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.Options;
using Trainingsplanner.Postgres.Data.Models;
using Trainingsplanner.Postgres.Data.Configurations;

namespace Trainingsplanner.Postgres.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<TrainingsAppointment> TrainingsAppointments { get; set; }
        public DbSet<TrainingsExercise> TrainingsExercises { get; set; }
        public DbSet<TrainingsModule> TrainingsModules { get; set; }
        public DbSet<TrainingsGroup> TrainingsGroups { get; set; }
        public DbSet<TrainingsModuleTag> TrainingsModuleTags { get; set; }
        public DbSet<TrainingsAppointmentTrainingsModule> TrainingsAppointmentsTrainingsModules { get; set; }
        public DbSet<TrainingsModuleTrainingsExercise> TrainingsModulesTrainingsExercises { get; set; }
        public DbSet<TrainingsModuleTrainingsModuleTag> TrainingsModulesTrainingsModuleTags { get; set; }
        public DbSet<TrainingsGroupApplicationUser> TrainingsGroupsApplicationUsers { get; set; }
        public DbSet<TrainingsModuleFollow> TrainingsModuleFollows { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (null == modelBuilder)
                throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder
                .ApplyConfiguration(new TrainingsAppointmentEntityTypeConfiguration())
                .ApplyConfiguration(new TrainingsModuleEntityTypeConfiguration())
                .ApplyConfiguration(new TrainingsExerciseEntityTypeConfiguration())
                .ApplyConfiguration(new TrainingsGroupEntityTypeConfiguration())
                .ApplyConfiguration(new TrainingsModuleTagEntityTypeConfiguration())
                .ApplyConfiguration(new TrainingsModuleTrainingsExerciseEntityTypeConfiguration())
                .ApplyConfiguration(new TrainingsAppointmentTrainingsModuleEntityTypeConfiguration())
                .ApplyConfiguration(new TrainingsModuleTrainingsModuleTagEntityTypeConfiguration())
                .ApplyConfiguration(new TrainingsModuleFollowEntityTypeConfiguration())
                .ApplyConfiguration(new TrainingsGroupApplicationUserEntityTypeConfiguration());

        }



    }
}
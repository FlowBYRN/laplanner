using System;
using Microsoft.EntityFrameworkCore;
using TrainingsPlanner.Infrastructure.Configurations;
using TrainingsPlanner.Infrastructure.Models;
using TrainingsPlanner.ViewModels;

namespace TrainingsPlanner.Infrastructure
{
    public class TrainingDbContext : DbContext
    {
        public TrainingDbContext(DbContextOptions<TrainingDbContext> options)
            : base(options)
        {
        }

        public DbSet<TrainingsAppointment> TrainingsAppointments { get; set; }
        public DbSet<TrainingsExercise> TrainingsExercises { get; set; }
        public DbSet<TrainingsModule> TrainingsModules { get; set; }
        public DbSet<TrainingsGroup> TrainingsGroups { get; set; }
        public DbSet<TrainingsModuleTag> TrainingsModuleTags { get; set; }
        public DbSet<TrainingsAppointmentTrainingsModule> TrainingsAppointmentsTrainingsModules { get; set; }
        public DbSet<TrainingsModuleTrainingsExercise> TrainingsModulesTrainingsExercises { get; set; }
        public DbSet<TrainingsModuleTrainingsModuleTag> TrainingsModulesTrainingsModuleTags { get; set; }
        public DbSet<TrainingsGroupApplicationUser> TrainingsGroupsApplicationUsers { get; set; }



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
                .ApplyConfiguration(new TrainingsGroupApplicationUserEntityTypeConfiguration());
            
        }



        public DbSet<TrainingsPlanner.ViewModels.TrainingsModuleTagDto> TrainingsModuleTagDto { get; set; }
    }
}
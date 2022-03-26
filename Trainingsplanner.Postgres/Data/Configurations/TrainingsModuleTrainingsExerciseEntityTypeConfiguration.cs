using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.Data.Configurations
{
    public class TrainingsModuleTrainingsExerciseEntityTypeConfiguration : IEntityTypeConfiguration<TrainingsModuleTrainingsExercise>
    {
        public void Configure(EntityTypeBuilder<TrainingsModuleTrainingsExercise> builder)
        {
            if (null == builder)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable("TrainingsModulesTrainingsExercises");

            // Id
            builder.HasKey(c => new { c.TrainingsModuleId, c.TrainingsExerciesId });

            // Properties
            builder.Property(b => b.Created).HasDefaultValueSql("GETUTCDATE()");

            // Navigation
            builder.HasOne(p => p.TrainingsModule)
                .WithMany(b => b.TrainingsModulesTrainingsExercises)
                .HasForeignKey(f => f.TrainingsModuleId);

            builder.HasOne(p => p.TrainingsExercise)
                .WithMany(b => b.TrainingsModulesTrainingsExercises)
                .HasForeignKey(f => f.TrainingsExerciesId);
        }
    }
}
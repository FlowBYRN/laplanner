using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.Data.Configurations
{
    public class TrainingsModuleTrainingsModuleTagEntityTypeConfiguration : IEntityTypeConfiguration<TrainingsModuleTrainingsModuleTag>
    {
        public void Configure(EntityTypeBuilder<TrainingsModuleTrainingsModuleTag> builder)
        {
            if (null == builder)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable("TrainingsModulesTrainingsModuleTags");

            // Id
            builder.HasKey(c => new { c.TrainingsModuleId, c.TrainingsModuleTagId });

            // Properties
            builder.Property(b => b.Created).HasDefaultValueSql("GETUTCDATE()");

            // Navigation
            builder.HasOne(p => p.TrainingsModule)
                .WithMany(b => b.TrainingsModulesTrainingsModuleTags)
                .HasForeignKey(f => f.TrainingsModuleId);

            builder.HasOne(p => p.TrainingsModuleTag)
                .WithMany(b => b.TrainingsModulesTrainingsModuleTags)
                .HasForeignKey(f => f.TrainingsModuleTagId);
        }
    }
}
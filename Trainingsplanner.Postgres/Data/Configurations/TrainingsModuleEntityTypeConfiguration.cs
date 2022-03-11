using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.Data.Configurations
{
    public class TrainingsModuleEntityTypeConfiguration : IEntityTypeConfiguration<TrainingsModule>
    {
        public void Configure(EntityTypeBuilder<TrainingsModule> builder)
        {
            if (null == builder)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable("TrainingsModules");

            // Id
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(b => b.Title).IsRequired().HasMaxLength(100);
            builder.Property(b => b.Description).HasMaxLength(500);

            //Navigation
            // builder.HasOne(p => p.ApplicationUser)
            //     .WithMany(b => b.TrainingsModules)
            //     .HasForeignKey(f => f.UserId);
        }
    }
}
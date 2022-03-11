using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.Data.Configurations
{
    public class TrainingsModuleTagEntityTypeConfiguration : IEntityTypeConfiguration<TrainingsModuleTag>
    {
        public void Configure(EntityTypeBuilder<TrainingsModuleTag> builder)
        {
            if (null == builder)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable("TrainingsModuleTags");

            // Id
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(b => b.Title).IsRequired().HasMaxLength(100);
        }
    }
}
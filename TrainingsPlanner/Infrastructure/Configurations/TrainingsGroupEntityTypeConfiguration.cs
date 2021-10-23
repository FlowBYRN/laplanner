﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingsPlanner.Infrastructure.Models;

namespace TrainingsPlanner.Infrastructure.Configurations
{
    public class TrainingsGroupEntityTypeConfiguration : IEntityTypeConfiguration<TrainingsGroup>
    {
        public void Configure(EntityTypeBuilder<TrainingsGroup> builder)
        {
            if (null == builder)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable("TrainingsGroups");

            // Id
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(b => b.Created).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(b => b.Title).IsRequired().HasMaxLength(100);
            builder.Property(b => b.Description).HasMaxLength(500);
        }
    }
}
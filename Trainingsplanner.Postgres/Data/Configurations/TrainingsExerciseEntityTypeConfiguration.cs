﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.Data.Configurations
{
    public class TrainingsExerciseEntityTypeConfiguration : IEntityTypeConfiguration<TrainingsExercise>
    {
        public void Configure(EntityTypeBuilder<TrainingsExercise> builder)
        {
            if (null == builder)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable("TrainingsExercises");

            // Id
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(b => b.Created).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(b => b.Title).IsRequired().HasMaxLength(100);
            builder.Property(b => b.Description).HasMaxLength(500);

        }
    }
}
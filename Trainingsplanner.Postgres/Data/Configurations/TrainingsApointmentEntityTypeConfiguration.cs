using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.Data.Configurations
{
    public class TrainingsAppointmentEntityTypeConfiguration : IEntityTypeConfiguration<TrainingsAppointment>
    {
        public void Configure(EntityTypeBuilder<TrainingsAppointment> builder)
        {
            if (null == builder)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable("TrainingsAppointments");

            // Id
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(b => b.Title).IsRequired().HasMaxLength(100);
            builder.Property(b => b.Description).HasMaxLength(3000);
            builder.Property(b => b.StartTime).IsRequired();
            builder.Property(b => b.EndTime).IsRequired();
            builder.Property(b => b.Created).HasDefaultValueSql("GETUTCDATE()");


            //builder.HasOne(p => p.TrainingsGroup)
            //    .WithMany(b => b.TrainingsAppointments)
            //    .HasForeignKey(f => f.TrainingsGroupId);
        }
    }
}
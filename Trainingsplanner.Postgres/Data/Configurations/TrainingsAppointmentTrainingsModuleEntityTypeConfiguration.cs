using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.Data.Configurations
{
    public class TrainingsAppointmentTrainingsModuleEntityTypeConfiguration : IEntityTypeConfiguration<TrainingsAppointmentTrainingsModule>
    {
        public void Configure(EntityTypeBuilder<TrainingsAppointmentTrainingsModule> builder)
        {
            if (null == builder)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable("TrainingsAppointmentsTrainingsModules");

            // Id
            builder.HasKey(e => new { e.TrainingsAppointmentId, e.TrainingsModuleId });

            // Properties

            // Navigation
            //builder.HasOne(p => p.TrainingsAppointment)
            //    .WithMany(b => b.TrainingsAppointmentsTrainingsModules)
            //    .HasForeignKey(f => f.TrainingsAppointmentId);

            builder.HasOne(p => p.TrainingsModule)
                .WithMany(b => b.TrainingsAppointmentsTrainingsModules)
                .HasForeignKey(f => f.TrainingsModuleId);

        }
    }
}
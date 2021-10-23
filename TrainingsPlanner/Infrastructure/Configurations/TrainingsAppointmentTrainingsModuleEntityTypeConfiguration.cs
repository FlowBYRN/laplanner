using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingsPlanner.Infrastructure.Models;

namespace TrainingsPlanner.Infrastructure.Configurations
{
    public class TrainingsAppointmentTrainingsModuleEntityTypeConfiguration : IEntityTypeConfiguration<TrainingsAppointmentTrainingsModule>
    {
        public void Configure(EntityTypeBuilder<TrainingsAppointmentTrainingsModule> builder)
        {
            if (null == builder)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable("TrainingsAppointmentsTrainingsModules");

            // Id
            builder.HasKey(e => new {e.TrainingsAppointmentId, e.TrainingsModuleId});

            // Properties
            builder.Property(b => b.Created).HasDefaultValueSql("GETUTCDATE()");
            
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
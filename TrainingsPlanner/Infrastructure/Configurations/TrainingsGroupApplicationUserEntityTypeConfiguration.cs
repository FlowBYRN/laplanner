using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingsPlanner.Infrastructure.Models;

namespace TrainingsPlanner.Infrastructure.Configurations
{
    public class TrainingsGroupApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<TrainingsGroupApplicationUser>
    {
        public void Configure(EntityTypeBuilder<TrainingsGroupApplicationUser> builder)
        {
            if (null == builder)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable("TrainingsGroupsApplicationUsers");

            // Id
            builder.HasKey(c => new {c.ApplicationUserId, c.TrainingsGroupId});

            // Properties
            builder.Property(b => b.Created).HasDefaultValueSql("GETUTCDATE()");
            
            // Navigation
            builder.HasOne(p => p.TrainingsGroup)
                .WithMany(b => b.TrainingsGroupsApplicationUsers)
                .HasForeignKey(f => f.TrainingsGroupId);
            
            // builder.HasOne(p => p.ApplicationUser)
            //     .WithMany(b => b.TrainingsGroupsApplicationUsers)
            //     .HasForeignKey(f => f.ApplicationUserId);
        }
    }
}
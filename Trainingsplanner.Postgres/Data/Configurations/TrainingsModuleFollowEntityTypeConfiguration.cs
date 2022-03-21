using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Trainingsplanner.Postgres.Data.Models;

namespace Trainingsplanner.Postgres.Data.Configurations
{
    public class TrainingsModuleFollowEntityTypeConfiguration : IEntityTypeConfiguration<TrainingsModuleFollow>
    {
        public void Configure(EntityTypeBuilder<TrainingsModuleFollow> builder)
        {
            if (null == builder)
                throw new ArgumentNullException(nameof(builder));

            builder.ToTable("TrainingsModuleFollows");

            // Id
            builder.HasKey(c => c.Id);

            //Navigation
            builder.HasOne(p => p.User)
                .WithMany(b => b.Follows)
                .HasForeignKey(f => f.UserId);

            builder.HasOne(p => p.TrainingsModule)
                .WithMany(b => b.Followers)
                .HasForeignKey(f => f.TrainingsModuleId);
        }
    }
}

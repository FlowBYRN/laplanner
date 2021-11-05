﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TrainingsPlanner.Infrastructure;

namespace TrainingsPlanner.Infrastructure.Migrations.Training
{
    [DbContext(typeof(TrainingDbContext))]
    partial class TrainingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsAppointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("TrainingsGroupId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TrainingsGroupId");

                    b.ToTable("TrainingsAppointments");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsAppointmentTrainingsModule", b =>
                {
                    b.Property<int>("TrainingsAppointmentId")
                        .HasColumnType("int");

                    b.Property<int>("TrainingsModuleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("TrainingsAppointmentId", "TrainingsModuleId");

                    b.HasIndex("TrainingsModuleId");

                    b.ToTable("TrainingsAppointmentsTrainingsModules");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsExercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<int>("Repetitions")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("TrainingsExercises");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("TrainingsGroups");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsGroupApplicationUser", b =>
                {
                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TrainingsGroupId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("isTrainer")
                        .HasColumnType("bit");

                    b.HasKey("ApplicationUserId", "TrainingsGroupId");

                    b.HasIndex("TrainingsGroupId");

                    b.ToTable("TrainingsGroupsApplicationUsers");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsModule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("Difficulty")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TrainingsModules");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsModuleTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("TrainingsModuleTags");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsModuleTrainingsExercise", b =>
                {
                    b.Property<int>("TrainingsModuleId")
                        .HasColumnType("int");

                    b.Property<int>("TrainingsExerciesId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("TrainingsModuleId", "TrainingsExerciesId");

                    b.HasIndex("TrainingsExerciesId");

                    b.ToTable("TrainingsModulesTrainingsExercises");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsModuleTrainingsModuleTag", b =>
                {
                    b.Property<int>("TrainingsModuleId")
                        .HasColumnType("int");

                    b.Property<int>("TrainingsModuleTagId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int?>("TrainingsModuleTagDtoId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("TrainingsModuleId", "TrainingsModuleTagId");

                    b.HasIndex("TrainingsModuleTagDtoId");

                    b.HasIndex("TrainingsModuleTagId");

                    b.ToTable("TrainingsModulesTrainingsModuleTags");
                });

            modelBuilder.Entity("TrainingsPlanner.ViewModels.TrainingsModuleTagDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TrainingsModuleTagDto");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsAppointment", b =>
                {
                    b.HasOne("TrainingsPlanner.Infrastructure.Models.TrainingsGroup", "TrainingsGroup")
                        .WithMany("TrainingsAppointments")
                        .HasForeignKey("TrainingsGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrainingsGroup");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsAppointmentTrainingsModule", b =>
                {
                    b.HasOne("TrainingsPlanner.Infrastructure.Models.TrainingsAppointment", "TrainingsAppointment")
                        .WithMany("TrainingsAppointmentsTrainingsModules")
                        .HasForeignKey("TrainingsAppointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainingsPlanner.Infrastructure.Models.TrainingsModule", "TrainingsModule")
                        .WithMany("TrainingsAppointmentsTrainingsModules")
                        .HasForeignKey("TrainingsModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrainingsAppointment");

                    b.Navigation("TrainingsModule");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsGroupApplicationUser", b =>
                {
                    b.HasOne("TrainingsPlanner.Infrastructure.Models.TrainingsGroup", "TrainingsGroup")
                        .WithMany("TrainingsGroupsApplicationUsers")
                        .HasForeignKey("TrainingsGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrainingsGroup");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsModuleTrainingsExercise", b =>
                {
                    b.HasOne("TrainingsPlanner.Infrastructure.Models.TrainingsExercise", "TrainingsExercise")
                        .WithMany("TrainingsModulesTrainingsExercises")
                        .HasForeignKey("TrainingsExerciesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainingsPlanner.Infrastructure.Models.TrainingsModule", "TrainingsModule")
                        .WithMany("TrainingsModulesTrainingsExercises")
                        .HasForeignKey("TrainingsModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrainingsExercise");

                    b.Navigation("TrainingsModule");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsModuleTrainingsModuleTag", b =>
                {
                    b.HasOne("TrainingsPlanner.Infrastructure.Models.TrainingsModule", "TrainingsModule")
                        .WithMany("TrainingsModulesTrainingsModuleTags")
                        .HasForeignKey("TrainingsModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TrainingsPlanner.ViewModels.TrainingsModuleTagDto", null)
                        .WithMany("TrainingsModulesTrainingsModuleTags")
                        .HasForeignKey("TrainingsModuleTagDtoId");

                    b.HasOne("TrainingsPlanner.Infrastructure.Models.TrainingsModuleTag", "TrainingsModuleTag")
                        .WithMany("TrainingsModulesTrainingsModuleTags")
                        .HasForeignKey("TrainingsModuleTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TrainingsModule");

                    b.Navigation("TrainingsModuleTag");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsAppointment", b =>
                {
                    b.Navigation("TrainingsAppointmentsTrainingsModules");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsExercise", b =>
                {
                    b.Navigation("TrainingsModulesTrainingsExercises");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsGroup", b =>
                {
                    b.Navigation("TrainingsAppointments");

                    b.Navigation("TrainingsGroupsApplicationUsers");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsModule", b =>
                {
                    b.Navigation("TrainingsAppointmentsTrainingsModules");

                    b.Navigation("TrainingsModulesTrainingsExercises");

                    b.Navigation("TrainingsModulesTrainingsModuleTags");
                });

            modelBuilder.Entity("TrainingsPlanner.Infrastructure.Models.TrainingsModuleTag", b =>
                {
                    b.Navigation("TrainingsModulesTrainingsModuleTags");
                });

            modelBuilder.Entity("TrainingsPlanner.ViewModels.TrainingsModuleTagDto", b =>
                {
                    b.Navigation("TrainingsModulesTrainingsModuleTags");
                });
#pragma warning restore 612, 618
        }
    }
}
using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingsPlanner.Infrastructure.Migrations.Training
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingsExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Repetitions = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingsExercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingsGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingsGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingsModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingsModules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingsModuleTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingsModuleTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingsAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrainingsGroupId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingsAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingsAppointments_TrainingsGroups_TrainingsGroupId",
                        column: x => x.TrainingsGroupId,
                        principalTable: "TrainingsGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingsGroupsApplicationUsers",
                columns: table => new
                {
                    TrainingsGroupId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingsGroupsApplicationUsers", x => new { x.ApplicationUserId, x.TrainingsGroupId });
                    table.ForeignKey(
                        name: "FK_TrainingsGroupsApplicationUsers_TrainingsGroups_TrainingsGroupId",
                        column: x => x.TrainingsGroupId,
                        principalTable: "TrainingsGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingsModulesTrainingsExercises",
                columns: table => new
                {
                    TrainingsModuleId = table.Column<int>(type: "int", nullable: false),
                    TrainingsExerciesId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingsModulesTrainingsExercises", x => new { x.TrainingsModuleId, x.TrainingsExerciesId });
                    table.ForeignKey(
                        name: "FK_TrainingsModulesTrainingsExercises_TrainingsExercises_TrainingsExerciesId",
                        column: x => x.TrainingsExerciesId,
                        principalTable: "TrainingsExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingsModulesTrainingsExercises_TrainingsModules_TrainingsModuleId",
                        column: x => x.TrainingsModuleId,
                        principalTable: "TrainingsModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingsModulesTrainingsModuleTags",
                columns: table => new
                {
                    TrainingsModuleId = table.Column<int>(type: "int", nullable: false),
                    TrainingsModuleTagId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingsModulesTrainingsModuleTags", x => new { x.TrainingsModuleId, x.TrainingsModuleTagId });
                    table.ForeignKey(
                        name: "FK_TrainingsModulesTrainingsModuleTags_TrainingsModules_TrainingsModuleId",
                        column: x => x.TrainingsModuleId,
                        principalTable: "TrainingsModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingsModulesTrainingsModuleTags_TrainingsModuleTags_TrainingsModuleTagId",
                        column: x => x.TrainingsModuleTagId,
                        principalTable: "TrainingsModuleTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingsAppointmentsTrainingsModules",
                columns: table => new
                {
                    TrainingsModuleId = table.Column<int>(type: "int", nullable: false),
                    TrainingsAppointmentId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingsAppointmentsTrainingsModules", x => new { x.TrainingsAppointmentId, x.TrainingsModuleId });
                    table.ForeignKey(
                        name: "FK_TrainingsAppointmentsTrainingsModules_TrainingsAppointments_TrainingsAppointmentId",
                        column: x => x.TrainingsAppointmentId,
                        principalTable: "TrainingsAppointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingsAppointmentsTrainingsModules_TrainingsModules_TrainingsModuleId",
                        column: x => x.TrainingsModuleId,
                        principalTable: "TrainingsModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingsAppointments_TrainingsGroupId",
                table: "TrainingsAppointments",
                column: "TrainingsGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingsAppointmentsTrainingsModules_TrainingsModuleId",
                table: "TrainingsAppointmentsTrainingsModules",
                column: "TrainingsModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingsGroupsApplicationUsers_TrainingsGroupId",
                table: "TrainingsGroupsApplicationUsers",
                column: "TrainingsGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingsModulesTrainingsExercises_TrainingsExerciesId",
                table: "TrainingsModulesTrainingsExercises",
                column: "TrainingsExerciesId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingsModulesTrainingsModuleTags_TrainingsModuleTagId",
                table: "TrainingsModulesTrainingsModuleTags",
                column: "TrainingsModuleTagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingsAppointmentsTrainingsModules");

            migrationBuilder.DropTable(
                name: "TrainingsGroupsApplicationUsers");

            migrationBuilder.DropTable(
                name: "TrainingsModulesTrainingsExercises");

            migrationBuilder.DropTable(
                name: "TrainingsModulesTrainingsModuleTags");

            migrationBuilder.DropTable(
                name: "TrainingsAppointments");

            migrationBuilder.DropTable(
                name: "TrainingsExercises");

            migrationBuilder.DropTable(
                name: "TrainingsModules");

            migrationBuilder.DropTable(
                name: "TrainingsModuleTags");

            migrationBuilder.DropTable(
                name: "TrainingsGroups");
        }
    }
}

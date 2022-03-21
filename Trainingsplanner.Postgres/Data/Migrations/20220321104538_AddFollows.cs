using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trainingsplanner.Postgres.Data.Migrations
{
    public partial class AddFollows : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "TrainingsModules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TrainingsModuleFollows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingsModuleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updatet = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingsModuleFollows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingsModuleFollows_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingsModuleFollows_TrainingsModules_TrainingsModuleId",
                        column: x => x.TrainingsModuleId,
                        principalTable: "TrainingsModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingsModuleFollows_TrainingsModuleId",
                table: "TrainingsModuleFollows",
                column: "TrainingsModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingsModuleFollows_UserId",
                table: "TrainingsModuleFollows",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingsModuleFollows");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "TrainingsModules");
        }
    }
}

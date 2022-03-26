using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trainingsplanner.Postgres.Data.Migrations
{
    public partial class ChangeKeyFollow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingsModuleFollows_AspNetUsers_UserId",
                table: "TrainingsModuleFollows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingsModuleFollows",
                table: "TrainingsModuleFollows");

            migrationBuilder.DropIndex(
                name: "IX_TrainingsModuleFollows_UserId",
                table: "TrainingsModuleFollows");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TrainingsModuleFollows");

            migrationBuilder.DropColumn(
                name: "Updatet",
                table: "TrainingsModuleFollows");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TrainingsModules",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TrainingsModuleFollows",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingsModuleFollows",
                table: "TrainingsModuleFollows",
                columns: new[] { "UserId", "TrainingsModuleId" });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingsModules_UserId",
                table: "TrainingsModules",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingsModuleFollows_AspNetUsers_UserId",
                table: "TrainingsModuleFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingsModules_AspNetUsers_UserId",
                table: "TrainingsModules");

            migrationBuilder.DropIndex(
                name: "IX_TrainingsModules_UserId",
                table: "TrainingsModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainingsModuleFollows",
                table: "TrainingsModuleFollows");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TrainingsModules",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TrainingsModuleFollows",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TrainingsModuleFollows",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updatet",
                table: "TrainingsModuleFollows",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainingsModuleFollows",
                table: "TrainingsModuleFollows",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingsModuleFollows_UserId",
                table: "TrainingsModuleFollows",
                column: "UserId");
        }
    }
}

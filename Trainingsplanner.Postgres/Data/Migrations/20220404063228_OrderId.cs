using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trainingsplanner.Postgres.Data.Migrations
{
    public partial class OrderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "TrainingsAppointmentsTrainingsModules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "TrainingsAppointmentsTrainingsModules");
        }
    }
}

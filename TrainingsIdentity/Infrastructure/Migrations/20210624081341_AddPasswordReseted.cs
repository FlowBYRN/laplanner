using Microsoft.EntityFrameworkCore.Migrations;

namespace StsServerIdentity.Infrastructure.Migrations
{
    public partial class AddPasswordReseted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PasswordReseted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordReseted",
                table: "AspNetUsers");
        }
    }
}

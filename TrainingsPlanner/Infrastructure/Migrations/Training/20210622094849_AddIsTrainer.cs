using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainingsPlanner.Infrastructure.Migrations.Training
{
    public partial class AddIsTrainer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainingsModuleTagDtoId",
                table: "TrainingsModulesTrainingsModuleTags",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isTrainer",
                table: "TrainingsGroupsApplicationUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TrainingsModuleTagDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingsModuleTagDto", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingsModulesTrainingsModuleTags_TrainingsModuleTagDtoId",
                table: "TrainingsModulesTrainingsModuleTags",
                column: "TrainingsModuleTagDtoId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingsModulesTrainingsModuleTags_TrainingsModuleTagDto_TrainingsModuleTagDtoId",
                table: "TrainingsModulesTrainingsModuleTags",
                column: "TrainingsModuleTagDtoId",
                principalTable: "TrainingsModuleTagDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingsModulesTrainingsModuleTags_TrainingsModuleTagDto_TrainingsModuleTagDtoId",
                table: "TrainingsModulesTrainingsModuleTags");

            migrationBuilder.DropTable(
                name: "TrainingsModuleTagDto");

            migrationBuilder.DropIndex(
                name: "IX_TrainingsModulesTrainingsModuleTags_TrainingsModuleTagDtoId",
                table: "TrainingsModulesTrainingsModuleTags");

            migrationBuilder.DropColumn(
                name: "TrainingsModuleTagDtoId",
                table: "TrainingsModulesTrainingsModuleTags");

            migrationBuilder.DropColumn(
                name: "isTrainer",
                table: "TrainingsGroupsApplicationUsers");
        }
    }
}

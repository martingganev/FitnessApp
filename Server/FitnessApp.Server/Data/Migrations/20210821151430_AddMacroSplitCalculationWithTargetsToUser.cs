using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessApp.Server.Data.Migrations
{
    public partial class AddMacroSplitCalculationWithTargetsToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TargetCarbs",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TargetFats",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TargetProteins",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetCarbs",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TargetFats",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TargetProteins",
                table: "AspNetUsers");
        }
    }
}

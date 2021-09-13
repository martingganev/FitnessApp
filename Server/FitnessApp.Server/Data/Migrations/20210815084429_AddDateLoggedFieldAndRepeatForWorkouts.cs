using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessApp.Server.Data.Migrations
{
    public partial class AddDateLoggedFieldAndRepeatForWorkouts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersRecipes",
                table: "UsersRecipes");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLogged",
                table: "UsersRecipes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersRecipes",
                table: "UsersRecipes",
                columns: new[] { "UserId", "RecipeId", "DateLogged" });

            migrationBuilder.CreateTable(
                name: "UsersWorkouts",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WorkoutId = table.Column<int>(type: "int", nullable: false),
                    DateLogged = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersWorkouts", x => new { x.UserId, x.WorkoutId, x.DateLogged });
                    table.ForeignKey(
                        name: "FK_UsersWorkouts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersWorkouts_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersWorkouts_WorkoutId",
                table: "UsersWorkouts",
                column: "WorkoutId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersWorkouts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersRecipes",
                table: "UsersRecipes");

            migrationBuilder.DropColumn(
                name: "DateLogged",
                table: "UsersRecipes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersRecipes",
                table: "UsersRecipes",
                columns: new[] { "UserId", "RecipeId" });
        }
    }
}

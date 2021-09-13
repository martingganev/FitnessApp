using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessApp.Server.data.Migrations
{
    public partial class MakeLoggedRecipeAndWorkoutDeleatable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "UsersWorkouts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "UsersWorkouts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "UsersWorkouts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UsersWorkouts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UsersWorkouts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "UsersWorkouts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "UsersWorkouts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "UsersRecipes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "UsersRecipes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "UsersRecipes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UsersRecipes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UsersRecipes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "UsersRecipes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "UsersRecipes",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UsersWorkouts");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UsersWorkouts");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "UsersWorkouts");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UsersWorkouts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UsersWorkouts");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "UsersWorkouts");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "UsersWorkouts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UsersRecipes");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UsersRecipes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "UsersRecipes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UsersRecipes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UsersRecipes");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "UsersRecipes");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "UsersRecipes");
        }
    }
}

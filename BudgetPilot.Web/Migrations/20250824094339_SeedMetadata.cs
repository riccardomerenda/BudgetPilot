using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPilot.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "currency",
                schema: "core",
                table: "families",
                type: "text",
                nullable: false,
                defaultValue: "EUR");

            migrationBuilder.AddColumn<string>(
                name: "culture",
                schema: "core",
                table: "families",
                type: "text",
                nullable: false,
                defaultValue: "it-IT");

            migrationBuilder.AddColumn<string>(
                name: "color",
                schema: "core",
                table: "categories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "icon",
                schema: "core",
                table: "categories",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currency",
                schema: "core",
                table: "families");

            migrationBuilder.DropColumn(
                name: "culture",
                schema: "core",
                table: "families");

            migrationBuilder.DropColumn(
                name: "color",
                schema: "core",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "icon",
                schema: "core",
                table: "categories");
        }
    }
}

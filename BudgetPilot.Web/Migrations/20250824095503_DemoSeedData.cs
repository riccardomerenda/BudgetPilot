using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPilot.Web.Migrations
{
    /// <inheritdoc />
    public partial class DemoSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var now = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            migrationBuilder.InsertData(
                schema: "core",
                table: "families",
                columns: new[] { "id", "name", "created_at", "created_by", "updated_at", "updated_by", "currency", "culture" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "Demo Family", now, null, null, null, "EUR", "it-IT" }
            );

            migrationBuilder.InsertData(
                schema: "core",
                table: "categories",
                columns: new[] { "id", "name", "parent_id", "created_at", "created_by", "updated_at", "updated_by", "family_id", "color", "icon" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Essenziali (50%)", null, now, null, null, null, new Guid("11111111-1111-1111-1111-111111111111"), "#1f77b4", "home" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Desideri (30%)", null, now, null, null, null, new Guid("11111111-1111-1111-1111-111111111111"), "#ff7f0e", "gift" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "Risparmi (20%)", null, now, null, null, null, new Guid("11111111-1111-1111-1111-111111111111"), "#2ca02c", "piggy-bank" }
                }
            );

            migrationBuilder.InsertData(
                schema: "core",
                table: "accounts",
                columns: new[] { "id", "name", "created_at", "created_by", "updated_at", "updated_by", "family_id" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Conto Principale", now, null, null, null, new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Carta", now, null, null, null, new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Contanti", now, null, null, null, new Guid("11111111-1111-1111-1111-111111111111") }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "core",
                table: "accounts",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222")
            );
            migrationBuilder.DeleteData(
                schema: "core",
                table: "accounts",
                keyColumn: "id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333")
            );
            migrationBuilder.DeleteData(
                schema: "core",
                table: "accounts",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444")
            );

            migrationBuilder.DeleteData(
                schema: "core",
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555")
            );
            migrationBuilder.DeleteData(
                schema: "core",
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666")
            );
            migrationBuilder.DeleteData(
                schema: "core",
                table: "categories",
                keyColumn: "id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777")
            );

            migrationBuilder.DeleteData(
                schema: "core",
                table: "families",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111")
            );
        }
    }
}

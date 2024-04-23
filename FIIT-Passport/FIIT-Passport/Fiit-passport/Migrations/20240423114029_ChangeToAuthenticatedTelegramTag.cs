using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiit_passport.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToAuthenticatedTelegramTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_authenticated_user",
                table: "passports");

            migrationBuilder.AddColumn<string>(
                name: "authenticated_telegram_tag",
                table: "passports",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "authenticated_telegram_tag",
                table: "passports");

            migrationBuilder.AddColumn<bool>(
                name: "is_authenticated_user",
                table: "passports",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

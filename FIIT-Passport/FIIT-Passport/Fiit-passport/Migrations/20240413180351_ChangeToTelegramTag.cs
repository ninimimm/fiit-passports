using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiit_passport.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToTelegramTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "admin_telegram_tag",
                table: "passports",
                newName: "telegram_tag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "telegram_tag",
                table: "passports",
                newName: "admin_telegram_tag");
        }
    }
}

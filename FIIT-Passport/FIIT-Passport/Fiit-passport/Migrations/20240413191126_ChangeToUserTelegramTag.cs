using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiit_passport.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToUserTelegramTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "admin_telegram_tag",
                table: "authenticated_users",
                newName: "user_telegram_tag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "user_telegram_tag",
                table: "authenticated_users",
                newName: "admin_telegram_tag");
        }
    }
}

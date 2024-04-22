using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiit_passport.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthenticatedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authenticated_users",
                columns: table => new
                {
                    admin_telegram_tag = table.Column<string>(type: "character varying(33)", maxLength: 33, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authenticated_users", x => x.admin_telegram_tag);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "authenticated_users");
        }
    }
}

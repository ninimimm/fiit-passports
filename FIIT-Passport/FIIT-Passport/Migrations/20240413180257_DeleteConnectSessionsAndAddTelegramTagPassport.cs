using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiit_passport.Migrations
{
    /// <inheritdoc />
    public partial class DeleteConnectSessionsAndAddTelegramTagPassport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "connect_sessions");

            migrationBuilder.AddColumn<string>(
                name: "admin_telegram_tag",
                table: "passports",
                type: "character varying(33)",
                maxLength: 33,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "admin_telegram_tag",
                table: "passports");

            migrationBuilder.CreateTable(
                name: "connect_sessions",
                columns: table => new
                {
                    telegram_tag = table.Column<string>(type: "character varying(33)", maxLength: 33, nullable: false),
                    session_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_connect_sessions", x => new { x.telegram_tag, x.session_id });
                    table.ForeignKey(
                        name: "FK_connect_sessions_passports_session_id",
                        column: x => x.session_id,
                        principalTable: "passports",
                        principalColumn: "session_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_connect_sessions_session_id",
                table: "connect_sessions",
                column: "session_id",
                unique: true);
        }
    }
}

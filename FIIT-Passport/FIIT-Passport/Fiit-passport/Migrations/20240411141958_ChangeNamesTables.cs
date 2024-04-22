using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiit_passport.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNamesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_connects_passports_session_id",
                table: "connects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_connects",
                table: "connects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConnectIds",
                table: "ConnectIds");

            migrationBuilder.RenameTable(
                name: "connects",
                newName: "connect_sessions");

            migrationBuilder.RenameTable(
                name: "ConnectIds",
                newName: "connect_ids");

            migrationBuilder.RenameIndex(
                name: "IX_connects_session_id",
                table: "connect_sessions",
                newName: "IX_connect_sessions_session_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_connect_sessions",
                table: "connect_sessions",
                columns: new[] { "telegram_tag", "session_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_connect_ids",
                table: "connect_ids",
                column: "user_telegram_tag");

            migrationBuilder.AddForeignKey(
                name: "FK_connect_sessions_passports_session_id",
                table: "connect_sessions",
                column: "session_id",
                principalTable: "passports",
                principalColumn: "session_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_connect_sessions_passports_session_id",
                table: "connect_sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_connect_sessions",
                table: "connect_sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_connect_ids",
                table: "connect_ids");

            migrationBuilder.RenameTable(
                name: "connect_sessions",
                newName: "connects");

            migrationBuilder.RenameTable(
                name: "connect_ids",
                newName: "ConnectIds");

            migrationBuilder.RenameIndex(
                name: "IX_connect_sessions_session_id",
                table: "connects",
                newName: "IX_connects_session_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_connects",
                table: "connects",
                columns: new[] { "telegram_tag", "session_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConnectIds",
                table: "ConnectIds",
                column: "user_telegram_tag");

            migrationBuilder.AddForeignKey(
                name: "FK_connects_passports_session_id",
                table: "connects",
                column: "session_id",
                principalTable: "passports",
                principalColumn: "session_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

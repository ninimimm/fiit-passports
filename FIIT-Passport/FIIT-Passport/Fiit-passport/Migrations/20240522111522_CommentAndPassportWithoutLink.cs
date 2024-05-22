using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiit_passport.Migrations
{
    /// <inheritdoc />
    public partial class CommentAndPassportWithoutLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_passports_PassportSessionId",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_PassportSessionId",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "PassportSessionId",
                table: "comments");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "comments",
                newName: "session_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "session_id",
                table: "comments",
                newName: "SessionId");

            migrationBuilder.AddColumn<string>(
                name: "PassportSessionId",
                table: "comments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_comments_PassportSessionId",
                table: "comments",
                column: "PassportSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_passports_PassportSessionId",
                table: "comments",
                column: "PassportSessionId",
                principalTable: "passports",
                principalColumn: "session_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

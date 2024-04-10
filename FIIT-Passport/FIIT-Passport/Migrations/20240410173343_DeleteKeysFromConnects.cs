using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiit_passport.Migrations
{
    /// <inheritdoc />
    public partial class DeleteKeysFromConnects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_connects",
                table: "connects");

            migrationBuilder.AddPrimaryKey(
                name: "PK_connects",
                table: "connects",
                columns: new[] { "telegram_tag", "session_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_connects",
                table: "connects");

            migrationBuilder.AddPrimaryKey(
                name: "PK_connects",
                table: "connects",
                column: "telegram_tag");
        }
    }
}

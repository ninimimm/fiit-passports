using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiit_passport.Migrations
{
    /// <inheritdoc />
    public partial class AddAttributeAndCreateTablePassports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "adminlink",
                table: "admins",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "telegramtag",
                table: "admins",
                type: "character varying(33)",
                maxLength: 33,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(33)",
                oldMaxLength: 33);

            migrationBuilder.CreateTable(
                name: "passports",
                columns: table => new
                {
                    SessionId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    OrdererName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProjectName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ProjectDescription = table.Column<string>(type: "text", nullable: false),
                    Goal = table.Column<string>(type: "text", nullable: false),
                    Result = table.Column<string>(type: "text", nullable: false),
                    AcceptanceCriteria = table.Column<string>(type: "text", nullable: false),
                    CopiesNumber = table.Column<int>(type: "integer", nullable: false),
                    MeetingLocation = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passports", x => x.SessionId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "passports");

            migrationBuilder.AlterColumn<string>(
                name: "adminlink",
                table: "admins",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "telegramtag",
                table: "admins",
                type: "nvarchar(33)",
                maxLength: 33,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(33)",
                oldMaxLength: 33);
        }
    }
}

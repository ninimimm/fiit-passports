using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiit_passport.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableConnectionsAndChangeNamesColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "passports",
                newName: "surname");

            migrationBuilder.RenameColumn(
                name: "Result",
                table: "passports",
                newName: "result");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "passports",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Goal",
                table: "passports",
                newName: "goal");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "passports",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "ProjectName",
                table: "passports",
                newName: "project_name");

            migrationBuilder.RenameColumn(
                name: "ProjectDescription",
                table: "passports",
                newName: "project_description");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "passports",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "OrdererName",
                table: "passports",
                newName: "orderer_name");

            migrationBuilder.RenameColumn(
                name: "MeetingLocation",
                table: "passports",
                newName: "meeting_location");

            migrationBuilder.RenameColumn(
                name: "CopiesNumber",
                table: "passports",
                newName: "copies_number");

            migrationBuilder.RenameColumn(
                name: "AcceptanceCriteria",
                table: "passports",
                newName: "error_message");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "passports",
                newName: "session_id");

            migrationBuilder.RenameColumn(
                name: "adminlink",
                table: "admins",
                newName: "admin_link");

            migrationBuilder.RenameColumn(
                name: "telegramtag",
                table: "admins",
                newName: "admin_telegram_tag");

            migrationBuilder.CreateTable(
                name: "connects",
                columns: table => new
                {
                    telegram_tag = table.Column<string>(type: "character varying(33)", maxLength: 33, nullable: false),
                    session_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_connects", x => x.telegram_tag);
                    table.ForeignKey(
                        name: "FK_connects_passports_session_id",
                        column: x => x.session_id,
                        principalTable: "passports",
                        principalColumn: "session_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_connects_session_id",
                table: "connects",
                column: "session_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "connects");

            migrationBuilder.RenameColumn(
                name: "surname",
                table: "passports",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "result",
                table: "passports",
                newName: "Result");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "passports",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "goal",
                table: "passports",
                newName: "Goal");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "passports",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "project_name",
                table: "passports",
                newName: "ProjectName");

            migrationBuilder.RenameColumn(
                name: "project_description",
                table: "passports",
                newName: "ProjectDescription");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "passports",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "orderer_name",
                table: "passports",
                newName: "OrdererName");

            migrationBuilder.RenameColumn(
                name: "meeting_location",
                table: "passports",
                newName: "MeetingLocation");

            migrationBuilder.RenameColumn(
                name: "error_message",
                table: "passports",
                newName: "AcceptanceCriteria");

            migrationBuilder.RenameColumn(
                name: "copies_number",
                table: "passports",
                newName: "CopiesNumber");

            migrationBuilder.RenameColumn(
                name: "session_id",
                table: "passports",
                newName: "SessionId");

            migrationBuilder.RenameColumn(
                name: "admin_link",
                table: "admins",
                newName: "adminlink");

            migrationBuilder.RenameColumn(
                name: "admin_telegram_tag",
                table: "admins",
                newName: "telegramtag");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiit_passport.Migrations
{
    /// <inheritdoc />
    public partial class TelegramTagCanBeNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "telegram_tag",
                table: "passports",
                type: "character varying(33)",
                maxLength: 33,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(33)",
                oldMaxLength: 33);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "telegram_tag",
                table: "passports",
                type: "character varying(33)",
                maxLength: 33,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(33)",
                oldMaxLength: 33,
                oldNullable: true);
        }
    }
}

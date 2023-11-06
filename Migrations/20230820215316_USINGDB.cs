using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backoffice_ANCFCC.Migrations
{
    /// <inheritdoc />
    public partial class USINGDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Agent",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Agent",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Administrateur",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Administrateur",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Agent");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Agent");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Administrateur");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Administrateur");
        }
    }
}

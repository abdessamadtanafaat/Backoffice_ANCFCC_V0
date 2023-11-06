using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backoffice_ANCFCC.Migrations
{
    /// <inheritdoc />
    public partial class USINGDB1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Agent",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Administrateur",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Agent");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Administrateur");
        }
    }
}

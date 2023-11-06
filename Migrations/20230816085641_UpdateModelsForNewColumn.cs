using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backoffice_ANCFCC.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelsForNewColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Diplomes");

            migrationBuilder.DropColumn(
                name: "recapUser",
                table: "Candidatures");

            migrationBuilder.AlterColumn<int>(
                name: "statusGlobal",
                table: "Candidatures",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatutInfoPerso",
                table: "Candidatures",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatutFormation",
                table: "Candidatures",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatutExp",
                table: "Candidatures",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatutDoc",
                table: "Candidatures",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatusAgent",
                table: "Candidatures",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldUnicode: false,
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "Candidatures",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Candidatures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Verrouille",
                table: "Candidatures",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Candidatures");

            migrationBuilder.DropColumn(
                name: "Verrouille",
                table: "Candidatures");

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Diplomes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "statusGlobal",
                table: "Candidatures",
                type: "varchar(1000)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StatutInfoPerso",
                table: "Candidatures",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StatutFormation",
                table: "Candidatures",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StatutExp",
                table: "Candidatures",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StatutDoc",
                table: "Candidatures",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StatusAgent",
                table: "Candidatures",
                type: "varchar(1000)",
                unicode: false,
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                table: "Candidatures",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "recapUser",
                table: "Candidatures",
                type: "varchar(1000)",
                unicode: false,
                maxLength: 1000,
                nullable: true);
        }
    }
}

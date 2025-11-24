using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mobix.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddStorageField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayHz",
                table: "Smartphones",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplaySize",
                table: "Smartphones",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ram",
                table: "Smartphones",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Storage",
                table: "Smartphones",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayHz",
                table: "Smartphones");

            migrationBuilder.DropColumn(
                name: "DisplaySize",
                table: "Smartphones");

            migrationBuilder.DropColumn(
                name: "Ram",
                table: "Smartphones");

            migrationBuilder.DropColumn(
                name: "Storage",
                table: "Smartphones");
        }
    }
}

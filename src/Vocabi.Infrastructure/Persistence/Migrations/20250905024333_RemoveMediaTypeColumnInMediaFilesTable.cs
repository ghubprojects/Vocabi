using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vocabi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMediaTypeColumnInMediaFilesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "MediaFiles");

            migrationBuilder.DropColumn(
                name: "SourceCategory",
                table: "MediaFiles");

            migrationBuilder.RenameColumn(
                name: "SourceName",
                table: "MediaFiles",
                newName: "Provider");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Provider",
                table: "MediaFiles",
                newName: "SourceName");

            migrationBuilder.AddColumn<string>(
                name: "MediaType",
                table: "MediaFiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceCategory",
                table: "MediaFiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

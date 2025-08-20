using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vocabi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExportedToAnkiAtColumnInVocabulariesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSyncedToAnki",
                table: "Vocabularies");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExportedToAnkiAt",
                table: "Vocabularies",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExportedToAnkiAt",
                table: "Vocabularies");

            migrationBuilder.AddColumn<bool>(
                name: "IsSyncedToAnki",
                table: "Vocabularies",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

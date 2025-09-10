using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vocabi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPronunciationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pronunciations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Word = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Ipa = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pronunciations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pronunciations_Word",
                table: "Pronunciations",
                column: "Word",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pronunciations");
        }
    }
}

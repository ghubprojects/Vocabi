using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DictionaryService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitDictionary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Dictionary");

            migrationBuilder.CreateTable(
                name: "DictionaryEntry",
                schema: "Dictionary",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    headword = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    part_of_speech = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    pronunciation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    source = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dictionary_entry", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryDefinition",
                schema: "Dictionary",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    order_index = table.Column<int>(type: "integer", nullable: false),
                    dictionary_entry_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dictionary_definition", x => x.id);
                    table.ForeignKey(
                        name: "fk_dictionary_definition_dictionary_entries_dictionary_entry_id",
                        column: x => x.dictionary_entry_id,
                        principalSchema: "Dictionary",
                        principalTable: "DictionaryEntry",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryExample",
                schema: "Dictionary",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    dictionary_definition_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dictionary_example", x => x.id);
                    table.ForeignKey(
                        name: "fk_dictionary_example_dictionary_definition_dictionary_definitio",
                        column: x => x.dictionary_definition_id,
                        principalSchema: "Dictionary",
                        principalTable: "DictionaryDefinition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_dictionary_definition_dictionary_entry_id",
                schema: "Dictionary",
                table: "DictionaryDefinition",
                column: "dictionary_entry_id");

            migrationBuilder.CreateIndex(
                name: "ix_dictionary_entry_headword_part_of_speech",
                schema: "Dictionary",
                table: "DictionaryEntry",
                columns: new[] { "headword", "part_of_speech" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_dictionary_example_dictionary_definition_id",
                schema: "Dictionary",
                table: "DictionaryExample",
                column: "dictionary_definition_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DictionaryExample",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "DictionaryDefinition",
                schema: "Dictionary");

            migrationBuilder.DropTable(
                name: "DictionaryEntry",
                schema: "Dictionary");
        }
    }
}

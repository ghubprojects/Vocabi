using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VocabularyService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitVocabulary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Vocabulary");

            migrationBuilder.CreateTable(
                name: "Vocabulary",
                schema: "Vocabulary",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    headword = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    part_of_speech = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    pronunciation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cloze = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    definition = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    translation = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    audit_created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    audit_created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    audit_last_modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    audit_last_modified_by = table.Column<Guid>(type: "uuid", nullable: true),
                    soft_delete_is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    soft_delete_deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    soft_delete_deleted_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vocabulary", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "VocabularyExample",
                schema: "Vocabulary",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    audit_created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    audit_created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    audit_last_modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    audit_last_modified_by = table.Column<Guid>(type: "uuid", nullable: true),
                    soft_delete_is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    soft_delete_deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    soft_delete_deleted_by = table.Column<Guid>(type: "uuid", nullable: true),
                    vocabulary_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vocabulary_example", x => x.id);
                    table.ForeignKey(
                        name: "fk_vocabulary_example_vocabulary_vocabulary_id",
                        column: x => x.vocabulary_id,
                        principalSchema: "Vocabulary",
                        principalTable: "Vocabulary",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_vocabulary_example_vocabulary_id",
                schema: "Vocabulary",
                table: "VocabularyExample",
                column: "vocabulary_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VocabularyExample",
                schema: "Vocabulary");

            migrationBuilder.DropTable(
                name: "Vocabulary",
                schema: "Vocabulary");
        }
    }
}

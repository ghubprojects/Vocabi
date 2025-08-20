using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vocabi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FlattenVocabularyFlashcardsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VocabularyFlashcards");

            migrationBuilder.AddColumn<DateTime>(
                name: "Flashcard_CreatedAt",
                table: "Vocabularies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Flashcard_Id",
                table: "Vocabularies",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Flashcard_NoteId",
                table: "Vocabularies",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Flashcard_Status",
                table: "Vocabularies",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flashcard_CreatedAt",
                table: "Vocabularies");

            migrationBuilder.DropColumn(
                name: "Flashcard_Id",
                table: "Vocabularies");

            migrationBuilder.DropColumn(
                name: "Flashcard_NoteId",
                table: "Vocabularies");

            migrationBuilder.DropColumn(
                name: "Flashcard_Status",
                table: "Vocabularies");

            migrationBuilder.CreateTable(
                name: "VocabularyFlashcards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VocabularyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NoteId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocabularyFlashcards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VocabularyFlashcards_Vocabularies_VocabularyId",
                        column: x => x.VocabularyId,
                        principalTable: "Vocabularies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VocabularyFlashcards_VocabularyId",
                table: "VocabularyFlashcards",
                column: "VocabularyId",
                unique: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vocabi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LookupEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Headword = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PartOfSpeech = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Pronunciation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    ContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MediaType = table.Column<int>(type: "integer", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    SourceCategory = table.Column<int>(type: "integer", nullable: false),
                    SourceName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vocabularies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Word = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PartOfSpeech = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Pronunciation = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Cloze = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Definition = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Example = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Meaning = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsSyncedToAnki = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vocabularies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LookupEntryDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LookupEntryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupEntryDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LookupEntryDefinitions_LookupEntries_LookupEntryId",
                        column: x => x.LookupEntryId,
                        principalTable: "LookupEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LookupEntryMeanings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LookupEntryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupEntryMeanings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LookupEntryMeanings_LookupEntries_LookupEntryId",
                        column: x => x.LookupEntryId,
                        principalTable: "LookupEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LookupEntryMediaFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LookupEntryId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaFileId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupEntryMediaFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LookupEntryMediaFiles_LookupEntries_LookupEntryId",
                        column: x => x.LookupEntryId,
                        principalTable: "LookupEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LookupEntryMediaFiles_MediaFiles_MediaFileId",
                        column: x => x.MediaFileId,
                        principalTable: "MediaFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VocabularyMediaFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VocabularyId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaFileId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocabularyMediaFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VocabularyMediaFiles_MediaFiles_MediaFileId",
                        column: x => x.MediaFileId,
                        principalTable: "MediaFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VocabularyMediaFiles_Vocabularies_VocabularyId",
                        column: x => x.VocabularyId,
                        principalTable: "Vocabularies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LookupEntryExamples",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LookupEntryDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupEntryExamples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LookupEntryExamples_LookupEntryDefinitions_LookupEntryDefin~",
                        column: x => x.LookupEntryDefinitionId,
                        principalTable: "LookupEntryDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LookupEntryDefinitions_LookupEntryId",
                table: "LookupEntryDefinitions",
                column: "LookupEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_LookupEntryExamples_LookupEntryDefinitionId",
                table: "LookupEntryExamples",
                column: "LookupEntryDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_LookupEntryMeanings_LookupEntryId",
                table: "LookupEntryMeanings",
                column: "LookupEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_LookupEntryMediaFiles_LookupEntryId",
                table: "LookupEntryMediaFiles",
                column: "LookupEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_LookupEntryMediaFiles_MediaFileId",
                table: "LookupEntryMediaFiles",
                column: "MediaFileId");

            migrationBuilder.CreateIndex(
                name: "IX_VocabularyMediaFiles_MediaFileId",
                table: "VocabularyMediaFiles",
                column: "MediaFileId");

            migrationBuilder.CreateIndex(
                name: "IX_VocabularyMediaFiles_VocabularyId",
                table: "VocabularyMediaFiles",
                column: "VocabularyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LookupEntryExamples");

            migrationBuilder.DropTable(
                name: "LookupEntryMeanings");

            migrationBuilder.DropTable(
                name: "LookupEntryMediaFiles");

            migrationBuilder.DropTable(
                name: "VocabularyMediaFiles");

            migrationBuilder.DropTable(
                name: "LookupEntryDefinitions");

            migrationBuilder.DropTable(
                name: "MediaFiles");

            migrationBuilder.DropTable(
                name: "Vocabularies");

            migrationBuilder.DropTable(
                name: "LookupEntries");
        }
    }
}

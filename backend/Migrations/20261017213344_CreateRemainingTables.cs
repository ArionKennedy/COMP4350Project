using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Palette.Api.Migrations
{
    /// <inheritdoc />
    public partial class CreateRemainingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COLLECTIONS",
                columns: table => new
                {
                    CollectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COLLECTIONS", x => x.CollectionId);
                    table.ForeignKey(
                        name: "FK_COLLECTIONS_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MEDIA",
                columns: table => new
                {
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PosterUrl = table.Column<string>(type: "text", nullable: true),
                    MediaType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDIA", x => x.MediaId);
                });

            migrationBuilder.CreateTable(
                name: "BOOKS",
                columns: table => new
                {
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    OpenLibraryId = table.Column<string>(type: "text", nullable: false),
                    ISBN = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOOKS", x => x.MediaId);
                    table.ForeignKey(
                        name: "FK_BOOKS_MEDIA_MediaId",
                        column: x => x.MediaId,
                        principalTable: "MEDIA",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "COLLECTION_ITEMS",
                columns: table => new
                {
                    CollectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_COLLECTION_ITEMS", x => new { x.CollectionId, x.MediaId });
                    table.ForeignKey(
                        name: "FK_COLLECTION_ITEMS_COLLECTIONS_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "COLLECTIONS",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_COLLECTION_ITEMS_MEDIA_MediaId",
                        column: x => x.MediaId,
                        principalTable: "MEDIA",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GAMES",
                columns: table => new
                {
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    RawgGameId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GAMES", x => x.MediaId);
                    table.ForeignKey(
                        name: "FK_GAMES_MEDIA_MediaId",
                        column: x => x.MediaId,
                        principalTable: "MEDIA",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MOVIES",
                columns: table => new
                {
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    TmdbMovieId = table.Column<int>(type: "integer", nullable: false),
                    RuntimeMinutes = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MOVIES", x => x.MediaId);
                    table.ForeignKey(
                        name: "FK_MOVIES_MEDIA_MediaId",
                        column: x => x.MediaId,
                        principalTable: "MEDIA",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "REVIEWS",
                columns: table => new
                {
                    ReviewId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REVIEWS", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_REVIEWS_MEDIA_MediaId",
                        column: x => x.MediaId,
                        principalTable: "MEDIA",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_REVIEWS_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TV_SHOWS",
                columns: table => new
                {
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    TmdbTvId = table.Column<int>(type: "integer", nullable: false),
                    NumberOfSeasons = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TV_SHOWS", x => x.MediaId);
                    table.ForeignKey(
                        name: "FK_TV_SHOWS_MEDIA_MediaId",
                        column: x => x.MediaId,
                        principalTable: "MEDIA",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USER_MEDIA_LOGS",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: true),
                    LoggedDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_MEDIA_LOGS", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_USER_MEDIA_LOGS_MEDIA_MediaId",
                        column: x => x.MediaId,
                        principalTable: "MEDIA",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USER_MEDIA_LOGS_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "USERS",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BOOKS_OpenLibraryId",
                table: "BOOKS",
                column: "OpenLibraryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_COLLECTION_ITEMS_MediaId",
                table: "COLLECTION_ITEMS",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_COLLECTIONS_UserId",
                table: "COLLECTIONS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GAMES_RawgGameId",
                table: "GAMES",
                column: "RawgGameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MOVIES_TmdbMovieId",
                table: "MOVIES",
                column: "TmdbMovieId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_REVIEWS_MediaId",
                table: "REVIEWS",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_REVIEWS_UserId",
                table: "REVIEWS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TV_SHOWS_TmdbTvId",
                table: "TV_SHOWS",
                column: "TmdbTvId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USER_MEDIA_LOGS_MediaId",
                table: "USER_MEDIA_LOGS",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_USER_MEDIA_LOGS_UserId",
                table: "USER_MEDIA_LOGS",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BOOKS");

            migrationBuilder.DropTable(
                name: "COLLECTION_ITEMS");

            migrationBuilder.DropTable(
                name: "GAMES");

            migrationBuilder.DropTable(
                name: "MOVIES");

            migrationBuilder.DropTable(
                name: "REVIEWS");

            migrationBuilder.DropTable(
                name: "TV_SHOWS");

            migrationBuilder.DropTable(
                name: "USER_MEDIA_LOGS");

            migrationBuilder.DropTable(
                name: "COLLECTIONS");

            migrationBuilder.DropTable(
                name: "MEDIA");
        }
    }
}

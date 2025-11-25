using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindfulWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AffirmationEntries_Users_UserId",
                table: "AffirmationEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_MeditationSessions_Users_UserId",
                table: "MeditationSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_MoodEntries_Users_UserId",
                table: "MoodEntries");

            migrationBuilder.DropIndex(
                name: "IX_MoodEntries_UserId",
                table: "MoodEntries");

            migrationBuilder.DropIndex(
                name: "IX_MeditationSessions_UserId",
                table: "MeditationSessions");

            migrationBuilder.DropIndex(
                name: "IX_AffirmationEntries_UserId",
                table: "AffirmationEntries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MoodEntries_UserId",
                table: "MoodEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MeditationSessions_UserId",
                table: "MeditationSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AffirmationEntries_UserId",
                table: "AffirmationEntries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AffirmationEntries_Users_UserId",
                table: "AffirmationEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MeditationSessions_Users_UserId",
                table: "MeditationSessions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MoodEntries_Users_UserId",
                table: "MoodEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

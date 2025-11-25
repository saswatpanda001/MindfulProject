using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindfulWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FullName",
                table: "Users",
                column: "FullName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Phone",
                table: "Users",
                column: "Phone",
                unique: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FullName",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Phone",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_MoodEntries_UserId",
                table: "MoodEntries");

            migrationBuilder.DropIndex(
                name: "IX_MeditationSessions_UserId",
                table: "MeditationSessions");

            migrationBuilder.DropIndex(
                name: "IX_AffirmationEntries_UserId",
                table: "AffirmationEntries");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Users",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}

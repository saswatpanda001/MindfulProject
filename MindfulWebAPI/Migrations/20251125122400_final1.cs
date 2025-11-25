using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindfulWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class final1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Email_MinLength",
                table: "Users",
                sql: "LEN(Email) >= 4");

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_FullName_MinLength",
                table: "Users",
                sql: "LEN(FullName) >= 4");

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Location_MinLength",
                table: "Users",
                sql: "LEN(Location) >= 4 OR Location IS NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Password_MinLength",
                table: "Users",
                sql: "LEN(Password) >= 4");

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Phone_ExactLength",
                table: "Users",
                sql: "LEN(Phone) = 10");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MoodEntry_Mood_MinLength",
                table: "MoodEntries",
                sql: "LEN(Mood) >= 3");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MoodEntry_Note_MinLength",
                table: "MoodEntries",
                sql: "LEN(Note) >= 4");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MeditationSession_DurationMinutes_Range",
                table: "MeditationSessions",
                sql: "DurationMinutes >= 1 AND DurationMinutes <= 600");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AffirmationEntry_Category_MinLength",
                table: "AffirmationEntries",
                sql: "LEN(Category) >= 4");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AffirmationEntry_Text_MinLength",
                table: "AffirmationEntries",
                sql: "LEN(Text) >= 4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Email_MinLength",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_FullName_MinLength",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Location_MinLength",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Password_MinLength",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Phone_ExactLength",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_MoodEntry_Mood_MinLength",
                table: "MoodEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_MoodEntry_Note_MinLength",
                table: "MoodEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_MeditationSession_DurationMinutes_Range",
                table: "MeditationSessions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AffirmationEntry_Category_MinLength",
                table: "AffirmationEntries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_AffirmationEntry_Text_MinLength",
                table: "AffirmationEntries");
        }
    }
}

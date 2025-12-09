using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindfulWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class final212 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Email_Valid",
                table: "Users");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Email_Valid",
                table: "Users",
                sql: "[Email] LIKE '%_@_%._%' AND LEN([Email]) <= 254 AND [Email] NOT LIKE '%[^a-zA-Z0-9@._]%' AND [Email] NOT LIKE '%@%@%' AND [Email] NOT LIKE '%.@%' AND [Email] NOT LIKE '%@.%' AND [Email] NOT LIKE '%.' AND [Email] NOT LIKE '@%' AND [Email] NOT LIKE '.%'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Email_Valid",
                table: "Users");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Email_Valid",
                table: "Users",
                sql: "[Email] LIKE '%_@_%._%' AND LEN([Email]) <= 254 AND [Email] NOT LIKE '%[^a-zA-Z0-9@._%-]%' AND [Email] NOT LIKE '%@%@%' AND [Email] NOT LIKE '%.@%' AND [Email] NOT LIKE '%@.%' AND [Email] NOT LIKE '%.' AND [Email] NOT LIKE '@%' AND [Email] NOT LIKE '.%'[Email] NOT LIKE '-%'");
        }
    }
}

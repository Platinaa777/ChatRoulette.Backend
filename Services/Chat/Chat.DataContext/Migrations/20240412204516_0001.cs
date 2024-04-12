using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class _0001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chat_users",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    connection_id = table.Column<string>(type: "text", nullable: false),
                    points = table.Column<int>(type: "integer", nullable: false),
                    peers_list = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    peers_emails = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    closed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rounds",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    correct_word = table.Column<string>(type: "text", nullable: false),
                    player1_email = table.Column<string>(type: "text", nullable: false),
                    player1_answer_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    player1_answer = table.Column<string>(type: "text", nullable: true),
                    player2_email = table.Column<string>(type: "text", nullable: false),
                    player2_answer_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    player2_answer = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rounds", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "translation_games",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    word = table.Column<string>(type: "text", nullable: false),
                    correct_word = table.Column<string>(type: "text", nullable: false),
                    options = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_translation_games", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chat_users_email",
                table: "chat_users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_users");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "rounds");

            migrationBuilder.DropTable(
                name: "translation_games");
        }
    }
}

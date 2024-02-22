using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class _0003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "age",
                table: "users");

            migrationBuilder.DropColumn(
                name: "nickname",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "age",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "nickname",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

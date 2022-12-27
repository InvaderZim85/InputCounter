using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InputCounter.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KeyboardClickCount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Day = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyboardClickCount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeyboardKeyCount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KeyCode = table.Column<int>(type: "INTEGER", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyboardKeyCount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MouseClickCount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Day = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LeftCount = table.Column<int>(type: "INTEGER", nullable: false),
                    RightCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MouseClickCount", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeyboardClickCount_Day",
                table: "KeyboardClickCount",
                column: "Day",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KeyboardKeyCount_KeyCode",
                table: "KeyboardKeyCount",
                column: "KeyCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MouseClickCount_Day",
                table: "MouseClickCount",
                column: "Day",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeyboardClickCount");

            migrationBuilder.DropTable(
                name: "KeyboardKeyCount");

            migrationBuilder.DropTable(
                name: "MouseClickCount");
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarcagS.Common.Demo.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Male",
                table: "Entries");

            migrationBuilder.AddColumn<int>(
                name: "GenderId",
                table: "Entries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_GenderId",
                table: "Entries",
                column: "GenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Genders_GenderId",
                table: "Entries",
                column: "GenderId",
                principalTable: "Genders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Genders_GenderId",
                table: "Entries");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropIndex(
                name: "IX_Entries_GenderId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "Entries");

            migrationBuilder.AddColumn<bool>(
                name: "Male",
                table: "Entries",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}

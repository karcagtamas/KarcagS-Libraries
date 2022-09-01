using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarcagS.Common.Demo.Migrations
{
    public partial class OtherGender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OtherGenderId",
                table: "Entries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entries_OtherGenderId",
                table: "Entries",
                column: "OtherGenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Genders_OtherGenderId",
                table: "Entries",
                column: "OtherGenderId",
                principalTable: "Genders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Genders_OtherGenderId",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_OtherGenderId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "OtherGenderId",
                table: "Entries");
        }
    }
}

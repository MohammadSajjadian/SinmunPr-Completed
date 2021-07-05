using Microsoft.EntityFrameworkCore.Migrations;

namespace SinmunPr.Migrations
{
    public partial class x18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactUs",
                table: "ContactUs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AboutUs",
                table: "AboutUs");

            migrationBuilder.RenameTable(
                name: "ContactUs",
                newName: "contactUs");

            migrationBuilder.RenameTable(
                name: "AboutUs",
                newName: "aboutUs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contactUs",
                table: "contactUs",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_aboutUs",
                table: "aboutUs",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_contactUs",
                table: "contactUs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_aboutUs",
                table: "aboutUs");

            migrationBuilder.RenameTable(
                name: "contactUs",
                newName: "ContactUs");

            migrationBuilder.RenameTable(
                name: "aboutUs",
                newName: "AboutUs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactUs",
                table: "ContactUs",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AboutUs",
                table: "AboutUs",
                column: "id");
        }
    }
}

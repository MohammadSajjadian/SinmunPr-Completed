using Microsoft.EntityFrameworkCore.Migrations;

namespace SinmunPr.Migrations
{
    public partial class x7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "tags");

            migrationBuilder.AddColumn<string>(
                name: "Ename",
                table: "tags",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pname",
                table: "tags",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ename",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "Pname",
                table: "tags");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "tags",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

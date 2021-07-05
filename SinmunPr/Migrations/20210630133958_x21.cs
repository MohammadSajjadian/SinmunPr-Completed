using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SinmunPr.Migrations
{
    public partial class x21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "accountCrationTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "accountCrationTime",
                table: "AspNetUsers");
        }
    }
}

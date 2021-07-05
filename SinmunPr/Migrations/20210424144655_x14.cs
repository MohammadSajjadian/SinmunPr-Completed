using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SinmunPr.Migrations
{
    public partial class x14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "postCreation",
                table: "posts");

            migrationBuilder.AddColumn<DateTime>(
                name: "ePostCreation",
                table: "posts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "pPostCreation",
                table: "posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ePostCreation",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "pPostCreation",
                table: "posts");

            migrationBuilder.AddColumn<string>(
                name: "postCreation",
                table: "posts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SinmunPr.Migrations
{
    public partial class x6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<byte[]>(
                name: "bigAvatar",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "smallAvatar",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bigAvatar",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "smallAvatar",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<byte[]>(
                name: "avatar",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}

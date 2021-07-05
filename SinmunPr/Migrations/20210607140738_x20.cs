using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SinmunPr.Migrations
{
    public partial class x20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tokenCreationTime",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "registerTokenCreationTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ressPassTokenCreationTime",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "registerTokenCreationTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ressPassTokenCreationTime",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "tokenCreationTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

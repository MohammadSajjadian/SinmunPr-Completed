using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SinmunPr.Migrations
{
    public partial class x12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar",
                table: "subComments");

            migrationBuilder.DropColumn(
                name: "firstName",
                table: "subComments");

            migrationBuilder.DropColumn(
                name: "lastName",
                table: "subComments");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "subComments");

            migrationBuilder.DropColumn(
                name: "username",
                table: "subComments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "avatar",
                table: "subComments",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "subComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lastName",
                table: "subComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "subComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "subComments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

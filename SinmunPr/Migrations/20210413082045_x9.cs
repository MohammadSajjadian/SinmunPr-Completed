using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SinmunPr.Migrations
{
    public partial class x9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "creationTime",
                table: "subComments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "persianCreationTime",
                table: "subComments",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "creationTime",
                table: "mainComments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "persianCreationTime",
                table: "mainComments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "persianCreationTime",
                table: "subComments");

            migrationBuilder.DropColumn(
                name: "persianCreationTime",
                table: "mainComments");

            migrationBuilder.AlterColumn<string>(
                name: "creationTime",
                table: "subComments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "creationTime",
                table: "mainComments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SinmunPr.Migrations
{
    public partial class x11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar",
                table: "mainComments");

            migrationBuilder.DropColumn(
                name: "firstName",
                table: "mainComments");

            migrationBuilder.DropColumn(
                name: "lastName",
                table: "mainComments");

            migrationBuilder.DropColumn(
                name: "username",
                table: "mainComments");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "mainComments",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_mainComments_userId",
                table: "mainComments",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_mainComments_AspNetUsers_userId",
                table: "mainComments",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mainComments_AspNetUsers_userId",
                table: "mainComments");

            migrationBuilder.DropIndex(
                name: "IX_mainComments_userId",
                table: "mainComments");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "mainComments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "avatar",
                table: "mainComments",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "mainComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lastName",
                table: "mainComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "mainComments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

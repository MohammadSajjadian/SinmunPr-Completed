using Microsoft.EntityFrameworkCore.Migrations;

namespace SinmunPr.Migrations
{
    public partial class x13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "subComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_subComments_userId",
                table: "subComments",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_subComments_AspNetUsers_userId",
                table: "subComments",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subComments_AspNetUsers_userId",
                table: "subComments");

            migrationBuilder.DropIndex(
                name: "IX_subComments_userId",
                table: "subComments");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "subComments");
        }
    }
}

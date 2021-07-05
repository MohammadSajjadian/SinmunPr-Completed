using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SinmunPr.Migrations
{
    public partial class x8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.CreateTable(
                name: "mainComments",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(nullable: true),
                    creationTime = table.Column<string>(nullable: true),
                    postId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mainComments", x => x.id);
                    table.ForeignKey(
                        name: "FK_mainComments_posts_postId",
                        column: x => x.postId,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subComments",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(nullable: true),
                    creationTime = table.Column<string>(nullable: true),
                    mainCommentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subComments", x => x.id);
                    table.ForeignKey(
                        name: "FK_subComments_mainComments_mainCommentId",
                        column: x => x.mainCommentId,
                        principalTable: "mainComments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mainComments_postId",
                table: "mainComments",
                column: "postId");

            migrationBuilder.CreateIndex(
                name: "IX_subComments_mainCommentId",
                table: "subComments",
                column: "mainCommentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "subComments");

            migrationBuilder.DropTable(
                name: "mainComments");

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    creationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    postId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_comments_posts_postId",
                        column: x => x.postId,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_comments_postId",
                table: "comments",
                column: "postId");
        }
    }
}

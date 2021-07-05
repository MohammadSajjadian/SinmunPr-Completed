using Microsoft.EntityFrameworkCore.Migrations;

namespace SinmunPr.Migrations
{
    public partial class x2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tags_posts_postId",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "IX_tags_postId",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "postId",
                table: "tags");

            migrationBuilder.CreateTable(
                name: "postTagIds",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    postId = table.Column<int>(nullable: false),
                    tagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_postTagIds", x => x.id);
                    table.ForeignKey(
                        name: "FK_postTagIds_posts_postId",
                        column: x => x.postId,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_postTagIds_tags_tagId",
                        column: x => x.tagId,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_postTagIds_postId",
                table: "postTagIds",
                column: "postId");

            migrationBuilder.CreateIndex(
                name: "IX_postTagIds_tagId",
                table: "postTagIds",
                column: "tagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "postTagIds");

            migrationBuilder.AddColumn<int>(
                name: "postId",
                table: "tags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tags_postId",
                table: "tags",
                column: "postId");

            migrationBuilder.AddForeignKey(
                name: "FK_tags_posts_postId",
                table: "tags",
                column: "postId",
                principalTable: "posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

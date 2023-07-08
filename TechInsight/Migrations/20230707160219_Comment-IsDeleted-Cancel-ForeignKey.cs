using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechInsight.Migrations
{
    /// <inheritdoc />
    public partial class CommentIsDeletedCancelForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_article_deleted_t_articles_ad_article_id",
                table: "t_article_deleted");

            migrationBuilder.DropIndex(
                name: "IX_t_article_deleted_ad_article_id",
                table: "t_article_deleted");

            migrationBuilder.DropColumn(
                name: "ad_article_id",
                table: "t_article_deleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ad_article_id",
                table: "t_article_deleted",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_t_article_deleted_ad_article_id",
                table: "t_article_deleted",
                column: "ad_article_id");

            migrationBuilder.AddForeignKey(
                name: "FK_t_article_deleted_t_articles_ad_article_id",
                table: "t_article_deleted",
                column: "ad_article_id",
                principalTable: "t_articles",
                principalColumn: "ar_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

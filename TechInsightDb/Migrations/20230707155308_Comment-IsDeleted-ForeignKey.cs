using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechInsight.Migrations
{
    /// <inheritdoc />
    public partial class CommentIsDeletedForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_comment_deleted_t_comments_cd_comment_id",
                table: "t_comment_deleted");

            migrationBuilder.DropIndex(
                name: "IX_t_comment_deleted_cd_comment_id",
                table: "t_comment_deleted");

            migrationBuilder.DropColumn(
                name: "cd_comment_id",
                table: "t_comment_deleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "cd_comment_id",
                table: "t_comment_deleted",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_t_comment_deleted_cd_comment_id",
                table: "t_comment_deleted",
                column: "cd_comment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_t_comment_deleted_t_comments_cd_comment_id",
                table: "t_comment_deleted",
                column: "cd_comment_id",
                principalTable: "t_comments",
                principalColumn: "co_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

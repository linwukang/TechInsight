using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechInsight.Migrations
{
    /// <inheritdoc />
    public partial class ArticleTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ar_tags",
                table: "t_articles",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ar_tags",
                table: "t_articles");
        }
    }
}

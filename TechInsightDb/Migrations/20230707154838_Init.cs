using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace TechInsight.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_user_profiles",
                columns: table => new
                {
                    up_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    up_phone_number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    up_date_of_birth = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    up_gender = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true),
                    up_profile_picture = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    up_bio = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_user_profiles", x => x.up_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_article_deleted",
                columns: table => new
                {
                    ad_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ad_article_id = table.Column<int>(type: "int", nullable: false),
                    ad_delete_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ad_operator_id = table.Column<int>(type: "int", nullable: false),
                    ad_delete_reasons = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_article_deleted", x => x.ad_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_article_review",
                columns: table => new
                {
                    are_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    are_article_id = table.Column<int>(type: "int", nullable: false),
                    are_review_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    are_is_approved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    are_not_approved_reasons = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true),
                    ReviewerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_article_review", x => x.are_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_articles",
                columns: table => new
                {
                    ar_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ar_title = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    ar_content = table.Column<string>(type: "longtext", nullable: false),
                    ar_submission_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ar_publication_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ar_last_modified_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ar_publisher_id = table.Column<int>(type: "int", nullable: false),
                    ar_read = table.Column<int>(type: "int", nullable: false),
                    ar_likes = table.Column<int>(type: "int", nullable: false),
                    ar_dislikes = table.Column<int>(type: "int", nullable: false),
                    ar_deleted_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_articles", x => x.ar_id);
                    table.ForeignKey(
                        name: "FK_t_articles_t_article_deleted_ar_deleted_id",
                        column: x => x.ar_deleted_id,
                        principalTable: "t_article_deleted",
                        principalColumn: "ad_id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_comment_deleted",
                columns: table => new
                {
                    cd_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cd_comment_id = table.Column<int>(type: "int", nullable: false),
                    cd_delete_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    cd_operator_id = table.Column<int>(type: "int", nullable: false),
                    cd_delete_reasons = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_comment_deleted", x => x.cd_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_comments",
                columns: table => new
                {
                    co_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    co_article_id = table.Column<int>(type: "int", nullable: false),
                    co_comment = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    co_publisher_id = table.Column<int>(type: "int", nullable: false),
                    co_publication_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    co_reply_comment_id = table.Column<int>(type: "int", nullable: true),
                    co_likes = table.Column<int>(type: "int", nullable: false),
                    co_dislikes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_comments", x => x.co_id);
                    table.ForeignKey(
                        name: "FK_t_comments_t_articles_co_article_id",
                        column: x => x.co_article_id,
                        principalTable: "t_articles",
                        principalColumn: "ar_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_comments_t_comments_co_reply_comment_id",
                        column: x => x.co_reply_comment_id,
                        principalTable: "t_comments",
                        principalColumn: "co_id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_user_account_deleted",
                columns: table => new
                {
                    uad_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    uad_delete_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    uad_operator_id = table.Column<int>(type: "int", nullable: false),
                    uad_delete_reasons = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_user_account_deleted", x => x.uad_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "t_user_accounts",
                columns: table => new
                {
                    ua_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ua_username = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    ua_password = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    ua_email = table.Column<string>(type: "longtext", nullable: false),
                    ua_role = table.Column<int>(type: "int", nullable: false),
                    ua_user_profile_id = table.Column<int>(type: "int", nullable: false),
                    ua_create_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ua_deleted_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_user_accounts", x => x.ua_id);
                    table.ForeignKey(
                        name: "FK_t_user_accounts_t_user_account_deleted_ua_deleted_id",
                        column: x => x.ua_deleted_id,
                        principalTable: "t_user_account_deleted",
                        principalColumn: "uad_id");
                    table.ForeignKey(
                        name: "FK_t_user_accounts_t_user_profiles_ua_user_profile_id",
                        column: x => x.ua_user_profile_id,
                        principalTable: "t_user_profiles",
                        principalColumn: "up_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_t_article_deleted_ad_article_id",
                table: "t_article_deleted",
                column: "ad_article_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_article_deleted_ad_operator_id",
                table: "t_article_deleted",
                column: "ad_operator_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_article_review_are_article_id",
                table: "t_article_review",
                column: "are_article_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_article_review_ReviewerId",
                table: "t_article_review",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_t_articles_ar_deleted_id",
                table: "t_articles",
                column: "ar_deleted_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_articles_ar_publisher_id",
                table: "t_articles",
                column: "ar_publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_comment_deleted_cd_comment_id",
                table: "t_comment_deleted",
                column: "cd_comment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_comment_deleted_cd_operator_id",
                table: "t_comment_deleted",
                column: "cd_operator_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_comments_co_article_id",
                table: "t_comments",
                column: "co_article_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_comments_co_publisher_id",
                table: "t_comments",
                column: "co_publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_comments_co_reply_comment_id",
                table: "t_comments",
                column: "co_reply_comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_user_account_deleted_uad_operator_id",
                table: "t_user_account_deleted",
                column: "uad_operator_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_user_accounts_ua_deleted_id",
                table: "t_user_accounts",
                column: "ua_deleted_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_user_accounts_ua_user_profile_id",
                table: "t_user_accounts",
                column: "ua_user_profile_id");

            migrationBuilder.AddForeignKey(
                name: "FK_t_article_deleted_t_articles_ad_article_id",
                table: "t_article_deleted",
                column: "ad_article_id",
                principalTable: "t_articles",
                principalColumn: "ar_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_t_article_deleted_t_user_accounts_ad_operator_id",
                table: "t_article_deleted",
                column: "ad_operator_id",
                principalTable: "t_user_accounts",
                principalColumn: "ua_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_t_article_review_t_articles_are_article_id",
                table: "t_article_review",
                column: "are_article_id",
                principalTable: "t_articles",
                principalColumn: "ar_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_t_article_review_t_user_accounts_ReviewerId",
                table: "t_article_review",
                column: "ReviewerId",
                principalTable: "t_user_accounts",
                principalColumn: "ua_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_t_articles_t_user_accounts_ar_publisher_id",
                table: "t_articles",
                column: "ar_publisher_id",
                principalTable: "t_user_accounts",
                principalColumn: "ua_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_t_comment_deleted_t_comments_cd_comment_id",
                table: "t_comment_deleted",
                column: "cd_comment_id",
                principalTable: "t_comments",
                principalColumn: "co_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_t_comment_deleted_t_user_accounts_cd_operator_id",
                table: "t_comment_deleted",
                column: "cd_operator_id",
                principalTable: "t_user_accounts",
                principalColumn: "ua_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_t_comments_t_user_accounts_co_publisher_id",
                table: "t_comments",
                column: "co_publisher_id",
                principalTable: "t_user_accounts",
                principalColumn: "ua_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_t_user_account_deleted_t_user_accounts_uad_operator_id",
                table: "t_user_account_deleted",
                column: "uad_operator_id",
                principalTable: "t_user_accounts",
                principalColumn: "ua_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_t_article_deleted_t_articles_ad_article_id",
                table: "t_article_deleted");

            migrationBuilder.DropForeignKey(
                name: "FK_t_user_account_deleted_t_user_accounts_uad_operator_id",
                table: "t_user_account_deleted");

            migrationBuilder.DropTable(
                name: "t_article_review");

            migrationBuilder.DropTable(
                name: "t_comment_deleted");

            migrationBuilder.DropTable(
                name: "t_comments");

            migrationBuilder.DropTable(
                name: "t_articles");

            migrationBuilder.DropTable(
                name: "t_article_deleted");

            migrationBuilder.DropTable(
                name: "t_user_accounts");

            migrationBuilder.DropTable(
                name: "t_user_account_deleted");

            migrationBuilder.DropTable(
                name: "t_user_profiles");
        }
    }
}

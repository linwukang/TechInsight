﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechInsightDb.Data;

#nullable disable

namespace TechInsight.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230726184104_ArticleTagsToRequired")]
    partial class ArticleTagsToRequired
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TechInsightDb.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ar_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("ar_content");

                    b.Property<int>("Dislikes")
                        .HasColumnType("int")
                        .HasColumnName("ar_dislikes");

                    b.Property<DateTime>("LastModifiedTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("ar_last_modified_date");

                    b.Property<int>("Likes")
                        .HasColumnType("int")
                        .HasColumnName("ar_likes");

                    b.Property<DateTime?>("PublicationTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("ar_publication_date");

                    b.Property<int>("Read")
                        .HasColumnType("int")
                        .HasColumnName("ar_read");

                    b.Property<DateTime>("SubmissionTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("ar_submission_time");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)")
                        .HasColumnName("ar_tags");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)")
                        .HasColumnName("ar_title");

                    b.Property<int?>("ar_deleted_id")
                        .HasColumnType("int");

                    b.Property<int>("ar_publisher_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ar_deleted_id");

                    b.HasIndex("ar_publisher_id");

                    b.ToTable("t_articles");
                });

            modelBuilder.Entity("TechInsightDb.Models.ArticleDeleted", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ad_id");

                    b.Property<string>("DeleteReasons")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("ad_delete_reasons");

                    b.Property<DateTime>("DeleteTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("ad_delete_time");

                    b.Property<int>("ad_operator_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ad_operator_id");

                    b.ToTable("t_article_deleted");
                });

            modelBuilder.Entity("TechInsightDb.Models.ArticleReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("are_id");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("are_is_approved");

                    b.Property<string>("NotApprovedReasons")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("are_not_approved_reasons");

                    b.Property<DateTime>("ReviewTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("are_review_time");

                    b.Property<int>("ReviewerId")
                        .HasColumnType("int");

                    b.Property<int>("are_article_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReviewerId");

                    b.HasIndex("are_article_id");

                    b.ToTable("t_article_review");
                });

            modelBuilder.Entity("TechInsightDb.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("co_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("varchar(2000)")
                        .HasColumnName("co_comment");

                    b.Property<int>("Dislikes")
                        .HasColumnType("int")
                        .HasColumnName("co_dislikes");

                    b.Property<int>("Likes")
                        .HasColumnType("int")
                        .HasColumnName("co_likes");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("co_publication_date");

                    b.Property<int>("co_article_id")
                        .HasColumnType("int");

                    b.Property<int?>("co_deleted_id")
                        .HasColumnType("int");

                    b.Property<int>("co_publisher_id")
                        .HasColumnType("int");

                    b.Property<int?>("co_reply_comment_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("co_article_id");

                    b.HasIndex("co_deleted_id");

                    b.HasIndex("co_publisher_id");

                    b.HasIndex("co_reply_comment_id");

                    b.ToTable("t_comments");
                });

            modelBuilder.Entity("TechInsightDb.Models.CommentDeleted", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("cd_id");

                    b.Property<string>("DeleteReasons")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("cd_delete_reasons");

                    b.Property<DateTime>("DeleteTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("cd_delete_time");

                    b.Property<int>("cd_operator_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("cd_operator_id");

                    b.ToTable("t_comment_deleted");
                });

            modelBuilder.Entity("TechInsightDb.Models.UserAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ua_id");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("ua_create_time");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("ua_email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("ua_password");

                    b.Property<int>("Role")
                        .HasColumnType("int")
                        .HasColumnName("ua_role");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("ua_username");

                    b.Property<int?>("ua_deleted_id")
                        .HasColumnType("int");

                    b.Property<int>("ua_user_profile_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ua_deleted_id");

                    b.HasIndex("ua_user_profile_id");

                    b.ToTable("t_user_accounts");
                });

            modelBuilder.Entity("TechInsightDb.Models.UserAccountDeleted", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("uad_id");

                    b.Property<string>("DeleteReasons")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("uad_delete_reasons");

                    b.Property<DateTime>("DeleteTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("uad_delete_time");

                    b.Property<int>("uad_operator_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("uad_operator_id");

                    b.ToTable("t_user_account_deleted");
                });

            modelBuilder.Entity("TechInsightDb.Models.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("up_id");

                    b.Property<string>("Bio")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("up_bio");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("up_date_of_birth");

                    b.Property<string>("Gender")
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)")
                        .HasColumnName("up_gender");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("up_phone_number");

                    b.Property<string>("ProfilePicture")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)")
                        .HasColumnName("up_profile_picture");

                    b.HasKey("Id");

                    b.ToTable("t_user_profiles");
                });

            modelBuilder.Entity("TechInsightDb.Models.Article", b =>
                {
                    b.HasOne("TechInsightDb.Models.ArticleDeleted", "IsDeleted")
                        .WithMany()
                        .HasForeignKey("ar_deleted_id");

                    b.HasOne("TechInsightDb.Models.UserAccount", "Publisher")
                        .WithMany()
                        .HasForeignKey("ar_publisher_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IsDeleted");

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("TechInsightDb.Models.ArticleDeleted", b =>
                {
                    b.HasOne("TechInsightDb.Models.UserAccount", "Operator")
                        .WithMany()
                        .HasForeignKey("ad_operator_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Operator");
                });

            modelBuilder.Entity("TechInsightDb.Models.ArticleReview", b =>
                {
                    b.HasOne("TechInsightDb.Models.UserAccount", "Reviewer")
                        .WithMany()
                        .HasForeignKey("ReviewerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechInsightDb.Models.Article", "Article")
                        .WithMany("ArticleReviews")
                        .HasForeignKey("are_article_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("Reviewer");
                });

            modelBuilder.Entity("TechInsightDb.Models.Comment", b =>
                {
                    b.HasOne("TechInsightDb.Models.Article", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("co_article_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechInsightDb.Models.CommentDeleted", "IsDeleted")
                        .WithMany()
                        .HasForeignKey("co_deleted_id");

                    b.HasOne("TechInsightDb.Models.UserAccount", "Publisher")
                        .WithMany()
                        .HasForeignKey("co_publisher_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TechInsightDb.Models.Comment", "ReplyComment")
                        .WithMany()
                        .HasForeignKey("co_reply_comment_id");

                    b.Navigation("Article");

                    b.Navigation("IsDeleted");

                    b.Navigation("Publisher");

                    b.Navigation("ReplyComment");
                });

            modelBuilder.Entity("TechInsightDb.Models.CommentDeleted", b =>
                {
                    b.HasOne("TechInsightDb.Models.UserAccount", "Operator")
                        .WithMany()
                        .HasForeignKey("cd_operator_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Operator");
                });

            modelBuilder.Entity("TechInsightDb.Models.UserAccount", b =>
                {
                    b.HasOne("TechInsightDb.Models.UserAccountDeleted", "IsDeleted")
                        .WithMany()
                        .HasForeignKey("ua_deleted_id");

                    b.HasOne("TechInsightDb.Models.UserProfile", "UserProfile")
                        .WithMany()
                        .HasForeignKey("ua_user_profile_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IsDeleted");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("TechInsightDb.Models.UserAccountDeleted", b =>
                {
                    b.HasOne("TechInsightDb.Models.UserAccount", "Operator")
                        .WithMany()
                        .HasForeignKey("uad_operator_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Operator");
                });

            modelBuilder.Entity("TechInsightDb.Models.Article", b =>
                {
                    b.Navigation("ArticleReviews");

                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
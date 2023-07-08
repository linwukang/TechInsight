using Microsoft.EntityFrameworkCore;
using TechInsight.Models;

namespace TechInsight.Data;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

public class ApplicationDbContext : DbContext
{
    public DbSet<Article> Articles { get; set; }

    public DbSet<ArticleDeleted> ArticleDeleted { get; set; }

    // public DbSet<ArticleDislike> ArticleDislikes { get; set; }

    // public DbSet<ArticleLike> ArticleLikes { get; set; }

    public DbSet<ArticleReview> ArticleReviews { get; set; }

    public DbSet<Comment> Comments { get; set; }

    // public DbSet<CommentDislike> CommentsDislikes { get; set; }

    // public DbSet<CommentLike> CommentsLikes { get; set; }

    public DbSet<UserAccount> UserAccounts { get; set; }

    public DbSet<UserAccountDeleted> UserAccountDeleted { get; set; }

    public DbSet<UserProfile> UserProfiles { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        string connectionString = "Server=localhost;Port=3306;Database=tech_insight; User=root;Password=root;sslmode=none;";
        optionsBuilder.UseMySQL(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
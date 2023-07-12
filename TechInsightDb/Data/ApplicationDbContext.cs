using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TechInsight.Models;
using TechInsightDb.Models;

namespace TechInsightDb.Data;
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

    private readonly IConfiguration _connectionConfiguration;
    public ApplicationDbContext(IConfiguration connectionConfiguration)
    {
        _connectionConfiguration = connectionConfiguration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var server = _connectionConfiguration["Server"];
        var port = _connectionConfiguration["Port"];
        var database = _connectionConfiguration["Database"];
        var user = _connectionConfiguration["User"];
        var password = _connectionConfiguration["Password"];
        var sslmode = _connectionConfiguration["sslmode"];

        base.OnConfiguring(optionsBuilder);
        var connectionString = $"Server={server};Port={port};Database={database}; User={user};Password={password};sslmode={sslmode};";

        optionsBuilder.UseMySQL(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}
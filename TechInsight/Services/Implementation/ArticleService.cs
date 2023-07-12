using Microsoft.EntityFrameworkCore;
using TechInsight.Configurations;
using TechInsightDb.Data;
using TechInsightDb.Models;

namespace TechInsight.Services.Implementation;

public class ArticleService : IArticleService
{
    public ApplicationDbContext Repositories { get; set; }
    public ILoginAccountService LoginAccountService { get; set; }
    public StackExchange.Redis.IDatabase Redis { get; set; }

    public ArticleService(ILoginAccountService loginAccountService, StackExchange.Redis.IDatabase redis, ApplicationDbContext repositories)
    {
        LoginAccountService = loginAccountService;
        Repositories = repositories;
        Redis = redis;
    }

    public const string LikesPrefix = "ArticleService:Likes:";
    public const string DislikesPrefix = "ArticleService:Dislikes:";

    public Article? GetById(int articleId)
    {
        return Repositories
            .Articles
            .Include(article => article.Publisher)
            .FirstOrDefault(article => article.Id == articleId);
    }

    public int? PublishArticle(int publisherId, string title, string content)
    {
        if (!LoginAccountService.Logged(publisherId))
        {
            // 未登录用户无法发布文章
            return null;
        }

        var publisher = Repositories.UserAccounts.Find(publisherId);

        if (publisher is null)
        {
            return null;
        }

        var article = new Article
        {
            Title = title,
            Content = content,
            SubmissionTime = DateTime.Now,
            PublicationTime = null,
            LastModifiedTime = DateTime.Now,
            Publisher = publisher,
            Read = 0,
            Likes = 0,
            Dislikes = 0,
            IsDeleted = null
        };

        var entryArticle = Repositories.Articles.Add(article).Entity;
        var changes = Repositories.SaveChanges();

        if (changes == 0)
        {
            return null;
        }

        return entryArticle.Id;
    }

    public bool EditArticle(int articleId, string title, string content)
    {
        var article = Repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId);
        if (article is null)
        {
            return false;
        }

        article.Title = title;
        article.Content = content;
        article.LastModifiedTime = DateTime.Now;

        var changes = Repositories.SaveChanges();

        return changes != 0;
    }

    public bool LikeArticle(int articleId, int likerId)
    {
        lock (this)
        {
            // 查看是否已点赞
            var score = Redis.SortedSetScore(LikesPrefix + articleId, likerId);
            if (score is not null)
            {
                return false;
            }

            var dislikeScore = Redis.SortedSetScore(DislikesPrefix + articleId, likerId);
            if (dislikeScore is not null)
            {
                // 取消点踩
                UnDislikeArticle(articleId, likerId);
            }

            var result = Redis.SortedSetAdd(LikesPrefix + articleId, likerId, DateTimeOffset.Now.ToUnixTimeSeconds());
            if (!result) return false;
            
            var article = Repositories.Articles.Single(article => article.Id == articleId);
            article.Likes += 1;
            return Repositories.SaveChanges() != 0;
        }
    }

    public bool UnLikeArticle(int articleId, int likerId)
    {
        lock (this)
        {
            // 查看是否已点赞
            var score = Redis.SortedSetScore(LikesPrefix + articleId, likerId);
            if (score is null)
            {
                return false;
            }

            var result = Redis.SortedSetRemove(LikesPrefix + articleId, likerId);
            if (!result) return false;

            var article = Repositories.Articles.Single(article => article.Id == articleId);
            article.Likes -= 1;
            return Repositories.SaveChanges() != 0;
        }
    }

    public bool DislikeArticle(int articleId, int dislikerId)
    {
        lock (this)
        {
            // 查看是否已点踩
            var score = Redis.SortedSetScore(DislikesPrefix + articleId, dislikerId);
            if (score is not null)
            {
                return false;
            }

            // 查看是否已点赞
            var likeScore = Redis.SortedSetScore(LikesPrefix + articleId, dislikerId);
            if (likeScore is not null)
            {
                // 取消点赞
                UnLikeArticle(articleId, dislikerId);
            }

            var result = Redis.SortedSetAdd(DislikesPrefix + articleId, dislikerId, DateTimeOffset.Now.ToUnixTimeSeconds());
            if (!result) return false;

            var article = Repositories.Articles.Single(article => article.Id == articleId);
            article.Dislikes += 1;
            return Repositories.SaveChanges() != 0;
        }
    }

    public bool UnDislikeArticle(int articleId, int dislikerId)
    {
        lock (this)
        {
            // 查看是否已点踩
            var score = Redis.SortedSetScore(DislikesPrefix + articleId, dislikerId);
            if (score is null)
            {
                return false;
            }

            var result = Redis.SortedSetRemove(DislikesPrefix + articleId, dislikerId);
            if (!result) return false;

            var article = Repositories.Articles.Single(article => article.Id == articleId);
            article.Dislikes -= 1;
            return Repositories.SaveChanges() != 0;
        }
    }

    public int? Likes(int articleId)
    {
        return Repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId)
            ?.Likes;
    }

    public int? Dislikes(int articleId)
    {
        return Repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId)
            ?.Dislikes;
    }

    public int? Read(int articleId)
    {
        return Repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId)
            ?.Read;
    }

    public string? GetTitle(int articleId)
    {
        return Repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId)
            ?.Title;
    }

    public string? GetContent(int articleId)
    {
        return Repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId)
            ?.Content;
    }

    public int? GetPublisherId(int articleId)
    {
        return Repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId)
            ?.Publisher
            ?.Id;
    }

    public bool DeleteArticle(int articleId, int operatorId, string? reasons)
    {
        var article = Repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId);

        if (article is null) return false;

        var operatorAccount = Repositories
            .UserAccounts
            .FirstOrDefault(ua => ua.Id == operatorId);

        if (operatorAccount is null) return false;

        article.IsDeleted = new ArticleDeleted
        {
            DeleteTime = DateTime.Now,
            Operator = operatorAccount,
            DeleteReasons = reasons
        };

        return Repositories.SaveChanges() != 0;
    }

    public IList<Article> GetArticlesOfSortedByPublicationTime(int skipCount, int takeCount)
    {
        return Repositories
            .Articles
            .Include(article => article.Publisher)
            .Include(article => article.Publisher.UserProfile)
            .OrderBy(article => article.PublicationTime)
            .Skip(skipCount)
            .Take(takeCount)
            .ToList();
    }

    public IList<Comment> GetComments(int articleId, int pages, int size)
    {
        return Repositories
                   .Articles
                   .Include(article => article.Comments)
                   .Where(article => article.Id == articleId)
                   .Skip(pages * size)
                   .Take(size)
                   .FirstOrDefault()
                   ?.Comments
               ?? new List<Comment>();
    }
}
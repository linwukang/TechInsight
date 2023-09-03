using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using TechInsightDb.Data;
using TechInsightDb.Models;

namespace TechInsight.Services.Implementation;

public class ArticleService : IArticleService
{
    private readonly IArticleReviewService _articleReviewService;
    private readonly ILogger<ArticleService> _logger;
    public ArticleService(
        ILoginAccountService loginAccountService,
        IDatabase redis,
        ApplicationDbContext repositories,
        IArticleReviewService articleReviewService, 
        ILogger<ArticleService> logger)
    {
        _loginAccountService = loginAccountService;
        _redis = redis;
        _repositories = repositories;
        _articleReviewService = articleReviewService;
        _logger = logger;
    }

    private readonly ApplicationDbContext _repositories;
    private readonly ILoginAccountService _loginAccountService;
    private readonly IDatabase _redis;

    private const string LikesPrefix = "ArticleService:Likes:";
    private const string DislikesPrefix = "ArticleService:Dislikes:";

    public Article? GetById(int articleId)
    {
        return _repositories
            .Articles
            .Include(article => article.Publisher)
            .FirstOrDefault(article => article.Id == articleId);
    }

    public int? PublishArticle(int publisherId, string title, string content, IList<string> tags)
    {
        if (!_loginAccountService.Logged(publisherId))
        {
            // 未登录用户无法发布文章
            _logger.LogWarning("未登录用户无法发布文章: userId={}", publisherId);
            return null;
        }

        var publisher = _repositories.UserAccounts.Find(publisherId);

        if (publisher is null)
        {
            _logger.LogWarning("用户不存在: userId={}", publisherId);
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
            Tags = tags,
            IsDeleted = null
        };

        var entryArticle = _repositories.Articles.Add(article).Entity;
        var changes = _repositories.SaveChanges();

        if (changes == 0)
        {
            return null;
        }

        _logger.LogInformation("文章已发布: publisherId={}, articleId={}", publisherId, entryArticle.Id);
        
        // 将文章加入到审核列表中
        _articleReviewService.AddArticleToPendingReviewList(entryArticle.Id);
        return entryArticle.Id;
    }

    public bool EditArticle(int articleId, string? title, string? content, IList<string>? tags)
    {
        var article = _repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId);
        if (article is null)
        {
            _logger.LogWarning("编辑文章失败: articleId={}", articleId);
            return false;
        }

        article.Title = title ?? article.Title;
        article.Content = content ?? article.Content;
        article.Tags = tags ?? article.Tags;
        article.LastModifiedTime = DateTime.Now;

        var changes = _repositories.SaveChanges();
        
        if (changes == 0)
        {
            _logger.LogWarning("编辑文章失败: articleId={}", articleId);
            return false;
        }
        
        // 将文章加入到审核列表中
        _articleReviewService.AddArticleToPendingReviewList(article.Id);
        _logger.LogInformation("编辑文章: articleId={}", articleId);
        return true;
    }

    public bool LikeArticle(int articleId, int likerId)
    {
        lock (this)
        {
            // 查看是否已点赞
            var score = _redis.SortedSetScore(LikesPrefix + articleId, likerId);
            if (score is not null)
            {
                return false;
            }

            var dislikeScore = _redis.SortedSetScore(DislikesPrefix + articleId, likerId);
            if (dislikeScore is not null)
            {
                // 取消点踩
                UnDislikeArticle(articleId, likerId);
            }

            var result = _redis.SortedSetAdd(LikesPrefix + articleId, likerId, DateTimeOffset.Now.ToUnixTimeSeconds());
            if (!result) return false;
            
            var article = _repositories.Articles.Single(article => article.Id == articleId);
            article.Likes += 1;
            return _repositories.SaveChanges() != 0;
        }
    }

    public bool UnLikeArticle(int articleId, int likerId)
    {
        lock (this)
        {
            // 查看是否已点赞
            var score = _redis.SortedSetScore(LikesPrefix + articleId, likerId);
            if (score is null)
            {
                return false;
            }

            var result = _redis.SortedSetRemove(LikesPrefix + articleId, likerId);
            if (!result) return false;

            var article = _repositories.Articles.Single(article => article.Id == articleId);
            article.Likes -= 1;
            return _repositories.SaveChanges() != 0;
        }
    }

    public bool DislikeArticle(int articleId, int dislikerId)
    {
        lock (this)
        {
            // 查看是否已点踩
            var score = _redis.SortedSetScore(DislikesPrefix + articleId, dislikerId);
            if (score is not null)
            {
                return false;
            }

            // 查看是否已点赞
            var likeScore = _redis.SortedSetScore(LikesPrefix + articleId, dislikerId);
            if (likeScore is not null)
            {
                // 取消点赞
                UnLikeArticle(articleId, dislikerId);
            }

            var result = _redis.SortedSetAdd(DislikesPrefix + articleId, dislikerId, DateTimeOffset.Now.ToUnixTimeSeconds());
            if (!result) return false;

            var article = _repositories.Articles.Single(article => article.Id == articleId);
            article.Dislikes += 1;
            return _repositories.SaveChanges() != 0;
        }
    }

    public bool UnDislikeArticle(int articleId, int dislikerId)
    {
        lock (this)
        {
            // 查看是否已点踩
            var score = _redis.SortedSetScore(DislikesPrefix + articleId, dislikerId);
            if (score is null)
            {
                return false;
            }

            var result = _redis.SortedSetRemove(DislikesPrefix + articleId, dislikerId);
            if (!result) return false;

            var article = _repositories.Articles.Single(article => article.Id == articleId);
            article.Dislikes -= 1;
            return _repositories.SaveChanges() != 0;
        }
    }

    public int? Likes(int articleId)
    {
        return _repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId)
            ?.Likes;
    }

    public int? Dislikes(int articleId)
    {
        return _repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId)
            ?.Dislikes;
    }

    public int? Read(int articleId)
    {
        return _repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId)
            ?.Read;
    }

    public string? GetTitle(int articleId)
    {
        return _repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId)
            ?.Title;
    }

    public string? GetContent(int articleId)
    {
        return _repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId)
            ?.Content;
    }

    public int? GetPublisherId(int articleId)
    {
        return _repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId)
            ?.Publisher
            ?.Id;
    }

    public bool DeleteArticle(int articleId, int operatorId, string? reasons)
    {
        var article = _repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId);

        if (article is null) return false;

        var operatorAccount = _repositories
            .UserAccounts
            .FirstOrDefault(ua => ua.Id == operatorId);

        if (operatorAccount is null) return false;

        article.IsDeleted = new ArticleDeleted
        {
            DeleteTime = DateTime.Now,
            Operator = operatorAccount,
            DeleteReasons = reasons
        };

        return _repositories.SaveChanges() != 0;
    }

    public IList<Article> GetArticlesOfSortedByPublicationTime(int skipCount, int takeCount)
    {
        return _repositories
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
        var article = _repositories
            .Articles
            .FirstOrDefault(article => article.Id == articleId);
        if (article is null)
        {
            return new List<Comment>();
        }


        return _repositories
            .Comments
            .Include(comment => comment.Article)
            .Include(comment => comment.Publisher)
            .Include(comment => comment.Publisher.UserProfile)
            .Where(comment => comment.Article.Id == articleId)
            .Skip(pages * size)
            .Take(size)
            .ToList();
    }
}
using Microsoft.EntityFrameworkCore;
using TechInsight.Configurations;
using TechInsightDb.Data;
using TechInsightDb.Models;

namespace TechInsight.Services.Implementation;

public class CommentService : ICommentService
{
    public CommentService(ILoginAccountService loginAccountService, StackExchange.Redis.IDatabase redis, DbConnectionConfiguration dbConfiguration)
    {
        Repositories = Repositories = new ApplicationDbContext(dbConfiguration);
        LoginAccountService = loginAccountService;
        Redis = redis;
    }

    public ApplicationDbContext Repositories { get; set; }
    public ILoginAccountService LoginAccountService { get; set; }
    public StackExchange.Redis.IDatabase Redis { get; set; }

    public const string LikesPrefix = "CommentService:Likes:";
    public const string DislikesPrefix = "CommentService:Dislikes:";

    public Comment? GetById(int commentId)
    {
        return Repositories.Comments.Find(commentId);
    }

    public int? CommentArticle(int articleId, int commenterId, string commentContent)
    {
        if (!LoginAccountService.Logged(commenterId))
        {
            // 未登录用户无法发布文章
            return null;
        }

        var commenter = Repositories.UserAccounts.Find(commenterId);
        var article = Repositories.Articles.Find(articleId);
        if (article is null || commenter is null)
        {
            return null;
        }

        var comment = new Comment
        {
            Article = article,
            Content = commentContent,
            Publisher = commenter,
            PublicationDate = DateTime.Now,
            ReplyComment = null,
            Likes = 0,
            Dislikes = 0,
            IsDeleted = null
        };

        var entryComment = Repositories.Comments.Add(comment).Entity;
        var changes = Repositories.SaveChanges();

        if (changes == 0)
        {
            return null;
        }

        return entryComment.Id;
    }

    public int? ReplyComment(int commentId, int replierId, string replyContent)
    {
        if (!LoginAccountService.Logged(replierId))
        {
            // 未登录用户无法发布文章
            return null;
        }

        var replier = Repositories.UserAccounts.Find(replierId);
        // 获取被回复的评论
        var comment = Repositories
            .Comments
            .Include(comment => comment.Article)
            .FirstOrDefault(comment => comment.Id == commentId);

        if (comment is null || replier is null)
        {
            return null;
        }

        var replyComment = new Comment
        {
            Article = comment.Article,
            Content = replyContent,
            Publisher = replier,
            PublicationDate = DateTime.Now,
            ReplyComment = comment,
            Likes = 0,
            Dislikes = 0,
            IsDeleted = null
        };

        var entryReplyComment = Repositories.Comments.Add(replyComment).Entity;
        var changes = Repositories.SaveChanges();

        if (changes == 0)
        {
            return null;
        }

        return entryReplyComment.Id;

    }

    public bool LikeComment(int commentId, int likerId)
    {
        lock (this)
        {
            // 查看是否已点赞
            var score = Redis.SortedSetScore(LikesPrefix + commentId, likerId);
            if (score is not null)
            {
                return false;
            }
            // 查看是否已点踩
            var dislikeScore = Redis.SortedSetScore(DislikesPrefix + commentId, likerId);
            if (dislikeScore is not null)
            {
                UnDislikeComment(commentId, likerId);
            }

            var result = Redis.SortedSetRemove(LikesPrefix + commentId, likerId);
            if (!result) return false;

            var comment = Repositories.Comments.Single(comment => comment.Id == commentId);
            comment.Likes += 1;
            return Repositories.SaveChanges() != 0;
        }
    }

    public bool UnLikeComment(int commentId, int likerId)
    {
        lock (this)
        {
            // 查看是否已点赞
            var score = Redis.SortedSetScore(LikesPrefix + commentId, likerId);
            if (score is null)
            {
                return false;
            }

            var result = Redis.SortedSetRemove(LikesPrefix + commentId, likerId);
            if (!result) return false;

            var comment = Repositories.Comments.Single(comment => comment.Id == commentId);
            comment.Likes -= 1;
            return Repositories.SaveChanges() != 0;
        }
    }

    public bool DislikeComment(int commentId, int dislikerId)
    {
        lock (this)
        {
            // 查看是否已点踩
            var score = Redis.SortedSetScore(DislikesPrefix + commentId, dislikerId);
            if (score is not null)
            {
                return false;
            }
            // 查看是否已点赞
            var likeScore = Redis.SortedSetScore(LikesPrefix + commentId, dislikerId);
            if (likeScore is not null)
            {
                UnLikeComment(commentId, dislikerId);
            }

            var result = Redis.SortedSetRemove(DislikesPrefix + commentId, dislikerId);
            if (!result) return false;

            var comment = Repositories.Comments.Single(comment => comment.Id == commentId);
            comment.Likes += 1;
            return Repositories.SaveChanges() != 0;
        }
    }

    public bool UnDislikeComment(int commentId, int dislikerId)
    {
        lock (this)
        {
            // 查看是否已点踩
            var score = Redis.SortedSetScore(DislikesPrefix + commentId, dislikerId);
            if (score is null)
            {
                return false;
            }

            var result = Redis.SortedSetRemove(DislikesPrefix + commentId, dislikerId);
            if (!result) return false;

            var comment = Repositories.Comments.Single(comment => comment.Id == commentId);
            comment.Likes -= 1;
            return Repositories.SaveChanges() != 0;
        }
    }

    public int? Likes(int commentId)
    {
        return Repositories
            .Comments
            .FirstOrDefault(comment => comment.Id == commentId)
            ?.Likes;
    }

    public int? Dislikes(int commentId)
    {
        return Repositories
            .Comments
            .FirstOrDefault(comment => comment.Id == commentId)
            ?.Dislikes;
    }

    public int? GetArticleId(int commentId)
    {
        return Repositories
            .Comments
            .FirstOrDefault(comment => comment.Id == commentId)
            ?.Article
            ?.Id;
    }

    public int? GetCommenterId(int commentId)
    {
        return Repositories
            .Comments
            .FirstOrDefault(comment => comment.Id == commentId)
            ?.Publisher
            ?.Id;
    }

    public int? ReplyComment(int commentId)
    {
        return Repositories
            .Comments
            .FirstOrDefault(comment => comment.Id == commentId)
            ?.ReplyComment
            ?.Id;
    }

    public bool DeleteArticle(int commentId, int operatorId, string? reasons)
    {
        var comment = Repositories
            .Comments
            .FirstOrDefault(comment => comment.Id == commentId);

        if (comment is null) return false;

        var operatorAccount = Repositories
            .UserAccounts
            .FirstOrDefault(ua => ua.Id == operatorId);

        if (operatorAccount is null) return false;

        comment.IsDeleted = new CommentDeleted
        {
            DeleteTime = DateTime.Now,
            Operator = operatorAccount,
            DeleteReasons = reasons
        };

        return Repositories.SaveChanges() != 0;
    }
}
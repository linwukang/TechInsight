using StackExchange.Redis;
using TechInsightDb.Data;

namespace TechInsight.Services.Implementation;

/// <summary>
/// 未完成：
/// - 审核记录
/// </summary>
public class ArticleReviewService : IArticleReviewService
{
    public ArticleReviewService(IDatabase redis, ApplicationDbContext repositories)
    {
        Redis = redis;
        Repositories = repositories;
    }

    public readonly IDatabase Redis;
    public readonly ApplicationDbContext Repositories;

    public const string PendingArticles = "ArticleReviewService:PendingArticles";
    public const string ProcessingArticles = "ArticleReviewService:ProcessingArticles:";
    public const string RejectedArticles = "ArticleReviewService:RejectedArticles";

    public IList<int> GetPendingArticle()
    {
        return 
            Redis
                .SetMembers(PendingArticles)
                .ToList()
                .Select(mem => int.Parse(mem.ToString()))
                .ToList();
    }

    public long GetPendingArticleCount()
    {
        return Redis.SetLength(PendingArticles);
    }

    public IList<int> GetRejectedArticle()
    {
        return
            Redis
                .SetMembers(RejectedArticles)
                .ToList()
                .Select(mem => int.Parse(mem.ToString()))
                .ToList();
    }

    public long GetRejectedArticleCount()
    {
        return Redis.HashLength(RejectedArticles);
    }

    public bool AddArticleToPendingReviewList(int articleId)
    {
        Redis.HashDelete(RejectedArticles, articleId);

        return Redis.SetAdd(PendingArticles, articleId);
    }

    public bool IsArticlePendingReview(int articleId)
    {
        return Redis.SetContains(PendingArticles, articleId);
    }

    public void ApproveArticle(int articleId, int reviewerId)
    {
        Redis.SetRemove(PendingArticles, articleId);
    }

    public void RejectArticle(int articleId, int reviewerId, string reasons)
    {
        Redis.HashSet(RejectedArticles, articleId, reasons);
    }

    public bool IsArticleApproved(int articleId)
    {
        return !Redis.SetContains(PendingArticles, articleId) 
               && !Redis.HashExists(RejectedArticles, articleId);
    }

    public bool IsArticleRejected(int articleId)
    {
        return Redis.HashExists(RejectedArticles, articleId);
    }

    public int? ReviewArticle(int reviewerId)
    {
        lock (Redis)
        {
            var member = Redis.SetRandomMember(PendingArticles);
            if (member.IsNull)
            {
                return null;
            }
            Redis.SetRemove(PendingArticles, member);
            var articleId = int.Parse(member.ToString());
            Redis.StringSet(ProcessingArticles + articleId, reviewerId);
            return articleId;
        }
    }

    public bool InReviewArticle(int articleId, int reviewerId)
    {
        var processorId = Redis.StringGet(ProcessingArticles + articleId);

        if (processorId.IsNull)
        {
            return false;
        }

        return reviewerId == int.Parse(processorId.ToString());
    }

    public void CancelReview(int articleId)
    {
        Redis.HashDeleteAsync(ProcessingArticles, articleId);
    }
}
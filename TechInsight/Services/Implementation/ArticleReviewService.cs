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
        _redis = redis;
        _repositories = repositories;
    }

    private readonly IDatabase _redis;
    private readonly ApplicationDbContext _repositories;

    private const string PendingArticles = "ArticleReview:PendingArticles";
    private const string RejectedArticles = "ArticleReview:RejectedArticles";
    // Key      ArticleReview:ProcessingArticles:<articleId>
    // Value    <reviewerId>
    private const string ProcessingArticles = "ArticleReview:ProcessingArticles:";

    public IList<int> GetPendingArticle()
    {
        return 
            _redis
                .SetMembers(PendingArticles)
                .ToList()
                .Select(mem => int.Parse(mem.ToString()))
                .ToList();
    }

    public long GetPendingArticleCount()
    {
        return _redis.SetLength(PendingArticles);
    }

    public IList<int> GetRejectedArticle()
    {
        return
            _redis
                .SetMembers(RejectedArticles)
                .ToList()
                .Select(mem => int.Parse(mem.ToString()))
                .ToList();
    }

    public long GetRejectedArticleCount()
    {
        return _redis.HashLength(RejectedArticles);
    }

    public bool AddArticleToPendingReviewList(int articleId)
    {
        _redis.HashDelete(RejectedArticles, articleId);

        return _redis.SetAdd(PendingArticles, articleId);
    }

    public bool IsArticlePendingReview(int articleId)
    {
        return _redis.SetContains(PendingArticles, articleId);
    }

    public void ApproveArticle(int articleId, int reviewerId)
    {
        _redis.SetRemove(PendingArticles, articleId);
    }

    public void RejectArticle(int articleId, int reviewerId, string reasons)
    {
        _redis.HashSet(RejectedArticles, articleId, reasons);
    }

    public bool IsArticleApproved(int articleId)
    {
        return !_redis.SetContains(PendingArticles, articleId) 
               && !_redis.HashExists(RejectedArticles, articleId);
    }

    public bool IsArticleRejected(int articleId)
    {
        return _redis.HashExists(RejectedArticles, articleId);
    }

    public int? ReviewArticle(int reviewerId)
    {
        lock (_redis)
        {
            var member = _redis.SetRandomMember(PendingArticles);
            if (member.IsNull)
            {
                return null;
            }
            _redis.SetRemove(PendingArticles, member);
            var articleId = int.Parse(member.ToString());
            _redis.StringSet(ProcessingArticles + articleId, reviewerId);
            return articleId;
        }
    }

    public bool InReviewArticle(int articleId, int reviewerId)
    {
        var processorId = _redis.StringGet(ProcessingArticles + articleId);

        if (processorId.IsNull)
        {
            return false;
        }

        return reviewerId == int.Parse(processorId.ToString());
    }

    public void CancelReview(int articleId)
    {
        _redis.HashDeleteAsync(ProcessingArticles, articleId);
    }
}
namespace TechInsight.Services;

public interface IReviewService
{
    IList<int> GetPendingArticle();

    long GetPendingArticleCount();

    IList<int> GetRejectedArticle();

    long GetRejectedArticleCount();

    bool AddArticleToPendingReviewList(int articleId);

    bool IsArticlePendingReview(int articleId);

    void ApproveArticle(int articleId);

    void RejectArticle(int articleId, string reasons);

    bool IsArticleApproved(int articleId);

    bool IsArticleRejected(int articleId);
}
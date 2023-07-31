namespace TechInsight.Services;

public interface IArticleReviewService
{
    /// <summary>
    /// 获取待审核的文章 id 的列表
    /// </summary>
    /// <returns></returns>
    IList<int> GetPendingArticle();

    /// <summary>
    /// 获取待审核文章的个数
    /// </summary>
    /// <returns></returns>
    long GetPendingArticleCount();

    /// <summary>
    /// 获取审核不通过的文章 id 的列表
    /// </summary>
    /// <returns></returns>
    IList<int> GetRejectedArticle();

    /// <summary>
    /// 获取审核不通过的文章的个数
    /// </summary>
    /// <returns></returns>
    long GetRejectedArticleCount();

    /// <summary>
    /// 将文章添加到待审核列表中
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <returns></returns>
    bool AddArticleToPendingReviewList(int articleId);

    /// <summary>
    /// 判断文章是否在等待审核
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <returns></returns>
    bool IsArticlePendingReview(int articleId);

    /// <summary>
    /// 使文章通过审核
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <param name="reviewerId">审核员 id</param>
    void ApproveArticle(int articleId, int reviewerId);

    /// <summary>
    /// 使文章不通过审核
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <param name="reviewerId">审核员 id</param>
    /// <param name="reasons">不通过的原因</param>
    void RejectArticle(int articleId, int reviewerId, string reasons);

    /// <summary>
    /// 判断文章是否是已经通过审核的
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <returns></returns>
    bool IsArticleApproved(int articleId);

    /// <summary>
    /// 判断文章是否是已经不通过审核的
    /// </summary>
    /// <param name="articleId"></param>
    /// <returns></returns>
    bool IsArticleRejected(int articleId);

    /// <summary>
    /// 从待审核列表获取一篇文章供审核员审核
    /// </summary>
    /// <param name="reviewerId">审核员 id</param>
    /// <returns></returns>
    int? ReviewArticle(int reviewerId);

    /// <summary>
    /// 判断文章是否正在被审核员审核
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <param name="reviewerId">审核员 id</param>
    /// <returns></returns>
    bool InReviewArticle(int articleId, int reviewerId);

    /// <summary>
    /// 取消审核
    /// </summary>
    /// <param name="articleId">文章 id</param>
    void CancelReview(int articleId);

    /*/// <summary>
    /// 取消指定审核员的所有审核
    /// </summary>
    /// <param name="reviewerId">审核员 id</param>
    void CancelAllReview(int reviewerId);*/
}
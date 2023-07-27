using TechInsightDb.Models;

namespace TechInsight.Services;

public interface IArticleService
{
    /// <summary>
    /// 通过文章 id 获取文章实体对象
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <returns>获取成功返回文章实体对象，否则返回 null</returns>
    Article? GetById(int articleId);

    /// <summary>
    /// 发布文章
    /// </summary>
    /// <param name="publisherId">发布者用户 id</param>
    /// <param name="title">文章标题</param>
    /// <param name="content">文章内容</param>
    /// <returns>发布成功返回文章 id，否则返回 null</returns>
    int? PublishArticle(int publisherId, string title, string content, IList<string> tags);

    /// <summary>
    /// 编辑文章
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <param name="title">文章标题</param>
    /// <param name="content">文章内容</param>
    /// <returns>编辑是否成功</returns>
    bool EditArticle(int articleId, string? title, string? content, IList<string>? tags);

    /// <summary>
    /// 为文章点赞
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <param name="likerId">点赞用户 id</param>
    /// <returns>点赞是否成功</returns>
    bool LikeArticle(int articleId, int likerId);

    /// <summary>
    /// 为文章取消点赞
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <param name="likerId">点赞用户 id</param>
    /// <returns>取消点赞是否成功</returns>
    bool UnLikeArticle(int articleId, int likerId);

    /// <summary>
    /// 为文章点踩
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <param name="dislikerId">点踩用户 id</param>
    /// <returns>点踩是否成功</returns>
    bool DislikeArticle(int articleId, int dislikerId);

    /// <summary>
    /// 为文章取消点踩
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <param name="dislikerId">点踩用户 id</param>
    /// <returns>取消点踩是否成功</returns>
    bool UnDislikeArticle(int articleId, int dislikerId);

    /// <summary>
    /// 获取文章点赞数量
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <returns>点赞数</returns>
    int? Likes(int articleId);

    /// <summary>
    /// 获取文章点踩数量
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <returns>点踩数</returns>
    int? Dislikes(int articleId);

    /// <summary>
    /// 文章的阅读量
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <returns></returns>
    int? Read(int articleId);

    /// <summary>
    /// 通过文章 id 获取文章标题
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <returns>文章标题</returns>
    string? GetTitle(int articleId);

    /// <summary>
    /// 通过文章 id 获取文章内容
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <returns>文章内容</returns>
    string? GetContent(int articleId);

    /// <summary>
    /// 通过文章 id 获取发布者 id
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <returns>发布者 id</returns>
    int? GetPublisherId(int articleId);

    /// <summary>
    /// 删除文章
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <param name="operatorId">操作者 id</param>
    /// <param name="reasons">删除原因</param>
    /// <returns>是否删除成功</returns>
    bool DeleteArticle(int articleId, int operatorId, string? reasons);

    /// <summary>
    /// 获取通过发布日期排序的文章列表
    /// </summary>
    /// <param name="skipCount">跳过文章数</param>
    /// <param name="takeCount">获取文章数</param>
    /// <returns>文章列表</returns>
    IList<Article> GetArticlesOfSortedByPublicationTime(int skipCount, int takeCount);

    /// <summary>
    /// 通过文章 id 获取评论列表
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <param name="pages">页数，从 0 开始计算</param>
    /// <param name="size">每页的评论数</param>
    /// <returns></returns>
    IList<Comment> GetComments(int articleId, int pages, int size);
}
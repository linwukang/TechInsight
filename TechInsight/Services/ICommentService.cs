using TechInsight.Models;

namespace TechInsight.Services;

public interface ICommentService
{
    /// <summary>
    /// 通过评论 id 获取评论实体对象
    /// </summary>
    /// <param name="commentId">评论 id</param>
    /// <returns>评论实体对象</returns>
    Comment? GetById(int commentId);

    /// <summary>
    /// 发布文章评论
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <param name="commenterId">评论用户 id</param>
    /// <param name="commentContent">评论内容</param>
    /// <returns>发布成功返回评论 id，否则返回 null</returns>
    int? CommentArticle(int articleId, int commenterId, string commentContent);

    /// <summary>
    /// 回复文章评论
    /// </summary>
    /// <param name="commentId">被回复的评论 id</param>
    /// <param name="replierId">回复者 id</param>
    /// <param name="replyContent">回复内容</param>
    /// <returns>回复成功返回回复评论 id，否则返回 null</returns>
    int? ReplyComment(int commentId, int replierId, string replyContent);

    /// <summary>
    /// 为评论点赞
    /// </summary>
    /// <param name="commentId">评论 id</param>
    /// <param name="likerId">点赞用户 id</param>
    /// <returns>点赞是否成功</returns>
    bool LikeComment(int commentId, int likerId);

    /// <summary>
    /// 为评论取消点赞
    /// </summary>
    /// <param name="commentId">评论 id</param>
    /// <param name="likerId">点赞用户 id</param>
    /// <returns>取消点赞是否成功</returns>
    bool UnLikeComment(int commentId, int likerId);


    /// <summary>
    /// 为评论点踩
    /// </summary>
    /// <param name="commentId">评论 id</param>
    /// <param name="dislikerId">点踩用户 id</param>
    /// <returns>点踩是否成功</returns>
    bool DislikeComment(int commentId, int dislikerId);

    /// <summary>
    /// 为评论取消点踩
    /// </summary>
    /// <param name="commentId">评论 id</param>
    /// <param name="dislikerId">点踩用户 id</param>
    /// <returns>取消点踩是否成功</returns>
    bool UnDislikeComment(int commentId, int dislikerId);

    /// <summary>
    /// 获取评论点赞数量
    /// </summary>
    /// <param name="commentId">评论 id</param>
    /// <returns>点赞数</returns>
    int? Likes(int commentId);

    /// <summary>
    /// 获取评论点踩数量
    /// </summary>
    /// <param name="commentId">评论 id</param>
    /// <returns>点踩数</returns>
    int? Dislikes(int commentId);

    /// <summary>
    /// 获取被评论的文章 id
    /// </summary>
    /// <param name="commentId">评论 id</param>
    /// <returns>文章 id</returns>
    int? GetArticleId(int commentId);

    /// <summary>
    /// 获取评论用户 id
    /// </summary>
    /// <param name="commentId">评论 id</param>
    /// <returns>评论用户 id</returns>
    int? GetCommenterId(int commentId);

    /// <summary>
    /// 获取该评论所回复的评论 id
    /// </summary>
    /// <param name="commentId">评论 id</param>
    /// <returns>该评论所回复的评论 id</returns>
    int? ReplyComment(int commentId);

    /// <summary>
    /// 删除评论
    /// </summary>
    /// <param name="commentId">评论 id</param>
    /// <param name="operatorId">操作者 id</param>
    /// <param name="reasons">删除原因</param>
    /// <returns>是否删除成功</returns>
    bool DeleteArticle(int commentId, int operatorId, string? reasons);
}
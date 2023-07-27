using Microsoft.AspNetCore.Mvc;
using TechInsight.Services;

namespace TechInsight.Controllers;

[ApiController]
[Route("comments")]
public class CommentController : Controller
{
    public readonly ICommentService CommentService;

    public readonly ILoginAccountService LoginAccountService;

    public readonly IArticleService ArticleService;

    public readonly ILogger<CommentController> Logger;

    public CommentController(ICommentService commentService, ILoginAccountService loginAccountService, IArticleService articleService, ILogger<CommentController> logger)
    {
        CommentService = commentService;
        LoginAccountService = loginAccountService;
        Logger = logger;
        ArticleService = articleService;
    }

    [HttpGet("post-comment")]
    public IActionResult PostComment([FromQuery] int articleId, [FromQuery] int userId, [FromQuery] string content)
    {
        if (!LoginAccountService.Logged(userId))
        {
            Logger.LogDebug($"用户 id {userId} 未登录，无法发布评论");
            return Ok(new
            {
                message = "用户未登录"
            });
        }

        var article = ArticleService.GetById(articleId);

        if (article is null)
        {
            Logger.LogDebug($"文章 id {articleId} 不存在");
            return NotFound(new
            {
                message = "未找到文章"
            });
        }

        var commentId = CommentService.CommentArticle(articleId, userId, content);
        if (commentId is null)
        {
            Logger.LogWarning($"发布评论失败，articleId={articleId}, userId={commentId}, content={content}");
            return Ok(new
            {
                message = "服务器繁忙，请稍后重试"
            });
        }

        return Ok(new
        {
            commentId = commentId
        });
    }

    [HttpGet("reply-comment")]
    public IActionResult ReplyComment([FromQuery] int commentId, [FromQuery] int userId, [FromQuery] string content)
    {
        var success = CommentService.ReplyComment(commentId, userId, content);
        return Ok(new
        {
            success = success
        });
    }

    [HttpGet("delete-comment")]
    public IActionResult DeleteComment([FromQuery] int commentId, [FromQuery] int operatorId, [FromQuery] string? reasons)
    {
        var success = CommentService.DeleteArticle(commentId, operatorId, reasons);

        return Ok(new
        {
            success = success
        });
    }

    [HttpGet("like-comment")]
    public IActionResult LikeComment([FromQuery] int commentId, [FromQuery] int userId)
    {
        var success = CommentService.LikeComment(commentId, userId);

        return Ok(new
        {
            success = success
        });
    }

    [HttpGet("unlike-comment")]
    public IActionResult UnLikeComment([FromQuery] int commentId, [FromQuery] int userId)
    {
        var success = CommentService.UnLikeComment(commentId, userId);

        return Ok(new
        {
            success = success
        });
    }

    [HttpGet("dislike-comment")]
    public IActionResult DislikeComment([FromQuery] int commentId, [FromQuery] int userId)
    {
        var success = CommentService.DislikeComment(commentId, userId);

        return Ok(new
        {
            success = success
        });
    }

    [HttpGet("undislike-comment")]
    public IActionResult UnDislikeComment([FromQuery] int commentId, [FromQuery] int userId)
    {
        var success = CommentService.UnDislikeComment(commentId, userId);

        return Ok(new
        {
            success = success
        });
    }
}
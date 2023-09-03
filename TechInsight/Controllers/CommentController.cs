using Microsoft.AspNetCore.Mvc;
using TechInsight.Services;

namespace TechInsight.Controllers;

[ApiController]
[Route("comments")]
public class CommentController : Controller
{
    private readonly ICommentService _commentService;

    private readonly ILoginAccountService _loginAccountService;

    private readonly IArticleService _articleService;

    private readonly ILogger<CommentController> _logger;

    public CommentController(ICommentService commentService, ILoginAccountService loginAccountService, IArticleService articleService, ILogger<CommentController> logger)
    {
        _commentService = commentService;
        _loginAccountService = loginAccountService;
        _logger = logger;
        _articleService = articleService;
    }

    [HttpPost("post-comment/{articleId:int}")]
    public IActionResult PostComment([FromRoute] int articleId, [FromQuery] int userId, [FromQuery] string content)
    {
        if (!_loginAccountService.Logged(userId))
        {
            _logger.LogDebug($"用户 id {userId} 未登录，无法发布评论");
            return Ok(new
            {
                message = "用户未登录"
            });
        }

        var article = _articleService.GetById(articleId);

        if (article is null)
        {
            _logger.LogDebug($"文章 id {articleId} 不存在");
            return NotFound(new
            {
                message = "未找到文章"
            });
        }

        var commentId = _commentService.CommentArticle(articleId, userId, content);
        if (commentId is null)
        {
            _logger.LogWarning($"发布评论失败，articleId={articleId}, userId={commentId}, content={content}");
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

    [HttpPost("reply-comment/{commentId:int}")]
    public IActionResult ReplyComment([FromRoute] int commentId, [FromQuery] int userId, [FromQuery] string content)
    {
        var success = _commentService.ReplyComment(commentId, userId, content);
        return Ok(new
        {
            success = success
        });
    }

    [HttpDelete("delete-comment/{commentId:int}")]
    public IActionResult DeleteComment([FromRoute] int commentId, [FromQuery] int operatorId, [FromQuery] string? reasons)
    {
        var success = _commentService.DeleteArticle(commentId, operatorId, reasons);

        return Ok(new
        {
            success = success
        });
    }

    [HttpPost("like-comment/{commentId:int}")]
    public IActionResult LikeComment([FromRoute] int commentId, [FromQuery] int userId)
    {
        var success = _commentService.LikeComment(commentId, userId);

        return Ok(new
        {
            success = success
        });
    }

    [HttpPost("unlike-comment/{commentId:int}")]
    public IActionResult UnLikeComment([FromRoute] int commentId, [FromQuery] int userId)
    {
        var success = _commentService.UnLikeComment(commentId, userId);

        return Ok(new
        {
            success = success
        });
    }

    [HttpPost("dislike-comment/{commentId:int}")]
    public IActionResult DislikeComment([FromRoute] int commentId, [FromQuery] int userId)
    {
        var success = _commentService.DislikeComment(commentId, userId);

        return Ok(new
        {
            success = success
        });
    }

    [HttpPost("undislike-comment/{commentId:int}")]
    public IActionResult UnDislikeComment([FromRoute] int commentId, [FromQuery] int userId)
    {
        var success = _commentService.UnDislikeComment(commentId, userId);

        return Ok(new
        {
            success = success
        });
    }
    
    /// <summary>
    /// 通过文章的 id 获取文章的评论，并分页返回
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <param name="pages">页数，从 0 开始计数</param>
    /// <param name="size">每页评论的数量</param>
    /// <returns></returns>
    [HttpGet("comment-list/{articleId:int}")]
    public IActionResult CommentList([FromRoute] int articleId, [FromQuery] int pages, [FromQuery] int size)
    {
        return Ok(new 
        {
            comments = _articleService
                .GetComments(articleId, pages, size)
                .Select(comment => new
                {
                    id = comment.Id,
                    profilePicture = comment.Publisher.UserProfile.ProfilePicture,
                    userId = comment.Publisher.Id,
                    username = comment.Publisher.UserName,
                    publishDate = comment.PublicationDate,
                    content = comment.Content,
                    likes = comment.Likes,
                    dislikes = comment.Dislikes,
                })
                .ToList()
        });
    }
}
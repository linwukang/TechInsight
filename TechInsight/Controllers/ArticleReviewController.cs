using Microsoft.AspNetCore.Mvc;
using TechInsight.Services;
using TechInsight.Services.Implementation;

namespace TechInsight.Controllers;

[ApiController]
[Route("articles-review")]
public class ArticleReviewController : Controller
{
    public ArticleReviewController(IArticleReviewService articleReviewService, ILoginAccountService loginAccountService, IArticleService articleService)
    {
        _articleReviewService = articleReviewService;
        _loginAccountService = loginAccountService;
        _articleService = articleService;
    }

    private readonly IArticleReviewService _articleReviewService;
    private readonly ILoginAccountService _loginAccountService;
    private readonly IArticleService _articleService;

    /// <summary>
    /// 使文章通过审核
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <param name="reviewerId">审核员 id</param>
    /// <returns></returns>
    [HttpPost("approve-article/{articleId:int}")]
    public IActionResult ApproveArticle([FromRoute] int articleId, [FromQuery] int reviewerId)
    {
        if (!_loginAccountService.IsReviewer(reviewerId))
        {
            return Forbid();
        }

        if (_articleReviewService.InReviewArticle(articleId, reviewerId))
        {
            return Forbid();
        }

        _articleReviewService.ApproveArticle(articleId, reviewerId);
        return Ok("文章已通过审核");
    }

    /// <summary>
    /// 使文章不通过审核
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <param name="reviewerId">审核员 id</param>
    /// <param name="reasons">审核不通过原因</param>
    /// <returns></returns>
    [HttpPost("reject-article/{articleId:int}")]
    public IActionResult RejectArticle([FromRoute] int articleId, [FromQuery] int reviewerId, [FromQuery] string reasons)
    {
        if (!_loginAccountService.IsReviewer(reviewerId))
        {
            return Forbid();
        }

        if (_articleReviewService.InReviewArticle(articleId, reviewerId))
        {
            return Forbid();
        }

        _articleReviewService.RejectArticle(articleId, reviewerId, reasons);
        return Ok();
    }
}
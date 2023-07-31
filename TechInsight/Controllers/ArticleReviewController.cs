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
        ArticleReviewService = articleReviewService;
        LoginAccountService = loginAccountService;
        ArticleService = articleService;
    }

    public readonly IArticleReviewService ArticleReviewService;
    public readonly ILoginAccountService LoginAccountService;
    public readonly IArticleService ArticleService;

    [HttpGet("approve-article")]
    public IActionResult ApproveArticle([FromQuery] int articleId, [FromQuery] int reviewerId)
    {
        if (!LoginAccountService.IsReviewer(reviewerId))
        {
            return Forbid();
        }

        if (ArticleReviewService.InReviewArticle(articleId, reviewerId))
        {
            return Forbid();
        }

        ArticleReviewService.ApproveArticle(articleId, reviewerId);
        return Ok("文章已通过审核");
    }

    [HttpGet("reject-article")]
    public IActionResult RejectArticle([FromQuery] int articleId, [FromQuery] int reviewerId, [FromQuery] string reasons)
    {
        if (!LoginAccountService.IsReviewer(reviewerId))
        {
            return Forbid();
        }

        if (ArticleReviewService.InReviewArticle(articleId, reviewerId))
        {
            return Forbid();
        }

        ArticleReviewService.RejectArticle(articleId, reviewerId, reasons);
        return Ok();
    }
}
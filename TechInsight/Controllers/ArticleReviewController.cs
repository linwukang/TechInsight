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
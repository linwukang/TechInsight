using Microsoft.AspNetCore.Mvc;

namespace TechInsight.Controllers;

[ApiController]
[Route("articles-review")]
public class ArticleReviewController : Controller
{
    [HttpGet("article-approved")]
    public IActionResult ArticleApproved(int articleId, int reviewerId)
    {
        return null;
    }
}
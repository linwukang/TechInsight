using Microsoft.AspNetCore.Mvc;
using TechInsight.Dto;
using TechInsight.Services;

namespace TechInsight.Controllers;

[ApiController]
[Route("articles")]
public class ArticleController : Controller
{
    private readonly IArticleService _articleService;

    public ArticleController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    /// <summary>
    /// 发布文章。调用该 api 时需要拦截器验证 token，确保发布者 id 和 token 匹配
    /// </summary>
    /// <param name="article">请求体，包含发布用户 id、文章标题、文章内容</param>
    /// <returns>
    /// 发布成功:
    /// { succeed: true }
    /// 发布失败:
    /// { succeed: false, message: "服务器繁忙，请稍后重试" }
    /// </returns>
    [HttpPost("publish-article")]
    public IActionResult PublishArticle([FromBody] PublishArticleDto article)
    {
        if (article.Title is null || article.Title.Length == 0)
        {
            return BadRequest(new
            {
                succeed = false,
                message = "标题不能为空"
            });
        }
        if (article.Content is null || article.Content.Length == 0)
        {
            return BadRequest(new
            {
                succeed = false,
                message = "文章内容不能为空"
            });
        }
        if (article.Tags is null || article.Tags.Count == 0)
        {
            return BadRequest(new
            {
                succeed = false,
                message = "标签不能为空"
            });
        }
        var articleId = _articleService.PublishArticle(article.UserId, article.Title, article.Content, article.Tags);

        if (articleId is null)
        {
            return BadRequest(new
            {
                succeed = false,
                message = "服务器繁忙，请稍后重试"
            });
        }

        return Ok(new
        {
            succeed = true,
            articleId = (int) articleId
        });
    }

    /// <summary>
    /// 编辑文章
    /// </summary>
    /// <param name="article"></param>
    /// <returns></returns>
    [HttpPut("edit-article")]
    public IActionResult EditArticle([FromBody] EditArticleDto article)
    {
        var editResult = _articleService.EditArticle(article.Id, article.Title, article.Content, article.Tags);

        return Ok(new
        {
            succeed = editResult
        });
    }

    /// <summary>
    /// 加载预览文章列表
    /// </summary>
    /// <param name="userId">申请加载文章列表的用户 id</param>
    /// <param name="startIndex">开始下标</param>
    /// <param name="count">加载文章数，最大 20</param>
    /// <returns>
    /// 加载成功:
    /// { articles: Article[] }
    /// </returns>
    [HttpGet("load-preview-articles")]
    public IActionResult LoadArticles([FromQuery] int userId, [FromQuery] int startIndex, [FromQuery] int count)
    {
        if (count > 20)
        {
            count = 20;
        }

        var articles = _articleService.GetArticlesOfSortedByPublicationTime(startIndex, count);

        return Ok(new
        {
            articles = articles.Select(article => new
            {
                id = article.Id,
                title = article.Title,
                content = article.Content[..500],
                url = "/article?id=" + article.Id,
                publisherId = article.Publisher.Id,
                publisherUsername = article.Publisher.UserName,
                publisherHomeUrl = "/user-home?userId=" + article.Publisher.Id,
                publisherProfilePictureUrl = article.Publisher.UserProfile.ProfilePicture

            })
        });
    }

    /// <summary>
    /// 通过 id 获取文章
    /// </summary>
    /// <param name="articleId">文章 id</param>
    /// <returns>
    /// 获取成功:
    /// { article: Article }
    /// 获取失败:
    /// 404
    /// </returns>
    [HttpGet("article/{articleId:int}")]
    public IActionResult GetArticleById([FromRoute] int articleId)
    {
        var article = _articleService.GetById(articleId);
        if (article is null)
        {
            return NotFound();
        }

        return Ok(new
        {
            id = article.Id,
            title = article.Title,
            content = article.Content,
            publisherId = article.Publisher.Id
        });
    }
}
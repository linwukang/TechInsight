using Microsoft.AspNetCore.Mvc;
using TechInsight.Dto;
using TechInsight.Services;

namespace TechInsight.Controllers;

[ApiController]
[Route("articles")]
public class ArticleController : Controller
{
    public readonly IArticleService ArticleService;

    public ArticleController(IArticleService articleService)
    {
        ArticleService = articleService;
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
        var articleId = ArticleService.PublishArticle(article.UserId, article.Title, article.Content, article.Tags);

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

    [HttpPost("edit-article")]
    public IActionResult EditArticle([FromBody] EditArticleDto article)
    {
        var editResult = ArticleService.EditArticle(article.Id, article.Title, article.Content, article.Tags);

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

        var articles = ArticleService.GetArticlesOfSortedByPublicationTime(startIndex, count);

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
    /// <param name="id">文章 id</param>
    /// <returns>
    /// 获取成功:
    /// { article: Article }
    /// 获取失败:
    /// 404
    /// </returns>
    [HttpGet("article")]
    public IActionResult Article([FromQuery] int id)
    {
        var article = ArticleService.GetById(id);
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

    /// <summary>
    /// 通过文章的 id 获取文章的评论，并分页返回
    /// </summary>
    /// <param name="id">文章 id</param>
    /// <param name="pages">页数，从 0 开始计数</param>
    /// <param name="size">每页评论的数量</param>
    /// <returns></returns>
    [HttpGet("comment-list")]
    public IActionResult CommentList([FromQuery] int id, [FromQuery] int pages, [FromQuery] int size)
    {
        return Ok(new 
        {
            comments = ArticleService
                .GetComments(id, pages, size)
                .Select(comment => new
                {
                    id = comment.Id,
                    profilePicture = comment.Publisher.UserProfile.ProfilePicture,
                    userHome = "/user-home?userId=" + comment.Publisher.Id,
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
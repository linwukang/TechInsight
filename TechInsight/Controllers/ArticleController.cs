using Microsoft.AspNetCore.Mvc;
using TechInsight.Dto;
using TechInsight.Models;
using TechInsight.Services;
using TechInsight.Services.Implementation;

namespace TechInsight.Controllers;

[ApiController]
[Route("[controller]")]
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
        var articleId = ArticleService.PublishArticle(article.UserId, article.Title, article.Content);

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
    /// 加载文章列表
    /// </summary>
    /// <param name="userId">申请加载文章列表的用户 id</param>
    /// <param name="startIndex">开始下标</param>
    /// <param name="count">加载文章数，最大 20</param>
    /// <returns>
    /// 加载成功:
    /// { articles: Article[] }
    /// </returns>
    [HttpGet("load-articles")]
    public IActionResult LoadArticles([FromQuery] int userId, [FromQuery] int startIndex, [FromQuery] int count)
    {
        if (count > 20)
        {
            count = 20;
        }

        var articles = ArticleService.GetArticlesOfSortedByPublicationTime(startIndex, count);


        // {      // 文章列表
        //     id: number,         // 文章 id
        //     title: string,      // 文章标题
        //     content: string,    // 文章内容
        //     url: string,
        //     publisher: {
        //         id: number,         // 作者 id
        //         username: string,   // 作者名称
        //         homeUrl: string,    // 作者主页地址
        //         profileUrl: string, // 作者头像地址
        //     }
        // }
        return Ok(new
        {
            articles = articles.Select(article => new
            {
                id = article.Id,
                title = article.Title,
                content = article.Content,
                url = "/article?id=" + article.Id,
                publisher = new
                {
                    id = article.Publisher.Id,
                    username = article.Publisher.UserName,
                    homeUrl = "/user-home?userId=" + article.Publisher.Id,
                    profileUrl = ""
                }
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
}
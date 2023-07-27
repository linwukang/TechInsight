using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using StackExchange.Redis;
using TechInsight;
using TechInsight.Models;
using TechInsight.Services;
using TechInsight.Services.Implementation;
using TechInsightDb.Data;
using TechInsightDb.Models;
using Utils.Tokens;

namespace TechInsightTest.Services;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
#pragma warning disable CS8601
#pragma warning disable CS8602

[TestClass]
public class ArticleServiceTests
{
    private IArticleService _articleService;
    private ILoginAccountService _loginAccountService;
    private IRegisterAccountService _registerAccountService;
    private ApplicationDbContext _context;
    public const string Username1 = "xiaoming";
    public const string Password1 = "123456";
    public const string Username2 = "xiaohua";
    public const string Password2 = "123456";
    public const string Username3 = "xiaohong";
    public const string Password3 = "123456";

    private UserAccount xiaoming;
    private UserAccount xiaohua;
    private UserAccount xiaohong;

    private string token1;
    private string token2;
    private string token3;

    [TestInitialize]
    public void Init()
    {
        var services = new ServiceCollection();
        services.AddServices();
        var serviceProvider = services.BuildServiceProvider();

        _articleService = serviceProvider.GetService<IArticleService>();
        _loginAccountService = serviceProvider.GetService<ILoginAccountService>();
        _registerAccountService = serviceProvider.GetService<IRegisterAccountService>();
        _context = serviceProvider.GetService<ApplicationDbContext>();



        if (!_registerAccountService.IsUserNameExists(Username1))
        {
            _registerAccountService.RegisterAccount(Username1, Password1, "sb1@qq.com");
        }

        xiaoming = _context.UserAccounts.Single(ua => ua.UserName == Username1);

        if (!_registerAccountService.IsUserNameExists(Username2))
        {
            _registerAccountService.RegisterAccount(Username2, Password2, "sb2@qq.com");
        }

        xiaohua = _context.UserAccounts.Single(ua => ua.UserName == Username2);

        if (!_registerAccountService.IsUserNameExists(Username3))
        {
            _registerAccountService.RegisterAccount(Username3, Password3, "sb3@qq.com");
        }

        xiaohong = _context.UserAccounts.Single(ua => ua.UserName == Username3);
    }

    private void Login()
    {
        token1 = _loginAccountService.Login(Username1, Password1);
        Assert.IsNotNull(token1);
        token2 = _loginAccountService.Login(Username2, Password2);
        Assert.IsNotNull(token2);
        token3 = _loginAccountService.Login(Username3, Password3);
        Assert.IsNotNull(token3);
    }
    
    private void Logout()
    {
        _loginAccountService.Logout(Username1, token1);
        _loginAccountService.Logout(Username2, token2);
        _loginAccountService.Logout(Username3, token3);
    }

    private Article PublishArticle()
    {
        Assert.IsFalse(_loginAccountService.Logged(xiaoming.Id));
        // 未登录状态无法发布文章
        Assert.IsNull(_articleService.PublishArticle(xiaoming.Id, "全体人员向我看齐", "我宣布个事！！！", new List<string> { "test" }));

        Login();

        var articleId = _articleService.PublishArticle(xiaoming.Id, "全体人员向我看齐", "我宣布个事！！！", new List<string> { "test" });
        Assert.IsNotNull(articleId);

        var article = _articleService.GetById((int)articleId);
        Assert.IsNotNull(article);

        // 文章发布者 id 一致
        Assert.AreEqual(xiaoming.Id, _articleService.GetPublisherId(article.Id));

        Assert.AreEqual(0, _articleService.Likes(article.Id));
        Assert.AreEqual(0, _articleService.Dislikes(article.Id));

        return article;
    }

    private void DeleteArticle(int articleId)
    {
        Assert.IsTrue(_articleService.DeleteArticle(articleId, xiaoming.Id, "Test"));
    }

    private void Like(int articleId)
    {
        Assert.IsTrue(_articleService.LikeArticle(articleId, xiaoming.Id));
        Assert.AreEqual(1, _articleService.Likes(articleId));
    }

    [TestMethod]
    public void Test()
    {
        Logout();
        var article = PublishArticle();

        Like(article.Id);

        DeleteArticle(article.Id);
        Logout();
    }
}
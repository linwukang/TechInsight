using Microsoft.Extensions.DependencyInjection;
using TechInsight;
using TechInsight.Models;
using TechInsight.Services;
using TechInsightDb.Data;
using TechInsightDb.Models;

namespace TechInsightTest.Services;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
#pragma warning disable CS8601
#pragma warning disable CS8602

[TestClass]
public class CommentServiceTests
{
    private IArticleService _articleService;
    private ICommentService _commentService;
    private ILoginAccountService _loginAccountService;
    private IRegisterAccountService _registerAccountService;
    private ApplicationDbContext _context;
    public const string Username1 = "xiaoming";
    public const string Password1 = "123456";
    public const string Username2 = "xiaohua";
    public const string Password2 = "123456";
    public const string Username3 = "xiaohong";
    public const string Password3 = "123456";

    private Article _article;

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
        services.AddTechInsightServices();
        var serviceProvider = services.BuildServiceProvider();

        _articleService = serviceProvider.GetService<IArticleService>();
        _commentService = serviceProvider.GetService<ICommentService>();
        _loginAccountService = serviceProvider.GetService<ILoginAccountService>();
        _registerAccountService = serviceProvider.GetService<IRegisterAccountService>();
        _context = serviceProvider.GetService<ApplicationDbContext>();
        // 
        if (!_registerAccountService.IsUserNameExists(Username1))
            _registerAccountService.RegisterAccount(Username1, Password1, "sb1@qq.com");

        xiaoming = _context.UserAccounts.Single(ua => ua.UserName == Username1);

        if (!_registerAccountService.IsUserNameExists(Username2))
            _registerAccountService.RegisterAccount(Username2, Password2, "sb2@qq.com");

        xiaohua = _context.UserAccounts.Single(ua => ua.UserName == Username2);

        if (!_registerAccountService.IsUserNameExists(Username3))
            _registerAccountService.RegisterAccount(Username3, Password3, "sb3@qq.com");

        xiaohong = _context.UserAccounts.Single(ua => ua.UserName == Username3);
        //
        _article = _context.Articles.Find(3);
    }

    [TestMethod]
    private void Login()
    {
        token1 = _loginAccountService.Login(Username1, Password1);
        Assert.IsNotNull(token1);
        token2 = _loginAccountService.Login(Username2, Password2);
        Assert.IsNotNull(token2);
        token3 = _loginAccountService.Login(Username3, Password3);
        Assert.IsNotNull(token3);
    }
    
    [TestMethod]
    private void Logout()
    {
        _loginAccountService.Logout(Username1, token1);
        _loginAccountService.Logout(Username2, token2);
        _loginAccountService.Logout(Username3, token3);
    }

    // 发布评论
    private void PublishComment()
    {
        // 未登录状态无法评论
        Assert.IsNull(_commentService.CommentArticle(_article.Id, xiaohua.Id, "woc!!"));
        Login();
        var commentId = _commentService.CommentArticle(_article.Id, xiaohua.Id, "woc!!");
        Assert.IsNotNull(commentId);

        var comment = _commentService.GetById((int)commentId) ?? throw new NullReferenceException();

        var articleId = _commentService.GetArticleId(comment.Id) ?? throw new NullReferenceException();
        Assert.AreEqual(_article.Id, articleId);

        var commenterId = _commentService.GetCommenterId(comment.Id) ?? throw new NullReferenceException();
        Assert.AreEqual(xiaohua.Id, commenterId);

    }

    [TestMethod]
    public void Test()
    {
        Logout();
        PublishComment();

        Logout();
    }
}
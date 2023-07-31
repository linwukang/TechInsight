using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TechInsight;
using TechInsight.Services;
using TechInsight.Services.Implementation;
using TechInsightDb.Data;
using Utils.Tokens;

namespace TechInsightTest.Services;
#pragma warning disable CS8618
#pragma warning disable CS8601

[TestClass]
public class LoginAccountTests
{
    private ILoginAccountService _loginAccountService;
    private IRegisterAccountService _registerAccountService;
    private ApplicationDbContext _context;

    [TestInitialize]
    public void Init()
    {
        var services = new ServiceCollection();
        services.AddTechInsightServices();

        var serviceProvider = services.BuildServiceProvider();

        _loginAccountService = serviceProvider.GetService<ILoginAccountService>();
        _registerAccountService = serviceProvider.GetService<IRegisterAccountService>();
        _context = serviceProvider.GetService<ApplicationDbContext>();
    }

    [TestMethod]
    public void Test()
    {
        const string username = "xiaoming";
        const string password = "123456";

        if (!_registerAccountService.IsUserNameExists(username))
        {
            _registerAccountService.RegisterAccount(username, password, "sb@qq.com");
        }

        var xiaoming = _context.UserAccounts.Single(ua => ua.UserName == username);


        Assert.IsFalse(_loginAccountService.Logged(xiaoming.Id));
        var token = _loginAccountService.Login(username, password);

        Assert.IsNotNull(token);
        Assert.IsTrue(Token.ValidateToken(token, LoginAccountService.Issuer, xiaoming.Id.ToString()));

        Assert.IsNull(_loginAccountService.Login(username, password));
        Assert.IsNull(_loginAccountService.Login(username, password));
        Assert.IsNull(_loginAccountService.Login(username, password));

        Assert.IsTrue(_loginAccountService.Logout(username, token));
        Assert.IsFalse(_loginAccountService.Logout(username, token));
        Assert.IsFalse(_loginAccountService.Logout(username, token));
        Assert.IsFalse(_loginAccountService.Logout(username, token));
        
    }
}
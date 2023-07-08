using StackExchange.Redis;
using TechInsight.Data;
using TechInsight.Services.Implementation;
using TechInsight.Services;

namespace TechInsight;

public class DI
{
    public static void Configuration(IServiceCollection serviceCollection)
    {
        /*serviceCollection.AddSingleton<IRedisDictionary<string, string>>(
    new RedisDictionary<string, string>(
        "localhost:6379",
        0,
        new StringSerializer(),
        new StringSerializer(),
        "TechInsight",
        ":"));*/

        // 添加 redis 数据库操作对象到容器
        var config = ConfigurationOptions.Parse("localhost:6379");
        var connect = ConnectionMultiplexer.Connect(config);
        serviceCollection.AddSingleton<IDatabase>(connect.GetDatabase(0));

        //添加 EF Core 数据库上下文对象到容器
        serviceCollection.AddSingleton(new ApplicationDbContext());

        serviceCollection
            .AddSingleton<ILoginAccountService, LoginAccountService>()
            .AddSingleton<IRegisterAccountService, RegisterAccountService>()
            .AddSingleton<IArticleService, ArticleService>()
            .AddSingleton<ICommentService, CommentService>();
    }
}
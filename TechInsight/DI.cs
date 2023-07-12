using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using TechInsight.Services.Implementation;
using TechInsight.Services;
using Microsoft.Extensions.Logging;
using TechInsight.Configurations;
using TechInsightDb.Data;

namespace TechInsight;

public class DI
{
    public static readonly Logger<DI> Logger = new(new LoggerFactory());

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
        /*var config = ConfigurationOptions.Parse("localhost:6379");
        var connect = ConnectionMultiplexer.Connect(config);
        serviceCollection.AddSingleton<IDatabase>(connect.GetDatabase(0));*/

        try
        {
            var config = ConfigurationOptions.Parse("localhost:6379");
            var connect = ConnectionMultiplexer.Connect(config);
            serviceCollection.AddSingleton(connect.GetDatabase(0));
            // 添加分布式 redis 缓存对象到容器
            serviceCollection.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
                options.InstanceName = "TechInsight";
            });
        }
        catch (Exception _)
        {
            Logger.LogError("redis connection failed");
            // 添加 redis 缓存对象失败，则将分布式内存缓存对象到容器
            serviceCollection.AddDistributedMemoryCache();
        }

        var dbConfig = new ConfigurationBuilder()
            .AddJsonFile("dbsettings.json")
            .Build();

        serviceCollection.AddSingleton<DbConnectionConfiguration>(new DbConnectionConfiguration(dbConfig.GetSection("EFCore:DbContext:Connection")));

        //添加 EF Core 数据库上下文对象到容器
        serviceCollection.AddSingleton(new ApplicationDbContext(dbConfig.GetSection("EFCore:DbContext:Connection")));
        

        serviceCollection
            .AddSingleton<ILoginAccountService, LoginAccountService>()
            .AddSingleton<IRegisterAccountService, RegisterAccountService>()
            .AddSingleton<IArticleService, ArticleService>()
            .AddSingleton<ICommentService, CommentService>()
            .AddSingleton<IUserInfoService, UserInfoService>();
    }
}
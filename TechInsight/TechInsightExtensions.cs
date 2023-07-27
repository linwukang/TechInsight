using StackExchange.Redis;
using TechInsight.Services.Implementation;
using TechInsight.Services;
using TechInsightDb.Data;

namespace TechInsight;

public static class TechInsightExtensions
{
    /*public static void Configuration(IServiceCollection serviceCollection)
    {
        var config = ConfigurationOptions.Parse("localhost:6379");
        var connect = ConnectionMultiplexer.Connect(config);
        serviceCollection.AddSingleton(connect.GetDatabase(0));

        var appConfig = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var redisConfig = appConfig.GetSection("Redis:Connection");
        // 添加分布式 redis 缓存对象到容器
        serviceCollection.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"{redisConfig["Host"]}:{redisConfig["Post"]}";
            options.InstanceName = redisConfig["InstanceName"];
        });


        serviceCollection
            // 添加 EF Core 数据库上下文对象到容器
            .AddScoped(_ => new ApplicationDbContext(appConfig.GetSection("EFCore:DbContext:Connection")))
            // 添加服务到容器
            .AddScoped<ILoginAccountService, LoginAccountService>()
            .AddScoped<IRegisterAccountService, RegisterAccountService>()
            .AddScoped<IArticleService, ArticleService>()
            .AddScoped<ICommentService, CommentService>()
            .AddScoped<IUserInfoService, UserInfoService>();
    }*/

    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        var config = ConfigurationOptions.Parse("localhost:6379");
        var connect = ConnectionMultiplexer.Connect(config);
        serviceCollection.AddSingleton(connect.GetDatabase(0));

        var appConfig = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var redisConfig = appConfig.GetSection("Redis:Connection");
        // 添加分布式 redis 缓存对象到容器
        serviceCollection.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = $"{redisConfig["Host"]}:{redisConfig["Post"]}";
            options.InstanceName = redisConfig["InstanceName"];
        });


        serviceCollection
            // 添加 EF Core 数据库上下文对象到容器
            .AddScoped(_ => new ApplicationDbContext(appConfig.GetSection("EFCore:DbContext:Connection")))
            // 添加服务到容器
            .AddScoped<ILoginAccountService, LoginAccountService>()
            .AddScoped<IRegisterAccountService, RegisterAccountService>()
            .AddScoped<IArticleService, ArticleService>()
            .AddScoped<ICommentService, CommentService>()
            .AddScoped<IUserInfoService, UserInfoService>()
            .AddScoped<ITagService, TagService>();
        return serviceCollection;
    }
}
using Microsoft.Extensions.Primitives;

namespace TechInsight.Configurations;

public class DbConnectionConfiguration : IConfiguration
{
    private readonly IConfiguration _connectionConfiguration;

    public DbConnectionConfiguration(IConfiguration configuration)
    {
        _connectionConfiguration = configuration;
    }

    public IConfigurationSection GetSection(string key)
    {
        return _connectionConfiguration.GetSection(key);
    }

    public IEnumerable<IConfigurationSection> GetChildren()
    {
        return _connectionConfiguration.GetChildren();
    }

    public IChangeToken GetReloadToken()
    {
        return _connectionConfiguration.GetReloadToken();
    }

    public string? this[string key]
    {
        get => _connectionConfiguration[key];
        set => _connectionConfiguration[key] = value;
    }
}
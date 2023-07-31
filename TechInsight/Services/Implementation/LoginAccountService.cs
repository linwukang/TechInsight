using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using TechInsight.Configurations;
using TechInsight.Models.Types;
using TechInsightDb.Data;
using Utils.Tokens;

namespace TechInsight.Services.Implementation;

public class LoginAccountService : ILoginAccountService
{
    public LoginAccountService(IDistributedCache cache, ApplicationDbContext repositories, IDatabase redis)
    {
        Repositories = repositories;
        Cache = cache;
        Redis = redis;
    }

    public readonly ApplicationDbContext Repositories;
    public readonly IDatabase Redis;
    public readonly IDistributedCache Cache;

    public const string Issuer = "System";
    public const string UserIdToToken = "LoginAccountService:Tokens:UserId:";
    public const string TokenToUserId = "LoginAccountService:Tokens:Token:";

    

    public int? GetUserAccountId(string username)
    {
        return Repositories
            .UserAccounts
            .FirstOrDefault(ac => ac.UserName == username)
            ?.Id;
    }

    public string? Login(string username, string password)
    {
        var account = Repositories
            .UserAccounts
            .Where(ua => ua.IsDeleted == null)
            .FirstOrDefault(
                ua =>
                    ua.UserName == username
                    && ua.Password == password);

        if (account is null) return null;
        if (Logged(account.Id)) return null;

        var token = Token.GenerateToken(Issuer, account.Id.ToString());

        Cache.Set(UserIdToToken + account.Id, Encoding.UTF8.GetBytes(token));
        Cache.Set(TokenToUserId + token, Encoding.UTF8.GetBytes(account.Id.ToString()));

        return token;
    }

    public bool Logged(int userId)
    {
        var account = Repositories
            .UserAccounts
            .Where(ac => ac.IsDeleted == null)
            .FirstOrDefault(ac => ac.Id == userId);

        if (account is null)
        {
            return false;
        }

        var token = Encoding.UTF8.GetString(Cache.Get(UserIdToToken + account.Id) ?? new byte[] { });

        return Token.ValidateToken(token, Issuer, account.Id.ToString());
    }

    public bool Logged(string username)
    {
        var account = Repositories
            .UserAccounts
            .Where(ac => ac.IsDeleted == null)
            .FirstOrDefault(ac => ac.UserName == username);

        return account is not null && Logged(account.Id);
    }

    public bool Logout(string username, string token)
    {
        var account = Repositories
            .UserAccounts
            .Where(ac => ac.IsDeleted == null)
            .FirstOrDefault(ac => ac.UserName == username);

        return account is not null 
               && Logout(account.Id, token);
    }

    public bool Logout(int userId, string token)
    {

        if (!Logged(userId))
        {
            return false;
        }
        if (Encoding.UTF8.GetString(Cache.Get(UserIdToToken + userId) ?? new byte[]{}) != token)
        {
            return false;
        }

        Cache.Remove(UserIdToToken + userId);
        Cache.Remove(TokenToUserId + token);

        return true;
    }

    public bool IsReviewer(int userId)
    {
        return Repositories
            .UserAccounts
            .First(ac => ac.Id == userId)
            .Role == UserRole.Reviewer;
    }
}
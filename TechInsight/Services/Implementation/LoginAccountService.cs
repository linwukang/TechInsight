using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using TechInsight.Configurations;
using TechInsightDb.Data;
using Utils.Tokens;

namespace TechInsight.Services.Implementation;

public class LoginAccountService : ILoginAccountService
{
    public readonly ApplicationDbContext Repositories;
    // public IDatabase Redis { get; private set; }
    public readonly IDistributedCache Cache;

<<<<<<< HEAD
    public LoginAccountService(IDistributedCache cache, ApplicationDbContext repositories)
    {
        Repositories = repositories;
=======
    public LoginAccountService(IDistributedCache cache, DbConnectionConfiguration dbConfiguration)
    {
        Repositories = Repositories = new ApplicationDbContext(dbConfiguration);
>>>>>>> 135e118874173a81c0d269ddc4736f4d89ac62fd
        Cache = cache;
    }

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

        // Redis[UserIdToToken + account.Id] = token;
        // Redis[TokenToUserId + token] = account.Id.ToString();

        // Redis.StringSet(UserIdToToken + account.Id, token);
        // Redis.StringSet(TokenToUserId + token, account.Id.ToString());

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

        // return Redis.TryGetValue(UserIdToToken + account.Id, out var token) 
        //        && Token.ValidateToken(token, Issuer, account.Id.ToString());
        // var token = Redis.StringGet(UserIdToToken + account.Id);
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

        if (account is null)
        {
            return false;
        }

        return Logout(account.Id, token);
    }

    public bool Logout(int userId, string token)
    {
        /*if (!Logged(userId))
        {
            return false;
        }
        if (Redis.StringGet(UserIdToToken + userId).ToString() != token)
        {
            return false;
        }

        return Redis.KeyDelete(UserIdToToken + userId)
               && Redis.KeyDelete(TokenToUserId + token);*/

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
}
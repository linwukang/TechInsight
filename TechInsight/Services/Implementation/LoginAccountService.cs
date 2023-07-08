using StackExchange.Redis;
using TechInsight.Data;
using Utils.Interface.Implementation;
using Utils.Redis;
using Utils.Redis.Implementation;
using Utils.Tokens;

namespace TechInsight.Services.Implementation;

public class LoginAccountService : ILoginAccountService
{
    public readonly ApplicationDbContext Repositories;

    public LoginAccountService(ApplicationDbContext repositories, IDatabase redis)
    {
        Repositories = repositories;
        Redis = redis;
    }

    public const string Issuer = "System";
    public const string UserIdToToken = "LoginAccountService:Tokens:UserId:";
    public const string TokenToUserId = "LoginAccountService:Tokens:Token:";

    public IDatabase Redis { get; private set; }

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

        Redis.StringSet(UserIdToToken + account.Id, token);
        Redis.StringSet(TokenToUserId + token, account.Id.ToString());

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
        var token = Redis.StringGet(UserIdToToken + account.Id);

        return Token.ValidateToken(token.ToString(), Issuer, account.Id.ToString());
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

        if (account is null || !Logged(account.Id))
        {
            return false;
        }

        // if (Redis[UserIdToToken + account.Id] != token)
        // {
        //     return false;
        // }
        //
        // return Redis.Remove(UserIdToToken + account.Id) 
        //        && Redis.Remove(TokenToUserId + token);

        if (Redis.StringGet(UserIdToToken + account.Id).ToString() != token)
        {
            return false;
        }

        return Redis.KeyDelete(UserIdToToken + account.Id) 
               && Redis.KeyDelete(TokenToUserId + token);
    }
}
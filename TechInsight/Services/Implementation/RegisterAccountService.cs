using Microsoft.EntityFrameworkCore;
using TechInsight.Configurations;
using TechInsight.Models.Types;
using TechInsightDb.Data;
using TechInsightDb.Models;

namespace TechInsight.Services.Implementation;

public class RegisterAccountService : IRegisterAccountService, IDisposable
{
    public ApplicationDbContext Repositories { get; set; }

<<<<<<< HEAD
    public RegisterAccountService(ApplicationDbContext repositories)
    {
        Repositories = repositories;
=======
    public RegisterAccountService(DbConnectionConfiguration dbConfiguration)
    {
        Repositories = Repositories = new ApplicationDbContext(dbConfiguration);
>>>>>>> 135e118874173a81c0d269ddc4736f4d89ac62fd
    }

    public UserAccount? RegisterAccount(string username, string password, string email)
    {
        if (ExistsUserName(username))
        {
            return null;
        }

        var userAccount = new UserAccount()
        {
            UserName = username,
            Password = password,
            CreateTime = DateTime.Now,
            Email = email,
            Role = UserRole.Normal,
            UserProfile = new UserProfile()
        };

        var entityEntry = Repositories
            .UserAccounts
            .Add(userAccount);
        var changes = Repositories.SaveChanges();

        return changes != 0
            ? entityEntry.Entity
            : null;
    }

    public bool ExistsUserName(string username)
    {
        var count = Repositories
            .UserAccounts
            .Count(ua => ua.UserName == username);

        return count > 0;
    }

    public bool DeleteAccount(int id, string? deleteReasons, int operatorId)
    {
        var userAccounts = Repositories
            .UserAccounts
            .Where(ua => ua.Id == id)
            .ToList();

        var operatorAccounts = Repositories
            .UserAccounts
            .Where(ua => ua.Id == operatorId)
            .ToList();

        if (userAccounts.Count == 0 || operatorAccounts.Count == 0)
        {
            return false;
        }

        var userAccount = userAccounts.First();
        var operatorAccount = operatorAccounts.First();

        userAccount.IsDeleted = new UserAccountDeleted()
        {
            DeleteTime = DateTime.Now,
            DeleteReasons = deleteReasons,
            Operator = operatorAccount
        };

        Repositories.UserAccounts.Update(userAccount);
        var changes = Repositories.SaveChanges();

        return changes != 0;
    }

    public bool RealDeleteAccount(int id)
    {
        var userAccount = Repositories
            .UserAccounts
            .Include(ua => ua.IsDeleted)
            .IgnoreQueryFilters()
            .FirstOrDefault(ua => ua.Id == id);

        if (userAccount?.IsDeleted == null)
        {
            return false;
        }

        var deletedAccount = Repositories
            .UserAccountDeleted
            .First(uad => userAccount.IsDeleted.Id == uad.Id);
        userAccount.IsDeleted = null;
        Repositories.UserAccounts.Update(userAccount);
        Repositories.SaveChanges();

        Repositories.UserAccountDeleted.Remove(deletedAccount);
        Repositories.SaveChanges();

        Repositories.UserAccounts.Remove(userAccount);
        var changes = Repositories.SaveChanges();

        return changes != 0;
    }

    public void Dispose()
    {
        Repositories.Dispose();
    }
}
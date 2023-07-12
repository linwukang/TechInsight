using Microsoft.EntityFrameworkCore;
using TechInsight.Configurations;
using TechInsightDb.Data;
using TechInsightDb.Models;

namespace TechInsight.Services.Implementation;

public class UserInfoService : IUserInfoService
{
    public readonly ApplicationDbContext Repositories;

    public UserInfoService(DbConnectionConfiguration dbConfiguration)
    {
        this.Repositories = new ApplicationDbContext(dbConfiguration);
    }

    protected int? GetUserProfileByUserId(int userId)
    {
        return Repositories
            .UserAccounts
            .Where(ua => ua.Id == userId)
            .Select(ua => ua.UserProfile.Id)
            .FirstOrDefault();
    }

    public string? GetUserName(int userId)
    {
        return Repositories
            .UserAccounts
            .Find(userId)
            ?.UserName;
    }

    public UserProfile? GetUserProfile(int userId)
    {
        return Repositories
            .UserAccounts
            .Include(ua => ua.UserProfile)
            .FirstOrDefault(ua => ua.Id == userId)
            ?.UserProfile;
    }

    public bool SetUserProfile(UserProfile userProfile)
    {
        Repositories.UserProfiles.Update(userProfile);
        return Repositories.SaveChanges() != 0;
    }

    public bool SetPhoneNumber(int userId, string phoneNumber)
    {
        var userProfile = GetUserProfile(userId);
        if (userProfile is null) return false;

        userProfile.PhoneNumber = phoneNumber;

        Repositories.UserProfiles.Update(userProfile);

        return Repositories.SaveChanges() != 0;
    }

    public bool SetDateOfBirth(int userId, DateTime dateOfBirth)
    {
        var userProfile = GetUserProfile(userId);
        if (userProfile is null) return false;

        userProfile.DateOfBirth = dateOfBirth;

        Repositories.UserProfiles.Update(userProfile);

        return Repositories.SaveChanges() != 0;
    }

    public bool SetGender(int userId, string gender)
    {
        var userProfile = GetUserProfile(userId);
        if (userProfile is null) return false;

        userProfile.Gender = gender;

        Repositories.UserProfiles.Update(userProfile);

        return Repositories.SaveChanges() != 0;
    }

    public bool SetProfilePicture(int userId, string profilePicture)
    {
        var userProfile = GetUserProfile(userId);
        if (userProfile is null) return false;

        userProfile.ProfilePicture = profilePicture;

        Repositories.UserProfiles.Update(userProfile);

        return Repositories.SaveChanges() != 0;
    }

    public bool SetBio(int userId, string bio)
    {
        var userProfile = GetUserProfile(userId);
        if (userProfile is null) return false;

        userProfile.Bio = bio;

        Repositories.UserProfiles.Update(userProfile);

        return Repositories.SaveChanges() != 0;
    }
}
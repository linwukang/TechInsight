using TechInsightDb.Models;

namespace TechInsight.Services;

public interface IUserInfoService
{
    string? GetUserName(int userId);

    UserProfile? GetUserProfile(int userId);

    bool SetUserProfile(UserProfile userProfile);

    bool SetPhoneNumber(int userId, string phoneNumber);

    bool SetDateOfBirth(int userId, DateTime dateOfBirth);

    bool SetGender(int userId, string gender);

    bool SetProfilePicture(int userId, string profilePicture);

    bool SetBio(int userId, string bio);
}
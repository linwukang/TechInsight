using Microsoft.AspNetCore.Mvc;
using TechInsight.Services;

namespace TechInsight.Controllers;

[ApiController]
[Route("user-info")]
public class UserInfoController : Controller
{
    public readonly IUserInfoService UserInfoService;
    public readonly ILogger<UserInfoController> Logger;

    public UserInfoController(IUserInfoService userInfoService, ILogger<UserInfoController> logger)
    {
        UserInfoService = userInfoService;
        Logger = logger;
    }


    [HttpGet]
    public IActionResult UserInfo([FromQuery] int id)
    {
        var userName = UserInfoService.GetUserName(id);
        var userProfile = UserInfoService.GetUserProfile(id);

        if (userName is null || userProfile is null)
        {
            Logger.LogWarning($"User id {id} does not exist");
            return NotFound(new
            {
                message = "该用户不存在"
            });
        }

        return Ok(new
        {
            username = userName,
            bio = userProfile.Bio,
            dataOfBirth = userProfile.DateOfBirth,
            gender = userProfile.Gender,
            phoneNumber = userProfile.PhoneNumber,
            profilePicture = userProfile.ProfilePicture
        });
    }
}
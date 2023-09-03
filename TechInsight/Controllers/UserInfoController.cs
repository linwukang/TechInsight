using Microsoft.AspNetCore.Mvc;
using TechInsight.Services;

namespace TechInsight.Controllers;

[ApiController]
[Route("user-info")]
public class UserInfoController : Controller
{
    private readonly IUserInfoService _userInfoService;
    private readonly ILogger<UserInfoController> _logger;

    public UserInfoController(IUserInfoService userInfoService, ILogger<UserInfoController> logger)
    {
        _userInfoService = userInfoService;
        _logger = logger;
    }


    [HttpGet]
    public IActionResult UserInfo([FromQuery] int id)
    {
        var userName = _userInfoService.GetUserName(id);
        var userProfile = _userInfoService.GetUserProfile(id);

        if (userName is not null && userProfile is not null)
            return Ok(new
            {
                username = userName,
                bio = userProfile.Bio,
                dataOfBirth = userProfile.DateOfBirth,
                gender = userProfile.Gender,
                phoneNumber = userProfile.PhoneNumber,
                profilePicture = userProfile.ProfilePicture
            });
        
        _logger.LogWarning($"User id {id} does not exist");
        return NotFound(new
        {
            message = "该用户不存在"
        });

    }
}
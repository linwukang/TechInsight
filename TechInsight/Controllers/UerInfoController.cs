using Microsoft.AspNetCore.Mvc;
using TechInsight.Services;

namespace TechInsight.Controllers;

[ApiController]
[Route("[controller]")]
public class UerInfoController : Controller
{
    public readonly IUserInfoService UserInfoService;

    public UerInfoController(IUserInfoService userInfoService)
    {
        UserInfoService = userInfoService;
    }


    [HttpGet("user-info")]
    public IActionResult GetUserInfo([FromQuery] int id)
    {
        return Ok();
    }
}
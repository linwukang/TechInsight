using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechInsight.Services;
using TechInsight.Services.Implementation;

namespace TechInsight.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    public readonly LoginAccountService LoginAccountService;

    public AccountController(LoginAccountService loginAccountService)
    {
        LoginAccountService = loginAccountService;
    }

    [HttpGet(Name = "login")]
    public IActionResult Login([FromQuery] string username, [FromQuery] string password)
    {
        var userAccountId = LoginAccountService.GetUserAccountId(username);
        if (userAccountId is null)
        {
            return BadRequest(new
            {
                message = "登录失败，用户名或密码错误"
            });
        }

        var token = LoginAccountService.Login(username, password);

        if (token is null)
        {
            return BadRequest(new
            {
                message = "登录失败，用户名或密码错误"
            });
        }

        return Ok(new
        {
            token = token,
            userId = userAccountId
        });
    }
}
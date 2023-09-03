using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechInsight.Services;

namespace TechInsight.Controllers;

[ApiController]
[Route("accounts")]
public class AccountController : ControllerBase
{
    private readonly ILoginAccountService _loginAccountService;
    private readonly ILogger<AccountController> _logger;
    public AccountController(ILoginAccountService loginAccountService, ILogger<AccountController> logger)
    {
        _loginAccountService = loginAccountService;
        _logger = logger;
    }

    /// <summary>
    /// 账号登录
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <returns>
    /// 登录成功返回:
    /// { logged = true, token: string, userId: int }
    /// 登录失败返回:
    /// { logged = false, message: string }
    /// </returns>
    [HttpPost("login/{username}")]
    public IActionResult Login([FromRoute] string? username, [FromQuery] string? password)
    {
        if (username is null)
        {
            return Ok(new
            {
                logged = false,
                message = "请输入用户名"
            });
        }

        if (password is null)
        {
            return Ok(new
            {
                logged = false,
                message = "请输入密码"
            });
        }

        var userAccountId = _loginAccountService.GetUserAccountId(username);
        if (userAccountId is null)
        {
            _logger.LogInformation($"login failure. username: {username}, password: {password}.");
            return Ok(new
            {
                logged = false,
                message = "登录失败，用户名或密码错误"
            });
        }

        if (_loginAccountService.Logged(username))
        {
            return Ok(new
            {
                logged = false,
                message = "用户账号已登录"
            });
        }

        var token = _loginAccountService.Login(username, password);

        if (token is null)
        {
            _logger.LogInformation($"login failure. username: {username}, password: {password}.");
            return Ok(new
            {
                logged = false,
                message = "登录失败，用户名或密码错误"
            });
        }

        _logger.LogInformation($"login successfully. username: {username}, password: {password}.");
        return Ok(new
        {
            logged = true,
            token = token,
            userId = userAccountId
        });
    }

    /// <summary>
    /// 检查用户是否登录
    /// </summary>
    /// <param name="username">用户名</param>
    /// <returns>
    /// 用户名不存在:
    /// { message: "用户名不存在" }
    /// 用户未登录:
    /// { logged: false }
    /// 用户已登录:
    /// { logged: true }
    /// </returns>
    [HttpPost("logged/{username}")]
    public IActionResult Logged([FromRoute] string username)
    {
        var userId = _loginAccountService.GetUserAccountId(username);
        if (userId is null)
        {
            return Ok(new
            {
                logged = false,
                message = "用户名不存在"
            });
        }

        if (_loginAccountService.Logged((int) userId))
        {
            return Ok(new
            {
                logged = true
            });
        }

        return Ok(new
        {
            logged = false
        });
    }

    /// <summary>
    /// 账号登出
    /// </summary>
    /// <param name="userId">用户 id</param>
    /// <param name="token">token</param>
    /// <returns>
    /// 账号未登录:
    /// { logout: false, message: "用户未登录" }
    /// 登出成功:
    /// { logout: true }
    /// 登出失败:
    /// { logout: false }
    /// </returns>
    [HttpPost("logout/{userId:int}")]
    public IActionResult Logout([FromRoute] int userId, [FromQuery] string token)
    {
        if (!_loginAccountService.Logged(userId))
        {
            // 账号未登录
            return Ok(new
            {
                logout = false,
                message = "用户未登录"
            });
        }

        if (_loginAccountService.Logout(userId, token))
        {
            _logger.LogInformation($"Logout successfully. userId: {userId}.");
            // 登出成功
            return Ok(new
            {
                logout = true
            });
        }

        // 登出失败
        return Ok(new
        {
            logout = false
        });
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechInsight.Services;

namespace TechInsight.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    public readonly ILoginAccountService LoginAccountService;
    public readonly ILogger<AccountController> Logger;
    public AccountController(ILoginAccountService loginAccountService, ILogger<AccountController> logger)
    {
        LoginAccountService = loginAccountService;
        Logger = logger;
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
    [HttpGet("login")]
    public IActionResult Login([FromQuery] string? username, [FromQuery] string? password)
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

        var userAccountId = LoginAccountService.GetUserAccountId(username);
        if (userAccountId is null)
        {
            Logger.LogInformation($"login failure. username: {username}, password: {password}.");
            return Ok(new
            {
                logged = false,
                message = "登录失败，用户名或密码错误"
            });
        }

        if (LoginAccountService.Logged(username))
        {
            return Ok(new
            {
                logged = false,
                message = "用户账号已登录"
            });
        }

        var token = LoginAccountService.Login(username, password);

        if (token is null)
        {
            Logger.LogInformation($"login failure. username: {username}, password: {password}.");
            return Ok(new
            {
                logged = false,
                message = "登录失败，用户名或密码错误"
            });
        }

        Logger.LogInformation($"login successfully. username: {username}, password: {password}.");
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
    [HttpGet("logged")]
    public IActionResult Logged([FromQuery] string username)
    {
        var userId = LoginAccountService.GetUserAccountId(username);
        if (userId is null)
        {
            return Ok(new
            {
                logged = false,
                message = "用户名不存在"
            });
        }

        if (LoginAccountService.Logged((int) userId))
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
    [HttpGet("logout")]
    public IActionResult Logout([FromQuery] int userId, [FromQuery] string token)
    {
        if (!LoginAccountService.Logged(userId))
        {
            // 账号未登录
            return Ok(new
            {
                logout = false,
                message = "用户未登录"
            });
        }

        if (LoginAccountService.Logout(userId, token))
        {
            Logger.LogInformation($"Logout successfully. userId: {userId}.");
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
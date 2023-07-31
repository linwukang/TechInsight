namespace TechInsight.Services;

public interface ILoginAccountService
{
    /// <summary>
    /// 通过用户名获取用户 id
    /// </summary>
    /// <param name="username">用户名</param>
    /// <returns>用户存在返回 id，否则返回 null</returns>
    public int? GetUserAccountId(string username);
    /// <summary>
    /// 通过用户名和密码登录
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <returns>token</returns>
    string? Login(string username, string password);

    /// <summary>
    /// 判断用户是否已登录
    /// </summary>
    /// <param name="userId">用户 id</param>
    /// <returns>已登录返回 true，未登录返回 false</returns>
    bool Logged(int userId);

    /// <summary>
    /// 判断用户是否已登录
    /// </summary>
    /// <param name="username">用户名</param>
    /// <returns>已登录返回 true，未登录返回 false</returns>
    bool Logged(string username);

    /// <summary>
    /// 账号登出
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="token">token</param>
    /// <returns>登出成功返回 true，登出失败返回false</returns>
    bool Logout(string username, string token);

    /// <summary>
    /// 账号登出
    /// </summary>
    /// <param name="userId">用户 id</param>
    /// <param name="token">token</param>
    /// <returns>登出成功返回 true，登出失败返回false</returns>
    bool Logout(int userId, string token);

    /// <summary>
    /// 判断用户是否为审核员
    /// </summary>
    /// <param name="userId">用户 id</param>
    /// <returns>是审核员返回 true，否则返回 false</returns>
    bool IsReviewer(int userId);
}
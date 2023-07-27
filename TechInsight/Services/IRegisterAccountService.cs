using TechInsightDb.Models;

namespace TechInsight.Services;

public interface IRegisterAccountService
{
    /// <summary>
    /// 注册账号
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <param name="email">电子邮箱地址</param>
    /// <returns>注册成功返回 UserAccount 对象，失败返回 null</returns>
    UserAccount? RegisterAccount(string username, string password, string email);

    /// <summary>
    /// 判断用户名是否已存在
    /// </summary>
    /// <param name="username">用户名</param>
    /// <returns>存在返回 true，不存在返回 false</returns>
    bool IsUserNameExists(string username);

    /// <summary>
    /// 删除用户账号
    /// 在数据库中添加删除标记
    /// </summary>
    /// <param name="id">需要删除的账号 id</param>
    /// <param name="deleteReasons">删除原因</param>
    /// <param name="operatorId">删除操作者</param>
    /// <returns>是否删除成功</returns>
    bool DeleteAccount(int id, string? deleteReasons, int operatorId);

    /// <summary>
    /// 真正的删除用户账号，指定的账号必须是被标记为已删除的
    /// </summary>
    /// <param name="id">需要删除的用户 id</param>
    /// <returns>是否删除成功</returns>
    bool RealDeleteAccount(int id);
}
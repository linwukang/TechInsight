using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechInsight.Models;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

/**
 * <summary>
 * 用户账号表
 * </summary>
 */
[Table("t_user_accounts")]
public class UserAccount
{
    /**
     * <summary>
     * Id
     * </summary>
     */
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ua_id")]
    public int Id { get; set; }

    /**
     * <summary>
     * 用户名
     * 非空，3到20个字符
     * </summary>
     */
    [Required]
    [MinLength(3), MaxLength(20)]
    [Column("ua_username")]
    public string UserName { get; set; }

    /**
     * <summary>
     * 密码
     * 非空，6到20个 ASCII 字符
     * </summary>
     */
    [Required]
    [MinLength(6), MaxLength(20)]
    [Column("ua_password")]
    public string Password { get; set; }

    /**
     * <summary>
     * 邮箱地址
     * 非空
     * </summary>
     */
    [Required]
    [EmailAddress]
    [Column("ua_email")]
    public string Email { get; set; }

    /**
     * <summary>
     * 账号状态
     * </summary>
     */
    [Required]
    [Column("ua_state")]
    public string State { get; set; }

    /**
     * <summary>
     * 用户信息
     * </summary>
     */
    [Required]
    [ForeignKey("ua_user_profile_id")]
    public UserProfile UserProfile { get; set; }

    /**
     * <summary>
     * 用户账号创建时间
     * </summary>
     */
    [Required]
    [Column("ua_create_time")]
    public DateTime CreateTime { get; set; } = DateTime.Now;

    /**
     * <summary>
     * null 值则表示未删除
     * </summary>
     */
    [Column("ua_deleted_id")]
    public UserAccountDeleted? IsDeleted { get; set; }

    
}
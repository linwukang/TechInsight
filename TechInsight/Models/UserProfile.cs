using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechInsight.Models;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

/**
 * <summary>
 * 用户信息
 * 与用户账号一一对应
 * </summary>
 */
[Table("t_user_profiles")]
public class UserProfile
{
    /**
     * <summary>
     * Id
     * </summary>
     */
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("up_id")]
    public int Id { get; set; }

    /**
     * <summary>
     * 电话号码
     * 可空，0到20个字符
     * </summary>
     */
    [Column("up_phone_number")]
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    /**
     * <summary>
     * 出生日期
     * 可空
     * </summary>
     */
    [Column("up_date_of_birth")]
    public DateTime? DateOfBirth { get; set; }

    /**
     * <summary>
     * 性别
     * 可空，0到2个字符
     * </summary>
     */
    [Column("up_gender")]
    [MaxLength(2)]
    public string? Gender { get; set; }

    /**
     * <summary>
     * 头像地址
     * 可空，0到128个字符
     * </summary>
     */
    [Column("up_profile_picture")]
    [MaxLength(128)]
    public string? ProfilePicture { get; set; }

    /**
     * <summary>
     * 个人简介
     * 可空，0到1024个字符
     * </summary>
     */
    [Column("up_bio")]
    [MaxLength(1024)]
    public string? Bio { get; set; }
}
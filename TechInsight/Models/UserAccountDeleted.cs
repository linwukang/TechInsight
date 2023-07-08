using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechInsight.Models;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

/**
 * <summary>
 * 文章删除信息表
 * </summary>
 */
[Table("t_user_account_deleted")]
public class UserAccountDeleted
{
    /**
     * <summary>
     * Id
     * </summary>
     */
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("uad_id")]
    public int Id { get; set; }

    /*[Required]
    [ForeignKey("uad_user_account_id")]
    public UserAccount UserAccount { get; set; }*/

    [Required]
    [Column("uad_delete_time")]
    public DateTime DeleteTime { get; set; }

    [Required]
    [ForeignKey("uad_operator_id")]
    public UserAccount Operator { get; set; }

    [Column("uad_delete_reasons")]
    [MaxLength(1024)]
    public string? DeleteReasons { get; set; }
}
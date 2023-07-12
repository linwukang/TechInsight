using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechInsight.Models;

namespace TechInsightDb.Models;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

/**
 * <summary>
 * 文章删除信息表
 * </summary>
 */
[Table("t_comment_deleted")]
public class CommentDeleted
{
    /**
     * <summary>
     * Id
     * </summary>
     */
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("cd_id")]
    public int Id { get; set; }

    /*[Required]
    [ForeignKey("cd_comment_id")]
    public Comment Comment { get; set; }*/

    [Required]
    [Column("cd_delete_time")]
    public DateTime DeleteTime { get; set; }

    [Required]
    [ForeignKey("cd_operator_id")]
    public UserAccount Operator { get; set; }

    [Column("cd_delete_reasons")]
    [MaxLength(1024)]
    public string? DeleteReasons { get; set; }
}
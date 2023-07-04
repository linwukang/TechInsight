using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechInsight.Models;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

/**
 * <summary>
 * 文章删除信息表
 * </summary>
 */
[Table("t_article_deleted")]
public class ArticleDeleted
{
    /**
     * <summary>
     * Id
     * </summary>
     */
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ad_id")]
    public int Id { get; set; }

    [Required]
    [ForeignKey("ad_article_id")]
    public Article Article { get; set; }

    [Required]
    [Column("ad_delete_time")]
    public DateTime DeleteTime { get; set; }

    [Required]
    [ForeignKey("ad_operator_id")]
    public UserAccount Operator { get; set; }

    [Column("ad_delete_reasons")]
    public string? DeleteReasons { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechInsight.Models;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

/**
 * <summary>
 * 文章审核表
 * </summary>
 */
[Table("t_article_review")]
public class ArticleReview
{
    /**
     * <summary>
     * Id
     * </summary>
     */
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("are_id")]
    public int Id { get; set; }

    /**
     * <summary>
     * 审核的文章
     * 非空
     * </summary>
     */
    [Required]
    [ForeignKey("are_article_id")]
    public Article Article { get; set; }

    /**
     *
     */
    [Required]
    [Column("are_review_time")]
    public DateTime ReviewTime { get; set; }

    /**
     * <summary>
     * 审核是否通过
     * 非空
     * </summary>
     */
    [Required]
    [Column("are_is_approved")]
    public bool IsApproved { get; set; }

    /**
     * <summary>
     * 审核不通过原因
     * 可空
     * </summary>
     */
    [Column("are_not_approved_reasons")]
    public string? NotApprovedReasons { get; set; }

    /**
     * <summary>
     * 审核员账号
     * 非空
     * </summary>
     */
    [Required]
    [Column("are_reviewer")]
    public UserAccount Reviewer { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechInsight.Models;

namespace TechInsightDb.Models;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

/**
 * <summary>
 * 文章表
 * </summary>
 */
[Table("t_articles")]
public class Article
{
    /**
     * <summary>
     * Id
     * </summary>
     */
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ar_id")]
    public int Id { get; set; }

    /**
     * <summary>
     * 文章标题
     * 非空，最大长度64
     * </summary>
     */
    [Required]
    [Column("ar_title")]
    [MaxLength(64)]
    public string Title { get; set; }

    /**
     * <summary>
     * 文章内容
     * 非空，不限制长度
     * </summary>
     */
    [Required]
    [Column("ar_content")]
    public string Content { get; set; }

    /**
     * <summary>
     * 文章提交日期
     * 非空
     * </summary>
     */
    [Required]
    [Column("ar_submission_time")]
    public DateTime SubmissionTime { get; set; } = DateTime.Now;

    /**
     * <summary>
     * 文章发布日期，以审核通过时间为准
     * 可空
     * </summary>
     */
    [Column("ar_publication_date")]
    public DateTime? PublicationTime { get; set; }

    /**
     * <summary>
     * 文章最后的编辑日期，以审核通过时间为准
     * 非空
     * </summary>
     */
    [Required]
    [Column("ar_last_modified_date")]
    public DateTime LastModifiedTime { get; set; } = DateTime.Now;

    /**
     * <summary>
     * 文章发布者账号
     * 非空
     * </summary>
     */
    [Required]
    [ForeignKey("ar_publisher_id")]
    public UserAccount Publisher { get; set; }

    /**
     * <summary>
     * 文章阅读量
     * 非空
     * </summary>
     */
    [Required]
    [Column("ar_read")] 
    public int Read { get; set; } = 0;

    /**
     * <summary>
     * 文章点赞数
     * 非空
     * </summary>
     */
    [Required]
    [Column("ar_likes")]
    public int Likes { get; set; } = 0;

    /**
     * <summary>
     * 文章点踩数
     * 非空
     * </summary>
     */
    [Required]
    [Column("ar_dislikes")]
    public int Dislikes { get; set; } = 0;

    /**
     * <summary>
     * null 值表示文章未被删除
     * </summary>
     */
    [ForeignKey("ar_deleted_id")]
    public ArticleDeleted? IsDeleted { get; set; }

    /**
     * <summary>
     * 导航属性
     * 文章审核列表
     * </summary>
     */
    public IList<ArticleReview> ArticleReviews { get; set; } = new List<ArticleReview>();

    /**
     * <summary>
     * 导航属性
     * 文章的评论列表
     * </summary>
     */
    public IList<Comment> Comments { get; set; } = new List<Comment>();

    /*/**
     * <summary>
     * 导航属性
     * 文章的点赞列表
     * </summary>
     #1#
    public IList<ArticleLike> ArticleLikes { get; set; } = new List<ArticleLike>();

    /**
     * <summary>
     * 导航属性
     * 文章的点踩列表
     * </summary>
     #1#
    public IList<ArticleDislike> ArticleDislikes { get; set; } = new List<ArticleDislike>();*/
}
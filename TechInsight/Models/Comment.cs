using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechInsight.Models;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

[Table("t_comments")]
public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("co_id")]
    public int Id { get; set; }

    [Required]
    [ForeignKey("co_article_id")]
    public Article Article { get; set; }

    [Required]
    [MaxLength(2000)]
    [Column("co_comment")]
    public string Content { get; set; }

    [Required]
    [ForeignKey("co_publisher_id")]
    public UserAccount Publisher { get; set; }

    [Required]
    [Column("co_publication_date")]
    public DateTime PublicationDate { get; set; } = DateTime.Now;

    /**
    * <summary>
    * null 值表示评论未被删除
    * </summary>
    */
    [Column("co_deleted_id")]
    public CommentDeleted? IsDeleted { get; set; }

    public IList<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();

    public IList<CommentDislike> CommentDislikes { get; set; } = new List<CommentDislike>();
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechInsight.Models;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。


[Table("t_article_likes")]
public class ArticleLike
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("al_id")]
    public int Id { get; set; }

    [Required]
    [Column("al_like_time")]
    public DateTime LikeTime { get; set; }

    [Required]
    [ForeignKey("al_article_id")]
    public Article Article { get; set; }

    [Required]
    [ForeignKey("al_liker_id")]
    public UserAccount Liker { get; set; }
}
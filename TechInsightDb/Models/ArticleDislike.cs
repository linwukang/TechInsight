using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechInsight.Models;

namespace TechInsightDb.Models;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。


// [Table("t_article_dislikes")]
public class ArticleDislike
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ad_id")]
    public int Id { get; set; }

    [Required]
    [Column("ad_dislike_time")]
    public DateTime DislikeTime { get; set; }

    [Required]
    [ForeignKey("ad_article_id")]
    public Article Article { get; set; }

    [Required]
    [ForeignKey("ad_disliker_id")]
    public UserAccount Disliker { get; set; }
}
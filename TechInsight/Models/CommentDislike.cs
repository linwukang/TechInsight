using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechInsight.Models;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。


// [Table("t_comment_dislikes")]
public class CommentDislike
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("cd_id")]
    public int Id { get; set; }

    [Required]
    [Column("cd_dislike_time")]
    public DateTime DislikeTime { get; set; }

    [Required]
    [ForeignKey("cd_comment_id")]
    public Comment Comment { get; set; }

    [Required]
    [ForeignKey("cd_liker_id")]
    public UserAccount Disliker { get; set; }
}
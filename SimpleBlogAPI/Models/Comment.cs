using SimpleBlog.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBlogAPI.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(250)]
        public string UserComment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string UserId { get; set; }

        [Required]
        public int BlogPostId { get; set; }
        [ForeignKey("BlogPostId")]
        public BlogPost BlogPost { get; set; }
    }
}

using SimpleBlogAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlog.Models
{
    public class BlogPost
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Title { get; set; }

        [Required]
        [StringLength(150)]
        public string Summary { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string UserId { get; set; }
    }
}

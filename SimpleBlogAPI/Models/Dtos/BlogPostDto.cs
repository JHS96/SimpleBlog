using System.ComponentModel.DataAnnotations;

namespace SimpleBlogAPI.Models.Dtos
{
    public class BlogPostDto
    {
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

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        //public ICollection<CommentDto> Comments { get; set;} //To include comments with blog post
    }
}

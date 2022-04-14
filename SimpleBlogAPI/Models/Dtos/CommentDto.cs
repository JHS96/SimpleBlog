using System.ComponentModel.DataAnnotations;

namespace SimpleBlogAPI.Models.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(250)]
        public string UserComment { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public int BlogPostId { get; set; }

        //public BlogPostDto BlogPost { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SimpleBlogAPI.Models.Dtos
{
    public class UpdateBlogPostDto
    {
        [Required]
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

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SimpleBlogAPI.Models.Dtos
{
    public class CreateBlogPostDto
    {
        [Required]
        [StringLength(30)]
        public string Title { get; set; }

        [Required]
        [StringLength(150)]
        public string Summary { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; }
    }
}

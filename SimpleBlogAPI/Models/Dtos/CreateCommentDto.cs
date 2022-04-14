using System.ComponentModel.DataAnnotations;

namespace SimpleBlogAPI.Models.Dtos
{
    public class CreateCommentDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(250)]
        public string UserComment { get; set; }

        [Required]
        public int BlogPostId { get; set; }
    }
}

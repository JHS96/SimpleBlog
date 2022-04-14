using SimpleBlog.Models;

namespace SimpleBlogAPI.Repository.IRepository
{
    public interface IBlogPostRepository
    {
        ICollection<BlogPost> GetBlogPosts();
        BlogPost GetBlogPost(int id);
        string GetBlogPostUserId(int blogPostId);
        bool BlogPostExists(int id);
        bool CreateBlogPost(BlogPost blogPost);
        bool UpdateBlogPost(BlogPost blogPost);
        bool DeleteBlogPost(BlogPost blogPost);
        bool Save();
    }
}

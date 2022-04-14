using Microsoft.EntityFrameworkCore;
using SimpleBlog;
using SimpleBlog.Models;
using SimpleBlogAPI.Repository.IRepository;

namespace SimpleBlogAPI.Repository
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly SimpleBlogDbContext _db;

        public BlogPostRepository(SimpleBlogDbContext db)
        {
            _db = db;
        }

        public ICollection<BlogPost> GetBlogPosts()
        {
            return _db.BlogPost.ToList();
        }

        public BlogPost GetBlogPost(int id)
        {
            return _db.BlogPost.FirstOrDefault(p => p.Id == id);
        }

        public string GetBlogPostUserId(int blogPostId)
        {
            return _db.BlogPost.AsNoTracking().FirstOrDefault(p => p.Id == blogPostId).UserId;
        }

        public bool CreateBlogPost(BlogPost blogPost)
        {
            _db.BlogPost.Add(blogPost);
            return Save();
        }

        public bool UpdateBlogPost(BlogPost blogPost)
        {
            _db.BlogPost.Update(blogPost);
            return Save();
        }

        public bool DeleteBlogPost(BlogPost blogPost)
        {
            _db.BlogPost.Remove(blogPost);
            return Save();
        }

        public bool BlogPostExists(int id)
        {
            bool value = _db.BlogPost.Any(p => p.Id == id);
            return value;
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }
    }
}

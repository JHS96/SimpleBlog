using Microsoft.EntityFrameworkCore;
using SimpleBlog;
using SimpleBlogAPI.Models;
using SimpleBlogAPI.Repository.IRepository;

namespace SimpleBlogAPI.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly SimpleBlogDbContext _db;

        public CommentRepository(SimpleBlogDbContext db)
        {
            _db = db;
        }

        ICollection<Comment> ICommentRepository.GetCommentsForPost(int postId)
        {
            return _db.Comment.Include(c => c.BlogPost).Where(c => c.BlogPostId == postId).ToList();
        }

        public Comment GetComment(int id)
        {
            return _db.Comment.Include(c => c.BlogPost).FirstOrDefault(c => c.Id == id);
        }

        public bool CreateComment(Comment comment)
        {
            _db.Comment.Add(comment);
            return Save();
        }

        public bool DeleteComment(Comment comment)
        {
            _db.Comment.Remove(comment);
            return Save();
        }

        public bool CommentExists(int id)
        {
            bool value = _db.Comment.Any(c => c.Id == id);
            return value;
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }
    }
}

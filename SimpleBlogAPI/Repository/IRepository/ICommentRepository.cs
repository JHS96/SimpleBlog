using SimpleBlog.Models;
using SimpleBlogAPI.Models;

namespace SimpleBlogAPI.Repository.IRepository
{
    public interface ICommentRepository
    {
        ICollection<Comment> GetCommentsForPost(int postId);
        Comment GetComment(int id);
        bool CommentExists(int id);
        bool CreateComment(Comment comment);
        bool DeleteComment(Comment comment);
        bool Save();
    }
}

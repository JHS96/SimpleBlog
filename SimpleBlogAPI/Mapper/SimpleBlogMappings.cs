using AutoMapper;
using SimpleBlog.Models;
using SimpleBlogAPI.Models;
using SimpleBlogAPI.Models.Dtos;

namespace SimpleBlogAPI.Mapper
{
    public class SimpleBlogMappings : Profile
    {
        public SimpleBlogMappings()
        {
            CreateMap<BlogPost, BlogPostDto>().ReverseMap();
            CreateMap<BlogPost, CreateBlogPostDto>().ReverseMap();
            CreateMap<BlogPost, UpdateBlogPostDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, CreateCommentDto>().ReverseMap();
        }
    }
}

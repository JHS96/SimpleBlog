using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models;
using SimpleBlogAPI.Models.Dtos;
using SimpleBlogAPI.Repository.IRepository;
using System.Security.Claims;

namespace SimpleBlogAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/blogposts")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]

    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _bpRepo;
        //private readonly ICommentRepository _commentRepo; //To include comments with blog post
        private readonly IMapper _mapper;

        //public BlogPostsController(IBlogPostRepository bpRepo, ICommentRepository commentRepo, IMapper mapper)
        public BlogPostsController(IBlogPostRepository bpRepo, IMapper mapper)
        {
            _bpRepo = bpRepo;
            //_commentRepo = commentRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<BlogPostDto>))]
        [ProducesDefaultResponseType]
        public IActionResult GetBlogPosts(int? pageNumber, int? pageSize)
        {
            var blogPostList = _bpRepo.GetBlogPosts();
            var blogPostDto = new List<BlogPostDto>();
            foreach(var blogPost in blogPostList)
            {
                blogPostDto.Add(_mapper.Map<BlogPostDto>(blogPost));
            }

            //Pagination
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 5;
            var finalObj = blogPostDto.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize);

            return Ok(finalObj);
        }

        [HttpGet("{blogPostId:int}", Name="GetBlogPost")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(BlogPostDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetBlogPost(int blogPostId)
        {
            var blogPostObj = _bpRepo.GetBlogPost(blogPostId);
            if (blogPostObj == null)
            {
                return NotFound();
            }
            var blogPostDto = _mapper.Map<BlogPostDto>(blogPostObj);

            ////Below code allows comments that belong to this blog post to be included in the response
            //var commentObj = _commentRepo.GetCommentsForPost(blogPostId);
            //var commentList = new List<CommentDto>();
            //foreach (var comment in commentObj)
            //{
            //    commentList.Add(_mapper.Map<CommentDto>(comment));
            //}
            //blogPostDto.Comments = commentList;

            return Ok(blogPostDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(BlogPostDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateBlogPost([FromBody] CreateBlogPostDto createBlogPostDto)
        {
            if (createBlogPostDto == null)
            {
                return BadRequest(ModelState);
            }
            //if (_bpRepo.BlogPostExists(blogPostDto.Id))
            //{
            //    ModelState.AddModelError("", "This blog post already exists!");
            //    return StatusCode(400, ModelState);
            //}

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var blogPostObj = _mapper.Map<BlogPost>(createBlogPostDto);
            blogPostObj.UserId = userId;
            if (!_bpRepo.CreateBlogPost(blogPostObj))
            {
                ModelState.AddModelError("", "Something went wrong while saving the blog post {blogPostObj.Title}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetBlogPost", new { blogPostId = blogPostObj.Id }, blogPostObj);
        }

        [HttpPut("{blogPostId:int}", Name = "UpdateBlogPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateBlogPost(int blogPostId, [FromBody] UpdateBlogPostDto updateBlogPostDto)
        {
            if (updateBlogPostDto == null || blogPostId != updateBlogPostDto.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_bpRepo.BlogPostExists(blogPostId) || !_bpRepo.BlogPostExists(updateBlogPostDto.Id))
            {
                return NotFound();
            }

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var blogPostUserID = _bpRepo.GetBlogPostUserId(blogPostId);
            if (blogPostUserID != userId)
            {
                return BadRequest("Sorry, you may not update this post.");
            }


            var blogPostObj = _mapper.Map<BlogPost>(updateBlogPostDto);
            blogPostObj.UserId = blogPostUserID;
            if (!_bpRepo.UpdateBlogPost(blogPostObj))
            {
                ModelState.AddModelError("", $"Something went wrong while updatng the blog post {blogPostObj.Title}");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpDelete("{blogPostId:int}", Name ="DeleteBlogPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult DeleteBlogPost(int blogPostId)
        {
            if (!_bpRepo.BlogPostExists(blogPostId))
            {
                return NotFound();
            }

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var blogPostObj = _bpRepo.GetBlogPost(blogPostId);
            if (blogPostObj.UserId != userId)
            {
                return BadRequest("Sorry, you may not delete this post.");
            }

            if (!_bpRepo.DeleteBlogPost(blogPostObj))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting the blog post {blogPostObj.Title}");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
    }
}

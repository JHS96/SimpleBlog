using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models;
using SimpleBlogAPI.Models;
using SimpleBlogAPI.Models.Dtos;
using SimpleBlogAPI.Repository.IRepository;
using System.Security.Claims;

namespace SimpleBlogAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/comments")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]

    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IBlogPostRepository _bpRepo;
        private readonly IMapper _mapper;

        public CommentsController(ICommentRepository commentRepo, IBlogPostRepository bpRepo, IMapper mapper)
        {
            _commentRepo = commentRepo;
            _bpRepo = bpRepo;
            _mapper = mapper;
        }

        [HttpGet("[action]/{blogPostId:int}")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetCommentsForBlogPost(int blogPostId, int? pageNumber, int? pageSize)
        {
            var objList = _commentRepo.GetCommentsForPost(blogPostId);

            if (objList == null || objList.Count == 0)
            {
                return NotFound();
            }

            var objDto = new List<CommentDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<CommentDto>(obj));
            }

            //Pagination
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 3;
            var finalObj = objDto.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize);

            return Ok(finalObj);
        }

        [HttpGet("{commentId:int}", Name="GetComment")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetComment(int commentId)
        {
            var commentObj = _commentRepo.GetComment(commentId);
            if (commentObj == null)
            {
                return NotFound();
            }
            var commentDto = _mapper.Map<CommentDto>(commentObj);
            return Ok(commentDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CommentDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateComment([FromBody] CreateCommentDto createCommentDto)
        {
            if (createCommentDto == null)
            {
                return BadRequest(ModelState);
            }

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var commentObj = _mapper.Map<Comment>(createCommentDto);

            var blogPostObj = _bpRepo.GetBlogPost(commentObj.BlogPostId);
            if (blogPostObj == null)
            {
                return NotFound("The blog post on which you are trying to comment could not be found.");
            }

            commentObj.UserId = userId;
            if (!_commentRepo.CreateComment(commentObj))
            {
                ModelState.AddModelError("", "Something went wrong while saving the comment...");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetComment", new { commentId = commentObj.Id }, commentObj);
        }

        [HttpDelete("{commentId:int}", Name ="DeleteComment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult DeleteComment(int commentId)
        {
            if (!_commentRepo.CommentExists(commentId))
            {
                return NotFound();
            }            

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var commentObj = _commentRepo.GetComment(commentId);
            if (commentObj.UserId != userId)
            {
                return BadRequest("Sorry, you may not delete this comment.");
            }

            if (!_commentRepo.DeleteComment(commentObj))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting the comment...");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
    }
}

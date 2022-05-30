using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class PostsController : ControllerBase
    {
        private readonly DataContext _context;

        public PostsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Post>> Get()
        {
            return Ok(_context.Posts);
        }

        [HttpGet]
        [Route("{postId}")]
        public ActionResult<IEnumerable<Post>> Get(int postId)
        {
            Post post = _context.Posts.Find(postId);
            return post == null ? NotFound("Post not found") : Ok(post);
        }

        [HttpPost]
        public ActionResult Post(Post post)
        {
            var existingPost = _context.Posts.Find(post.Id);
            User existingUser =_context.Users.Find(post.UserId);
            if (existingPost!=null) {
                return Conflict("This Id is already being used.");
            } else {
                if (existingUser==null) {
                    return Conflict("This user doesn't exist");
                } else {
                    _context.Posts.Add(post);
                    existingUser.Posts.Add(post);
                    _context.SaveChanges();

                    var resourceUrl = Request.Path.ToString() + "/" + post.Id;
                    return Created(resourceUrl, post);
                }
            }
        }

        [HttpPut]
        [Route("{postId}")]
        public ActionResult Put(Post post, int postId) {
            Post existingPost = _context.Posts.Find(postId);
            if (existingPost==null) {
                return Conflict("There is no post with this Id");
            }
            existingPost.Title = post.Title;
            existingPost.Body = post.Body;
            _context.SaveChanges();
            var resourceUrl = Request.Path.ToString() + "/" + postId;
            return Ok();
        }

        [HttpDelete]
        [Route("{postId}")]
        public ActionResult Delete(int postId) {
            Post existingPost = _context.Posts.Find(postId);
            if (existingPost==null) {
                return Conflict("There is no post with this Id");
            } else {
                _context.Posts.Remove(existingPost);
                _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
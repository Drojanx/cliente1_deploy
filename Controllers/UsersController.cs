using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return Ok(_context.Users);
        }

        [HttpGet]
        [Route("{userId}")]
        public ActionResult<IEnumerable<User>> Get(int userId)
        {
            User user = _context.Users.Find(userId);
            return user == null ? NotFound("User not found") : Ok(user);
        }
        
        [HttpPost]
        public ActionResult Post(User user)
        {           

            foreach(User registeredUser in _context.Users) {
                if (registeredUser.Username.Equals(user.Username)) {
                    return Conflict("This Username is already being used.");
                }
            }

            var existingUser = _context.Users.Find(user.Id);
            if (existingUser!=null) {
                return Conflict("This Id is already being used.");
            } else {
            _context.Users.Add(user);
            _context.SaveChanges();
            var resourceUrl = Request.Path.ToString() + "/" + user.Id;
            return Created(resourceUrl, user);
            }
        }
        
        [HttpPut]
        [Route("{userId}")]
        public ActionResult Put(User user, int userId) {
            User existingUser = _context.Users.Find(userId);
            if (existingUser==null) {
                return Conflict("There is no user with this Id");
            }
            existingUser.Username = user.Username;
            _context.SaveChanges(); 
            var resourceUrl = Request.Path.ToString() + "/" + userId;
            return Ok();
        }
        
        [HttpDelete]
        [Route("{userId}")]
        public ActionResult Delete(int userId) {
            User existingUser = _context.Users.Find(userId);
            if (existingUser==null) {
                return Conflict("There is no user with this Id");
            } else {
                foreach (Post postToDelete in _context.Posts) {
                    if (postToDelete.UserId == userId) {
                        _context.Posts.Remove(postToDelete);
                    }
                }
                _context.Users.Remove(existingUser);
                _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
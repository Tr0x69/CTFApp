using CTFApp.Data;
using CTFApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CTFApp.Controllers
{

    [ApiController]
    [Route("api/Game")]
    public class GameApiController : Controller
    {
        private ApplicationDbContext _context;

        public GameApiController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }


        [HttpPost("submitscore")]
        public IActionResult SubmitScore([FromBody] User user)
        {
            if (string.IsNullOrEmpty(user.userName) || user.userScore < 0)
            {
                return BadRequest(new { message = "Invalid Data." });
            }

            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                userName = user.userName,
                userScore = user.userScore
            };

            _context.Add(newUser);
            _context.SaveChanges();

            if (user.userScore > 10000)
            {
                return Ok(new { message = "Great Job! Here your flag: ctf{fake_flag}" });
            }

            return Ok(new { message = "Score submitted succesfully!" });
        }


        [HttpGet("user")]
        public IActionResult GetUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest(new { message = "Username is required" });
            }
            string query = $"SELECT * FROM Users Where userName = '{username}'";

            try
            {
                var user = _context.Users.FromSqlRaw(query).FirstOrDefault();
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }
                return Ok(new { user.Id, user.userName, user.userScore });
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                return StatusCode(500, new { message = "Something went wrong while processing your request." });
            }
        }
    }
}

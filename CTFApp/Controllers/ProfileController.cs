using CTFApp.DataAccess.Data;
using CTFApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CTFApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {

        private readonly ApplicationDbContext _context;


        public ProfileController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? id)
        {

            User user;
            if (id == null)
            {
                user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
                if (user == null)
                {
                    return Unauthorized();
                }
            }

            user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return BadRequest("Invalid User ID");
            }
            return View(user);
        }



    }
}

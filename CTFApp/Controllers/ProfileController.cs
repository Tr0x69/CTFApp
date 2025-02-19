using CTFApp.DataAccess.Data;
using CTFApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CTFApp.Controllers
{
    public class ProfileController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;


        public ProfileController(ApplicationDbContext ctx, UserManager<User> userManager)
        {
            _context = ctx;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? id)
        {

            User user;
            if (id == null)
            {
                user = await _userManager.GetUserAsync(User);
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

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
        public async Task<IActionResult> Index()
        {

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }
            return View(user);
        }
    }
}

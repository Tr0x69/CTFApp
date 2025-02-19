using CTFApp.DataAccess.Data;
using CTFApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CTFApp.Controllers
{
    [Route("Game")]
    public class GameController : Controller
    {
        private ApplicationDbContext _context;


        public GameController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {


            var users = await _context.Users.OrderByDescending(u => u.userScore).Select(u => new UserScoreViewModel { Username = u.UserName, userScore = u.userScore }).ToListAsync();


            var currentUser = await _context.Users.Where(u => u.UserName == User.Identity.Name).Select(u => new UserScoreViewModel
            {
                Username = u.UserName,
                userScore = u.userScore,
            }).FirstOrDefaultAsync();

            if (currentUser == null)
            {
                return Unauthorized("Unauthorized");
            }

            var gameViewModel = new GameViewModel
            {
                Users = users,
                CurrentUser = currentUser
            };
            return View(gameViewModel);
        }





    }
}

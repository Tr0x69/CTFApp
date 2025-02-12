using System.Text;
using CTFApp.Data;
using CTFApp.Models;
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
        public IActionResult Index()
        {
         

            List<User> users = _context.Users.ToList();
            return View(users);
        }


        [HttpGet("preview")]
        public IActionResult Preview(string content= "")
        {
            string image = HttpContext.Session.GetString("UserContent");

            // Ensure that a valid content is passed
            ViewBag.BackgroundImage = image;


            ViewBag.UserContent = content; 
            return View();
        }



        
        
    }
}

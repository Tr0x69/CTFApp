using CTFApp.DataAccess.Data;
using CTFApp.Models;
using CTFApp.Services;
using CTFApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CTFApp.Controllers
{


    public class AccountController : Controller
    {
        private readonly JwtService _jwtService;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(JwtService jwtService, ApplicationDbContext ctx, IWebHostEnvironment webHostEnvironment)
        {
            _jwtService = jwtService;
            _context = ctx;
            _webHostEnvironment = webHostEnvironment;

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    var token = _jwtService.GenerateToken(user.Id, user.Username, user.Role);
                    HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    });
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");


                if (!Directory.Exists(uploadsFolder))
                {
                    //create a new directory
                    Directory.CreateDirectory(uploadsFolder);
                }

                string defaultImagePath = "\\images\\cat4.jpg";
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                User newUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = model.Username,
                    Password = hashedPassword,
                    userScore = 0,
                    ImageAva = defaultImagePath,
                    Role = "User"
                };
                var result = await _context.Users.AddAsync(newUser);

                await _context.SaveChangesAsync();

                return RedirectToAction("Login");

            }
            return View(model);
        }

    }

}

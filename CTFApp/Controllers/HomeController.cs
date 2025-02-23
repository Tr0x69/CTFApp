using System.Diagnostics;
using CTFApp.DataAccess.Data;
using CTFApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CTFApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var users = string.IsNullOrEmpty(search) ? await _context.Users.ToListAsync() : await _context.Users.Where(u => u.UserName.Contains(search)).ToListAsync();
            ViewBag.SearchTerm = search;
            return View(users);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

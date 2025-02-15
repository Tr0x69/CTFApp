using System.Text;
using CTFApp.DataAccess.Data;
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
        public IActionResult Preview()
        {
             
            return View();
        }

        [HttpPost("preview")]
        public async Task<IActionResult> uploadFile(IFormFile file)
        {
            Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self';";
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No File Uploaded" });

            }

            //Concat current directory to wwwroot/uploads
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            //create a new directory
            Directory.CreateDirectory(uploadsFolder);

            //extions allowed
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            var allowedExtensions = new[] { ".jpg", ".png", ".gif", ".js" };

            //check extensions
            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new
                {
                    message = "Invalid File Type"
                });
                
            }
            //create a new file at the uploadFolder path
            var filePath = Path.Combine(uploadsFolder, file.FileName);

            //start to copy the content 

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new {success = true, message = "File Uploaded Succesfully", url = $"/uploads/{file.FileName}" });


        }


    }
}

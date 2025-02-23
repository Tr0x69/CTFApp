using CTFApp.DataAccess.Data;
using CTFApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;

namespace CTFApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api")]
    public class ApiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;


        public ApiController(ApplicationDbContext ctx, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager)
        {
            _context = ctx;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }



        [HttpPost("game/submitscore")]
        public IActionResult SubmitScore([FromBody] User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.UserName == user.UserName);

            if (existingUser == null)
            {
                return BadRequest(new { message = "Invalid Data." });
            }
            if (string.IsNullOrEmpty(existingUser.UserName) || user.userScore < 0)
            {
                return BadRequest(new { message = "Invalid Data." });
            }





            if (user.userScore > 10000)
            {
                existingUser.userScore = user.userScore;
                _context.Update(existingUser);
                _context.SaveChanges();
                return Ok(new { message = "Great Job! Here your flag: ctf{fake_flag}" });
            }

            existingUser.userScore = user.userScore;
            existingUser.ImageAva = existingUser.ImageAva;

            _context.Update(existingUser);
            _context.SaveChanges();
            return Ok(new { message = "Score submitted succesfully!" });
        }


        [HttpGet("game/user")]
        public IActionResult GetUser(int? score)
        {
            if (score < 0 || score == null)
            {
                return BadRequest(new { message = "Score is required" });
            }
            //https://learn.microsoft.com/en-us/ef/core/querying/sql-queries?tabs=sqlserver
            //The SQL query must return data for all properties of the entity type.
            string query = $@"SELECT Id, UserName, userScore,  
        '' as Email, '' as NormalizedEmail, CAST(0 AS BIT) as EmailConfirmed,
        '' as PasswordHash, '' as SecurityStamp, '' as ConcurrencyStamp,
        '' as PhoneNumber, CAST(0 AS BIT) as PhoneNumberConfirmed, CAST(0 AS BIT) as TwoFactorEnabled,
        CAST(0 AS BIT) as LockoutEnabled, NULL as LockoutEnd, 0 as AccessFailedCount,
        '' as NormalizedUserName,'test' as ImageAva
        FROM AspNetUsers WHERE userScore > '{score}'";

            try
            {
                var user = _context.Users.FromSqlRaw(query).FirstOrDefault();
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }
                return Ok(new { user.Id, user.UserName, user.userScore, user.Email, user.EmailConfirmed, user.PasswordHash, user.NormalizedEmail, user.SecurityStamp, user.ConcurrencyStamp, user.PhoneNumber, user.PhoneNumberConfirmed, user.TwoFactorEnabled, user.LockoutEnabled, user.LockoutEnd, user.AccessFailedCount, user.NormalizedUserName });
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                return StatusCode(500, new { message = "Something went wrong while processing your request." });
            }
            catch (InvalidCastException ex)
            {
                return StatusCode(500, new { message = "Something went wrong while processing your request." });
            }
        }




        [HttpGet("secret/submit")]
        public async Task<IActionResult> SecretPage([FromQuery] string url)
        {
            //string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();

            //if (remoteIp != "127.0.0.1")
            //{
            //    return StatusCode(403, "Access denied.");
            //}

            if (!isURlValid(url, out string errorMessage))
            {
                return BadRequest(new { message = errorMessage });
            }

            //admin bot
            var result = await VisitLinkWithPuppeteer(url);
            //testing
            Console.WriteLine(result);
            Console.WriteLine(url);
            return Ok(new { message = "Our team will visit the link ASAP!" });

        }



        [HttpPost("profile")]
        public async Task<IActionResult> Profile(IFormFile file)
        {
            //Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self';";

            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Invalid File" });

            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            if (!string.IsNullOrEmpty(user.ImageAva))
            {
                var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, user.ImageAva.TrimStart('\\'));
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

            }
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
            var filePath = Path.Combine(uploadsFolder, fileName);

            //start to copy the content 

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            user.ImageAva = $"\\uploads\\{fileName}";

            await _userManager.UpdateAsync(user);

            return Ok(new { success = true, message = "File Uploaded Succesfully", url = $"/uploads/{fileName}" });


        }









        private bool isURlValid(string url, out string errorMessage) // return additional string in true/false
        {

            errorMessage = string.Empty;



            if (string.IsNullOrWhiteSpace(url))
            {
                errorMessage = "Invalid URL.";
                return false;
            }

            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                errorMessage = "Url must start with http:// or https://";
                return false;

            }

            if (url.IndexOf("imgur.com", StringComparison.OrdinalIgnoreCase) < 0)
            {
                errorMessage = "Url must contain imgur.com.";
                return false;
            }


            return true;
        }


        private async Task<string> VisitLinkWithPuppeteer(string url)
        {
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();



            //var fetcher = new BrowserFetcher();
            //var revisionInfo = await fetcher.DownloadAsync();

            try
            {
                var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true
                });
                var page = await browser.NewPageAsync();
                await page.SetCookieAsync(new CookieParam
                {
                    Name = "admin_flag",
                    Value = "Flag{Admin_Bot_XSS_Capture}",
                    Domain = "localhost",
                    Path = "/"
                });

                //Go to url provided by user
                await page.GoToAsync(url);
                //Close the browser
                await browser.CloseAsync();

                return $"Bot successfully visited {url}.";
            }
            catch (System.Exception ex)
            {
                return $"Error: {ex.Message}";
            }


        }
    }
}

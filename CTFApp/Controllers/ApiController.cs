using CTFApp.DataAccess.Data;
using CTFApp.Models;
using CTFApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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


        public ApiController(ApplicationDbContext ctx, IWebHostEnvironment webHostEnvironment)
        {
            _context = ctx;
            _webHostEnvironment = webHostEnvironment;
        }



        [HttpPost("game/submitscore")]
        public async Task<IActionResult> SubmitScore([FromBody] UserScoreViewModel user)
        {
            if (user == null || string.IsNullOrEmpty(user.Username) || user.userScore < 0)
            {
                return BadRequest(new { message = "Invalid Data" });
            }

            var usernameFromToken = User.FindFirst("username")?.Value;
            if (string.IsNullOrEmpty(usernameFromToken) || usernameFromToken != user.Username)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (existingUser == null)
            {
                return BadRequest(new { message = "User not found." });
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


        //[HttpGet("game/user")]
        //public IActionResult GetUser(string score)
        //{
        //    if (string.IsNullOrEmpty(score))
        //    {
        //        return BadRequest(new { message = "Score is required" });
        //    }

        //    string query = $"SELECT * FROM Users WHERE userScore = {score}";

        //    try
        //    {
        //        var user = _context.Users.FromSqlRaw(query).ToList();
        //        if (user == null)
        //        {
        //            return NotFound(new { message = "User not found." });
        //        }
        //        return Ok(user.Select(user => new { user.Id, user.Username, user.userScore, user.Email, user.EmailConfirmed, user.PasswordHash, user.NormalizedEmail, user.SecurityStamp, user.ConcurrencyStamp, user.PhoneNumber, user.PhoneNumberConfirmed, user.TwoFactorEnabled, user.LockoutEnabled, user.LockoutEnd, user.AccessFailedCount, user.NormalizedUserName }));
        //    }
        //    catch (Microsoft.Data.SqlClient.SqlException ex)
        //    {
        //        return StatusCode(500, new { message = "Something went wrong while processing your request." });
        //    }
        //    catch (InvalidCastException ex)
        //    {
        //        return StatusCode(500, new { message = "Something went wrong while processing your request." });
        //    }
        //}




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

            var username = User.FindFirst("username")?.Value;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized(new { message = "Not authenticated." });
            }
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return Unauthorized(new { message = "User not found." });
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            if (!string.IsNullOrEmpty(user.ImageAva))
            {
                var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, user.ImageAva.TrimStart('\\').Replace("\\", "/"));
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

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "File Uploaded Succesfully", url = $"/uploads/{fileName}" });


        }




        [HttpPost("admin/import-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportUserUpdate()
        {
            try
            {
                if (Request.Form.Files.Count == 0)
                {
                    return BadRequest(new { message = "No file uploaded." });
                }

                var file = Request.Form.Files[0];
                if (file.Length == 0)
                {
                    return BadRequest(new { message = "Empty file." });
                }

                using var stream = new StreamReader(file.OpenReadStream());
                string jsonContent = await stream.ReadToEndAsync();

                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };
                var updates = JsonConvert.DeserializeObject<List<Object>>(jsonContent, settings);


                if (updates == null || updates.Count == 0)
                {
                    return BadRequest(new { message = "Invalid or empty JSON data." });
                }


                foreach (var update in updates)
                {
                    if (update is UserUpdateDto userUpdate)
                    {
                        var user = await _context.Users.FindAsync(userUpdate.Id);
                        if (user == null)
                        {
                            continue;
                        }
                        if (userUpdate.Username != null)
                            user.Username = userUpdate.Username;
                        if (userUpdate.userScore != 0)
                            user.userScore = userUpdate.userScore.Value;
                        if (userUpdate.ImageAva != null)
                            user.ImageAva = userUpdate.ImageAva;
                        if (userUpdate.Role != null)
                            user.Role = userUpdate.Role;
                    }
                }
                await _context.SaveChangesAsync();
                return Ok($"Processed {updates.Count} updates.");

            }
            catch (JsonSerializationException ex)
            {
                return BadRequest(new { message = "Invalid JSON format." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("admin/export-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportUserUpdate()
        {
            try
            {
                var users = await _context.Users.ToListAsync();

                if (users == null || users.Count == 0)
                {
                    return NotFound(new { message = "No users found." });
                }

                var exportUsers = users.Select(u => new UserUpdateDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    userScore = u.userScore,
                    ImageAva = u.ImageAva,
                    Role = u.Role
                }).ToList();
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                };
                string json = JsonConvert.SerializeObject(exportUsers, Formatting.Indented, settings);
                return File(new System.Text.UTF8Encoding().GetBytes(json), "application/json", "users_export.json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
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

using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace CTFApp.Controllers
{

    [Route("Admin/Edit")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();


        string htmlContent = @"
                <h2>Edit Canvas Background</h2>
                <h3>This site is currently under maintenance</h3>
                <form method='post' asp-controller='Admin' asp-action='EditBackground'>
                    <div class='form-group'>
                        <label>Select a background image:</label>
                        <div class='d-flex flex-wrap'>
                            <div class='m-2 text-center'>
                             <img src='/images/cat1.jpg' alt='cat1.jpg' style='width: 200px; height: auto; display: block; border: 2px solid transparent;' />
                                <div class='form-check mt-1'>
                                    <input class='form-check-input' type='radio' name='selectedBackground' id='bg_cat1.jpg' value='cat1.jpg' />
                                    <label class='form-check-label' for='bg_cat1.jpg'>Select</label>
                                </div>
                         </div>
                         <div class='m-2 text-center'>
                            <img src='/images/cat2.jpg' alt='cat2.jpg' style='width: 200px; height: auto; display: block; border: 2px solid transparent;' />
                <div class='form-check mt-1'>
                    <input class='form-check-input' type='radio' name='selectedBackground' id='bg_cat2.jpg' value='cat2.jpg' />
                    <label class='form-check-label' for='bg_cat2.jpg'>Select</label>
                </div>
                </div>
                <div class='m-2 text-center'>
                <img src='/images/cat3.jpg' alt='cat3.jpg' style='width: 200px; height: auto; display: block; border: 2px solid transparent;' />
                <div class='form-check mt-1'>
                    <input class='form-check-input' type='radio' name='selectedBackground' id='bg_cat3.jpg' value='cat3.jpg' />
                    <label class='form-check-label' for='bg_cat3.jpg'>Select</label>
                </div>
                </div>
                </div>
                </div>
                <button type='submit' class='btn btn-primary mt-2'>Save</button>
                </form>";

        [HttpGet]
        public IActionResult EditBackground()
        {
            
            Response.StatusCode = 301; // Set HTTP status to 301 Moved Permanently
            Response.Headers["Location"] = "/Game/preview"; // Set the redirect target

            return Content(htmlContent, "text/html"); // Return the HTML content with the 301 status
        }

        [HttpPost]
        public async Task<IActionResult> EditBackground([FromForm] string selectedBackground)
        {

            
            if (string.IsNullOrEmpty(selectedBackground))
            {
                selectedBackground = "cat1.jpg";
            }
            selectedBackground = selectedBackground.Trim();
            

            string[] allowedImages = { "cat1.jpg", "cat2.jpg", "cat3.jpg" };
            string newBackgroundImage = null;
            if (allowedImages.Contains(selectedBackground))
            {
                //string encodedImage = Convert.ToBase64String(Encoding.UTF8.GetBytes(selectedBackground));
                //HttpContext.Session.SetString("UserContent", selectedBackground);
                //return Ok(new { message = "Successfully Selected" });
                newBackgroundImage = $"/images/{selectedBackground}";
            }

            if(Regex.IsMatch(selectedBackground, @"^(https|http)?:\/\/", RegexOptions.IgnoreCase))
            {
                try
                {
                    byte[] imageBytes = await _httpClient.GetByteArrayAsync(selectedBackground);
                    newBackgroundImage = "data:image/png;base64," + Convert.ToBase64String(imageBytes);
                    //string encodedUrl = Convert.ToBase64String(Encoding.UTF8.GetBytes(responseContent));
                    //HttpContext.Session.SetString("UserContent", responseContent);
                    //return Content($"<h3>Background Updated</h3><p>Fetched content: {responseContent}</p>", "text/html");
                }
                catch (HttpRequestException)
                {
                    return BadRequest(new { message = "Failed to fetch external content." });
                }
            }
            if (newBackgroundImage != null)
            {
                

                // Inject JavaScript to change the background dynamically
                string script = $@"
                    <script>
                        document.body.style.backgroundImage = 'url({newBackgroundImage})';
                        alert('Background updated successfully!');
                    </script>";

                // Return the HTML with the injected JS
                string updatedContent = htmlContent + script;
                return Content(updatedContent, "text/html");
            }
            return BadRequest(new { message = "Invalid background selection." });
        }


    }
}

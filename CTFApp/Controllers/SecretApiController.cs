using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;



namespace CTFApp.Controllers
{
    [Route("/api/secret")]
    [ApiController]
    public class SecretApiController : Controller
    {
        [HttpGet("submit")]
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

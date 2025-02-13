using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;

namespace CTFApp.Controllers
{

    [Route("/secret")]
    [ApiController]
    public class SecretController : Controller
    {

        [HttpGet]
        public IActionResult SecretPage()
        {
            string? remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (remoteIp == "127.0.0.1" || remoteIp == "::1")
            {
                return View("SecretPage");
            }
            return StatusCode(403, "Access denied.");
        }


        [HttpPost("/secret/submit")]
        public async Task<IActionResult> SubmitLink([FromForm] string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return BadRequest("Invalid URL.");
            }

            var result = await VisitLinkWithPuppeteer(url);
            Console.WriteLine(result);
            Console.WriteLine(url);
            ViewBag.ConfirmationMessage = "Our admin bot will visit the link ASAP!";
            return View("SecretPage");
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

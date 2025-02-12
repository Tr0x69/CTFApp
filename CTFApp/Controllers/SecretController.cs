using Microsoft.AspNetCore.Mvc;

namespace CTFApp.Controllers
{

    [Route("/secret")]
    [ApiController]
    public class SecretController : Controller
    {
        public IActionResult SecretPage()
        {
            string? remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (remoteIp == "127.0.0.1" || remoteIp == "::1")
            {
                return Ok("Internal data: Flag{Hidden_SSRF_Secret}");
            }
            return StatusCode(403, "Access denied.");
        }
    }
}

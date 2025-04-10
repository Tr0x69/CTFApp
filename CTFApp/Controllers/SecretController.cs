using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CTFApp.Controllers
{

    [Route("/secret")]
    [ApiController]
    [Authorize]
    public class SecretController : Controller
    {

        [HttpGet]
        public IActionResult SecretPage()
        {
            return View("SecretPage");
        }


    }
}

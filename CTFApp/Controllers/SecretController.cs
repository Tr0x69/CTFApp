using Microsoft.AspNetCore.Mvc;

namespace CTFApp.Controllers
{

    [Route("/secret")]
    [ApiController]
    public class SecretController : Controller
    {

        [HttpGet]
        public IActionResult SecretPage()
        {
            return View("SecretPage");
        }




    }
}

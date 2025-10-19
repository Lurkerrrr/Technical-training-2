using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotHotel.Presentation.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        [Route("/")]
        [Route("/home")]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/about-us")]
        public IActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/contact-us")]
        public IActionResult Contact()
        {
            return View();
        }
    }
}

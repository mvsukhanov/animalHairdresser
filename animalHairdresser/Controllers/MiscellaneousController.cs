using Microsoft.AspNetCore.Mvc;

namespace animalHairdresser.Controllers
{
    public class MiscellaneousController : Controller
    {
        [Route("AcessDenied")]
        [HttpGet]
        public IActionResult AcessDenied()
        {
            return View();
        }

        [Route("AcessDenied")]
        [HttpPost]
        public IActionResult AcessDeniedPost(string action)
        {
            if (action == "Login")
            {
                return RedirectToAction("Login", "Login");
            }
            if (action == "Home")
            {
                return RedirectToAction("Home", "Home");
            }
            return RedirectToAction("AcessDenied", "Miscellaneous");
        }
    }
}

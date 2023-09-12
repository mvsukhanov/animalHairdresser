using Microsoft.AspNetCore.Mvc;

namespace animalHairdresser.Controllers
{
    public class AboutCompanyController : Controller
    {
        [Route("AboutCompany")]
        [HttpGet]
        public IActionResult AboutCompany()
        {
            return View();
        }

        [Route("AboutCompany")]
        [HttpPost]
        public IActionResult AboutCompanyPost(string action)
        {
            if (action == "Home")
            {
                return RedirectToAction("Home", "Home");
            }
            return RedirectToAction("AboutCompany", "AboutCompany");
        }
    }
}

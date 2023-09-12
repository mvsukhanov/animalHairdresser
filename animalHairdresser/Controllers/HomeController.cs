using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace animalHairdresser.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }

        [Route("/")]
        [HttpPost]
        public IActionResult HomePost(string action)
        {
            if (action == "AboutCompany")
                return RedirectToAction("AboutCompany", "AboutCompany");

            if (action == "PersonalArea")
                return RedirectToAction("PersonalArea", "PersonalArea");

            if (action == "CreateUser")
                return RedirectToAction("CreateUser", "CreateUser");

            return RedirectToAction("Home", "Home");
        }
    }
}

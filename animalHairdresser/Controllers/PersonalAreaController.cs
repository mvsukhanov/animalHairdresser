using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Authentication;
using System.Xml.Linq;

namespace animalHairdresser.Controllers
{
    public class PersonalAreaController : Controller
    {
        public IClientBaseService ClientBaseService { get; set; }
        public IOrderBaseService OrderBaseService { get; set; }

        public PersonalAreaController(IClientBaseService clientBaseService, IOrderBaseService orderBaseService)
        {
            ClientBaseService = clientBaseService;
            OrderBaseService = orderBaseService;
        }

        [Authorize]
        [Route("PersonalArea")]
        [HttpGet]
        public async Task<IActionResult> PersonalArea()
        {
            string name = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            string connString = HttpContext.User.FindFirst("connString").Value;

            ViewBag.Orders = await OrderBaseService.GetOrderListAsync(HttpContext);
            ViewBag.Animals = await ClientBaseService.SelectAnimalsFromClientAsync(connString, name);
            ViewBag.Name = name;

            return View();
        }

        [Authorize]
        [Route("PersonalArea")]
        [HttpPost]
        public async Task<IActionResult> PersonalAreaPost(DateTime dateTime, string kindOfAnimal, string breed, string animalName, string action)
        {
            if (action == "DeleteOrder")
            {
                await OrderBaseService.DeleteOrderAsync(dateTime, HttpContext);
                return RedirectToAction("PersonalArea", "PersonalArea");
            }

            if (action == "DeleteAnimal")
            {
                string name = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                string connString = HttpContext.User.FindFirst("connString").Value;
                Animals animal = new Animals(kindOfAnimal, breed, animalName);
                await ClientBaseService.DeleteAnimalAsync(connString, name, animal);
                return RedirectToAction("PersonalArea", "PersonalArea");
            }
            
            if (action == "Disconnect")
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Home", "Home");
            }
            
            if (action == "MakeNewOrder")
                return RedirectToAction("StepOne", "CreateOrder");
            
            if (action == "Home")
                return RedirectToAction("Home", "Home");
            
            return RedirectToAction("PersonalArea", "PersonalArea");
        }
    }
}

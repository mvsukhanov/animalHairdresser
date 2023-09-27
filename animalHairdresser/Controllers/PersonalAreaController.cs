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
        private readonly IClientBaseService _clientBaseService;
        private readonly IOrderBaseService _orderBaseService;

        public PersonalAreaController(IClientBaseService clientBaseService, IOrderBaseService orderBaseService)
        {
            _clientBaseService = clientBaseService;
            _orderBaseService = orderBaseService;
        }

        [Authorize]
        [Route("PersonalArea")]
        [HttpGet]
        public async Task<IActionResult> PersonalArea()
        {
            string name = HttpContext.User.FindFirst(ClaimTypes.Name).Value;

            ViewBag.Orders = await _orderBaseService.GetOrderListAsync(name);
            ViewBag.Animals = await _clientBaseService.SelectAnimalsFromClientAsync(name);
            ViewBag.Name = name;

            return View();
        }

        [Authorize]
        [Route("PersonalArea")]
        [HttpPost]
        public async Task<IActionResult> PersonalAreaPost(DateTime dateTime, string kindOfAnimal, string breed, string animalName, string action)
        {
            string name = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            if (action == "DeleteOrder")
            {
                await _orderBaseService.DeleteOrderAsync(dateTime, name);
                return RedirectToAction("PersonalArea", "PersonalArea");
            }

            if (action == "DeleteAnimal")
            {
                Animal animal = new Animal(kindOfAnimal, breed, animalName);
                await _clientBaseService.DeleteAnimalAsync(name, animal);
                return RedirectToAction("PersonalArea", "PersonalArea");
            }
            
            if (action == "Disconnect")
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Home", "Home");
            }
            
            if (action == "MakeNewOrder")
                return RedirectToAction("StepOne", "CreateOrder");
            
            return RedirectToAction("PersonalArea", "PersonalArea");
        }
    }
}

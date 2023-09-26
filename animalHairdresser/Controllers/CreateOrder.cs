using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Numerics;
using System.Runtime.Serialization;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace animalHairdresser.Controllers
{
    [Authorize]
    public class CreateOrderController : Controller
    {
        private readonly IClientBaseService _clientBaseService;
        private readonly IOrderBaseService _orderBaseService;
        public IAnimalsBreedsAndPriceService _animalsBreedsAndPriceCervice;

        public CreateOrderController(IClientBaseService clientBaseService, 
            IOrderBaseService orderBaseService, IAnimalsBreedsAndPriceService animalsBreedsAndPriceCervice)
        {
            _clientBaseService = clientBaseService;
            _orderBaseService = orderBaseService;
            _animalsBreedsAndPriceCervice = animalsBreedsAndPriceCervice;
        }

        [Route("StepOne")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> StepOne()
        {
            string connString = HttpContext.User.FindFirst("connString").Value;
            List<string> kindOfAnimals = await _animalsBreedsAndPriceCervice.KindOfAnimalsListAsync(connString);
            return View(kindOfAnimals);
        }
        
        [Route("StepOne")]
        [HttpPost]
        [Authorize]
        public IActionResult StepOnePost(DateOnly date, string phone, string kindOfAnimal)
        {
            if (phone is null || DateTime.Now.AddDays(1) > date.ToDateTime(new TimeOnly(23,59)))
                return RedirectToAction("StepOneError", "CreateOrder");
        
            return RedirectToAction("StepTwo", "CreateOrder",
                new {  date, phone, kindOfAnimal });
        }
        
        [Route("StepOneError")]
        [HttpGet]
        [Authorize]
        public IActionResult StepOneError()
        {
            return View();
        }
        
        [Route("StepOneError")]
        [HttpPost]
        [Authorize]
        public IActionResult StepOneErrorPost(string action)
        {
            if(action == "StepOne") 
                return RedirectToAction("StepOne", "CreateOrder");
            return RedirectToAction("Home", "Home");
        }
        
        [Route("StepTwo")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> StepTwo(DateOnly date, string phone, string kindOfAnimal)
        {
            List<TimeOnly> time = await _orderBaseService.SelectFreeTimeFromDateTimeAsync(date, HttpContext);
            ViewData["date"] = date;
            ViewData["phone"] = phone;
            ViewData["kindOfAnimal"] = kindOfAnimal;
            return View(time);
        }
        
        [Route("StepTwo")]
        [HttpPost]
        [Authorize]
        public IActionResult StepTwoPost(DateOnly date, TimeOnly time, string phone, string kindOfAnimal)
        {
            DateTime dateTime = date.ToDateTime(time);
        
            return RedirectToAction("StepThree", "CreateOrder",
                new { dateTime, phone, kindOfAnimal });
        }
        
        
        [Route("StepThree")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> StepThree(DateTime dateTime, string phone, string kindOfAnimal)
        {
            string connString = HttpContext.User.FindFirst("connString").Value;
            List<string> breeds = await _animalsBreedsAndPriceCervice.BreedFromKindOfAnimalsAsync(kindOfAnimal, connString);
        
            ViewData["dateTime"] = dateTime;
            ViewData["phone"] = phone;
            ViewData["kindOfAnimal"] = kindOfAnimal;
        
            return View(breeds);
        }
        
        [Route("StepThree")]
        [HttpPost]
        [Authorize]
        public IActionResult StepThreePost(DateTime dateTime, string phone, string kindOfAnimal, string breed, string animalName)
        {
            if (animalName is null)
                return RedirectToAction("StepThreeError", "CreateOrder",
                new {dateTime, phone, kindOfAnimal });
        
            return RedirectToAction("StepFour", "CreateOrder",
                new {dateTime, phone, kindOfAnimal, breed, animalName });
        }
        
        [Route("StepThreeError")]
        [HttpGet]
        [Authorize]
        public IActionResult StepThreeError(DateTime dateTime, string phone, string kindOfAnimal)
        {
            return View();
        }
        
        [Route("StepThreeError")]
        [HttpPost]
        [Authorize]
        public IActionResult StepThreeErrorPost(DateTime dateTime, string phone, string kindOfAnimal)
        {
            return RedirectToAction("StepThree", "CreateOrder",
                new { dateTime, phone, kindOfAnimal });
        }
        
        
        [Route("StepFour")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> StepFour(DateTime dateTime, string phone, string kindOfAnimal, string breed, string animalName)
        {
            string connString = HttpContext.User.FindFirst("connString").Value;
            ViewData["dateTime"] = dateTime;
            ViewData["phone"] = phone;
            ViewData["kindOfAnimal"] = kindOfAnimal;
            ViewData["animalBreed"] = breed;
            ViewData["animalName"] = animalName;
            int price = await _animalsBreedsAndPriceCervice.GetPriceAsync(kindOfAnimal, breed, connString);
            ViewData["price"] = price;
            return View();
        }
        
        [Route("StepFour")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> StepFourPost(DateTime dateTime, string phone, string kindOfAnimal, string breed, string animalName, int price)
        {
            try
            {
                string name = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                string connString = HttpContext.User.FindFirst("connString").Value;
        
                Order order = new Order(dateTime, name, phone, kindOfAnimal, animalName, breed, price);
        
                await _orderBaseService.AddOrderBaseAsync(HttpContext.User.FindFirst("connString").Value, order);
        
                if (!await _clientBaseService.ContainsClientAsync(name, connString))
                    await _clientBaseService.AddClientListAsync(name, phone, connString);
        
                await _clientBaseService.ChangePhoneAsync(connString, name, phone);
        
                Animal animal = new Animal(kindOfAnimal, breed, animalName);
                if (!await _clientBaseService.ClientContainsAnimalsAsync(connString, name, animal))
                    await _clientBaseService.AddAnimalToClientAsync(connString, name, animal);
        
            }
            catch (Exception) { return RedirectToAction("OrderNotMade", "CreateOrder"); }
            
            return RedirectToAction("PersonalArea", "PersonalArea");
        }
        
        [Route("OrderNotMade")]
        [HttpGet]
        [Authorize]
        public IActionResult OrderNotMade()
        {
            return View();
        }
    }
}

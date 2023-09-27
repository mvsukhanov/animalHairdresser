using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace animalHairdresser.Controllers
{
    public class CreateUserController : Controller
    {
        private readonly IClientBaseService _clientBaseService;

        public CreateUserController(IClientBaseService clientBaseService)
        {
            _clientBaseService = clientBaseService;
        }

        [Route("CreateUser")]
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }
        [Route("CreateUser")]
        [HttpPost]
        public async Task<IActionResult> CreateUserPostAsync(string name, string password, string action)
        {
            if (action == "CreateUser")
            {
                try
                {
                    if (password is null) throw new Exception(); 
                    await _clientBaseService.CreateUserAsync(name, password);
                }
                catch (Exception) { return RedirectToAction("UserAlreadyExists", "CreateUser"); }
                return RedirectToAction("UserCreated", "CreateUser");
            }

            return RedirectToAction("CreateUser", "CreateUser");
        }

        [Route("UserCreated")]
        [HttpGet]
        public IActionResult UserCreated()
        {
            return View();
        }

        [Route("UserAlreadyExists")]
        [HttpGet]
        public IActionResult UserAlreadyExists()
        {
            return View();
        }

        [Route("UserAlreadyExists")]
        [HttpPost]
        public IActionResult UserAlreadyExistsPost(string action)
        {
            if (action == "CreateUser")
            {
                return RedirectToAction("CreateUser", "CreateUser");
            }
            return RedirectToAction("Home", "Home");
        }
    }
}

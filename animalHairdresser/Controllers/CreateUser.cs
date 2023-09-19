using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace animalHairdresser.Controllers
{
    public class CreateUserController : Controller
    {
        private readonly IUserBaseService _usersBaseService;

        public CreateUserController(IUserBaseService userBaseService)
        {
            _usersBaseService = userBaseService;
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
                    await _usersBaseService.CreateUserAsync(name, password);
                }
                catch (Exception) { return RedirectToAction("UserAlreadyExists", "CreateUser"); }
                return RedirectToAction("UserCreated", "CreateUser");
            }
            if (action == "Home")
                return RedirectToAction("Home", "Home");

            return RedirectToAction("CreateUser", "CreateUser");
        }

        [Route("UserCreated")]
        [HttpGet]
        public IActionResult UserCreated()
        {
            return View();
        }

        [Route("UserCreated")]
        [HttpPost]
        public IActionResult UserCreatedPost(string action)
        {
            if (action == "Home")
                return RedirectToAction("Home", "Home");

            if (action == "PersonalArea")
                return RedirectToAction("PersonalArea", "PersonalArea");

            return RedirectToAction("UserCreated", "PersonalArea");
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
            if (action == "Home")
            {
                return RedirectToAction("Home", "Home");
            }
            return RedirectToAction("Home", "Home");
        }

    }
}

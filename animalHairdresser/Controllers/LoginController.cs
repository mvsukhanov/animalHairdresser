using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace animalHairdresser.Controllers
{
    public class LoginController : Controller
    {
        private readonly IClientBaseService _clientBaseService;

        public LoginController(IClientBaseService clientBaseService)
        {
            _clientBaseService = clientBaseService;
        }

        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> LoginPostAsync(string name, string password, string action)
        {
            if (action == "CreateUser")
                return RedirectToAction("CreateUser", "CreateUser");

            if (name == null || password == null)
                return RedirectToAction("EmptyString", "Login");

            try
            {
                await _clientBaseService.UserExistsOrNotAsync(name, password);
            }
            catch { return RedirectToAction("UserIsNotExist", "Login"); }
            
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, name)};
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("PersonalArea", "PersonalArea");
        }

        [Route("EmptyString")]
        [HttpGet]
        public IActionResult EmptyString()
        {
            return View();
        }
        [Route("EmptyString")]
        [HttpPost]
        public IActionResult EmptyStringPost(string action)
        {

            if (action == "Login")
                return RedirectToAction("Login", "Login");
            return RedirectToAction("EmptyString", "Login");
        }

        [Route("UserIsNotExist")]
        [HttpGet]
        public IActionResult UserIsNotExist()
        {
            return View();
        }

        [Route("UserIsNotExist")]
        [HttpPost]
        public IActionResult UserIsNotExistPost(string action)
        {
            if (action == "Login")
                return RedirectToAction("Login", "Login");
            if (action == "CreateUser")
                return RedirectToAction("CreateUser", "CreateUser");
            return RedirectToAction("Login", "Login");
        }
    }
}

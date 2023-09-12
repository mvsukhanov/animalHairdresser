using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace animalHairdresser.Controllers
{
    public class LoginController : Controller
    {
        public IUserBaseService UsersBaseService { get; set; }

        public LoginController(IUserBaseService userBaseService)
        {
            UsersBaseService = userBaseService;
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

            string connString = String.Format("Host=localhost;Username={0};Port=5432;Password={1};Database=AnimalShop", name, password);
            try
            {
                await UsersBaseService.UserExistsOrNotAsync(connString);
            }
            catch { return RedirectToAction("UserIsNotExist", "Login"); }
            
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, name), new Claim("connString", connString) };
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
            if (action == "Home")
                return RedirectToAction("Home", "Home");
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
            if (action == "Home")
                return RedirectToAction("Home", "Home");
            if (action == "Login")
                return RedirectToAction("Login", "Login");
            if (action == "CreateUser")
                return RedirectToAction("CreateUser", "CreateUser");
            return RedirectToAction("Login", "Login");
        }
    }
}

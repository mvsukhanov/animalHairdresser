using animalHairdresser.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace TestsLibrary
{
    [TestFixture]
    public class HomeControllerTest
    {
        [TestCase("AboutCompany")]
        [TestCase("PersonalArea")]
        [TestCase("CreateUser")]
        public void HomePostRedirectIsTrue(string action)
        {
            var controller = new HomeController();

            var result = (RedirectToActionResult)controller.HomePost(action);

            Assert.AreEqual(action, result.ActionName);
        }
    }
}
using animalHairdresser;
using animalHairdresser.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsLibrary
{
    internal class CreateUserControllerTest
    {
        [Test]
        public void CreateUserPostTestAsTask()
        {
            var mockUserBaseService = new Mock<IUserBaseService>();
            var controller = new CreateUserController(mockUserBaseService.Object);

            var result = controller.CreateUserPostAsync("", "", "");

            Assert.AreEqual(true, result.IsCompleted);
        }

        [TestCase("Home")]
        [TestCase("PersonalArea")]
        public void UserCreatedPostTest(string action)
        {
            var controller = new CreateUserController(new UsersBaseService());

            var result = (RedirectToActionResult)controller.UserCreatedPost(action);

            Assert.AreEqual(action, result.ActionName);
        }

        [TestCase("Home")]
        [TestCase("CreateUser")]
        public void UserAlreadyExistsPostTest(string action)
        {
            var controller = new CreateUserController(new UsersBaseService());

            var result = (RedirectToActionResult)controller.UserAlreadyExistsPost(action);

            Assert.AreEqual(action, result.ActionName);
        }
    }
}

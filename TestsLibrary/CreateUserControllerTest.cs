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
            var mockClientBaseService = new Mock<IClientBaseService>();
            var controller = new CreateUserController(mockClientBaseService.Object);

            var result = controller.CreateUserPostAsync("", "", "");

            Assert.AreEqual(true, result.IsCompleted);
        }

        [TestCase("Home")]
        [TestCase("CreateUser")]
        public void UserAlreadyExistsPostTest(string action)
        {
            var controller = new CreateUserController(new ClientBaseService());

            var result = (RedirectToActionResult)controller.UserAlreadyExistsPost(action);

            Assert.AreEqual(action, result.ActionName);
        }
    }
}

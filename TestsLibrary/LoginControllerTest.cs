using animalHairdresser.Controllers;
using animalHairdresser;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using static System.Collections.Specialized.BitVector32;

namespace TestsLibrary
{
    internal class LoginControllerTest
    {
        [Test]
        public void LoginPostAsyncTestWhenPressCreateUser()
        {
            var controller = new LoginController(new ClientBaseService());

            var result = (RedirectToActionResult)controller.LoginPostAsync("", "", "CreateUser").Result;
            
            Assert.AreEqual("CreateUser", result.ActionName);
        }

        [Test]
        public void LoginPostAsyncTestIfEmptyString()
        {
            var controller = new LoginController(new ClientBaseService());
        
            var result = (RedirectToActionResult)controller.LoginPostAsync(null, null, "Login").Result;
        
            Assert.AreEqual("EmptyString", result.ActionName);
        }

        [TestCase("Login")]
        public void EmptyStringPostTest(string action)
        {
            var controller = new LoginController(new ClientBaseService());

            var result = (RedirectToActionResult)controller.EmptyStringPost(action);

            Assert.AreEqual(action, result.ActionName);
        }

        [TestCase("Login")]
        [TestCase("CreateUser")]
        public void UserIsNotExistPostTest(string action)
        {
            var controller = new LoginController(new ClientBaseService());

            var result = (RedirectToActionResult)controller.UserIsNotExistPost(action);

            Assert.AreEqual(action, result.ActionName);
        }
    }
}

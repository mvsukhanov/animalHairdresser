using animalHairdresser.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsLibrary
{
    internal class MiscelaneousControllerTest
    {
        [TestCase("Login")]
        [TestCase("Home")]
        public void AcessDeniedPost(string action)
        {
            var controller = new MiscellaneousController();

            var result = (RedirectToActionResult)controller.AcessDeniedPost(action);

            Assert.AreEqual(action, result.ActionName);
        }
    }
}

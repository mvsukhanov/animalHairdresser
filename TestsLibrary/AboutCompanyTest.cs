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
    internal class AboutCompanyTest
    {
        [Test]
        public void AboutCompanyPost()
        {
            var controller = new AboutCompanyController();

            var result = (RedirectToActionResult)controller.AboutCompanyPost("Home");

            Assert.AreEqual("Home", result.ActionName);
        }
    }
}

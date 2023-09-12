using animalHairdresser.Controllers;
using animalHairdresser;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TestsLibrary
{
    internal class PersonalAreaControllerTest
    {
        [Test]
        public void PersonalAreaPostActionDeleteOrderTest()
        {
            var mockOrderBaseService = new Mock<IOrderBaseService>();
            var mockClientBaseService = new Mock<IClientBaseService>();

            var controller = new PersonalAreaController(mockClientBaseService.Object, mockOrderBaseService.Object);
            
            var result = controller.PersonalAreaPost(DateTime.Now, "пес", "овчарка", "Бобик", "DeleteOrder");

            Assert.AreEqual(true, result.IsCompleted);
        }

        [Test]
        public void PersonalAreaPostActionDeleteAnimalTest()
        {
            var mockOrderBaseService = new Mock<IOrderBaseService>();
            var mockClientBaseService = new Mock<IClientBaseService>();
        
            var controller = new PersonalAreaController(mockClientBaseService.Object, mockOrderBaseService.Object);
            //устанавливаю значения Claim
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim("connString", "Host=localhost;Username=Administrator;Port=5432;Password=123;Database=AnimalShop"),
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(claimsIdentity)
                }
            };
            
            var result = controller.PersonalAreaPost(DateTime.Now, "пес", "овчарка", "Бобик", "DeleteAnimal");

            Assert.AreEqual(true, result.IsCompleted);
        }


        [Test]
        public void PersonalAreaPostActionMakeNewOrderTest()
        {
            var mockOrderBaseService = new Mock<IOrderBaseService>();
            var mockClientBaseService = new Mock<IClientBaseService>();
            var controller = new PersonalAreaController(mockClientBaseService.Object, mockOrderBaseService.Object);
            
            var result = controller.PersonalAreaPost(DateTime.Now, "пес", "овчарка", "Бобик", "MakeNewOrder");

            Assert.AreEqual(true, result.IsCompleted);
        }
        
        [Test]
        public void PersonalAreaPostActionHomeTest()
        {
            var mockOrderBaseService = new Mock<IOrderBaseService>();
            var mockClientBaseService = new Mock<IClientBaseService>();
            var controller = new PersonalAreaController(mockClientBaseService.Object, mockOrderBaseService.Object);
            
            var result = controller.PersonalAreaPost(DateTime.Now, "пес", "овчарка", "Бобик", "Home");

            Assert.AreEqual(true, result.IsCompleted);
        }
    }
}

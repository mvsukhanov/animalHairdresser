using animalHairdresser;
using animalHairdresser.Controllers;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestsLibrary
{
    [TestFixture]
    public class CreateOrderControllerTest
    {
        Task<List<string>> TaskForStepOne = new Task<List<string>>(() => ReturnKindOfAnimalsList());

        public static List<string> ReturnKindOfAnimalsList()
        { 
            return new List<string> { "пес", "кот" }; 
        }

        Task<List<TimeOnly>> taskForStepTwo = new Task<List<TimeOnly>>(() => ReturnFreeTimes());
        public static List<TimeOnly> ReturnFreeTimes()
        {
            return new List<TimeOnly>
            {
                new TimeOnly(8, 00),
                new TimeOnly(8, 30),
                new TimeOnly(9, 00),
                new TimeOnly(9, 30),
                new TimeOnly(10, 00),
                new TimeOnly(10, 30),
                new TimeOnly(11, 00),
                new TimeOnly(11, 30),
                new TimeOnly(13, 00),
                new TimeOnly(13, 30),
                new TimeOnly(14, 00),
                new TimeOnly(14, 30),
                new TimeOnly(15, 00),
                new TimeOnly(15, 30),
                new TimeOnly(16, 00),
                new TimeOnly(16, 30)
            };
        }
        Task<List<string>> TaskForStepThree = new Task<List<string>>(() => ReturnBreeds());

        public static List<string> ReturnBreeds()
        {
            return new List<string> { "овчарка", "Той_терьер" };
        }

        [Test]
        public void StepOneTest()
        {
            //arrange
            var mockAnimalsBreedsAndPrice = new Mock<IAnimalsBreedsAndPriceService>();
            mockAnimalsBreedsAndPrice.Setup(AnimalsBreedsAndPrice =>
                AnimalsBreedsAndPrice.KindOfAnimalsListAsync()).Returns(TaskForStepOne);
            
            var controller = new CreateOrderController(new ClientBaseService(),
                new OrderBaseService(), mockAnimalsBreedsAndPrice.Object);
            //act
            Task result = controller.StepOne();
            //assert
            Assert.AreEqual(true, result.IsCompleted);
        }

        [Test]
        public void StepOnePostTest()
        {
            //arrange
            var controller = new CreateOrderController(new ClientBaseService(), 
                new OrderBaseService(), new AnimalsBreedsAndPriceCervice());
            //act
            var result = (RedirectToActionResult)controller.StepOnePost(DateOnly.FromDateTime(DateTime.Now.AddDays(1)), "111111", "пес");
            //assert
            Assert.AreEqual("StepTwo", result.ActionName);
            //act
            result = (RedirectToActionResult)controller.StepOnePost(DateOnly.FromDateTime(DateTime.Now.AddDays(1)), null, "пес");
            //assert
            Assert.AreEqual("StepOneError", result.ActionName);
            //act
            result = (RedirectToActionResult)controller.StepOnePost(DateOnly.Parse("01.01.2022"), "111111", "пес");
            //assert
            Assert.AreEqual("StepOneError", result.ActionName);
        }

        [Test]
        public void StepOneErrorTest() 
        {
            var controller = new CreateOrderController(new ClientBaseService(),
                new OrderBaseService(), new AnimalsBreedsAndPriceCervice());

            var result = controller.StepOneError();

            Assert.AreEqual("Microsoft.AspNetCore.Mvc.ViewResult", result.ToString());
        }

        [Test]
        public void StepOneErrorPostTest() 
        {
            var controller = new CreateOrderController(new ClientBaseService(),
                new OrderBaseService(), new AnimalsBreedsAndPriceCervice());

            var result = (RedirectToActionResult)controller.StepOneErrorPost("StepOne");

            Assert.AreEqual("StepOne", result.ActionName);
        }
        [Test]
        public void StepTwoTest() 
        {
            //arrange
            var mockOrderBaseService =new Mock<IOrderBaseService>();

            mockOrderBaseService.Setup(OrderBaseService =>
                OrderBaseService.SelectFreeTimeFromDateTimeAsync(DateOnly.FromDateTime(DateTime.Now))).
                    Returns(taskForStepTwo);
            var controller = new CreateOrderController(new ClientBaseService(),
               mockOrderBaseService.Object, new AnimalsBreedsAndPriceCervice());
            //act
            var result = controller.StepTwo(DateOnly.Parse("01.01.2022"), "111111", "пес");
            //assert
            Assert.AreEqual(true, result.IsCompleted);
        }

        [Test]
        public void StepTwoPostTest()
        {
            var controller = new CreateOrderController(new ClientBaseService(),
                new OrderBaseService(), new AnimalsBreedsAndPriceCervice());

            var result = (RedirectToActionResult)controller.
                StepTwoPost(DateOnly.Parse("01.01.2022"), TimeOnly.Parse("01:01"), "11111", "пес");

            Assert.AreEqual("StepThree", result.ActionName);
        }

        [Test]
        public void StepThreeTest()
        {
            //arrange
            var mockAnimalsBreedsAndPrice = new Mock<IAnimalsBreedsAndPriceService>();
            mockAnimalsBreedsAndPrice.Setup(AnimalsBreedsAndPrice =>
                AnimalsBreedsAndPrice.BreedFromKindOfAnimalsAsync("пес")).Returns(TaskForStepThree);
        
            var controller = new CreateOrderController(new ClientBaseService(),
                new OrderBaseService(), mockAnimalsBreedsAndPrice.Object);
            //act
            var result = controller.StepThree(DateTime.Now, "11111", "пес");
            //assert
            Assert.AreEqual(true, result.IsCompleted);
        }
        [Test]
        public void StepThreePostTest() 
        {
            var controller = new CreateOrderController(new ClientBaseService(),
                new OrderBaseService(), new AnimalsBreedsAndPriceCervice());

            var result = (RedirectToActionResult)controller.
                StepThreePost(DateTime.Now, "11111", "пес", "овчарка", "Бобик");

            Assert.AreEqual("StepFour", result.ActionName);

            result = (RedirectToActionResult)controller.
                StepThreePost(DateTime.Now, "11111", "пес", "овчарка", null);
            
            Assert.AreEqual("StepThreeError", result.ActionName);
        }

        [Test]
        public void StepThreeErrorTest() 
        {
            var controller = new CreateOrderController(new ClientBaseService(),
                new OrderBaseService(), new AnimalsBreedsAndPriceCervice());

            var result = controller.StepThreeError(DateTime.Now, "11111", "пес");

            Assert.AreEqual("Microsoft.AspNetCore.Mvc.ViewResult", result.ToString());
        }

        [Test]
        public void StepThreeErrorPostTest()
        {
            var controller = new CreateOrderController(new ClientBaseService(),
                new OrderBaseService(), new AnimalsBreedsAndPriceCervice());

            var result = (RedirectToActionResult)controller.
                StepThreeErrorPost(DateTime.Now, "11111", "пес");

            Assert.AreEqual("StepThree", result.ActionName);
        }

        [Test]
        public void StepFourTest() 
        {
            //arrange
            var mockAnimalsBreedsAndPrice = new Mock<IAnimalsBreedsAndPriceService>();
            mockAnimalsBreedsAndPrice.Setup(_ =>_.GetPriceAsync("пес", "овчарка")).Returns(new Task<int>(() => 1));

            var controller = new CreateOrderController(new ClientBaseService(),
                new OrderBaseService(), mockAnimalsBreedsAndPrice.Object);
            //act
            var result = controller.StepFour(DateTime.Now, "11111", "пес", "овчарка", "Бобик");
            //assert
            Assert.AreEqual(true, result.IsCompleted);
        }

        [Test]
        public void StepFourPostTest()
        {
            //arrange
            var mockClientBaseService =new Mock<IClientBaseService>();
            var mockOrderBaseService = new Mock<IOrderBaseService>();
            var mockAnimalsBreedsAndPrice = new Mock<IAnimalsBreedsAndPriceService>();

            mockOrderBaseService.Setup(_ => _.AddOrderBaseAsync(It.IsAny<Order>())).Returns(new Task<bool>(() => true));

            mockClientBaseService.Setup(_=>_.ContainsClientAsync("")).Returns(new Task<bool>(()=>true));
            mockClientBaseService.Setup(_ => _.ChangePhoneAsync("", "")).Returns(new Task<bool>(() => true));
            mockClientBaseService.Setup(_ => _.ClientContainsAnimalsAsync("", It.IsAny<Animal>())).Returns(new Task<bool>(() => true));

            var controller = new CreateOrderController(mockClientBaseService.Object,
               mockOrderBaseService.Object, mockAnimalsBreedsAndPrice.Object);

            //устанавливаю значения Claim
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "TestUser")
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(claimsIdentity)
                }
            };

            //act
            var result = controller.StepFourPost(DateTime.Now, "11111", "пес", "овчарка", "Бобик", 1);
            //assert
            Assert.AreEqual(true, result.IsCompleted);
        }

        [Test]
        public void OrderNotMadeTest()
        {
            var controller = new CreateOrderController(new ClientBaseService(),
                new OrderBaseService(), new AnimalsBreedsAndPriceCervice());

            var result = controller.OrderNotMade();

            Assert.AreEqual("Microsoft.AspNetCore.Mvc.ViewResult", result.ToString());
        }
    }
}

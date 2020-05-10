using MovieCRUD.Controllers;
using NUnit.Framework;
using System.Web.Mvc;

namespace MovieCRUD.Tests
{
    public class HomeControllerTest
    {
        [Test]
        public void Index_ReturnsView()
        {
            var homeController = new HomeController();

            var viewResult = homeController.Index() as ViewResult;

            Assert.IsNotNull(viewResult);
        }

        [Test]
        public void About_ReturnsView()
        {
            var homeController = new HomeController();

            var viewResult = homeController.About() as ViewResult;

            Assert.IsNotNull(viewResult);
        }

        [Test]
        public void Contact_ReturnsView()
        {
            var homeController = new HomeController();

            var viewResult = homeController.Contact() as ViewResult;

            Assert.IsNotNull(viewResult);
        }
    }
}

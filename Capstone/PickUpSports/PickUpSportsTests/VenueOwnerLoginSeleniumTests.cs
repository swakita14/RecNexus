using System;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
namespace PickUpSportsTests
{
    [TestFixture]
    public class VenueOwnerLoginSeleniumTests
    {
        private readonly IWebDriver _driver;

        public VenueOwnerLoginSeleniumTests()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://pickupsports-development.azurewebsites.net/Account/Login");
        }

        [Test]
        public void OwnerLogin_VenueOwnerLinkPresentInRegularLogin()
        {
            var element = _driver.FindElement(By.LinkText("Venue Owner"));

            element.Displayed.Should().BeTrue();

            element.Text.Should().Be("Venue Owner");
        }

        [Test]
        public void OwnerLogin_VenueOwnerLoginPage()
        {
            _driver.FindElement(By.LinkText("Venue Owner")).Click();

            //var title = _driver.FindElement(By.Id("venueOwner-login"));
            var email = _driver.FindElement(By.Id("Email"));
            var password = _driver.FindElement(By.Id("Password"));

            //title.Text.Should().Be("Venue Owner Log In");

            //title.Displayed.Should().BeTrue();
            email.Displayed.Should().BeTrue();
            password.Displayed.Should().BeTrue();
        }
    }
}

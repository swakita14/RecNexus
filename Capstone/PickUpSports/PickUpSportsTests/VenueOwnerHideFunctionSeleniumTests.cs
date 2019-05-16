using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Web.Configuration;

namespace PickUpSportsTests
{
    [TestFixture]
    public class VenueOwnerHideFunctionSeleniumTests
    {
        private readonly IWebDriver _driver;

        public VenueOwnerHideFunctionSeleniumTests()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://pickupsports-development.azurewebsites.net/Account/Login");
        }
        [Test]
        public void FindGame_PickUpGamesPage()
        {
            _driver.FindElement(By.LinkText("Log in as Venue Owner")).Click();
            _driver.FindElement(By.Id("Email")).SendKeys("jcx22092@icloud.com");
            _driver.FindElement(By.Id("Password")).SendKeys("Passw0rd!1819");
            _driver.FindElement(By.Id("login-button")).Click();
            _driver.FindElement(By.LinkText("View Games")).Click();

            // Sleep to give page time to load before last click
            Thread.Sleep(2000);

            _driver.FindElement(By.LinkText("CLICK HERE FOR MORE DETAILS ON GAME")).Click();
        }
    }
}
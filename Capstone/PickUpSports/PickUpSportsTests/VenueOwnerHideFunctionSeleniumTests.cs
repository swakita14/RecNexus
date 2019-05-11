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
            _driver.Navigate().GoToUrl("https://localhost:44341/Account/Login");
        }
        [Test]
        public void FindGame_PickUpGamesPage()
        {
            _driver.FindElement(By.LinkText("Venue Owner")).Click();
            _driver.FindElement(By.Id("Email")).SendKeys("jcx22092@icloud.com");
            _driver.FindElement(By.Id("Password")).SendKeys("Passw0rd!1819");
            _driver.FindElement(By.Id("login-button")).Click();
            _driver.FindElement(By.LinkText("PICK UP GAMES")).Click();
            _driver.FindElement(By.LinkText("CLICK HERE FOR MORE DETAILS ON GAME")).Click();
        }

    }
}
﻿using System;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace PickUpSportsTests
{


    [TestFixture]
    public class VenueOwnerSeleniumTests
    {
        private readonly IWebDriver _driver;


        public VenueOwnerSeleniumTests()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://pickupsports-development.azurewebsites.net/Game/CreateGame");
        }

        [SetUp]
        public void LogIn()
        {
            IWebElement username = _driver.FindElement(By.Id("Email"));
            username.Clear();
            username.SendKeys("");

            IWebElement password = _driver.FindElement(By.Id("Password"));
            password.Clear();
            password.SendKeys("");


            _driver.FindElement(By.XPath("//button[contains.,'Log In')]")).Click();

            //var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            
        }
        
        [Test]
        public void CreateGame_VenueHasNoOwner()
        {
            //selecting the dropdown
            var venues = _driver.FindElement(By.Name("VenueId"));
            var selectVenues = new SelectElement(venues);

            //select certain venue on dropdown
            selectVenues.SelectByText("Bryan Johnston Park");

            //expect an error message that owner is not there
            var element = _driver.FindElement(By.Id("error-msg"));
            element.Displayed.Should().BeTrue();
        }

        [TearDown]
        public void CleanUp()
        {
            _driver.Close();
        }
    }
}

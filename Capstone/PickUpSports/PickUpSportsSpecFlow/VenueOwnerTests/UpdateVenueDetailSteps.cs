using System;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;

namespace PickUpSportsSpecFlow
{
    [Binding]
    public class UpdateVenueDetailSteps : IDisposable
    {
        private readonly IWebDriver _driver;

        public UpdateVenueDetailSteps()
        {
            _driver = new ChromeDriver();
        }

        [Given(@"I have logged in as a venue owner for Real Sports Venue")]
        public void GivenIHaveLoggedInAsAVenueOwnerForRealSportsVenue()
        {
            _driver.Navigate().GoToUrl("https://pickupsports-development.azurewebsites.net/Account/Login");

            IWebElement username = _driver.FindElement(By.Id("Email"));
            username.Clear();
            username.SendKeys("");

            IWebElement password = _driver.FindElement(By.Id("Password"));
            password.Clear();
            password.SendKeys("");


            _driver.FindElement(By.Id("login-button")).Click();
        }
        
        [Given(@"I have navigated to my venue's detail page")]
        public void GivenIHaveNavigatedToMyVenueSDetailPage()
        {
            _driver.Navigate().GoToUrl("https://pickupsports-development.azurewebsites.net/Venue/Details");
        }
        
        [When(@"I press edit venue")]
        public void WhenIPressEditVenue()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should be taken to a page where I can edit all my venue fields")]
        public void ThenIShouldBeTakenToAPageWhereICanEditAllMyVenueFields()
        {
            ScenarioContext.Current.Pending();
        }

        public void Dispose()
        {
            if (_driver != null)
            {
                _driver.Dispose();
            }
        }
    }
}

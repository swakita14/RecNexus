using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace PickUpSportsTests
{
    class PreferenceNotificationSeleniumTests
    {
        private readonly IWebDriver _driver;

        public PreferenceNotificationSeleniumTests()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://localhost:44341/Account/Login");
        }
        [Test]
        public void UserLogin_UserLoginPage()
        {
            _driver.FindElement(By.Id("Email")).SendKeys("kpan17@mail.wou.edu");
            _driver.FindElement(By.Id("Password")).SendKeys("Zx4110740_");
            _driver.FindElement(By.Id("login-button")).Click();
            _driver.FindElement(By.LinkText("START A GAME")).Click();
            // select the drop down list
            var venue = _driver.FindElement(By.Id("VenueId"));
            //create select element object 
            var selectVenue = new SelectElement(venue);
            //select by value
            selectVenue.SelectByValue("8");
            // select the drop down list
            var sport = _driver.FindElement(By.Id("SportId"));
            //create select element object 
            var selectSport = new SelectElement(sport);
            //select by value
            selectSport.SelectByValue("9");
            //select time range
            IWebElement datePicker = _driver.FindElement(By.Id("datetimes"));
            //write new time range to the datePicker
            datePicker.Clear();
            datePicker.SendKeys("06/14/2019 04:00 PM - 06/14/2019 06:00 PM");
            _driver.FindElement(By.XPath("//body")).Click();
            _driver.FindElement(By.XPath("//div/center/button[text()='Create Your Game']")).Click();


            /*
             * _driver.Navigate().GoToUrl("https://mail.google.com");
             * _driver.FindElement(By.Id("identifierId")).SendKeys("wouscrumlords");
            _driver.FindElement(By.XPath("//span[@class='RveJvd snByac']")).Click();
            _driver.FindElement(By.XPath("//input[@class='whsOnd zHQkBf']")).SendKeys("Passw0rd!");
            _driver.FindElement(By.XPath("//span[@class='RveJvd snByac']")).Click();
             */
        }
    }
}

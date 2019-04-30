using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PickUpSportsTests
{
    [TestFixture]
    public class VenueDetailsSeleniumTests
    {
        private readonly IWebDriver _driver;
        public VenueDetailsSeleniumTests()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://pickupsports-development.azurewebsites.net/Venue/Details/1");
        }

        [Test]
        public void VenueDetails_HasReviewNameOnPage ()
        {
            var element = _driver.FindElement(By.LinkText("Leave a review!"));
            Assert.IsTrue(element.Displayed);
        }
    }
}

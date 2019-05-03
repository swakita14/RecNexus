using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PickUpSportsTests
{
    /**
     * PBI 404 - Shayna Conner
     * Wrote these tests before changing Venue/Details view so when view
     * is updated, we can ensure all necessary elements are added back
     */
    [TestFixture]
    public class VenueDetailsSeleniumTests
    {
        private readonly IWebDriver _driver;

        public VenueDetailsSeleniumTests()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://localhost:44341/Venue/Details/1");
        }

        [Test]
        public void VenueDetails_HasVenueName()
        {
            var element = _driver.FindElement(By.Id("Name"));
            element.Displayed.Should().BeTrue();
            element.Text.Should().Be("Bryan Johnston Park");
        }

        [Test]
        public void VenueDetails_HasAddressInformation()
        {
            var element = _driver.FindElement(By.Id("AddressInformation"));
            element.Displayed.Should().BeTrue();
            element.Text.Should().Contain("400 Mildred Ln SE");
        }

        [Test]
        public void VenueDetails_HasAverageRating()
        {
            var element = _driver.FindElement(By.Id("AverageRating"));
            element.Displayed.Should().BeTrue();
            element.Text.Should().Contain("Average Review Rating");
        }

        [Test]
        public void VenueDetails_HasBusinessHours()
        {
            var element = _driver.FindElement(By.Id("BusinessHours"));
            element.Displayed.Should().BeTrue();
            element.Text.Should().Contain("Hours of Operation");
        }

        [Test]
        public void VenueDetails_HasReviewLinkOnPage()
        {
            var element = _driver.FindElement(By.LinkText("Leave a review!"));
            element.Displayed.Should().BeTrue();
        }

        [Test]
        public void VenueDetails_HasReviewList()
        {
            var element = _driver.FindElement(By.Id("Reviews"));
            element.Displayed.Should().BeTrue();
        }

        [Test]
        public void VenueDetails_HasGameList()
        {
            var element = _driver.FindElement(By.Id("Games"));
            element.Displayed.Should().BeTrue();
        }
    }
}

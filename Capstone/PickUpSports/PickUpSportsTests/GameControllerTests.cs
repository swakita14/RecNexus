using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PickUpSports.Controllers;
using PickUpSports.DAL;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSportsTests
{
    [TestFixture]
    public class GameControllerTests
    {
        private readonly GameController _sut;
        
        public GameControllerTests()
        {
            // Mock Context to pass into service under test (GameController)
            var contextMock = new Mock<PickUpContext>();
            _sut = new GameController(contextMock.Object);
        }

        [Test]
        public void VenueIsAvailable_GivenValidDateRange_ReturnsTrue()
        {
            // Arrange
            List<BusinessHours> venueHours = new List<BusinessHours>();
            venueHours.Add(new BusinessHours{DayOfWeek = 1, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00")});
            venueHours.Add(new BusinessHours { DayOfWeek = 2, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });
            venueHours.Add(new BusinessHours { DayOfWeek = 3, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });

            DateTime startDateTime = DateTime.Parse("2019-04-01 01:00 PM");
            DateTime endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            // Act
            bool isVenueAvailable = _sut.VenueIsAvailable(venueHours, startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(isVenueAvailable, true);
        }

        [Test]
        public void VenueIsAvailable_GivenUnavailableDayOfWeek_ReturnsFalse()
        {
            // Arrange
            List<BusinessHours> venueHours = new List<BusinessHours>();
            venueHours.Add(new BusinessHours
                {DayOfWeek = 2, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00")});
            venueHours.Add(new BusinessHours
                {DayOfWeek = 3, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00")});

            DateTime startDateTime = DateTime.Parse("2019-04-01 01:00 PM");
            DateTime endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            // Act
            bool isVenueAvailable = _sut.VenueIsAvailable(venueHours, startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(isVenueAvailable, false);

        }

        [Test]
        public void VenueIsAvailable_GivenNonBusinessStartTime_ReturnsFalse()
        {
            // Arrange
            List<BusinessHours> venueHours = new List<BusinessHours>();
            venueHours.Add(new BusinessHours { DayOfWeek = 1, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });
            venueHours.Add(new BusinessHours { DayOfWeek = 2, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });
            venueHours.Add(new BusinessHours { DayOfWeek = 3, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });

            DateTime startDateTime = DateTime.Parse("2019-04-01 04:00 AM");
            DateTime endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            // Act
            bool isVenueAvailable = _sut.VenueIsAvailable(venueHours, startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(isVenueAvailable, false);
        }

        [Test]
        public void VenueIsAvailable_GivenNonBusinessEndTime_ReturnsFalse()
        {
            // Arrange
            List<BusinessHours> venueHours = new List<BusinessHours>();
            venueHours.Add(new BusinessHours { DayOfWeek = 1, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });
            venueHours.Add(new BusinessHours { DayOfWeek = 2, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });
            venueHours.Add(new BusinessHours { DayOfWeek = 3, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });

            DateTime startDateTime = DateTime.Parse("2019-04-01 1:00 PM");
            DateTime endDateTime = DateTime.Parse("2019-04-01 11:00 PM");

            // Act
            bool isVenueAvailable = _sut.VenueIsAvailable(venueHours, startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(isVenueAvailable, false);
        }

        [Test]
        public void VenueIsAvailable_BusinessDoesNotHaveHours_ReturnsFalse()
        {
            // Arrange
            List<BusinessHours> venueHours = new List<BusinessHours>();

            DateTime startDateTime = DateTime.Parse("2019-04-01 04:00 AM");
            DateTime endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            // Act
            bool isVenueAvailable = _sut.VenueIsAvailable(venueHours, startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(isVenueAvailable, false);
        }
    }
}

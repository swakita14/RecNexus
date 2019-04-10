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

        /**
         * PBI 252 - Shayna Conner
         */
        [Test]
        public void IsVenueAvailable_GivenValidDateRange_ReturnsTrue()
        {
            // Arrange
            var venueHours = new List<BusinessHours>();
            venueHours.Add(new BusinessHours
                {DayOfWeek = 1, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00")});
            venueHours.Add(new BusinessHours
                {DayOfWeek = 2, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00")});
            venueHours.Add(new BusinessHours
                {DayOfWeek = 3, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00")});

            var startDateTime = DateTime.Parse("2019-04-01 01:00 PM");
            var endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            // Act
            var isVenueAvailable = _sut.IsVenueAvailable(venueHours, startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(isVenueAvailable, true);
        }

        /**
         * PBI 252 - Shayna Conner
         */
        [Test]
        public void IsVenueAvailable_GivenUnavailableDayOfWeek_ReturnsFalse()
        {
            // Arrange
            var venueHours = new List<BusinessHours>();
            venueHours.Add(new BusinessHours
                {DayOfWeek = 2, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00")});
            venueHours.Add(new BusinessHours
                {DayOfWeek = 3, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00")});

            var startDateTime = DateTime.Parse("2019-04-01 01:00 PM");
            var endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            // Act
            var isVenueAvailable = _sut.IsVenueAvailable(venueHours, startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(isVenueAvailable, false);
        }

        /**
         * PBI 252 - Shayna Conner
         */
        [Test]
        public void IsVenueAvailable_GivenNonBusinessStartTime_ReturnsFalse()
        {
            // Arrange
            var venueHours = new List<BusinessHours>();
            venueHours.Add(new BusinessHours
                {DayOfWeek = 1, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00")});
            venueHours.Add(new BusinessHours
                {DayOfWeek = 2, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00")});
            venueHours.Add(new BusinessHours
                {DayOfWeek = 3, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00")});

            var startDateTime = DateTime.Parse("2019-04-01 04:00 AM");
            var endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            // Act
            var isVenueAvailable = _sut.IsVenueAvailable(venueHours, startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(isVenueAvailable, false);
        }

        /**
         * PBI 252 - Shayna Conner
         */
        [Test]
        public void IsVenueAvailable_GivenNonBusinessEndTime_ReturnsFalse()
        {
            // Arrange
            var venueHours = new List<BusinessHours>();
            venueHours.Add(new BusinessHours
                {DayOfWeek = 1, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00")});
            venueHours.Add(new BusinessHours
                {DayOfWeek = 2, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00")});
            venueHours.Add(new BusinessHours
                {DayOfWeek = 3, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00")});

            var startDateTime = DateTime.Parse("2019-04-01 1:00 PM");
            var endDateTime = DateTime.Parse("2019-04-01 11:00 PM");

            // Act
            var isVenueAvailable = _sut.IsVenueAvailable(venueHours, startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(isVenueAvailable, false);
        }

        /**
         * PBI 252 - Shayna Conner
         */
        [Test]
        public void IsVenueAvailable_BusinessDoesNotHaveHours_ReturnsFalse()
        {
            // Arrange
            var venueHours = new List<BusinessHours>();

            var startDateTime = DateTime.Parse("2019-04-01 04:00 AM");
            var endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            // Act
            var isVenueAvailable = _sut.IsVenueAvailable(venueHours, startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(isVenueAvailable, false);
        }
    }
}
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

        /**
         * PBI 142 - Shion Wakita
         */
        [Test]
        public void IsCreatorOfGame_UserIsNotCreator_ReturnsFalse()
        {
            //Arrange 
            var startDateTime = DateTime.Parse("2019-04-01 04:00 AM");
            var endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            Game gameTest1 = new Game(){ContactId = 1, VenueId = 2, GameId = 3, GameStatusId = 1, SportId = 1, StartTime = startDateTime, EndTime = endDateTime};
            Game gameTest2 = new Game() { ContactId = 2, VenueId = 2, GameId = 3, GameStatusId = 1, SportId = 1, StartTime = startDateTime, EndTime = endDateTime };

            //Act
            var isCreatorOfGameTest1 = _sut.IsCreatorOfGame(2, gameTest1);
            var isCreatorOfGameTest2 = _sut.IsCreatorOfGame(1, gameTest2);

            //Assert
            Assert.AreEqual(isCreatorOfGameTest1, false);
            Assert.AreEqual(isCreatorOfGameTest2, false);
        }

        /**
         * PBI 142 - Shion Wakita
         */
        [Test]
        public void IsCreatorOfGame_UserIsCreator_ReturnsTrue()
        {
            //Arrange 
            var startDateTime = DateTime.Parse("2019-04-01 04:00 AM");
            var endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            Game gameTest1 = new Game() { ContactId = 1, VenueId = 3, GameId = 4, GameStatusId = 1, SportId = 2, StartTime = startDateTime, EndTime = endDateTime };
            Game gameTest2 = new Game() { ContactId = 2, VenueId = 4, GameId = 3, GameStatusId = 1, SportId = 1, StartTime = startDateTime, EndTime = endDateTime };

            //Act
            var isCreatorOfGameTest1 = _sut.IsCreatorOfGame(1, gameTest1);
            var isCreatorOfGameTest2 = _sut.IsCreatorOfGame(2, gameTest2);

            //Assert
            Assert.AreEqual(isCreatorOfGameTest1, true);
            Assert.AreEqual(isCreatorOfGameTest2, true);

        }

        // Kexin Pan
        [Test]
        public void IsSelectedTimeValid_TimeIsValid_ReturnsTrue()
        {
            var startDateTime = DateTime.Parse("2019-04-11 09:00 PM");
            var endDateTime = DateTime.Parse("2019-04-11 10:00 PM");

            var isSelectedTimeValideTest = _sut.IsSelectedTimeValid(startDateTime, endDateTime);
            Assert.AreEqual(isSelectedTimeValideTest, true);
        }

        [Test]
        public void IsSelectedTimeValid_TimeIsNotValid_ReturnsFalse()
        {
            var startDateTime = DateTime.Parse("2019-04-12 09:00 PM");
            var endDateTime = DateTime.Parse("2019-04-22 10:00 PM");

            var isSelectedTimeValideTest = _sut.IsSelectedTimeValid(startDateTime, endDateTime);
            Assert.AreEqual(isSelectedTimeValideTest, false);
        }


        /**
         * PBI 254 - Shion Wakita
         */
        [Test]
        public void IsNotSignedUpForGame_UserIsNotSignedUp_ReturnsTrue()
        {
            //Arrange
            List<PickUpGame> playerList = new List<PickUpGame>();
            playerList.Add(new PickUpGame()
                { ContactId = 1, GameId = 2, PickUpGameId = 1} );
            playerList.Add(new PickUpGame()
                { ContactId = 2, GameId = 2, PickUpGameId = 2 });

            //Act
            var isNotSignedUpForGameTest = _sut.IsNotSignedUpForGame(3, playerList);

            //Assert
            Assert.AreEqual(isNotSignedUpForGameTest, true);

        }

        [Test]
        public void IsNotSignedUpForGame_UserIsSignedUp_ReturnsFalse()
        {
            //Arrange
            List<PickUpGame> playerList = new List<PickUpGame>();
            playerList.Add(new PickUpGame()
                { ContactId = 1, GameId = 2, PickUpGameId = 1 });
            playerList.Add(new PickUpGame()
                { ContactId = 2, GameId = 2, PickUpGameId = 2 });

            //Act
            var isNotSignedUpForGameTest = _sut.IsNotSignedUpForGame(1, playerList);

            //Assert
            Assert.AreEqual(isNotSignedUpForGameTest, false);
        }

    }
}
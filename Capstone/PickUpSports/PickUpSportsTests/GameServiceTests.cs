using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Services;

namespace PickUpSportsTests
{
    [TestFixture]
    public class GameServiceTests
    {
        private readonly Mock<IPickUpGameRepository> _pickUpGameRepoMock;
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly Mock<ISportRepository> _sportRepositoryMock;
        private readonly GameService _sut;

        public GameServiceTests()
        {
            _pickUpGameRepoMock = new Mock<IPickUpGameRepository>();
            _gameRepositoryMock = new Mock<IGameRepository>();
            _sportRepositoryMock = new Mock<ISportRepository>();

            _sut = new GameService(_pickUpGameRepoMock.Object, _gameRepositoryMock.Object, _sportRepositoryMock.Object);
        }

        /**
          * PBI 267 - Shayna Conner
          */
        [Test]
        public void GetSportNameById_GivenId_ReturnsName()
        {
            // Arrange
            _sportRepositoryMock.Setup(x => x.GetSportById(It.IsAny<int>())).Returns(new Sport
            {
                SportID = 1,
                SportName = "Hello"
            });

            // Act
            var result = _sut.GetSportNameById(It.IsAny<int>());

            // Assert
            Assert.AreEqual(result, "Hello");
        }

        /**
          * PBI 327 - Shayna Conner
          */
        [Test]
        public void GetAllGamesByContactId_GivenContactId_ReturnsList()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game {ContactId = 1, GameId = 1, VenueId = 1},
                new Game {ContactId = 1, GameId = 2, VenueId = 1},
                new Game {ContactId = 1, GameId = 3, VenueId = 1},
                new Game {ContactId = 2, GameId = 4, VenueId = 1},
                new Game {ContactId = 3, GameId = 5, VenueId = 1},
                new Game {ContactId = 4, GameId = 6, VenueId = 1}
            };
            _gameRepositoryMock.Setup(x => x.GetAllGames()).Returns(games);
            
            // Act
            var results = _sut.GetAllGamesByContactId(1);

            // Assert
            results.Count.Should().Be(3);
        }

        /**
          * PBI 327 - Shayna Conner
          */
        [Test]
        public void GetAllGamesByContactId_IfZeroGames_ReturnNull()
        {
            // Arrange
            var games = new List<Game>();
            _gameRepositoryMock.Setup(x => x.GetAllGames()).Returns(games);

            // Act
            var results = _sut.GetAllGamesByContactId(1);

            // Assert
            Assert.IsNull(results);
        }

        /**
          * PBI 327 - Shayna Conner
          */
        [Test]
        public void GetAllGamesByContactId_IfNull_ReturnNull()
        {
            // Arrange
            _gameRepositoryMock.Setup(x => x.GetAllGames()).Returns((List<Game>)null);

            // Act
            var results = _sut.GetAllGamesByContactId(1);

            // Assert
            Assert.IsNull(results);
        }

        /**
          * PBI 327 - Shayna Conner
          */
        [Test]
        public void GetCurrentOrderedGamesByContactId_GivenContactId_ReturnsList()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game {ContactId = 1, GameId = 1, StartTime = DateTime.Today.AddDays(-4), EndTime = DateTime.Today.AddDays(-4), VenueId = 1},
                new Game {ContactId = 1, GameId = 2, StartTime = DateTime.Today.AddDays(3), EndTime = DateTime.Today.AddDays(3), VenueId = 1},
                new Game {ContactId = 1, GameId = 3, StartTime = DateTime.Today.AddDays(6), EndTime = DateTime.Today.AddDays(6), VenueId = 1},
                new Game {ContactId = 2, GameId = 4, VenueId = 1},
                new Game {ContactId = 3, GameId = 5, VenueId = 1},
                new Game {ContactId = 4, GameId = 6, VenueId = 1},
                new Game {ContactId = 1, GameId = 7, StartTime = DateTime.Today.AddDays(-2), EndTime = DateTime.Today.AddDays(-2), VenueId = 1},

            };
            _gameRepositoryMock.Setup(x => x.GetAllGames()).Returns(games);

            // Act
            var results = _sut.GetCurrentOrderedGamesByContactId(1);

            // Assert
            results.Count.Should().Be(2);
        }

        /**
          * PBI 327 - Shayna Conner
          */
        [Test]
        public void GetCurrentOrderedGamesByContactId_IfNoGames_ReturnsNull()
        {
            // Arrange
            _gameRepositoryMock.Setup(x => x.GetAllGames()).Returns((List<Game>)null);

            // Act
            var results = _sut.GetCurrentOrderedGamesByContactId(1);

            // Assert
            Assert.IsNull(results);
        }

        /**
          * PBI 324 - Kexin Pan
          */
        [Test]
        public void GetCurrentGamesByVenueId_GivenVenueId_ReturnsList()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game {ContactId = 2, GameId = 4, VenueId = 23},
                new Game {ContactId = 3, GameId = 5, VenueId = 22},
                new Game {ContactId = 4, GameId = 6, VenueId = 23},
                new Game {ContactId = 1, GameId = 9, VenueId = 23},

            };
            _gameRepositoryMock.Setup(x => x.GetAllGames()).Returns(games);

            // Act
            var results = _sut.GetCurrentGamesByVenueId(23);

            // Assert
            results.Count.Should().Be(3);
        }

        /**
          * PBI 324 - Kexin Pan
          */
        [Test]
        public void GetCurrentGamesByVenueId_GivenVenueId_ReturnsNull()
        {
            // Arrange
            _gameRepositoryMock.Setup(x => x.GetAllGames()).Returns((List<Game>)null);

            // Act
            var results = _sut.GetCurrentGamesByVenueId(30);

            // Assert
            Assert.IsNull(results);
        }

    }

}

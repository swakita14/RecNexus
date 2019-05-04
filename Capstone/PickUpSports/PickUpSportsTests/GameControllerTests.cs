using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PickUpSports.Controllers;
using PickUpSports.DAL;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSportsTests
{
    [TestFixture]
    public class GameControllerTests
    {
        private readonly Mock<IContactService> _contactServiceMock;
        private readonly Mock<IGMailService> _gmailServiceMock;
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly Mock<IVenueService> _venueServiceMock;

        private readonly GameController _sut;

        public GameControllerTests()
        {

            _contactServiceMock = new Mock<IContactService>();
            _gmailServiceMock = new Mock<IGMailService>();
            _gameServiceMock = new Mock<IGameService>();
            _venueServiceMock = new Mock<IVenueService>();
            _sut = new GameController(_contactServiceMock.Object, _gmailServiceMock.Object, _gameServiceMock.Object, _venueServiceMock.Object);
        }
    }
}
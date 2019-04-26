using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Services;

namespace PickUpSportsTests
{
    [TestFixture]
    public class ContactServiceTests
    {
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly Mock<ITimePreferenceRepository> _timeRepositoryMock;
        private readonly Mock<ISportPreferenceRepository> _sportPrefRepositoryMock;
        private readonly Mock<IReviewRepository> _reviewRepositoryMock;
        private readonly Mock<ISportRepository> _sportRepositoryMock;
        private readonly Mock<IPickUpGameRepository> _pickUpGameRepositoryMock;
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly ContactService _sut;

        public ContactServiceTests()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _timeRepositoryMock = new Mock<ITimePreferenceRepository>();
            _sportPrefRepositoryMock = new Mock<ISportPreferenceRepository>();
            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _sportRepositoryMock = new Mock<ISportRepository>();
            _pickUpGameRepositoryMock = new Mock<IPickUpGameRepository>();
            _gameRepositoryMock = new Mock<IGameRepository>();

            _sut = new ContactService(_contactRepositoryMock.Object, _timeRepositoryMock.Object, _sportPrefRepositoryMock.Object, _reviewRepositoryMock.Object, _sportRepositoryMock.Object, _pickUpGameRepositoryMock.Object, _gameRepositoryMock.Object);
        }


        /**
         * PBI 321 - Shayna Conner
         */
        [Test]
        public void UsernameIsTaken_IsNotTaken_ReturnFalse()
        {
            // Arrange
            _contactRepositoryMock.Setup(x => x.GetContactByUsername(It.IsAny<string>())).Returns((Contact)null);

            // Act
            var result = _sut.UsernameIsTaken("hi");

            // Assert
            Assert.IsFalse(result);
        }

        /**
          * PBI 321 - Shayna Conner
          */
        [Test]
        public void UsernameIsTaken_IsTaken_ReturnTrue()
        {
            // Arrange
            Contact contact = new Contact
            {
                Username = "fakeUser"
            };

            _contactRepositoryMock.Setup(x => x.GetContactByUsername(It.IsAny<string>())).Returns(contact);

            // Act
            var result = _sut.UsernameIsTaken("fakeUser");

            // Assert
            Assert.IsTrue(result);
        }

        /**
         * PBI 321 - Shayna Conner
         */
        [Test]
        public void GetUserSportPreferences_GivenContactId_ReturnList()
        {
            // Arrange
            _sportPrefRepositoryMock.Setup(x => x.GetUsersSportPreferencesByContactId(It.IsAny<int>())).Returns(
                new List<SportPreference>
                {
                    new SportPreference {ContactID = 1, SportID = 1, SportPrefID = 1},
                    new SportPreference {ContactID = 1, SportID = 1, SportPrefID = 1}
                });

            _sportRepositoryMock.Setup(x => x.GetSportNameById(1)).Returns(It.IsAny<string>());

            // Act
            var result = _sut.GetUserSportPreferences(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<List<string>>();
        }

        /**
          * PBI 321 - Shayna Conner
          */
        [Test]
        public void GetUserSportPreferences_NoPreferences_ReturnNull()
        {
            // Arrange
            _sportPrefRepositoryMock.Setup(x => x.GetUsersSportPreferencesByContactId(It.IsAny<int>()))
                .Returns(new List<SportPreference>());

            // Act
            var result = _sut.GetUserSportPreferences(It.IsAny<int>());

            // Assert
            result.Should().BeNull();
        }

        /**
          * PBI 321 - Shayna Conner
          */
        [Test]
        public void GetUserTimePreferences_GivenContactId_ReturnList()
        {
            // Arrange
            _timeRepositoryMock.Setup(x => x.GetUsersTimePreferencesByContactId(It.IsAny<int>())).Returns(
                new List<TimePreference>
                {
                    It.IsAny<TimePreference>(),
                    It.IsAny<TimePreference>()
                });

            // Act
            var result = _sut.GetUserTimePreferences(It.IsAny<int>());

            // Assert
            result.Should().BeOfType<List<TimePreference>>();
        }

        /**
          * PBI 321 - Shayna Conner
          */
        [Test]
        public void GetUserTimePreferences_NoPreferences_ReturnNull()
        {
            // Arrange
            _timeRepositoryMock.Setup(x => x.GetUsersTimePreferencesByContactId(It.IsAny<int>()))
                .Returns(new List<TimePreference>());

            // Act
            var result = _sut.GetUserTimePreferences(It.IsAny<int>());

            // Assert
            result.Should().BeNull();
        }

        /**
          * PBI 321 - Shayna Conner
          */
        [Test]
        public void DeleteUser_GivenContact_DeleteIt()
        {
            // Arrange
            var contact = new Contact
            {
                ContactId = 1
            };

            _sportPrefRepositoryMock.Setup(x => x.GetUsersSportPreferencesByContactId(contact.ContactId))
                .Returns(new List<SportPreference>());
            _timeRepositoryMock.Setup(x => x.GetUsersTimePreferencesByContactId(contact.ContactId))
                .Returns(new List<TimePreference>());
            _reviewRepositoryMock.Setup(x => x.GetReviewsByContactId(contact.ContactId))
                .Returns(new List<Review>());
            _pickUpGameRepositoryMock.Setup(x => x.GetPickUpGameListByContactId(contact.ContactId))
                .Returns(new List<PickUpGame>());
            _gameRepositoryMock.Setup(x => x.GetGameListByContactId(contact.ContactId))
                .Returns(new List<Game>());

            // Act
            _sut.DeleteUser(contact);

            // Assert 
            _contactRepositoryMock.Verify(x => x.DeleteContact(It.IsAny<Contact>()), Times.Once());
        }

        /**
          * PBI 321 - Shayna Conner
          */
        [Test]
        public void DeleteUser_IfGamesHavePlayers_SetToNullThenDelete()
        {
            // Arrange
            var contact = new Contact
            {
                ContactId = 1
            };

            _sportPrefRepositoryMock.Setup(x => x.GetUsersSportPreferencesByContactId(contact.ContactId))
                .Returns(new List<SportPreference>());
            _timeRepositoryMock.Setup(x => x.GetUsersTimePreferencesByContactId(contact.ContactId))
                .Returns(new List<TimePreference>());
            _reviewRepositoryMock.Setup(x => x.GetReviewsByContactId(contact.ContactId))
                .Returns(new List<Review>());
            _pickUpGameRepositoryMock.Setup(x => x.GetPickUpGameListByContactId(contact.ContactId))
                .Returns(new List<PickUpGame>());

            // Return a game that has a players
            _gameRepositoryMock.Setup(x => x.GetGameListByContactId(contact.ContactId))
                .Returns(new List<Game>{new Game{ContactId = 1}});
            _pickUpGameRepositoryMock.Setup(x => x.GetPickUpGameListByGameId(It.IsAny<int>()))
                .Returns(new List<PickUpGame> {new PickUpGame {ContactId = 2}});

            // Act
            _sut.DeleteUser(contact);

            // Assert 
            _gameRepositoryMock.Verify(x => x.EditGame(It.IsAny<Game>()), Times.Once);
        }
    }
}

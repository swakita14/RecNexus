using Moq;
using NUnit.Framework;
using PickUpSports.Interface;
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
        private readonly Mock<IReviewRepository> _reviewRepository;
        private readonly Mock<ISportRepository> _sportRepository;
        private readonly ContactService _sut;

        public ContactServiceTests()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _timeRepositoryMock = new Mock<ITimePreferenceRepository>();
            _sportPrefRepositoryMock = new Mock<ISportPreferenceRepository>();
            _reviewRepository = new Mock<IReviewRepository>();
            _sportRepository = new Mock<ISportRepository>();

            _sut = new ContactService(_contactRepositoryMock.Object, _timeRepositoryMock.Object, _sportPrefRepositoryMock.Object, _reviewRepository.Object, _sportRepository.Object);
        }

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
    }
}

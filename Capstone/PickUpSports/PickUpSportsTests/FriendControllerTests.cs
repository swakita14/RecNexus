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
    public class FriendControllerTests
    {
        private readonly FriendsController _sut;

        public FriendControllerTests()
        {
            // Mock Context to pass into service under test (GameController)
            var contextMock = new Mock<PickUpContext>();
            _sut = new FriendsController(contextMock.Object);
        }

        [Test]
        public void IsAlreadyAFriend_IsAFriend_ReturnsTrue()
        {
            //Arrange
            Contact friend = new Contact()
                {ContactId = 2, Username = "testuser", FirstName = "Test", LastName = "Certified", Address1 = "345 Monmouth St.", Email = "testuser@gmail.com", City = "Testville", PhoneNumber = "9028472819", State = "Test", ZipCode = "902843"};

            var friendList = new List<Friend>();
            friendList.Add(new Friend
                {FriendID = 1, ContactID = 3, FriendContactID = 2});
            friendList.Add(new Friend
                { FriendID = 2, ContactID = 3, FriendContactID = 4 });
            friendList.Add(new Friend
                { FriendID = 3, ContactID = 4, FriendContactID = 5 });

            //Act
            var isAlreadyAFriend = _sut.IsAlreadyAFriend(3, friend, friendList);


            //Assert
            Assert.AreEqual(isAlreadyAFriend, true);
        }

        [Test]
        public void IsAlreadyAFriend_IsNotAFriend_ReturnsFalse()
        {
            //Arrange
            Contact friend = new Contact()
                { ContactId = 2, Username = "testuser", FirstName = "Test", LastName = "Certified", Address1 = "345 Monmouth St.", Email = "testuser@gmail.com", City = "Testville", PhoneNumber = "9028472819", State = "Test", ZipCode = "902843" };

            var friendList = new List<Friend>();
            friendList.Add(new Friend
                { FriendID = 1, ContactID = 2, FriendContactID = 3 });
            friendList.Add(new Friend
                { FriendID = 2, ContactID = 3, FriendContactID = 4 });
            friendList.Add(new Friend
                { FriendID = 3, ContactID = 4, FriendContactID = 5 });

            //Act
            var isAlreadyAFriend = _sut.IsAlreadyAFriend(3, friend, friendList);


            //Assert
            Assert.AreEqual(isAlreadyAFriend, false);
        }
    }
}

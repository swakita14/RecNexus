using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Moq;
using NUnit.Framework;
using PickUpSports.Interface;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Services;

namespace PickUpSportsTests
{
    [TestFixture]
    public class GMailServiceTests
    {
        private readonly Mock<NetworkCredential> _networkCredentialsMock;
        private readonly Mock<SmtpClient> _smtpClientMock;
        private readonly GMailService _sut;


        public GMailServiceTests()
        {
            _networkCredentialsMock = new Mock<NetworkCredential>();
            _smtpClientMock = new Mock<SmtpClient>();

            _sut = new GMailService(_smtpClientMock.Object, _networkCredentialsMock.Object);
        }

        [Test]
        public void Send_SentMessage_ReturnTrue()
        {
            var toEmailAddress = "test@gmail.com";
            var body = "This is a test message";

            var emailHasSent = _sut.Send(body, toEmailAddress);

            Assert.AreEqual(emailHasSent, true);
        }
    }
}

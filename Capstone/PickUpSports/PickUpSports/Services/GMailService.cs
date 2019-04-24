using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using PickUpSports.Interface;

namespace PickUpSports.Services
{
    public class GMailService : IGMailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly NetworkCredential _networkCredential;
        private bool _hasSent;

        public GMailService(SmtpClient smtpClient, NetworkCredential networkCredential)
        {
            _smtpClient = smtpClient;
            _networkCredential = networkCredential;
            _hasSent = false;
        }

        public string GetEmailAddress()
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["GMailUsername"] + "@gmail.com";
        }

        /**
         * This Method will send the message with the message content(body) and the email address passed in
         */
        public bool Send(string body, string toEmailAddress)
        {
            //Creating new Message with information passed in
            var message = new MailMessage(GetEmailAddress(), toEmailAddress)
            {
                Subject = "Change in Game Information",
                Body = body
            };

            //Checking it to true make sure its in html format
            message.IsBodyHtml = true;

            //Enabling credentials and ssl
            _smtpClient.Credentials = _networkCredential;
            _smtpClient.EnableSsl = true;

            //Send the message with the credentials 
            _smtpClient.Send(message);

            //Method reached here, so assuming message has been sent
            _hasSent = true;

            return _hasSent;
        }
    }
}
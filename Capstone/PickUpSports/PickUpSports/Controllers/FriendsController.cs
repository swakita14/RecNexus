using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PickUpSports.DAL;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel;
using PickUpSports.Models.ViewModel.ContactController;

namespace PickUpSports.Controllers
{
    public class FriendsController : Controller
    {
        private readonly PickUpContext _context;
        private readonly IContactService _contactService;
        private readonly IGMailService _gMailer;
        private readonly IGameService _gameService;

        public FriendsController(PickUpContext context)
        {
            _context = context;
        }

        public FriendsController(PickUpContext context, IContactService contactService, IGMailService gMailer, IGameService gameService)
        {
            _context = context;
            _contactService = contactService;
            _gMailer = gMailer;
            _gameService = gameService;
        }

        // GET: Friends

        [Authorize]
        [HttpPost]
        public ActionResult AddFriend(ProfileViewModel model)
        {
            //Get the current users email
            string email = User.Identity.GetUserName();

            //Find the user using the email
            Contact contact = _contactService.GetContactByEmail(email);

            //find the friend
            Contact contactFriend = _contactService.GetContactById(model.ContactId);

            //find the list of friends of the current logged-in user
            List<Friend> friendList = _context.Friends.Where(x => x.ContactID == contact.ContactId).ToList();

            //check if the current logged-on user has the person already added as a friend
            if (IsAlreadyAFriend(contact.ContactId, contactFriend, friendList))
            {
                ViewData.ModelState.AddModelError("CurrentFriend", "You have already added them to the list");
                return RedirectToAction("FriendList", "Friends", new { id = contact.ContactId });
            }

            //if the person's not on the list, initialize a friend object
            Friend friend = new Friend
            {
                ContactID = contact.ContactId,
                FriendContactID = contactFriend.ContactId
            };
        
            //add them into the db and save changes
            _context.Friends.Add(friend);
            _context.SaveChanges();

            //redirect them to the list with the friend added 
            return RedirectToAction("FriendList", "Friends", new { id = contact.ContactId });

        }


        [Authorize]
        [HttpGet]
        public ActionResult FriendList(int id)
        {
            //Find the list of friends using the contactId
            List<Friend> friendList = _context.Friends.Where(x => x.ContactID == id).ToList();

            //initializing list of ViewModel
            List<ViewFriendsViewModel> model = new List<ViewFriendsViewModel>();

            //assigning each value from the list to the ViewModel
            foreach (var friend in friendList)
            {
                model.Add(new ViewFriendsViewModel
                {
                    FriendId = friend.FriendID,
                    ContactId = id,
                    ContactFriendId = friend.FriendContactID,
                    FriendName = _context.Contacts.Find(friend.FriendContactID).Username,
                    FriendEmail = _context.Contacts.Find(friend.FriendContactID).Email,
                    FriendNumber = _context.Contacts.Find(friend.FriendContactID).PhoneNumber
                });
            }

            return View(model);

        }

        [Authorize]
        [HttpGet]
        public ActionResult FriendInvite(int id)
        {
            //Find the list of friends using the contactId
            List<Friend> friendList = _context.Friends.Where(x => x.ContactID == id).ToList();

            //Find the list of games using the contactId
            List<Game> gameList = _context.Games.Where(x => x.ContactId == id).ToList();

            //initializing list of ViewModel
            var model = new FriendInviteViewModel();
            model.Friends =friendList;

            ViewBag.FriendName = friends;
                return View();
            
          
        }


        /*
        * PBI 148 Austin Bergman
       */
        [Authorize]
        [HttpPost]
        public ActionResult FriendInvite(Friend friend, Game game, string button)
        {
            //find the current logged-on user
            string email = User.Identity.GetUserName();
            Contact currContactUser = _contactService.GetContactByEmail(email);
           
            if (button.Equals("Send Invite"))
            {
                SendInvite(game, friend.FriendID);
            }
            
           return View();
        }

        /*
         * PBI 148 Austin Bergman
         */
         public void SendInvite(Game game, int friendId)
        {
            string sendToEmail = "";
            string messageContent = "";

            
            string url = Url.Action("GameDetails", "Game",
                new System.Web.Routing.RouteValueDictionary(new { id = game.GameId }),
                "http", Request.Url.Host);

            int playerCount = _gameService.GetPickUpGameListByGameId(game.GameId).Count();

            // sending email to the friend 
            sendToEmail = _contactService.GetContactById(friendId).Email;
            messageContent = "Please join my game! the current player count is " + playerCount + " here is a link to the game " + url;


            MailMessage mailMessage = new MailMessage(_gMailer.GetEmailAddress(), sendToEmail)
            {
                Body = messageContent
            };

            _gMailer.Send(mailMessage);
        }

      

        public bool IsAlreadyAFriend(int contactId, Contact friend, List<Friend> friendList)
        { 

            
            if (friend == null) return false;


            foreach (var person in friendList)
            {
                if (person.FriendContactID == friend.ContactId && person.ContactID == contactId)
                {
                    return true;
                }
            }

            return false;

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

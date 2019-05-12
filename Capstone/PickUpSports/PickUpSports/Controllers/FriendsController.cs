using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
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
        public ActionResult FriendInvite(int gameId)
        {
            //find the current logged-on user
            string email = User.Identity.GetUserName();
            Contact currContactUser = _contactService.GetContactByEmail(email);

            // Get the id of the user
            int currID = currContactUser.ContactId;
          
            // Populate Dropdown list
            PopulateDropdownValues(currID);

            // populate model
            FriendInviteViewModel model = new FriendInviteViewModel();
            model.ContactId = currID;
            model.GameId = gameId;
            
            return View(model);
            

        }


        /*
        * PBI 148 Austin Bergman
       */
        [Authorize]
        [HttpPost]
        public ActionResult FriendInvite(FriendInviteViewModel model, int friendId)
        {
            //find the current logged-on user
            string email = User.Identity.GetUserName();
            Contact currContactUser = _contactService.GetContactByEmail(email);
            int currID = currContactUser.ContactId;

            // Get the Game from the Model
            Game game = _gameService.GetGameById(model.GameId);


            // get list of friends from logged in user
            List<Friend> friends = _context.Friends.Where(x => x.ContactID == currID).ToList();

            
            // Check model validation before doing anything
            if (!ModelState.IsValid)
                       {
                           PopulateDropdownValues(currID);
                           return View(model);
                       }

            // Get contact info from friend 
            Friend friendinv = friends.Find(f => f.FriendID == friendId);
            int friendInvId = friendinv.FriendContactID;


            // pass Game and Contactfriend to the send email function
            SendInvite(game, friendInvId );


            //redirect them back to the changed game detail
            return RedirectToAction("GameDetails", "Game",new { id = model.GameId });
        }

        /*
         * PBI 148 Austin Bergman
         */
         public void SendInvite(Game game, int friendContactId)
        {
            //find the current logged-on user
            string email = User.Identity.GetUserName();
            Contact currContactUser = _contactService.GetContactByEmail(email);

            string sendToEmail = "";
            string messageContent = "";

                     
            string url = Url.Action("GameDetails", "Game",
                new System.Web.Routing.RouteValueDictionary(new { id = game.GameId }),
                "http", Request.Url.Host);

            var existingPlayers = _gameService.GetPickUpGameListByGameId(game.GameId);
            if (existingPlayers == null) messageContent = "Please join my game! Here is a link to the details where you can also join: " + url;
            else
            {
                var playerCount = existingPlayers.Count;
                messageContent = "Please join my game! the current player count is " + playerCount + " here is a link to the game " + url;
            }

            // sending email to the friend 
            sendToEmail = _contactService.GetContactById(friendContactId).Email;
            
            MailMessage mailMessage = new MailMessage(_gMailer.GetEmailAddress(), sendToEmail)
            {
                Subject = $"{currContactUser.Username} has invited you to join a game!",
                Body = messageContent
            };

            _gMailer.Send(mailMessage);
        }


         public void PopulateDropdownValues(int id)
         {
           // Key: FriendId Value: Username of Friend
            ViewBag.Friends = _context.Friends.Where(x => x.ContactID == id).ToList().ToDictionary(f => f.FriendID, f => _context.Contacts.Find(f.FriendContactID).Username);
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

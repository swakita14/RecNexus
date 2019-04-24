using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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

        public FriendsController(PickUpContext context, IContactService contactService)
        {
            _context = context;
            _contactService = contactService;
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
            Contact contactFriend = _contactService.GetContactByUsername(model.Username);

            //find the list of friends of the current logged-in user
            List<Friend> friendList = _context.Friends.Where(x => x.ContactID == contact.ContactId).ToList();

            //check if the current logged-on user has the person already added as a friend
            if (IsAlreadyAFriend(contact.ContactId, contactFriend, friendList))
            {
                ViewData.ModelState.AddModelError("CurrentFriend", "You have already added them to the list");
                return RedirectToAction("FriendList", "Friends", new { contactId = contact.ContactId });
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
            return RedirectToAction("FriendList", "Friends", new { contactId = contact.ContactId });

        }


        [Authorize]
        [HttpGet]
        public ActionResult FriendList(int contactId)
        {
            //Find the list of friends using the contactId
            List<Friend> friendList = _context.Friends.Where(x => x.ContactID == contactId).ToList();

            //initializing list of ViewModel
            List<ViewFriendsViewModel> model = new List<ViewFriendsViewModel>();

            //assigning each value from the list to the ViewModel
            foreach (var friend in friendList)
            {
                model.Add(new ViewFriendsViewModel
                {
                    FriendId = friend.FriendID,
                    ContactId = contactId,
                    ContactFriendId = friend.FriendContactID,
                    FriendName = _context.Contacts.Find(friend.ContactID).Username,
                    FriendEmail = _context.Contacts.Find(friend.ContactID).Email,
                    FriendNumber = _context.Contacts.Find(friend.ContactID).PhoneNumber
                });
            }

            return View(model);

        }

        public bool IsAlreadyAFriend(int contactId, Contact friend, List<Friend> friendList)
        { 

            Contact contact = _context.Contacts.Find(contactId);
            if (contact == null) return false;


            foreach (var person in friendList)
            {
                if (person.FriendID == friend.ContactId && person.ContactID == contact.ContactId)
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

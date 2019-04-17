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
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel;

namespace PickUpSports.Controllers
{
    public class FriendsController : Controller
    {
        private readonly PickUpContext _context;

        public FriendsController(PickUpContext context)
        {
            _context = context;
        }

        // GET: Friends
        public ActionResult FriendList()
        {
            string email = User.Identity.GetUserName();

            Contact contact = _context.Contacts.FirstOrDefault(x => x.Email.Equals(email));

            Contact contactFriend = _context.Contacts.Find(contactId);

            List<Friend> friendList = _context.Friends.Where(x => x.ContactID == contact.ContactId).ToList();

            if (isAlreadyAFriend(contact.ContactId, contactFriend, friendList))
            {
                return RedirectToAction("FriendList", "Friends");
            }














            return View(_context.Friends.ToList());
        }

        public ActionResult FriendList(ViewContactFriendList model)
        {
            return RedirectToAction()
        }

        public bool isAlreadyAFriend(int contactId, Contact friend, List<Friend> FriendList)
        { 

            Contact contact = _context.Contacts.Find(contactId);

            foreach (var person in FriendList)
            {
                if (person.FriendID == friend.ContactId && person.ContactID == contact.ContactId)
                {
                    return true;
                }
            }

            return false;


        }

        // GET: Friends/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friend friend = _context.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        // GET: Friends/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Friends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FriendID,ContactID,FriendContactID")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                _context.Friends.Add(friend);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(friend);
        }

        // GET: Friends/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friend friend = _context.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        // POST: Friends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FriendID,ContactID,FriendContactID")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(friend).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(friend);
        }

        // GET: Friends/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friend friend = _context.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        // POST: Friends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Friend friend = _context.Friends.Find(id);
            _context.Friends.Remove(friend);
            _context.SaveChanges();
            return RedirectToAction("Index");
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

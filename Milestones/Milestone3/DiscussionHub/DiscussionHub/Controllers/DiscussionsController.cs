using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DiscussionHub.DAL;
using DiscussionHub.Models;
using DiscussionHub.Models.ViewModels;

namespace DiscussionHub.Controllers
{
    public class DiscussionsController : Controller
    {
        private DiscussionHubContext db = new DiscussionHubContext();

        // GET: Discussions
        public ActionResult ViewDiscussion()
        {
            return View(db.Discussions.ToList().OrderBy(x => x.Rank));
        }



        // GET: Discussions/Details/5
        public ActionResult ViewDiscussionDetails(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discussion discussion = db.Discussions.Find(id);
            if (discussion == null)
            {
                return HttpNotFound();
            }
            return View(discussion);
        }

        // GET: Discussions/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Discussions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DiscussionId,VoteCount,UpvoteCount,DownvoteCount,CommentCount,TotalViews,Rank,UserId,ArticleLink,Title,Contents")] Discussion discussion)
        {
            if (ModelState.IsValid)
            {
                db.Discussions.Add(discussion);
                db.SaveChanges();
                return RedirectToAction("Index", "Manage");
            }

            return View(discussion);
        }

        // GET: Discussions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discussion discussion = db.Discussions.Find(id);
            if (discussion == null)
            {
                return HttpNotFound();
            }
            return View(discussion);
        }

        // POST: Discussions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DiscussionId,VoteCount,UpvoteCount,DownvoteCount,CommentCount,TotalViews,Rank,UserId,ArticleLink,Title,Contents")] Discussion discussion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discussion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Manage");
            }
            return View(discussion);
        }

        // GET: Discussions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discussion discussion = db.Discussions.Find(id);
            if (discussion == null)
            {
                return HttpNotFound();
            }
            return View(discussion);
        }

        // POST: Discussions/Delete/5
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Discussion discussion = db.Discussions.Find(id);
            db.Discussions.Remove(discussion);
            db.SaveChanges();
            return RedirectToAction("Index", "Manage");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Search(String titlePart)
        {
            titlePart = Request.QueryString["search"];

            if (titlePart == null)
            {
                ViewBag.message = false;
                return View();
            }
            else
            {
                ViewBag.message = true;
                var titleMatch = db.Discussions.Where(p => p.Title.Contains(titlePart));
                return View(titleMatch.ToList());
            }
        }


    }
}

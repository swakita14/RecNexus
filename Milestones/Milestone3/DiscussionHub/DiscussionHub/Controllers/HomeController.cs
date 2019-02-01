using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DiscussionHub.Models;
using DiscussionHub.DAL;

namespace DiscussionHub.Controllers
{
    public class HomeController : Controller
    {
        private DiscussionHubContext db = new DiscussionHubContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Here is a list of current users";

            return View(db.DiscussionHubUsers.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ViewDiscussions()
        {
            return View(db.Discussions.ToList().OrderBy(x => x.Rank));
        }

        public ActionResult ViewDiscussionDetails(int? id)
        {
            Debug.WriteLine(id);

            var model = db.Discussions.Where(x => x.UserId == id);

            Debug.WriteLine(model);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            

            return View(model);
        }
    }
}
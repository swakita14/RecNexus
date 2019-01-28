using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            return View(db.Users.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
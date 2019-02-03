
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DiscussionHub.DAL;
using DiscussionHub.Models;



namespace DiscussionHub.Controllers
{
    public class HomeController : Controller
    {
        DiscussionHubContext db = new DiscussionHubContext();
        public ActionResult Index()
        {
            return View(db.Discussions.OrderByDescending(x => x.PostTime).Take(5).ToList());
        }
    }
}
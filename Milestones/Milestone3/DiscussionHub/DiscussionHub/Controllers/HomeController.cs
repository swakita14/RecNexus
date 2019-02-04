using System.Linq;
using System.Web.Mvc;
using DiscussionHub.DAL;


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
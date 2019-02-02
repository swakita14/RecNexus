using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DiscussionHub.DAL;

namespace DiscussionHub.Controllers
{
    public class DiscussionController : Controller
    {
        private DiscussionHubContext db = new DiscussionHubContext();


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
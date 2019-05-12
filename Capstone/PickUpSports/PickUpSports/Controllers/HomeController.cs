using System.Web.Mvc;

namespace PickUpSports.Controllers
{
    public class HomeController : Controller
    {
        [RequireHttps]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

    }
   
}
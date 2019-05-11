using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PickUpSports.Models.ViewModel;
using PickUpSports.Models.ViewModel.TrendingController;
using PickUpSports.Services;

namespace PickUpSports.Controllers
{
    public class TrendingController : Controller
    {

        private readonly TrendingWebServices _trendingWebServices;

        public TrendingController()
        {
            _trendingWebServices = new TrendingWebServices();
        }
         public TrendingModel TrendingResult { get; set; }


        // GET: Trending
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GetPredictionFromWebService()
        {
            var venueName = Request.Form["venueName"];
            var sportName = Request.Form["sportName"];

            if (!string.IsNullOrEmpty(venueName) && !string.IsNullOrEmpty(sportName))
            {
                var resultResponse = _trendingWebServices
                    .InvokeRequestResponseService<ResultOutcome>(venueName, sportName).Result;
                if (resultResponse != null)
                {
                    var result = resultResponse.Results.Output1.Value.Values;
                    TrendingResult = new TrendingModel()
                    {
                        VenueName = result[0, 0],
                        SportName = result[0, 1]
                    };

                }
            }

            ViewBag.myData = TrendingResult.VenueName;
            return View("Index");

        }
    }
}
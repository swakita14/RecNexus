using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PickUpSports.DAL;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel;
using PickUpSports.Models.ViewModel.TrendingController;
using PickUpSports.Services;
using static System.Int32;

namespace PickUpSports.Controllers
{
    public class TrendingController : Controller
    {
        private readonly PickUpContext _context;
        private readonly TrendingWebServices _trendingWebServices;
        private readonly IGameService _gameService;
        private readonly IVenueService _venueService;


        public TrendingController(PickUpContext context)
        {
            _context = context;
        }

        public TrendingController(IGameService gameService, IVenueService venueService)
        {
            
            _gameService = gameService;
           _venueService = venueService;
           _trendingWebServices = new TrendingWebServices();
        }

      public TrendingController()
        {
            _trendingWebServices = new TrendingWebServices();
        }


        public TrendingModel TrendingResult { get; set; }


        // GET: Trending
        public ActionResult Index()
        {
            PopulateDropdownValues();
            return View();
        }


        [HttpPost]
        public ActionResult GetPredictionFromWebService()
        {
           // var venueName = Request.Form["venueName"];
            int sportId = Parse(Request.Form["SportName"]);

            var sportName = _gameService.GetSportNameById(sportId);

            if (!string.IsNullOrEmpty(sportName))
            {
                var resultResponse = _trendingWebServices.InvokeRequestResponseService<ResultOutcome>(sportName).Result;
                if (resultResponse != null)
                {
                    var result = resultResponse.Results.Output1.Value.Values;
                    TrendingResult = new TrendingModel()
                    {
                        VenueName = result[0, 2],
                        SportName = result[0, 1]
                    };

                }
            }

            string venName = TrendingResult.VenueName;
            //Venue findVenue = _venueService.
           // int venID = findVenue.VenueId;


            ViewBag.myData = venName;
           // ViewBag.venID = venID;
            PopulateDropdownValues();
            return View("Index");

        }

        public void PopulateDropdownValues()
        {
            ViewBag.Sport = _gameService.GetAllSports().ToList().ToDictionary(s => s.SportID, s => s.SportName);
        }
    }
}
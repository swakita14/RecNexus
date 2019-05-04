using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PickUpSports.DAL;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.Enums;
using PickUpSports.Models.ViewModel;
using PickUpSports.Models.ViewModel.GameController;
using DayOfWeek = System.DayOfWeek;

namespace PickUpSports.Controllers
{
    public class GameController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IGMailService _gMailer;
        private readonly IGameService _gameService;
        private readonly IVenueService _venueService;

        public GameController(IContactService contactService, 
            IGMailService gMailer, 
            IGameService gameService, 
            IVenueService venueService)
        {
            _contactService = contactService;
            _gMailer = gMailer;
            _gameService = gameService;
            _venueService = venueService;
        }

        /**
         * Routes user to page that contains Create Game form
         */
        [Authorize]
        public ActionResult CreateGame()
        {
            ViewBag.GameCreated = false;
            PopulateDropdownValues();
            return View();
        }

        /**
         * Handles input from Create Game form 
         */
        [HttpPost]
        public ActionResult CreateGame(CreateGameViewModel model)
        {
            // Check model validation before doing anything
            if (!ModelState.IsValid)
            {
                PopulateDropdownValues();
                return View(model);
            }

            // Get start and end dates
            var dates = model.DateRange.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            var startDateTime = DateTime.Parse(dates[0], CultureInfo.InvariantCulture);
            var endDateTime = DateTime.Parse(dates[1], CultureInfo.CurrentCulture);

            // Return error to View if user picks different dates for start and end
            if (startDateTime.DayOfWeek != endDateTime.DayOfWeek)
            {
                ViewData.ModelState.AddModelError("DateRange", "Start date and end date must be same date.");
                PopulateDropdownValues();
                return View(model);
            }

            // Check if similar game already exists
            Game existingGame = _gameService.CheckForExistingGame(model.VenueId, model.SportId, startDateTime);
            if (existingGame != null)
            {
                // TODO - Add link to existing game details later when that page is created
                ViewData.ModelState.AddModelError("GameExists", "A game already exists with this venue, sport, and time.");
                PopulateDropdownValues();
                return View(model);
            }

            // Get venue by ID and business hours for that venue
            Venue venue = _venueService.GetVenueById(model.VenueId);
            List<BusinessHours> venueHours = _venueService.GetVenueBusinessHours(model.VenueId);

            // Return error to View if the venue is not available
            bool isVenueAvailable = _venueService.IsVenueAvailable(venueHours, startDateTime, endDateTime);
            if (!isVenueAvailable)
            {
                ViewData.ModelState.AddModelError("DateRange", $"Unfortunately, {venue.Name} is not available during the hours you chose.");
                PopulateDropdownValues();
                return View(model);
            }

            string email = User.Identity.GetUserName();
            Contact contact = _contactService.GetContactByEmail(email);

            // All validation passed so add game to database 
            Game newGame = new Game
            {
                ContactId = contact.ContactId,
                GameStatusId = (int)GameStatusEnum.Open,
                VenueId = model.VenueId,
                SportId = model.SportId,
                StartTime = startDateTime,
                EndTime = endDateTime
            };

            _gameService.CreateGame(newGame);

            ViewBag.GameCreated = true;
            PopulateDropdownValues();

            //send notification to users once a new game created and it includes the user's preference
            List<SportPreference> checkSportPreference = _contactService.GetAllSportPreferences();

            foreach (var item in checkSportPreference)
            {
                if (item.SportID == model.SportId && item.ContactID != newGame.ContactId)
                {
                    var fileContents = System.IO.File.ReadAllText(Server.MapPath("~/Content/EmailFormat.html"));
                    //add game link to the email
                    var directUrl = Url.Action("GameDetails", "Game", new { id = newGame.GameId }, protocol: Request.Url.Scheme);
                    fileContents = fileContents.Replace("{URL}", directUrl);
                    //replace the html contents to the game details
                    fileContents = fileContents.Replace("{VENUE}", venue.Name);
                    fileContents = fileContents.Replace("{SPORT}", _gameService.GetSportNameById(item.SportID));
                    fileContents = fileContents.Replace("{STARTTIME}", startDateTime.ToString());
                    fileContents = fileContents.Replace("{ENDTIME}", endDateTime.ToString());
                    SendMessage(newGame, item.ContactID, fileContents);
                }
            }

            // time preference
            List<TimePreference> checkTimePreferences = _contactService.GetAllTimePreferences();
            List<Contact> nonDuplicateUser = new List<Contact>();
            bool duplicate = false ;
            foreach (var item in checkTimePreferences)
            {
                if ((int)newGame.StartTime.DayOfWeek==item.DayOfWeek
                    && newGame.StartTime.TimeOfDay>item.BeginTime && newGame.EndTime.TimeOfDay<item.EndTime
                    && item.ContactID != newGame.ContactId)
                {
                    foreach(var user in nonDuplicateUser)
                    {
                        if (user.ContactId == item.ContactID)
                        {
                            duplicate = true;
                        }
                        else
                        {
                            duplicate = false;
                        }
                    }
                    if (!duplicate)
                    {
                        nonDuplicateUser.Add(_contactService.GetContactById(item.ContactID));
                    }
                    foreach (var checkeduser in nonDuplicateUser)
                    {
                        var fileContents = System.IO.File.ReadAllText(Server.MapPath("~/Content/EmailFormat.html"));
                        //add game link to the email
                        var directUrl = Url.Action("GameDetails", "Game", new { id = newGame.GameId }, protocol: Request.Url.Scheme);
                        fileContents = fileContents.Replace("{URL}", directUrl);
                        //replace the html contents to the game details
                        fileContents = fileContents.Replace("{VENUE}", venue.Name);
                        fileContents = fileContents.Replace("{SPORT}", _gameService.GetSportNameById(model.SportId));
                        fileContents = fileContents.Replace("{STARTTIME}", startDateTime.ToString());
                        fileContents = fileContents.Replace("{ENDTIME}", endDateTime.ToString());
                        SendMessage(newGame, item.ContactID, fileContents);
                    }
                }
            }


            return View();
        }

        /**
         * Routes user to page that shows a list of all current, open games
         */
        public ActionResult GameList()
        {
            ViewBag.Venue = new SelectList(_venueService.GetAllVenues(), "VenueId", "Name");
            ViewBag.Sport = new SelectList(_gameService.GetAllSports(), "SportId", "SportName");

            // Get games that are open and that have not already passed and order by games happening soonest
            List<Game> games = _gameService.GetAllCurrentOpenGames();

            List<GameListViewModel> model = new List<GameListViewModel>();
            foreach (var game in games)
            {
                var gameToAdd = new GameListViewModel();
                gameToAdd.GameId = game.GameId;
                gameToAdd.Sport = _gameService.GetSportNameById(game.SportId);
                gameToAdd.Venue = _venueService.GetVenueNameById(game.VenueId);
                gameToAdd.StartDate = game.StartTime;
                gameToAdd.EndDate = game.EndTime;

                if (game.ContactId != null)
                {
                    gameToAdd.ContactId = game.ContactId;
                    gameToAdd.ContactName = _contactService.GetContactById(game.ContactId).Username;
                }

                model.Add(gameToAdd);
            }

            return View(model);
        }

        /**
         * Routes user to GameDetails page to show details for single game
         */
        [Authorize]
        [HttpGet]
        public ActionResult GameDetails(int id)
        {
            ViewBag.IsCreator = false;

            //validating the id to make sure its not null
            if (id == 0) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //getting current logged in user information in case they want to join game
            string email = User.Identity.GetUserName();
            Contact contact = _contactService.GetContactByEmail(email);

            Game game = _gameService.GetGameById(id);
           
            if (_gameService.IsCreatorOfGame(contact.ContactId, game)) ViewBag.IsCreator = true;

            //if there are no games then return: 
            if (game == null) return HttpNotFound();

            //creating view model for the page
            ViewGameViewModel model = new ViewGameViewModel()
            {
                EndDate = game.EndTime.ToString(),
                GameId = game.GameId,
                Status = Enum.GetName(typeof(GameStatusEnum), game.GameStatusId),
                Sport = _gameService.GetSportNameById(game.SportId),
                StartDate = game.StartTime.ToString(),
                Venue = _venueService.GetVenueNameById(game.VenueId),
            };

            if (game.ContactId != null)
            {
                model.ContactId = game.ContactId;
                model.ContactName = _contactService.GetContactById(game.ContactId).Username;
            }

            //returning model to the view
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GameDetails(ViewGameViewModel model, string button)
        {
            ViewBag.IsCreator = false;

            string body = "";

            //Find all the players that are currently signed up for the game
            List<PickUpGame> checkGames = _gameService.GetPickUpGameListByGameId(model.GameId);

            Game game = _gameService.GetGameById(model.GameId);

            //find the current logged-on user
            string email = User.Identity.GetUserName();
            Contact currContactUser = _contactService.GetContactByEmail(email);

            //If the Join Game button was pressed 
            if (button.Equals("Join Game"))
            {
                //sending model back so values dont blank out
                ViewGameViewModel returnModel = new ViewGameViewModel()
                {
                    EndDate = game.EndTime.ToString(),
                    GameId = game.GameId,
                    Status = Enum.GetName(typeof(GameStatusEnum), game.GameStatusId),
                    Sport = _gameService.GetSportNameById(game.SportId),
                    StartDate = game.StartTime.ToString(),
                    Venue = _venueService.GetVenueNameById(game.VenueId)
                };

                if (game.ContactId != null)
                {
                    returnModel.ContactId = game.ContactId;
                    returnModel.ContactName = _contactService.GetContactById(game.ContactId).Username;
                }

                //if the game is cancelled, users are prevent to join this game
                if (game.GameStatusId == 2)
                {
                    //error message
                    ViewData.ModelState.AddModelError("SignedUp", "Sorry, this game is canceled, you can not join this game");
                    return View(returnModel);
                }

                //check if the person is already signed up for the game 
                if (!_gameService.IsNotSignedUpForGame(currContactUser.ContactId, checkGames))
                {
                    //error message
                    ViewData.ModelState.AddModelError("SignedUp", "You are already signed up for this game");

                    return View(returnModel);
                }

                //add new person to the pickupgame table
                PickUpGame newPickUpGame = new PickUpGame()
                {
                    ContactId = currContactUser.ContactId,
                    GameId = model.GameId,
                };

                //Compose the body of the Message
                body = currContactUser.Username + " has just joined the game. ";

                //save it       
                _gameService.AddPlayerToGame(newPickUpGame);
            }

            //If the Leave Game button was pressed 
            if (button.Equals("Leave Game"))
            {
                //check if the person is already signed up for the game 
                if (_gameService.IsNotSignedUpForGame(currContactUser.ContactId, checkGames))
                {
                    //error message
                    ViewData.ModelState.AddModelError("SignedUp", "You have not signed up for this game");

                    //sending model back so values dont blank out
                    ViewGameViewModel returnModel = new ViewGameViewModel()
                    {
                        EndDate = game.EndTime.ToString(),
                        GameId = game.GameId,
                        Status = Enum.GetName(typeof(GameStatusEnum), game.GameStatusId),
                        Sport = _gameService.GetSportNameById(game.SportId),
                        StartDate = game.StartTime.ToString(),
                        Venue = _venueService.GetVenueNameById(game.VenueId)
                    };

                    if (game.ContactId != null)
                    {
                        returnModel.ContactId = game.ContactId;
                        returnModel.ContactName = _contactService.GetContactById(game.ContactId).Username;
                    }

                    return View(returnModel);
                }

                Debug.Write(model);

                //Creating body for the mail notification
                body = currContactUser.Username + " has just left the game. ";

                //Remove the Player from the Game 
                var usersGames = _gameService.GetPickUpGamesByContactId(currContactUser.ContactId);
                var pickUpGame = usersGames.FirstOrDefault(x => x.GameId == model.GameId);
                _gameService.RemovePlayerFromGame(pickUpGame);
                                    
            }

            // If contact ID null then creator has deleted account, do not send email
            if (game.ContactId != null) SendMessage(game, (int)game.ContactId, body);

            return RedirectToAction("GameDetails", new { id = model.GameId });
        }

        public void SendMessage(Game game, int playerId, string body)
        {
            //Initializing Message Details 
            string sendingToEmail = "";
            string messageContent = "";
            int playerCount = 0;

            if (_gameService.GetPickUpGameListByGameId(game.GameId) == null)
            {
                playerCount = 0;
            }
            else
            {
                playerCount = _gameService.GetPickUpGameListByGameId(game.GameId).Count();
            }

          
            //Either sending the message to the Creator of the game or the Players in the game
            if (game.ContactId == playerId)
            {
                sendingToEmail = _contactService.GetContactById((int)game.ContactId).Email;
                messageContent = body + "The current number of players on this game is: " + playerCount;

            }
            else
            {
                //emailing to the players on the game list
                sendingToEmail = _contactService.GetContactById(playerId).Email;
                messageContent = body;
            }

            MailMessage mailMessage = new MailMessage(_gMailer.GetEmailAddress(), sendingToEmail)
            {
                Body = messageContent
            };

            //Send the Message
            _gMailer.Send(mailMessage);
        }



        /**
         * Partial view that to display business hours on CreateGame page
         */
        [HttpGet]
        public PartialViewResult BusinessHoursByVenueId(int id)
        {
            // Map business hours
            List<BusinessHours> businessHours = _venueService.GetVenueBusinessHours(id);
            List<BusinessHoursViewModel> model = new List<BusinessHoursViewModel>();

            foreach (var businessHour in businessHours)
            {
                var closeDateTime = new DateTime() + businessHour.CloseTime;
                var openDateTime = new DateTime() + businessHour.OpenTime;

                model.Add(new BusinessHoursViewModel
                {
                    DayOfWeek = Enum.GetName(typeof(DayOfWeek), businessHour.DayOfWeek),
                    CloseTime = closeDateTime.ToShortTimeString(),
                    OpenTime = openDateTime.ToShortTimeString()
                });
            }

            Venue venue = _venueService.GetVenueById(id);

            if (!_venueService.VenueHasOwner(venue))
            {
                ViewData.ModelState.AddModelError("NoOwner", "This venue has not been claimed by an owner so please be sure to contact them directly after creating your game to avoid scheduling conflicts");
            }

            // Partial view displaying bids for specific item
            return PartialView("_BusinessHours", model);
        }

        public PartialViewResult GetGamesResult(int venueId)
        {
            //list of games found using venue ID
            var gameList = _gameService.GetCurrentGamesByVenueId(venueId);

            if (gameList.Count == 0) ViewData.ModelState.AddModelError("GameSearch", "There are no games that matches your search");

            //List using ViewModel to format how I like 
            List<GameListViewModel> model = new List<GameListViewModel>();

            //Find right data for each variable 
            foreach (var game in gameList)
            {
                var gameToAdd = new GameListViewModel
                {
                    GameId = game.GameId,
                    Sport = _gameService.GetSportNameById(game.SportId),
                    Venue = _venueService.GetVenueNameById(game.VenueId),
                    StartDate = game.StartTime,
                    EndDate = game.EndTime
                };

                if (game.ContactId != null)
                {
                    gameToAdd.ContactId = game.ContactId;
                    gameToAdd.ContactName = _contactService.GetContactById(game.ContactId).Username;
                }

                model.Add(gameToAdd);
            }


            //returning it back to my Ajax js method
            return PartialView("_GameSearch", model);

        }

        public PartialViewResult SearchBySport(int sportId)
        {
            List<Game> gameList = _gameService.GetCurrentGamesBySportId(sportId);

            if (gameList.Count == 0)
            {
                ViewData.ModelState.AddModelError("GameSearch", "There are no games that matches your search");
            }

            //List using ViewModel to format how I like 
            List<GameListViewModel> model = new List<GameListViewModel>();


            //Find right data for each variable 
            foreach (var game in gameList)
            {
                var gameToAdd = new GameListViewModel
                {
                    GameId = game.GameId,
                    Sport = _gameService.GetSportNameById(game.SportId),
                    Venue = _venueService.GetVenueNameById(game.VenueId),
                    StartDate = game.StartTime,
                    EndDate = game.EndTime
                };

                if (game.ContactId != null)
                {
                    gameToAdd.ContactId = game.ContactId;
                    gameToAdd.ContactName = _contactService.GetContactById(game.ContactId).Username;
                }

                model.Add(gameToAdd);
            }
            return PartialView("_GameSearch", model);
        }

        public PartialViewResult TimeFilter(string dateRange)
        {
            var dates = dateRange.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            var startDateTime = DateTime.Parse(dates[0], CultureInfo.InvariantCulture);
            var endDateTime = DateTime.Parse(dates[1], CultureInfo.CurrentCulture);

            List<GameListViewModel> model = new List<GameListViewModel>();
            foreach (var game in _gameService.GetAllCurrentOpenGames())
            {
                if (_gameService.IsSelectedTimeValid(startDateTime, endDateTime))
                {
                    if (game.StartTime >= startDateTime && game.EndTime <= endDateTime)
                    {
                        var gameToAdd = new GameListViewModel
                        {
                            GameId = game.GameId,
                            Sport = _gameService.GetSportNameById(game.SportId),
                            Venue = _venueService.GetVenueNameById(game.VenueId),
                            StartDate = game.StartTime,
                            EndDate = game.EndTime
                        };

                        if (game.ContactId != null)
                        {
                            gameToAdd.ContactId = game.ContactId;
                            gameToAdd.ContactName = _contactService.GetContactById(game.ContactId).Username;
                        }

                        model.Add(gameToAdd);
                    }
                }
                else
                {
                    var gameToAdd = new GameListViewModel
                    {
                        GameId = game.GameId,
                        Sport = _gameService.GetSportNameById(game.SportId),
                        Venue = _venueService.GetVenueNameById(game.VenueId),
                        StartDate = game.StartTime,
                        EndDate = game.EndTime
                    };

                    if (game.ContactId != null)
                    {
                        gameToAdd.ContactId = game.ContactId;
                        gameToAdd.ContactName = _contactService.GetContactById(game.ContactId).Username;
                    }

                    model.Add(gameToAdd);
                }
            }                         
            if (model.Count == 0)
            {
                ViewData.ModelState.AddModelError("GameSearch", "There are no games that matches your search");
            }
            return PartialView("_GameSearch", model);
        }

        public PartialViewResult PlayerList(int gameId)
        {
            List<PickUpGame> playerList = _gameService.GetPickUpGameListByGameId(gameId);

            List<PickUpGameViewModel> model = new List<PickUpGameViewModel>();

            if (playerList == null) return PartialView("_PlayerList", null);

            foreach (var player in playerList)
            {
                var contact = _contactService.GetContactById(player.ContactId);

                model.Add(new PickUpGameViewModel
                {
                    PickUpGameId = player.PickUpGameId,
                    GameId = gameId,
                    Username = contact.Username,
                    Email = contact.Email,
                    PhoneNumber = contact.PhoneNumber
                });

            }

            return PartialView("_PlayerList", model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditGame(int id)
        {
            //Get current users email
            string email = User.Identity.GetUserName();

            //Find user by their email
            Contact currContact = _contactService.GetContactByEmail(email);

            //Find Game
            Game game = _gameService.GetGameById(id);

            //Populate dropdown using the current values of the existing game
            PopulateEditDropDownMethod(game);

            //check if the user is the creator of the game
            if (!_gameService.IsCreatorOfGame(currContact.ContactId, game))
            {
                return RedirectToAction("GameDetails", new {id = id});
            }

            Debug.Write(game);

            //Convert the strings so it matches
            string dateRange = game.StartTime.ToString("MM/dd/yyyy hh:mm tt") + " - " + game.EndTime.ToString("MM/dd/yyyy hh:mm tt");

            ////Create own View Model
            EditGameViewModel model = new EditGameViewModel()
            { 
                GameId = id,
                ContactId = (int) game.ContactId,
                VenueId = game.VenueId,
                SportId = game.SportId,
                Sport = _gameService.GetSportNameById(game.SportId),
                Venue = _venueService.GetVenueNameById(game.VenueId),
                Status = Enum.GetName(typeof(GameStatusEnum), game.GameStatusId),
                DateRange = dateRange,

            };

            //pass it back
            return View(model);
        }

        /**
         * Helper method that takes in the game object and creates a dropdown with the current game values
         */
        public void PopulateEditDropDownMethod(Game game)
        {
            ViewBag.Venue = new SelectList(_venueService.GetAllVenues(), "VenueId", "Name", game.VenueId);
            ViewBag.Sport = new SelectList(_gameService.GetAllSports(), "SportId", "SportName", game.SportId);

            var gameStatusDict = new Dictionary<int, string>();
            foreach (GameStatusEnum gameStatus in Enum.GetValues(typeof(GameStatusEnum)))
            {
                gameStatusDict.Add((int)gameStatus, gameStatus.ToString());
            }

            ViewBag.Status = new SelectList(gameStatusDict, "GameStatusId", "Status", game.GameStatusId);
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult EditGame(EditGameViewModel model)
        {
            //checking model state
            if (!ModelState.IsValid) return View(model);

            //Parse the date ranges
            var dates = model.DateRange.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            var startDateTime = DateTime.Parse(dates[0], CultureInfo.InvariantCulture);
            var endDateTime = DateTime.Parse(dates[1], CultureInfo.CurrentCulture);

            //find the existing game
            Game existingGame = _gameService.GetGameById(model.GameId);

            //populating dropdown with the existing game
            PopulateEditDropDownMethod(existingGame);

            //check if the game will happen in one hour
            if (!_gameService.IsThisGameCanCancel(startDateTime))
            {
                ViewData.ModelState.AddModelError("GameStart", "Sorry, you can only edit the game at least 1 hour long before it starts.");
                PopulateEditDropDownMethod(existingGame);
                return View(model);
            }

            //check if the dates match - Can't have games that start and end on different dates 
            if (!_gameService.IsSelectedTimeValid(startDateTime, endDateTime))
            {
                ViewData.ModelState.AddModelError("DateRange", "Start date and end date must be same date.");
                PopulateEditDropDownMethod(existingGame);
                return View(model);
            }

            //check for existing games
            Game gameCheck = _gameService.CheckForExistingGameExceptItself(model.VenueId, model.SportId, startDateTime, model.GameId);
            if (gameCheck != null)
            {
                ViewData.ModelState.AddModelError("GameExists", "A game already exists with this venue, sport, and time.");
                PopulateEditDropDownMethod(existingGame);
                return View(model);
            }

            // Get venue by ID and business hours for that venue
            Venue venue = _venueService.GetVenueById(model.VenueId);
            List<BusinessHours> venueHours = _venueService.GetVenueBusinessHours(model.VenueId);

            // Return error to View if the venue is not available
            bool isVenueAvailable = _venueService.IsVenueAvailable(venueHours, startDateTime, endDateTime);
            if (!isVenueAvailable)
            {
                ViewData.ModelState.AddModelError("DateRange", $"Unfortunately, {venue.Name} is not available during the hours you chose.");
                PopulateEditDropDownMethod(existingGame);
                return View(model);
            }

            //if all the checks passed, then pass the values into the existing game
            var gameStatusEnum = (GameStatusEnum) Enum.Parse(typeof(GameStatusEnum), model.Status);
            existingGame.VenueId = model.VenueId;
            existingGame.SportId = model.SportId;
            existingGame.GameStatusId = (int) gameStatusEnum;
            existingGame.GameId = model.GameId;
            existingGame.ContactId = model.ContactId;
            existingGame.StartTime = startDateTime;
            existingGame.EndTime = endDateTime;

            _gameService.EditGame(existingGame);

            //send email to users once the game is cancelled
            if (int.Parse(model.Status) == 2)
            {
                var venueName = _venueService.GetVenueNameById(existingGame.VenueId);
                var sportName = _gameService.GetSportNameById(existingGame.SportId);

                string body = "Sorry, this game is canceled by the creator. Venue: " + venueName
                                                                                     + "; Sport: " + sportName
                                                                                     + "; Start Time: " + startDateTime + "; End Time: " + endDateTime;

                List<PickUpGame> pickUpGameList = _gameService.GetPickUpGameListByGameId(existingGame.GameId);
                foreach (var player in pickUpGameList)
                {
                    SendMessage(existingGame, player.ContactId, body);
                }                              
            }

            //redirect them back to the changed game detail
            return RedirectToAction("GameDetails", new {id = model.GameId});
        }
        
        /**
         * Helper methods
        */
        public void PopulateDropdownValues()
        {
            ViewBag.Venues = _venueService.GetAllVenues().ToDictionary(v => v.VenueId, v => v.Name);
            ViewBag.Sports = _gameService.GetAllSports().ToList().ToDictionary(s => s.SportID, s => s.SportName);
        }
    }
}
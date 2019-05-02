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
        private readonly PickUpContext _context;
        private readonly IContactService _contactService;
        private readonly IGMailService _gMailer;
        private readonly IGameService _gameService;


        public GameController(PickUpContext context, IContactService contactService, IGMailService gMailer, IGameService gameService)
        {
            _context = context;
            _contactService = contactService;
            _gMailer = gMailer;
            _gameService = gameService;
        }

        /**
         * Routes user to page that contains Create Game form
         */
        [Authorize]
        public ActionResult CreateGame()
        {
            ViewBag.GameCreated = false;
            // Confirm user is logged in (visitors can't create game)
            string email = User.Identity.GetUserName();
            Contact contact = _contactService.GetContactByEmail(email);

            if (contact == null)
            {
                ModelState.AddModelError("NoContact", "Please login or register to start a game.");
                PopulateDropdownValues();
                return View();
            }

            PopulateDropdownValues();
            return View();
        }

        /**
         * Handles input from Create Game form 
         */
        [HttpPost]
        public ActionResult CreateGame(CreateGameViewModel model)
        {
            // Confirm user is logged in (visitors can't create game)
            string email = User.Identity.GetUserName();
            Contact contact = _contactService.GetContactByEmail(email);

            if (contact == null)
            {
                ModelState.AddModelError("NoContact", "Please login or register to start a game.");
                PopulateDropdownValues();
                return View();
            }

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
            Game existingGame = CheckForExistingGame(model.VenueId, model.SportId, startDateTime);
            if (existingGame != null)
            {
                // TODO - Add link to existing game details later when that page is created
                ViewData.ModelState.AddModelError("GameExists", "A game already exists with this venue, sport, and time.");
                PopulateDropdownValues();
                return View(model);
            }

            // Get venue by ID and business hours for that venue
            Venue venue = _context.Venues.Find(model.VenueId);
            List<BusinessHours> venueHours = _context.BusinessHours.Where(b => b.VenueId == venue.VenueId).ToList();

            // Return error to View if the venue is not available
            bool isVenueAvailable = IsVenueAvailable(venueHours, startDateTime, endDateTime);
            if (!isVenueAvailable)
            {
                ViewData.ModelState.AddModelError("DateRange", $"Unfortunately, {venue.Name} is not available during the hours you chose.");
                PopulateDropdownValues();
                return View(model);
            }

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

            _context.Games.Add(newGame);
            _context.SaveChanges();

            ViewBag.GameCreated = true;
            PopulateDropdownValues();

            //send notification to users once a new game created and it includes the user's preference
            List<SportPreference> checkSportPreference = _context.SportPreferences.ToList();
            foreach (var item in checkSportPreference)
            {
                if (item.SportID == model.SportId && item.ContactID!=newGame.ContactId)
                {
                    var fileContents = System.IO.File.ReadAllText(Server.MapPath("~/Content/EmailFormat.html"));
                    //add game link to the email
                    var directUrl = Url.Action("GameDetails", "Game", new { id = newGame.GameId }, protocol: Request.Url.Scheme);
                    fileContents = fileContents.Replace("{URL}", directUrl);
                    //replace the html contents to the game details
                    fileContents = fileContents.Replace("{VENUE}", venue.Name);
                    fileContents = fileContents.Replace("{SPORT}", _context.Sports.Find(model.SportId).SportName);
                    fileContents = fileContents.Replace("{STARTTIME}", startDateTime.ToString());
                    fileContents = fileContents.Replace("{ENDTIME}", endDateTime.ToString());
                    SendMessage(newGame, item.ContactID, fileContents);
                }
            }

            // time preference
            List<TimePreference> checkTimePreferences = _context.TimePreferences.ToList();
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
                        nonDuplicateUser.Add(_context.Contacts.First(x=>x.ContactId==item.ContactID));
                    }
                    foreach (var checkeduser in nonDuplicateUser)
                    {
                        var fileContents = System.IO.File.ReadAllText(Server.MapPath("~/Content/EmailFormat.html"));
                        //add game link to the email
                        var directUrl = Url.Action("GameDetails", "Game", new { id = newGame.GameId }, protocol: Request.Url.Scheme);
                        fileContents = fileContents.Replace("{URL}", directUrl);
                        //replace the html contents to the game details
                        fileContents = fileContents.Replace("{VENUE}", venue.Name);
                        fileContents = fileContents.Replace("{SPORT}", _context.Sports.Find(model.SportId).SportName);
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
            ViewBag.Venue = new SelectList(_context.Venues, "VenueId", "Name");
            ViewBag.Sport = new SelectList(_context.Sports, "SportId", "SportName");

            // Get games that are open and that have not already passed and
            // order by games happening soonest
            List<Game> games = _context.Games
                .Where(g => g.GameStatusId == (int)GameStatusEnum.Open && g.StartTime > DateTime.Now)
                .OrderBy(g => g.StartTime).ToList();

            List<GameListViewModel> model = new List<GameListViewModel>();
            foreach (var game in games)
            {
                var gameToAdd = new GameListViewModel();
                gameToAdd.GameId = game.GameId;
                gameToAdd.Sport = _context.Sports.Find(game.SportId).SportName;
                gameToAdd.Venue = _context.Venues.Find(game.VenueId).Name;
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
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //getting current logged in user information in case they want to join game
            string email = User.Identity.GetUserName();
            Contact contact = _contactService.GetContactByEmail(email);

            //find the game 
            Game game = _context.Games.Find(id);
           
            if (IsCreatorOfGame(contact.ContactId, game))
            {
                ViewBag.IsCreator = true;
            }
            

            //if there are no games then return: 
            if (game == null) return HttpNotFound();

            //creating view model for the page
            ViewGameViewModel model = new ViewGameViewModel()
            {
                EndDate = game.EndTime.ToString(),
                GameId = game.GameId,
                Status = _context.GameStatuses.Find(game.GameStatusId).Status,
                Sport = _context.Sports.Find(game.SportId).SportName,
                StartDate = game.StartTime.ToString(),
                Venue = _context.Venues.Find(game.VenueId).Name,
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

            //finding game
            Game game = _context.Games.Find(model.GameId);

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
                    Status = _context.GameStatuses.Find(game.GameStatusId).Status,
                    Sport = _context.Sports.Find(game.SportId).SportName,
                    StartDate = game.StartTime.ToString(),
                    Venue = _context.Venues.Find(game.VenueId).Name,
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
                if (!IsNotSignedUpForGame(currContactUser.ContactId, checkGames))
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
                _context.PickUpGames.Add(newPickUpGame);

            }

            //If the Leave Game button was pressed 
            if (button.Equals("Leave Game"))
            {
                //check if the person is already signed up for the game 
                if (IsNotSignedUpForGame(currContactUser.ContactId, checkGames))
                {
                    //error message
                    ViewData.ModelState.AddModelError("SignedUp", "You have not signed up for this game");

                    //sending model back so values dont blank out
                    ViewGameViewModel returnModel = new ViewGameViewModel()
                    {
                        EndDate = game.EndTime.ToString(),
                        GameId = game.GameId,
                        Status = _context.GameStatuses.Find(game.GameStatusId).Status,
                        Sport = _context.Sports.Find(game.SportId).SportName,
                        StartDate = game.StartTime.ToString(),
                        Venue = _context.Venues.Find(game.VenueId).Name,
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
                _context.PickUpGames.Remove(_context.PickUpGames.First(x => x.GameId == model.GameId && x.ContactId == currContactUser.ContactId));
                
            }

            _context.SaveChanges();

            // If contact ID null then creator has deleted account, do not send email
            if (game.ContactId != null)
            {
                SendMessage(game, (int)game.ContactId, body);

            }

            //redirect to the gamedetails page so that they could see that they are signed on
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

        /***
         * Helper method to see if user is already signed up for a game or not
         */
        public bool IsNotSignedUpForGame(int contactId, List<PickUpGame> games)
        {
            //Just in case a null "0" comes in stop it from coming in
            if (contactId == 0) return false;

            if (games == null) return true;

            foreach (var game in games)
            {
                if (game.ContactId == contactId)
                {
                    return false;
                }
                
            }

            // else this person hasn't signed up yet
            return true;
        }

        /**
         * Partial view that to display business hours on CreateGame page
         */
        [HttpGet]
        public PartialViewResult BusinessHoursByVenueId(int id)
        {
            // Map business hours
            List<BusinessHours> businessHours = _context.BusinessHours.Where(b => b.VenueId == id).ToList();
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

            Venue venue = _context.Venues.Find(id);

            if (!VenueHasOwner(venue))
            {
                ViewData.ModelState.AddModelError("NoOwner", "The Owner for this Venue currently does not exist.");
            }

            // Partial view displaying bids for specific item
            return PartialView("_BusinessHours", model);
        }


        public bool VenueHasOwner(Venue venue)
        {
            //Find the owner using the venue ID, again could be simplified using repo patterns
            VenueOwner owner = _context.VenueOwners.FirstOrDefault(x => x.VenueId == venue.VenueId);

            //if there is not an owner it would be null so return false
            if (owner == null) return false;

            //else there is an owner and the value is not null so return true 
            return true;
        }

        public PartialViewResult GetGamesResult(int venueId)
        {
            //list of games found using venue ID
            List<Game> gameList = _context.Games.Where(x => x.VenueId == venueId).ToList();

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
                    Sport = _context.Sports.Find(game.SportId).SportName,
                    Venue = _context.Venues.Find(game.VenueId).Name,
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
            //list of games found using venue ID
            List<Game> gameList = _context.Games.Where(x => x.SportId == sportId).ToList();

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
                    Sport = _context.Sports.Find(game.SportId).SportName,
                    Venue = _context.Venues.Find(game.VenueId).Name,
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
            foreach (var game in _context.Games)
            {
                if (IsSelectedTimeValid(startDateTime, endDateTime))
                {
                    if (game.StartTime >= startDateTime && game.EndTime <= endDateTime)
                    {
                        var gameToAdd = new GameListViewModel
                        {
                            GameId = game.GameId,
                            Sport = _context.Sports.Find(game.SportId).SportName,
                            Venue = _context.Venues.Find(game.VenueId).Name,
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
                        Sport = _context.Sports.Find(game.SportId).SportName,
                        Venue = _context.Venues.Find(game.VenueId).Name,
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
                model.Add(new PickUpGameViewModel
                {
                    PickUpGameId = player.PickUpGameId,
                    GameId = gameId,
                    Username = _context.Contacts.Find(player.ContactId).Username,
                    Email = _context.Contacts.Find(player.ContactId).Email,
                    PhoneNumber = _context.Contacts.Find(player.ContactId).PhoneNumber
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
            Game game = _context.Games.Find(id);

            //Populate dropdown using the current values of the existing game
            PopulateEditDropDownMethod(game);

            //check if the user is the creator of the game
            if (!IsCreatorOfGame(currContact.ContactId, game))
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
                Sport = _context.Sports.Find(game.SportId).SportName,
                Status = _context.GameStatuses.Find(game.GameStatusId).Status,
                Venue = _context.Venues.Find(game.VenueId).Name,
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
            ViewBag.Venue = new SelectList(_context.Venues, "VenueId", "Name", game.VenueId);
            ViewBag.Sport = new SelectList(_context.Sports, "SportId", "SportName", game.SportId);
            ViewBag.Status = new SelectList(_context.GameStatuses, "GameStatusId", "Status", game.GameStatusId);
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
            Game existingGame = _context.Games.Find(model.GameId);

            //populating dropdown with the existing game
            PopulateEditDropDownMethod(existingGame);

            //check if the game will happen in one hour
            if (!IsThisGameCanCancel(startDateTime))
            {
                ViewData.ModelState.AddModelError("GameStart", "Sorry, you can only edit the game at least 1 hour long before it starts.");
                PopulateEditDropDownMethod(existingGame);
                return View(model);
            }

            //check if the dates match - Can't have games that start and end on different dates 
            if (!IsSelectedTimeValid(startDateTime, endDateTime))
            {
                ViewData.ModelState.AddModelError("DateRange", "Start date and end date must be same date.");
                PopulateEditDropDownMethod(existingGame);
                return View(model);
            }

            //check for existing games
            Game gameCheck = CheckForExistingGameExceptItself(int.Parse(model.Venue), int.Parse(model.Sport), startDateTime, model.GameId);
            if (gameCheck != null)
            {
                // TODO - Add link to existing game details later when that page is created
                ViewData.ModelState.AddModelError("GameExists", "A game already exists with this venue, sport, and time.");
                PopulateEditDropDownMethod(existingGame);
                return View(model);
            }

            // Get venue by ID and business hours for that venue
            Venue venue = _context.Venues.Find(int.Parse(model.Venue));
            List<BusinessHours> venueHours = _context.BusinessHours.Where(b => b.VenueId == venue.VenueId).ToList();

            // Return error to View if the venue is not available
            bool isVenueAvailable = IsVenueAvailable(venueHours, startDateTime, endDateTime);
            if (!isVenueAvailable)
            {
                ViewData.ModelState.AddModelError("DateRange", $"Unfortunately, {venue.Name} is not available during the hours you chose.");
                PopulateEditDropDownMethod(existingGame);
                return View(model);
            }

            //if all the checks passed, then pass the values into the existing game
            existingGame.VenueId = int.Parse(model.Venue);
            existingGame.SportId = int.Parse(model.Sport);
            existingGame.GameStatusId = int.Parse(model.Status);
            existingGame.GameId = model.GameId;
            existingGame.ContactId = model.ContactId;
            existingGame.StartTime = startDateTime;
            existingGame.EndTime = endDateTime;

            //save changes
            _context.Entry(existingGame).State = EntityState.Modified;

            //send email to users once the game is cancelled
            if (int.Parse(model.Status) == 2)
            {
                string body = "Sorry, this game is canceled by the creator. Venue: " + _context.Venues.First(x => x.VenueId == existingGame.VenueId).Name
                                                                                     + "; Sport: " + _context.Sports.First(x => x.SportID == existingGame.SportId).SportName
                                                                                     + "; Start Time: " + startDateTime + "; End Time: " + endDateTime;

                List<PickUpGame> pickUpGameList = _gameService.GetPickUpGameListByGameId(existingGame.GameId);
                foreach (var player in pickUpGameList)
                {
                    SendMessage(existingGame, player.ContactId, body);
                }                              
            }

            _context.SaveChanges();

            //redirect them back to the changed game detail
            return RedirectToAction("GameDetails", new {id = model.GameId});
        }

        public void PopulateEditDropdown() { }

        public bool IsCreatorOfGame(int contactId, Game game)
        {
            //if blank value then return false
            if (contactId == 0 || game.ContactId == 0) return false;

            if (game == null) return false;

            //see if it matches, if so then the contact is the creator
            if (contactId == game.ContactId)
            {
                return true;
            }

            return false;
        }

        public bool IsSelectedTimeValid(DateTime startDateTime,DateTime endDataTime)
        {
            if(startDateTime == null|| endDataTime == null)
            {
                return false;
            }
            if (endDataTime.Date!=startDateTime.Date)
            {
                return false;
            }

            return true;
        }

        /**
         * Helper methods
        */
        public void PopulateDropdownValues()
        {
            ViewBag.Venues = _context.Venues.ToList().ToDictionary(v => v.VenueId, v => v.Name);
            ViewBag.Sports = _context.Sports.ToList().ToDictionary(s => s.SportID, s => s.SportName);
        }

        public bool IsVenueAvailable(List<BusinessHours> venueHours, DateTime startDateTime, DateTime endDateTime)
        {
            // If no business hours then venue has no hours and is therefore not available
            if (venueHours == null) return false;

            // Only checking start date because games should not span over a day 
            DayOfWeek startDate = startDateTime.DayOfWeek;
            BusinessHours venueOpenDate = venueHours.FirstOrDefault(x => x.DayOfWeek == (int)startDate);

            // Venue is open that date, check timeframes
            if (venueOpenDate != null)
            {
                TimeSpan startTime = startDateTime.TimeOfDay;
                TimeSpan endTime = endDateTime.TimeOfDay;

                // Change midnight to 11:59 PM for accurate time comparisons
                if (venueOpenDate.CloseTime == new TimeSpan(00, 00, 00))
                    venueOpenDate.CloseTime = new TimeSpan(23, 59, 00);

                // Ensure both start and end times are within range
                if (startTime > venueOpenDate.OpenTime && startTime < venueOpenDate.CloseTime)
                {
                    if (endTime > venueOpenDate.OpenTime && endTime < venueOpenDate.CloseTime) return true;
                }
            }

            return false;
        }

        public Game CheckForExistingGame(int venueId, int sportId, DateTime startDateTime)
        {
            // Check for all games that are happening at same venue
            List<Game> gamesAtVenue = _context.Games.Where(g => g.VenueId == venueId).ToList();
            if (gamesAtVenue.Count <= 0) return null;

            // Check for all games happening at that venue with same sport
            List<Game> sportsAtVenue = gamesAtVenue.Where(g => g.SportId == sportId).ToList();
            if (sportsAtVenue.Count <= 0) return null;

            // There are existing games with same sport and venue so check starting time
            foreach (var game in sportsAtVenue)
            {
                if (startDateTime >= game.StartTime && startDateTime <= game.EndTime)
                {
                    // If we get here, the new game will overlap with an existing game
                    // Check if status is Open and if so, return that game
                    if (game.GameStatusId == (int)GameStatusEnum.Open)
                    {
                        return game;
                    }
                }
            }

            return null;

        }

        // Added by Kexin, because the above method CheckForExistingGame is also used by creating games, there is no gameID to compare
        // make sure the existing game is not itself, user can save the game without any edition
        public Game CheckForExistingGameExceptItself(int venueId, int sportId, DateTime startDateTime, int gameId)
        {
            // Check for all games that are happening at same venue
            List<Game> gamesAtVenue = _context.Games.Where(g => g.VenueId == venueId && g.GameId != gameId).ToList();
            if (gamesAtVenue.Count <= 0) return null;

            // Check for all games happening at that venue with same sport
            List<Game> sportsAtVenue = gamesAtVenue.Where(g => g.SportId == sportId).ToList();
            if (sportsAtVenue.Count <= 0) return null;

            // There are existing games with same sport and venue so check starting time
            foreach (var game in sportsAtVenue)
            {
                if (startDateTime >= game.StartTime && startDateTime <= game.EndTime)
                {
                    // If we get here, the new game will overlap with an existing game
                    // Check if status is Open and if so, return that game
                    if (game.GameStatusId == (int)GameStatusEnum.Open)
                    {
                        return game;
                    }
                }
            }

            return null;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool IsThisGameCanCancel(DateTime dateTime)
        {
            if (dateTime.AddHours(-1) < DateTime.Now)
            {
                return false;
            }
            return true;
        }

    }
}
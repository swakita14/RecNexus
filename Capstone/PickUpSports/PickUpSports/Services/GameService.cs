using System;
using System.Collections.Generic;
using System.Linq;
using PickUpSports.Interface;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.Enums;

namespace PickUpSports.Services
{
    public class GameService : IGameService
    {
        private readonly IPickUpGameRepository _pickUpGameRepository;
        private readonly IGameRepository _gameRepository;
        private readonly ISportRepository _sportRepository;

        public GameService(IPickUpGameRepository pickUpGameRepository, 
            IGameRepository gameRepository, 
            ISportRepository sportRepository)
        {
            _pickUpGameRepository = pickUpGameRepository;
            _gameRepository = gameRepository;
            _sportRepository = sportRepository;
        }

        /*
         * Given a Game entity, add that game to the daabase
         */
        public Game CreateGame(Game game)
        {
            return _gameRepository.AddGame(game);
        }

        public PickUpGame AddPlayerToGame(PickUpGame pickUpGame)
        {
            return _pickUpGameRepository.AddPickUpGame(pickUpGame);
        }

        public void RemovePlayerFromGame(PickUpGame pickUpGame)
        {
            _pickUpGameRepository.DeletePickUpGame(pickUpGame);
        }

        /*
         * Given Game ID, get all pick up players for that game
         */
        public List<PickUpGame> GetPickUpGameListByGameId(int gameId)
        {
            var results = _pickUpGameRepository.GetPickUpGameListByGameId(gameId);
            return results;
        }

        /*
         * Given Contact ID, get all games that player is signed up for
         */
        public List<PickUpGame> GetPickUpGamesByContactId(int contactId)
        {
            var results = _pickUpGameRepository.GetPickUpGameListByContactId(contactId);
            if (results == null) return null;
            return results;
        }

        /*
         * Given Contact ID, get all games that player started 
         */
        public List<Game> GetAllGamesByContactId(int contactId)
        {
            var games = _gameRepository.GetAllGames();
            if (games == null) return null;
 
            var results = from g in games
                where g.ContactId.Equals(contactId)
                select g;

            if (!results.Any()) return null;
            return results.ToList();
        }

        /*
         * Get all games that are open that haven't passed yet.
         * Order by start time
         */
        public List<Game> GetAllCurrentOpenGames()
        {
            var games = _gameRepository.GetAllGames();
            var results = games.Where(g => g.GameStatusId == (int)GameStatusEnum.Open 
                                           && g.StartTime > DateTime.Now)
                .OrderBy(g => g.StartTime).ToList();

            return results;
        }

        /*
         * Given a GameID, return that game
         */
        public Game GetGameById(int id)
        {
            var result = _gameRepository.GetGameById(id);
            return result;
        }
        
        /*
         * Given ContactID, returns list of current games
         * in ascending order by the ending time of the game
         */
        public List<Game> GetCurrentOrderedGamesByContactId(int contactId)
        {
            var games = _gameRepository.GetAllGames();
            if (games == null) return null;

            var results = from g in games
                where g.ContactId == contactId &&
                      g.EndTime > DateTime.Today.AddDays(-1)
                      orderby g.StartTime ascending 
                select g;

            if (!results.Any()) return null;
            return results.ToList();
        }

        /*
         * Given VenueID, returns list of current games
         * in ascending order by the ending time of the game
         */
        public List<Game> GetCurrentGamesByVenueId(int venueId)
        {
            var games = _gameRepository.GetAllGames();
            if (games == null) return null;

            var results = games.Where(x => x.VenueId == venueId && x.EndTime > DateTime.Today.AddDays(-1));

            if (!results.Any()) return null;
            return results.ToList();
        }

        /*
         * Given a Sport ID, return name of sport
         */
        public string GetSportNameById(int sportId)
        {
            var sport = _sportRepository.GetSportById(sportId);
            return sport.SportName;
        }

        /*
         * Get list of all sports
         */
        public List<Sport> GetAllSports()
        {
            return _sportRepository.GetAllSports();
        }

        /*
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
    }
}
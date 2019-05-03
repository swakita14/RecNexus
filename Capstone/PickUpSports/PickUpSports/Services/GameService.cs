using System;
using System.Collections.Generic;
using System.Linq;
using PickUpSports.Interface;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

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

        public Game CreateGame(Game game)
        {
            return _gameRepository.AddGame(game);
        }

        public List<PickUpGame> GetPickUpGameListByGameId(int gameId)
        {
            var results = _pickUpGameRepository.GetPickUpGameListByGameId(gameId);
            return results;
        }

        public List<PickUpGame> GetPickUpGamesByContactId(int contactId)
        {
            var results = _pickUpGameRepository.GetPickUpGameListByContactId(contactId);
            if (results == null) return null;
            return results;
        }

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

        public string GetSportNameById(int sportId)
        {
            var sport = _sportRepository.GetSportById(sportId);
            return sport.SportName;
        }

        public Game GetGameById(int id)
        {
            var result = _gameRepository.GetGameById(id);
            return result;
        }

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

        public List<Game> GetCurrentGamesByVenueId(int venueId)
        {
            var games = _gameRepository.GetAllGames();
            if (games == null) return null;

            var results = games.Where(x => x.VenueId == venueId);

            if (!results.Any()) return null;
            return results.ToList();
        }
    }
}
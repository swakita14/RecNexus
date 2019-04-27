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

        public GameService(IPickUpGameRepository pickUpGameRepository, IGameRepository gameRepository, ISportRepository sportRepository)
        {
            _pickUpGameRepository = pickUpGameRepository;
            _gameRepository = gameRepository;
            _sportRepository = sportRepository;
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

        public List<Game> GetGamesByContactId(int contactId)
        {
            var games = _gameRepository.GetAllGames();
            var results = games.Where(x => x.ContactId == contactId).ToList();
            if (!results.Any()) return null;
            return results;
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
    }
}
using System.Collections.Generic;
using PickUpSports.Interface;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Services
{
    public class GameService : IGameService
    {
        private readonly IPickUpGameRepository _pickUpGameRepository;

        public GameService(IPickUpGameRepository pickUpGameRepository)
        {
            _pickUpGameRepository = pickUpGameRepository;
        }

        public List<PickUpGame> GetPickUpGameListByGameId(int gameId)
        {
            var results = _pickUpGameRepository.GetPickUpGameListByGameId(gameId);
            return results;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class PickUpGameRepository : IPickUpGameRepository
    {
        private readonly PickUpContext _context;

        public PickUpGameRepository(PickUpContext context)
        {
            _context = context;
        }

        public List<PickUpGame> GetPickUpGameListByGameId(int gameId)
        { 
            List<PickUpGame> pickUpGameList = _context.PickUpGames.Where(x => x.GameId == gameId).ToList();

            return pickUpGameList;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;
using System.Linq;

namespace PickUpSports.DAL.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly PickUpContext _context;

        public GameRepository(PickUpContext context)
        {
            _context = context;
        }

        public List<Game> GetGameListByContactId(int contactId)
        {
            List<Game> gameList = _context.Games.Where(x => x.ContactId == contactId).ToList();
            if (gameList.Count <= 0) return null;

            return gameList;
        }

        public void EditGame(Game game)
        {
            _context.Entry(game).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteGame(Game game)
        {
            Game existing = _context.Games.Find(game.GameId);
            if (existing == null) throw new ArgumentNullException($"Could not find existing game by ID {game.GameId}");

            _context.Games.Remove(existing);
            _context.SaveChanges();
        }
    }
}
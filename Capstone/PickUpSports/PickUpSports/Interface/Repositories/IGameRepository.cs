using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface IGameRepository
    {
        List<Game> GetAllGames();

        Game GetGameById(int id);

        void EditGame(Game game);

        void DeleteGame(Game game);
    }
}
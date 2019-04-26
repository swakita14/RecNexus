using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface IGameRepository
    {
        List<Game> GetGameListByContactId(int contactId);

        void EditGame(Game game);

        void DeleteGame(Game game);
    }
}
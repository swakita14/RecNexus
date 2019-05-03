using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface
{
    public interface IGameService
    {
        Game CreateGame(Game game);

        List<PickUpGame> GetPickUpGameListByGameId(int gameId);

        List<PickUpGame> GetPickUpGamesByContactId(int contactId);

        List<Game> GetAllGamesByContactId(int contactId);

        string GetSportNameById(int sportId);

        Game GetGameById(int id);

        List<Game> GetCurrentOrderedGamesByContactId(int contactId);

        List<Game> GetCurrentGamesByVenueId(int venueId);

    }
}
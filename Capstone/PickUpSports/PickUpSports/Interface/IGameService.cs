using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface
{
    public interface IGameService
    {
        Game CreateGame(Game game);

        PickUpGame AddPlayerToGame(PickUpGame pickUpGame);

        void RemovePlayerFromGame(PickUpGame pickUpGame);

        List<PickUpGame> GetPickUpGameListByGameId(int gameId);

        List<PickUpGame> GetPickUpGamesByContactId(int contactId);

        Game GetGameById(int id);

        List<Game> GetAllGamesByContactId(int contactId);

        List<Game> GetAllCurrentOpenGames();

        List<Game> GetCurrentOrderedGamesByContactId(int contactId);

        List<Game> GetCurrentGamesByVenueId(int venueId);

        string GetSportNameById(int sportId);

        List<Sport> GetAllSports();

        bool IsNotSignedUpForGame(int contactId, List<PickUpGame> games);

    }
}
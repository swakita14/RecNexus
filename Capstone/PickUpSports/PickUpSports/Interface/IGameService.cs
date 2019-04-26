using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface
{
    public interface IGameService
    {
        List<PickUpGame> GetPickUpGameListByGameId(int gameId);

    }
}
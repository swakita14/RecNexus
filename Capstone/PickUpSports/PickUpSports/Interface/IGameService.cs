﻿using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface
{
    public interface IGameService
    {
        List<PickUpGame> GetPickUpGameListByGameId(int gameId);

        List<PickUpGame> GetPickUpGamesByContactId(int contactId);

        List<Game> GetAllGamesByContactId(int contactId);

        string GetSportNameById(int sportId);

        Game GetGameById(int id);

        List<Game> GetCurrentOrderedGamesByContactId(int contactId);


    }
}
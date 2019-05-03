﻿using System;
using System.Collections.Generic;
using System.Linq;
using PickUpSports.Interface;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly ITimePreferenceRepository _timePreferenceRepository;
        private readonly ISportPreferenceRepository _sportPreferenceRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ISportRepository _sportRepository;
        private readonly IPickUpGameRepository _pickUpGameRepository;
        private readonly IGameRepository _gameRepository;

        public ContactService(IContactRepository contactRepository, 
            ITimePreferenceRepository timePreferenceRepository, 
            ISportPreferenceRepository sportPreferenceRepository, 
            IReviewRepository reviewRepository, 
            ISportRepository sportRepository, 
            IPickUpGameRepository pickUpGameRepository, IGameRepository gameRepository)
        {
            _contactRepository = contactRepository;
            _timePreferenceRepository = timePreferenceRepository;
            _sportPreferenceRepository = sportPreferenceRepository;
            _reviewRepository = reviewRepository;
            _sportRepository = sportRepository;
            _pickUpGameRepository = pickUpGameRepository;
            _gameRepository = gameRepository;
        }

        public Contact GetContactByEmail(string email)
        {
            return _contactRepository.GetContactByEmail(email);
        }

        public Contact GetContactById(int? id)
        {
            return _contactRepository.GetContactById(id);
        }

        public bool UsernameIsTaken(string username)
        {
            var existing = _contactRepository.GetContactByUsername(username);
            if (existing == null) return false;
            return true;
        }

        public Contact GetContactByUsername(string username)
        {
            return _contactRepository.GetContactByUsername(username);
        }

        public Contact CreateContact(Contact contact)
        {
            return _contactRepository.CreateContact(contact);
        }

        public void EditContact(Contact contact)
        {
            _contactRepository.EditContact(contact);
        }

        public void DeleteUser(Contact contact)
        {
            // Delete any SportPreferences related to Contact
            var sportPreferences = GetAllSportPreferences().Where(x => x.ContactID == contact.ContactId).ToList();
            if (sportPreferences.Count > 0)
            {
                foreach (var sportPreference in sportPreferences)
                {
                    _sportPreferenceRepository.Delete(sportPreference);
                }
            }

            // Delete any TimePreferences related to Contact
            var timePreferences = GetUserTimePreferences(contact.ContactId);
            if (timePreferences.Count > 0)
            {
                foreach (var timePreferece in timePreferences)
                {
                    _timePreferenceRepository.Delete(timePreferece);
                }
            }

            // Set ContactID for any Reviews to null
            var reviews = _reviewRepository.GetReviewsByContactId(contact.ContactId);

            if (reviews.Count > 0)
            {
                foreach (var review in reviews)
                {
                    review.ContactId = null;
                    _reviewRepository.EditReview(review);
                }
            }

            // Delete any pick up games that the user joined but did not create
            var pickUpGames = _pickUpGameRepository.GetPickUpGameListByContactId(contact.ContactId);

            if (pickUpGames != null)
            {
                foreach (var pickUpGame in pickUpGames)
                {
                    _pickUpGameRepository.DeletePickUpGame(pickUpGame);
                }
            }

            // Find any games that the user created. 
            // If no users in the games they created then delete them 
            // If there are joined users then set ContactID to null
            var allGames = _gameRepository.GetAllGames();
            var games = allGames.Where(x => x.ContactId == contact.ContactId);
            if (games != null)
            {
                foreach (var game in games)
                {
                    var players = _pickUpGameRepository.GetPickUpGameListByGameId(game.GameId);
                    if (players != null)
                    {
                        // There are people that joined game, set contact ID to null
                        game.ContactId = null;
                        _gameRepository.EditGame(game);
                    }
                    else
                    {
                        // No users, delete game
                        _gameRepository.DeleteGame(game);
                    }
                }
            }

            // Remove from Contact table
            _contactRepository.DeleteContact(contact);
        }

        public List<SportPreference> GetAllSportPreferences()
        {
            return _sportPreferenceRepository.GetAllSportsPreferences();
        }

        public List<TimePreference> GetAllTimePreferences()
        {
            return _timePreferenceRepository.GetAllTimePreferences();
        }


        public List<string> GetUserSportPreferences(int contactId)
        {
            var sportPreferences = _sportPreferenceRepository.GetAllSportsPreferences();
            var userPrefs = sportPreferences.Where(x => x.ContactID == contactId).ToList();

            var results = new List<string>();
            foreach (var sportPreference in userPrefs)
            {
                var sport = _sportRepository.GetSportById(sportPreference.SportID);
                results.Add(sport.SportName);
            }
            return results;
        }

        public List<TimePreference> GetUserTimePreferences(int contactId)
        {
            var timePreferences = _timePreferenceRepository.GetAllTimePreferences();
            var userPrefs = timePreferences.Where(x => x.ContactID == contactId).ToList();

            if (userPrefs.Count == 0) return null;
            return userPrefs;
        }
    }
}
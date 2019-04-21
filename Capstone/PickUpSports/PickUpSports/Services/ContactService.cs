using System.Collections.Generic;
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

        public ContactService(IContactRepository contactRepository, 
            ITimePreferenceRepository timePreferenceRepository, 
            ISportPreferenceRepository sportPreferenceRepository, 
            IReviewRepository reviewRepository, ISportRepository sportRepository)
        {
            _contactRepository = contactRepository;
            _timePreferenceRepository = timePreferenceRepository;
            _sportPreferenceRepository = sportPreferenceRepository;
            _reviewRepository = reviewRepository;
            _sportRepository = sportRepository;
        }

        public Contact GetContactByEmail(string email)
        {
            return _contactRepository.GetContactByEmail(email);
        }

        public Contact GetContactById(int id)
        {
            return _contactRepository.GetContactById(id);
        }

        public bool UsernameIsTaken(string username)
        {
            var existing = _contactRepository.GetContactByUsername(username);
            if (existing == null) return false;
            return true;
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
            var sportPreferences = _sportPreferenceRepository.GetUsersSportPreferencesByContactId(contact.ContactId);
            if (sportPreferences.Count > 0)
            {
                foreach (var sportPreference in sportPreferences)
                {
                    _sportPreferenceRepository.Delete(sportPreference);
                }
            }

            // Delete any TimePreferences related to Contact
            var timePreferences = _timePreferenceRepository.GetUsersTimePreferencesByContactId(contact.ContactId);
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

            // Remove from Contact table
            _contactRepository.DeleteContact(contact);
        }

        public List<string> GetSportPreferences(int contactId)
        {
            var sportPreferences = _sportPreferenceRepository.GetUsersSportPreferencesByContactId(contactId);
            var results = new List<string>();
            foreach (var sportPreference in sportPreferences)
            {
                var name = _sportRepository.GetSportNameById(sportPreference.SportID);
                results.Add(name);
            }
            return results;
        }

        public List<TimePreference> GetTimePreferences(int contactId)
        {
            var results = _timePreferenceRepository.GetUsersTimePreferencesByContactId(contactId);
            return results;
        }
    }
}
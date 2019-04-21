using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface
{
    public interface IContactService
    {
        Contact GetContactByEmail(string email);

        Contact GetContactById(int id);

        bool UsernameIsTaken(string username);

        Contact CreateContact(Contact contact);

        void EditContact(Contact contact);

        void DeleteUser(Contact contact);
    }
}
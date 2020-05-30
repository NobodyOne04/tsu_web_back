using Web.Models;
using System;

namespace Web.Services
{
    public interface IRegistrationService
    {
        void SendPerson(SignIn obj);
        String BornDate(String day, String month, String year);
        bool TryToFind(SignUp obj);
    }
}

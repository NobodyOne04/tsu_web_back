using Web.Models;
using System;

namespace Web.Services
{
    public interface IRegistrationService
    {
        void SendPerson(SignUpSec obj);
        String BornDate(String day, String month, String year);
        bool TryToFind(SignUpFst obj);
    }
}

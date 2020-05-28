using Web.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;


namespace Web.Services
{
    public sealed class RegistrationService : IRegistrationService
    {
        private readonly List<SignUpSec> Persons = new List<SignUpSec>();

        public void SendPerson(SignUpSec obj)
        {
            lock (this.Persons)
            {
                this.Persons.Add(obj);
            }
        }
        public String BornDate(String day, String month, String year) {
            String date = day + ' ' + month + ' ' + year;
            return date;
        }
        public bool TryToFind(SignUpFst obj) {
            foreach (SignUpFst a in Persons) {
                if (obj.FName == a.FName && obj.SName == a.FName && obj.gender == a.gender && obj.born == a.born) return true;
            }
            return false;
        }
    }
}

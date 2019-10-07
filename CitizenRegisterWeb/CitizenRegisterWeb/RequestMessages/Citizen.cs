using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CitizenRegisterWeb.RequestMessages
{
    /// <summary>
    /// Structure of Requests and Responses
    /// </summary>
    public class Citizen
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }

        [IgnoreDataMember]
        public bool IsValid
        {
            get
            {
                // Check for presence 
                if (string.IsNullOrEmpty(Surname) ||
                    string.IsNullOrEmpty(Name) ||
                    BirthDate == default(DateTime))
                        return false;

                // Check for bounds
                if (Surname.Length >= 30 ||
                    Name.Length >= 30 ||
                    (MiddleName != null && MiddleName.Length >= 40) ||
                    BirthDate.CompareTo(DateTime.Now) >= 0)
                        return false;
                
                return true;
            }
        }
    }
}

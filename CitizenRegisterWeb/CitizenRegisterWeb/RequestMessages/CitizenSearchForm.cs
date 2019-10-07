using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitizenRegisterWeb.RequestMessages
{
    public class CitizenSearchForm : Citizen
    {
        public DateTime BirthDateAfter { get; set; }
        public DateTime BirthDateBefore { get; set; }
    }
}

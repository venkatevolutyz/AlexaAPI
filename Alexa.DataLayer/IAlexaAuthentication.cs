using Alexa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexa.DataLayer
{
    public interface IAlexaAuthentication
    {
        FetchUsers FetchData(string deviceid);
        bool ValidateEmployeeID(string deviceid, string empid);
        ValidatePin ValidatePin(string deviceid, int pin);
        GetTimeSheet GetHours(int month, string deviceid);
    }
}

using Alexa.DataLayer;
using Alexa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexa.BusinessLayer
{
    public class AlexaAuthenticationBL
    {
        private readonly IAlexaAuthentication alexaRepository;
        public AlexaAuthenticationBL(IAlexaAuthentication alexaRepository)
        {
            this.alexaRepository = alexaRepository;
        }
        AlexaAuthenticationDL alexa = new AlexaAuthenticationDL();
        public AlexaAuthenticationBL()
        {
        }

        public FetchUsers FetchData(string deviceid)
        {
            return alexa.FetchData(deviceid);
        }

        public FetchUsers FetchUserDetails(string deviceid)
        {
            return alexa.FetchUserDetails(deviceid);
        }

        public bool ValidateEmployeeID(string deviceid, string empid)
        {
            return alexa.ValidateEmployeeID(deviceid, empid);
        }

        public ValidatePin ValidatePin(string deviceid, int pin)
        {
            return alexa.ValidatePin(deviceid, pin);
        }

        public GetTimeSheet GetHours(int month, string deviceid)
        {
            return alexa.GetHours(month, deviceid);
        }

        public bool GetAuthentication(string deviceid)
        {
            return alexa.GetAuthentication(deviceid);
        }

        public SubmitTimesheet SubmitTimeSheet(int month, string deviceid /*string phonenumber,*/)
        {
            return alexa.SubmitTimeSheet(month, deviceid);
        }

        public StatusFlags CheckStatus(string deviceid, int month)
        {
            return alexa.CheckStatus(deviceid, month);
        }

    }
}

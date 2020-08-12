using Alexa.BusinessLayer;
using Alexa.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AlexaAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class AlexaController : ApiController
    {
        private static log4net.ILog Log { get; set; }
        ILog log = log4net.LogManager.GetLogger("AlexaController");
        static bool isValidUsers = false;
        AlexaAuthenticationBL objAlexaAuthentication = new AlexaAuthenticationBL();
        [Route("api/Alexa/AlexaWakeUp")]
        [HttpPut]
        public HttpResponseMessage AlexaWakeUp([FromBody] Devices request)
        {
            FetchUsers objusers = new FetchUsers();
            try
            {
                //request.DeviceId = "abc"; 
                //objusers = objAlexaAuthentication.FetchData("abc");
                objusers = objAlexaAuthentication.FetchData(request.DeviceId);

                if (objusers.status != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, objusers);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, objusers);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.ToString());
            }

        }

        [HttpPut]
        public HttpResponseMessage ValidateEmployeeID([FromBody] ValidateEmployee request)
        {

            try
            {
                request.empid = "EITS" + request.empid;
                bool employeeExists = objAlexaAuthentication.ValidateEmployeeID(request.DeviceId, request.empid.ToUpper());

                if (employeeExists)
                {
                    var objusers = objAlexaAuthentication.FetchUserDetails(request.DeviceId);
                    return Request.CreateResponse(HttpStatusCode.OK, objusers);
                }
                else
                {
                    FetchUsers objusers = new FetchUsers();
                    objusers.status = 0;
                    return Request.CreateResponse(HttpStatusCode.OK, objusers);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.ToString());
            }

        }

        [HttpPut]
        public HttpResponseMessage ValidatePin([FromBody] UserValidatePin request)
        {

            ValidatePin objPin = new ValidatePin();
            //log.InfoFormat("validatepin :: DeviceId : {0} & Some Name : {1}", request.DeviceId,null);
            log.InfoFormat("validatepin :: DeviceId : {0} ", request.DeviceId);


            // AlexaMessages objmessages = new AlexaMessages();
            try
            {
                objPin = objAlexaAuthentication.ValidatePin(request.DeviceId, request.pin);

                if (objPin.pin != 0)
                {
                    isValidUsers = true;
                    return Request.CreateResponse(HttpStatusCode.OK, objPin);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, objPin);
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.ToString());

            }

        }

        [HttpPost]
        public HttpResponseMessage AlexaGetTimeSheet([FromBody] MonthList list)
        {

            GetTimeSheet objGetTimeSheet = new GetTimeSheet();
            //log.InfoFormat("GetUserTimeSheets :: DeviceId : {0}", list.Deviceid);
            //list.Month = System.DateTime.Now.Month;
            try
            {
                bool isAuthenticated = objAlexaAuthentication.GetAuthentication(list.Deviceid);
                if (isAuthenticated)
                {
                    objGetTimeSheet = objAlexaAuthentication.GetHours(list.Month, list.Deviceid /*,list.Pin*/);
                    //objAlexaAuthentication.RemoveAuthentication(list.Deviceid);
                    return Request.CreateResponse(HttpStatusCode.OK, objGetTimeSheet);
                }
                else
                {
                    objGetTimeSheet.status = 9;
                    return Request.CreateResponse(HttpStatusCode.OK, objGetTimeSheet);
                }
                
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.ToString());
            }

        }

        //Method 5
        [HttpPost]
        public HttpResponseMessage AlexaSubmittedTimesheet([FromBody] MonthList list)
        {
            SubmitTimesheet objtimesheet = new SubmitTimesheet();
            //log.InfoFormat("GetUserSubmittedTimeSheets :: DeviceId : {0}", list.Deviceid);

            try
            {
                bool isAuthenticated = objAlexaAuthentication.GetAuthentication(list.Deviceid);
                if (isAuthenticated)
                {
                    objtimesheet = objAlexaAuthentication.SubmitTimeSheet(list.Month, list.Deviceid /*,list.Pin*/);
                    if (objtimesheet != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, objtimesheet);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data Not Found");
                    }
                }
                else
                {
                    objtimesheet.Timesheetstatus = 9;
                    return Request.CreateResponse(HttpStatusCode.NotFound, objtimesheet);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.ToString());
            }
        }

        //Method 6
        [HttpPost]
        public HttpResponseMessage AlexaCheckStatus([FromBody] MonthList list)
        {
            StatusFlags objflags = new StatusFlags();
            log.InfoFormat("AlexaCheckStatus :: DeviceId : {0}", list.Deviceid);

            try
            {
                bool isAuthenticated = objAlexaAuthentication.GetAuthentication(list.Deviceid);
                if (isAuthenticated)
                {
                    objflags = objAlexaAuthentication.CheckStatus(list.Deviceid, list.Month);
                    if (objflags != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, objflags);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data Not Found");
                    }
                }
                else
                {
                    objflags.Response = "9";
                    return Request.CreateResponse(HttpStatusCode.NotFound, objflags);
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.InnerException.ToString());
            }

        }
    }
}

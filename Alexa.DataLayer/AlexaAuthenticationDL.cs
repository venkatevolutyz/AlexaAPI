using Alexa.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexa.DataLayer
{
    public class AlexaAuthenticationDL : IAlexaAuthentication
    {
        evolutyzlabsuserEntities alexaDBEntity = new evolutyzlabsuserEntities();

        public FetchUsers FetchData(string deviceid)
        {
            try
            {
                AlexaWakeUp_Result alexaWakeUp = alexaDBEntity.AlexaWakeUp(deviceid).FirstOrDefault();
                FetchUsers users = new FetchUsers();

                if (alexaWakeUp != null)
                {
                    users.name = alexaWakeUp.Titleprefix + " " + alexaWakeUp.UsrP_FirstName + " " + alexaWakeUp.UsrP_LastName;
                    users.mobileNumber = alexaWakeUp.Usrp_MobileNumber;
                    users.status = 1;
                    var otp = GenerateRandomNo();
                    //Updating the Otp
                    var userProfile = alexaDBEntity.UsersProfiles.Where(x => x.Usrp_MobileNumber == users.mobileNumber).FirstOrDefault();
                    userProfile.passcode = otp;
                    alexaDBEntity.SaveChanges();
                    //Sending Otp via Text Message
                    var message = "Otp : " + otp;
                    var client = new RestClient("http://msg.msgclub.net/rest/services/sendSMS/sendGroupSms?AUTH_KEY=a39d36115c841484ea31ddc31936ee4&message=" + message + "&senderId=SIGNUP&routeId=8&mobileNos=" + users.mobileNumber + "&smsContentType=english");
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Cache-Control", "no-cache");
                    IRestResponse response = client.Execute(request);

                }
                else
                {
                    users.status = 0;
                }
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public FetchUsers FetchUserDetails(string deviceid)
        {
            FetchUsers users = new FetchUsers();
            try
            {
                AlexaWakeUp_Result alexaWakeUp = alexaDBEntity.AlexaWakeUp(deviceid).FirstOrDefault();

                users.name = alexaWakeUp.Titleprefix + " " + alexaWakeUp.UsrP_FirstName + " " + alexaWakeUp.UsrP_LastName;
                users.mobileNumber = alexaWakeUp.Usrp_MobileNumber;
                users.status = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }
        public bool ValidateEmployeeID(string deviceid, string empid)
        {
            var userProfile = alexaDBEntity.UsersProfiles.Where(x => x.UsrP_EmployeeID == empid).FirstOrDefault();
            if (userProfile != null)
            {
                var deviceExists = alexaDBEntity.AlexaDevices.Where(x => x.EmployeeID == empid).FirstOrDefault();
                if (deviceExists != null)
                {
                    deviceExists.DeviceID = deviceid;
                }
                else
                {
                    AlexaDevice device = new AlexaDevice();
                    device.DeviceID = deviceid;
                    device.EmployeeID = empid;
                    device.IsAuthenticated = false;
                    alexaDBEntity.AlexaDevices.Add(device);
                }
                alexaDBEntity.SaveChanges();
                var otp = GenerateRandomNo();
                //Updating the Otp
                userProfile.passcode = otp;
                alexaDBEntity.SaveChanges();
                //Sending Otp via Text Message
                var message = "Otp : " + otp;
                var client = new RestClient("http://msg.msgclub.net/rest/services/sendSMS/sendGroupSms?AUTH_KEY=a39d36115c841484ea31ddc31936ee4&message=" + message + "&senderId=SIGNUP&routeId=8&mobileNos=" + userProfile.Usrp_MobileNumber + "&smsContentType=english");
                var request = new RestRequest(Method.GET);
                request.AddHeader("Cache-Control", "no-cache");
                IRestResponse response = client.Execute(request);


                return true;
            }
            else
            {
                return false;
            }

        }

        public ValidatePin ValidatePin(string deviceid, int pin)
        {
            ObjectParameter returnId = new ObjectParameter("message", typeof(int));
            alexaDBEntity.AlexaValidateOtp(deviceid, pin, returnId);
            ValidatePin vp = new ValidatePin();
            if (Convert.ToInt32(returnId.Value) == 1)
            {
                vp.pin = 1;
                vp.Flag = ResponseCodes.Exits;
                var user = alexaDBEntity.AlexaDevices.Where(x => x.DeviceID == deviceid).FirstOrDefault();
                user.IsAuthenticated = true;
                alexaDBEntity.SaveChanges();
            }
            else
            {
                vp.pin = 0;
                vp.Flag = ResponseCodes.NotExists;
                var user = alexaDBEntity.AlexaDevices.Where(x => x.DeviceID == deviceid).FirstOrDefault();
                user.IsAuthenticated = false;
                alexaDBEntity.SaveChanges();
            }
            return vp;
        }

        public GetTimeSheet GetHours(int month, string deviceid)
        {
            //var result = alexaDBEntity.AlexaGetWorkingHoursDetails(month, deviceid);
            var timeSheetDetails = alexaDBEntity.AlexaGetWorkingHoursDetails_JKTest2(month, deviceid).FirstOrDefault();
            GetTimeSheet getTimeSheet = new GetTimeSheet();

            if (timeSheetDetails != null)
            {
                getTimeSheet.Workingdays = Convert.ToInt32(timeSheetDetails.WorkingDays);
                getTimeSheet.FirstName = timeSheetDetails.FirstName;
                getTimeSheet.Workinghours = Convert.ToInt32(timeSheetDetails.WorkingHours);
                getTimeSheet.HolidaysCount = Convert.ToInt32(timeSheetDetails.HolidaysCount);
                getTimeSheet.status = 1;
                //var result = alexaDBEntity.AlexaDevices.Where(x => x.DeviceID == deviceid).FirstOrDefault();
                //result.IsAuthenticated = false;
                alexaDBEntity.SaveChanges();
            }
            else
            {
                getTimeSheet.status = 0;
            }
            return getTimeSheet;
        }

        public bool GetAuthentication(string deviceid)
        {
            var result = alexaDBEntity.AlexaDevices.Where(x => x.DeviceID == deviceid).FirstOrDefault();
            if (result != null)
            {
                return Convert.ToBoolean(result.IsAuthenticated);
            }
            else
            {
                return false;
            }
        }

        public SubmitTimesheet SubmitTimeSheet(int month, string deviceid /*string phonenumber,*/)
        {
            SubmitTimesheet objtimesheet = new SubmitTimesheet();
            try
            {
                var timeSheetDetails = alexaDBEntity.AlexaSubmitUserTimesheetdetails(month, deviceid).FirstOrDefault();
                objtimesheet.Timesheetstatus = Convert.ToInt32(timeSheetDetails.Column1);
                objtimesheet.GetWorkingHours = Convert.ToInt32(timeSheetDetails.Column2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objtimesheet;
        }

        public StatusFlags CheckStatus(string deviceid, int month)
        {
            StatusFlags objflags = new StatusFlags();
            try
            {
                objflags.Response = alexaDBEntity.AlexaCheckTimeSheetStatus(deviceid, month).ToString();

            }

            catch (Exception ex)
            {
                throw ex;
            }
            return objflags;

        }

        public static int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}








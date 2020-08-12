using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexa.Models
{
    public class UsersProfile
    {
        public int UsrP_UserProfileID { get; set; }
        public int UsrP_UserID { get; set; }
        public string UsrP_FirstName { get; set; }
        public string UsrP_LastName { get; set; }
        public string UsrP_EmployeeID { get; set; }
        public string UsrP_EmailID { get; set; }
        public string Usrp_MobileNumber { get; set; }
        public string Usrp_PhoneNumber { get; set; }
        public string Usrp_ProfilePicture { get; set; }
        public Nullable<System.DateTime> UsrP_DOB { get; set; }
        public Nullable<System.DateTime> Usrp_DOJ { get; set; }
        public bool UsrP_ActiveStatus { get; set; }
        public short UsrP_Version { get; set; }
        public System.DateTime UsrP_CreatedDate { get; set; }
        public int UsrP_CreatedBy { get; set; }
        public Nullable<System.DateTime> UsrP_ModifiedDate { get; set; }
        public Nullable<int> UsrP_ModifiedBy { get; set; }
        public bool UsrP_isDeleted { get; set; }
        public Nullable<int> Usr_Titleid { get; set; }
        public Nullable<int> Usr_GenderId { get; set; }
        public string Marital_Status { get; set; }
        public string PermanentAddress { get; set; }
        public string TemporaryAddress { get; set; }
        public Nullable<int> passcode { get; set; }

        //public virtual  User { get; set; }
    }
}

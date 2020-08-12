using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexa.Models
{
    public class GetTimeSheet
    {
        public List<HolidayCalender> lstholidays { get; set; }
        public int HolidaysCount { get; set; }
        public string FirstName { get; set; }
        public int Workingdays { get; set; }
        public int Workinghours { get; set; }
        public string TimesheetMonth { get; set; }

        public int status { get; set; }
    }
}

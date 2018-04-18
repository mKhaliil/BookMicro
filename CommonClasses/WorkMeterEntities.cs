using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System.CommonClasses
{
    public class WorkMeterEntities
    {
        //tblUserLogin Entities
        public string FullName { get; set; }
        public string Email { get; set; }
        //end

        //tblTaskSheet Entities
        public string UserId { get; set; }
        public string TaskCreationDate { get; set; }
        //end

        //tblTaskDetails Entities
        public string CatId { get; set; }
        public string BookId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string CalculatedTime { get; set; }
        public string Comments { get; set; }
        public string Current_Status { get; set; }
        public string Target { get; set; }
        public string Achived { get; set; }
        public string Complexity { get; set; }
        public string End_Date { get; set; }
        public string Expected_Pages { get; set; }
        public string Expected_Hours { get; set; }
        public string Result { get; set; }
        public string Productivity_Hours { get; set; }
        public string Tool_Used { get; set; }
        //end

        //TBL_DATEWISE_INFO Entities
        public List<DateWiseInfo> DailyTimeSpent { get; set; }
        //end
    }
}
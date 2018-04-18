using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System.CommonClasses
{
    public class UserRank
    {
        public string TaskType { get; set; }
        public string RankId { get; set; }
        public string RankName { get; set; }
        public int RequiredTasks { get; set; }
        public int MinApprovedTasks { get; set; }
        public int MaxApprovedTasks { get; set; }
    }
}
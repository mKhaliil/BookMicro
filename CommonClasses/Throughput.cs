using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System.CommonClasses
{
    public class Throughput
    {
        public string Tool { get; set; }
        public string Task { get; set; }
        public string Complexity { get; set; }
        public string OnePageTimeInSec { get; set; }
        public string ExpectedPagesPerHour { get; set; }
        public string BookUnitTime { get; set; }
    }
}
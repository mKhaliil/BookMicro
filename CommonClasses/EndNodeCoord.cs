using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System.CommonClasses
{
    public class EndNodeCoord
    {
        public double NormalX { get; set; }
        public double NormalIndentX { get; set; }
        public double NormalFontSize { get; set; }
        public string NormalFontName { get; set; }
        public int StartNumber { get; set; }
        public int EndNumber { get; set; }
        public int Page { get; set; }
        public string PageType { get; set; }
    }
}
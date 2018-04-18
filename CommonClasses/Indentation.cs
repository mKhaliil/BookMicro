using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System.CommonClasses
{
    public class Indentation
    {
        public string BookId { get; set; }
        public string PageType { get; set; }
        public double NormalX { get; set; }
        public double NormalIndentX { get; set; }
        public double Endx { get; set; }
        public double NormalY { get; set; }
        public double PrevParaY { get; set; }
        public double NextParaY { get; set; }
        public string FontName { get; set; }
        public double FontSize { get; set; }
        public string ParaType { get; set; }
        public int Page { get; set; }

    }
}
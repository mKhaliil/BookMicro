using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System.PdfCompare_Classes
{
    public class PdfCoord
    {
        public double Llx { get; set; }
        public double Lly { get; set; }
        public double Urx { get; set; }
        public double Ury { get; set; }
        public double FontSize { get; set; }
        public string FontName { get; set; }
        public string Y { get; set; }
    }
}
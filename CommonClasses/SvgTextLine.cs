using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System.CommonClasses
{
    public class SvgTextLine
    {
        public string Text { get; set; }
        public string RgbValue { get; set; }
        public string Color { get; set; }

        public double TopYCoord { get; set; }

        public double BottomYCoord { get; set; }
    }
}
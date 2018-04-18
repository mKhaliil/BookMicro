using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System.PdfCompare_Classes
{
    public class PdfIndentation
    {
        //public double NormalX { get; set; }
        //public double NormalIndentX { get; set; }
        //public double NormalEndX { get; set; }
        //public double NormalY { get; set; }
        //public double NormalFontSize { get; set; }
        //public string NormalFontName { get; set; }

        public double NormalXEvenPages { get; set; }
        public double NormalIndentXEvenPages { get; set; }
        public double NormalEndxEvenPages { get; set; }
        public double NormalYEvenPages { get; set; }
        public double NormalFontSizeEvenPages { get; set; }

        public double NormalXOddPages { get; set; }
        public double NormalIndentXOddPages { get; set; }
        public double NormalEndXOddPages { get; set; }
        public double NormalYOddPages { get; set; }
        public double NormalFontSizeOddPages { get; set; }

    }
}
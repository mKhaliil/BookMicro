using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System.PdfCompare_Classes
{
    public class PdfJsLine
    {
        public string Text { get; set; }
        public string Top { get; set; }
        public string Left { get; set; }
        public int LineNum { get; set; }
        public string DivNum { get; set; }
        public int PageNum { get; set; }
        public bool IsErrorLine { get; set; }
        public bool IsEmbededFontLine { get; set; }

        public string ParaType { get; set; }
    }
}
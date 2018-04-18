using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System.PdfCompare_Classes
{
    public class TetmlLine
    {
        public string Text { get; set; }
        public string Lly { get; set; }

        public string Llx { get; set; }
        public string Ury { get; set; }

        public string Urx { get; set; }

        public int LineNum { get; set; }

        public double Top { get; set; }
        public double Height { get; set; }
        public double Left { get; set; }

        public string Font { get; set; }

        public string FontSize { get; set; }

        public string FontType { get; set; }
    }
}
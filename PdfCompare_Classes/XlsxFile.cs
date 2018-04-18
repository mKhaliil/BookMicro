using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System.PdfCompare_Classes
{
    public class XlsxFile
    {
        public string Extension { get; set; }
        public string Name { get; set; }
        public int TableNum { get; set; }
        public int PageNum { get; set; }
    }
}
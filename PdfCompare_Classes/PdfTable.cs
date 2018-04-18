using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System.PdfCompare_Classes
{
    public class PdfTable
    {
        public List<string> Lines { get; set; }

        public List<TableLine> LinesByColumnNum { get; set; }

        public int TableNum { get; set; }

        public int PageNum { get; set; }
    }
}
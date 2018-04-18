using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System.PdfCompare_Classes
{
    public class TableLine
    {
        public string Text { get; set; }

        public string Type { get; set; }

        public int RowNum { get; set; }

        public string ColumnNum { get; set; }
    }
}
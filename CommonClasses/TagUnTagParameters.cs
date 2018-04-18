using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System.CommonClasses
{
    public class TagUnTagParameters
    {
        public bool IsIndex { get; set; }

        public int IndexStart { get; set; }

        public int IndexEnd { get; set; }

        public bool IsImage { get; set; }

        public bool IsTable { get; set; }

        public bool IsAlgo1 { get; set; }

        public bool IsAlgo2 { get; set; }

        public bool IsAlgo3 { get; set; }

        public bool IsNPara { get; set; }

        public bool IsNParaAlgo1 { get; set; }

        public bool IsNParaAlgo2 { get; set; }

        public bool IsSPara { get; set; }

        public bool IsFootNotes { get; set; }
    }
}
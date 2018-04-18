using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Outsourcing_System.CommonClasses
{
    public class PdfFootNote
    {
        public int Page { get; set; }

        public string SupScriptWord { get; set; }

        public string FootNoteText { get; set; }

        public XmlNode SupScriptXmlLine { get; set; }

        //public XmlNodeList FootNoteXmlLines { get; set; }

        public List<XmlNode> FootNoteXmlLines { get; set; }
    }
}
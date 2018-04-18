using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System
{
    public class FinalFonts
    {
        public FinalFonts()
        { 
        }
        private string font;

        public string Font
        {
            get { return font; }
            set { font = value; }
        }
        private string fontname;

        public string Fontname
        {
            get { return fontname; }
            set { fontname = value; }
        }
        private string fontsize;
        private string fontType;

        public string FontType
        {
            get { return fontType; }
            set { fontType = value; }
        }
        public string Fontsize
        {
            get { return fontsize; }
            set { fontsize = value; }
        }
        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        private string pagerange;

        public string Pagerange
        {
            get { return pagerange; }
            set { pagerange = value; }
        }
        private string sectiontype;

        public string Sectiontype
        {
            get { return sectiontype; }
            set { sectiontype = value; }
        }
        private bool iscaps;

        public bool Iscaps
        {
            get { return iscaps; }
            set { iscaps = value; }
        }
    }
}
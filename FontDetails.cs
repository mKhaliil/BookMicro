using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System
{
    public class FontDetails
    {
        public FontDetails()
        {
        }
        private string presection;

        public string Presection
        {
            get { return presection; }
            set { presection = value; }
        }
        private string body;

        public string Body
        {
            get { return body; }
            set { body = value; }
        }
        private string postsection;

        public string Postsection
        {
            get { return postsection; }
            set { postsection = value; }
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
        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        private string fontsize;

        public string Fontsize
        {
            get { return fontsize; }
            set { fontsize = value; }
        }
        private string pageno;

        public string Pageno
        {
            get { return pageno; }
            set { pageno = value; }
        }
    }
}
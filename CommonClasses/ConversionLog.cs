using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System
{
    public class ConversionLog
    {
        private string pageno;

        public string Pageno
        {
            get { return pageno; }
            set { pageno = value; }
        }
        private string merging;

        public string Merging
        {
            get { return merging; }
            set { merging = value; }
        }
        private string splitting;

        public string Splitting
        {
            get { return splitting; }
            set { splitting = value; }
        }
        private string textediting;

        public string Textediting
        {
            get { return textediting; }
            set { textediting = value; }
        }

        private string paraconversions;

        public string Paraconversions
        {
            get { return paraconversions; }
            set { paraconversions = value; }
        }
        private string tableinserted;

        public string Tableinserted
        {
            get { return tableinserted; }
            set { tableinserted = value; }
        }
        private string boxinserted;

        public string Boxinserted
        {
            get { return boxinserted; }
            set { boxinserted = value; }
        }
        private string imageinserted;

        public string Imageinserted
        {
            get { return imageinserted; }
            set { imageinserted = value; }
        }
        private string nodedeleted;

        public string Nodedeleted
        {
            get { return nodedeleted; }
            set { nodedeleted = value; }
        }
        private string paraaddition;

        public string Paraaddition
        {
            get { return paraaddition; }
            set { paraaddition = value; }
        }
        private string sectionaddition;

        public string Sectionaddition
        {
            get { return sectionaddition; }
            set { sectionaddition = value; }
        }

    }
}
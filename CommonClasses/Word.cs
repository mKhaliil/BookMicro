using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outsourcing_System
{
    public class Word : IComparable
    {
        private int pageNum;
        public bool HasCordinates
        {
            get
            {
                if (strCordinates != null && strCordinates.Trim() != "")
                    return true;
                else
                    return false;
            }
        }

        private string strCordinates;
        /// <summary>
        /// Gets or Sets the coordinates of the word 
        /// </summary>
        public string StrCordinates
        {
            get { return strCordinates; }
            set { this.strCordinates = value; }
        }

        public int PageNumber
        {
            get { return pageNum; }
            set { this.pageNum = value; }
        }
        private int lineNum;
        public int LineNumber
        {
            get { return lineNum; }
            set { this.lineNum = value; }
        }
        private string text;
        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
            }
        }
        public Word(int page, int line, string text)
        {
            this.lineNum = line;
            this.pageNum = page;
            this.text = text;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
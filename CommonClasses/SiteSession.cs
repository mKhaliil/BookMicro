using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;
using System.Collections;

namespace Outsourcing_System
{
    /// <summary>
    /// Summary description for SiteSession
    /// </summary>
    public class SiteSession
    {
        public SiteSession()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static XmlDocument xmlDoc
        {
            get
            {
                if (HttpContext.Current.Session["xmlDoc"] != null)
                {
                    return ((XmlDocument)HttpContext.Current.Session["xmlDoc"]);
                }
                return null;
            }
            set { HttpContext.Current.Session["xmlDoc"] = value; }
        }

        public static int MainCurrPage
        {
            get
            {
                if (HttpContext.Current.Session["MainCurrPage"] != null)
                {
                    return int.Parse(HttpContext.Current.Session["MainCurrPage"].ToString());
                }
                return -1;
            }
            set { HttpContext.Current.Session["MainCurrPage"] = value; }
        }

      

       
       

      

        public static ArrayList ReportListForComments
        {
            get
            {
                if (HttpContext.Current.Session["ReportListForComments"] != null)
                {
                    return (ArrayList)HttpContext.Current.Session["ReportListForComments"];
                }
                return new ArrayList();
            }
            set { HttpContext.Current.Session["ReportListForComments"] = value; }

        }

        public static ArrayList AllSeperatePages
        {
            get
            {
                if (HttpContext.Current.Session["AllSeperatePages"] != null)
                {
                    return (ArrayList)HttpContext.Current.Session["AllSeperatePages"];
                }
                return new ArrayList();
            }
            set { HttpContext.Current.Session["AllSeperatePages"] = value; }
        }

        public static String ModifiedWord
        {
            get { return (String)HttpContext.Current.Session["ModifiedWord"]; }
            set { HttpContext.Current.Session["ModifiedWord"] = value; }
        }

        public static int CurrentErrorIndex
        {
            get
            {
                if (HttpContext.Current.Session["CurrentErrorIndex"] != null)
                {
                    return (int)HttpContext.Current.Session["CurrentErrorIndex"];
                }
                return 0;
            }
            set { HttpContext.Current.Session["CurrentErrorIndex"] = value; }
        }

        public static int CurrentMistakeIndex
        {
            get
            {
                if (HttpContext.Current.Session["CurrentMistakeIndex"] != null)
                {
                    return (int)HttpContext.Current.Session["CurrentMistakeIndex"];
                }
                return 0;
            }
            set { HttpContext.Current.Session["CurrentMistakeIndex"] = value; }
        }

        public static int TotalMistakesCount
        {
            get
            {
                if (HttpContext.Current.Session["TotalMistakesCount"] != null)
                {
                    return (int)HttpContext.Current.Session["TotalMistakesCount"];
                }
                return 0;
            }
            set { HttpContext.Current.Session["TotalMistakesCount"] = value; }
        }

        /// <summary>
        /// Contains the object to reference Error List
        /// </summary>
     
        /// <summary>
        /// Contains the Comments in PDFViewer control
        /// </summary>
        public static String PDFViewerComments
        {
            get
            {
                if (HttpContext.Current.Session["PDFViewerComments"] != null)
                {
                    return (String)HttpContext.Current.Session["PDFViewerComments"];
                }
                return null;
            }
            set { HttpContext.Current.Session["PDFViewerComments"] = value; }
        }

    }
}

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
using Outsourcing_System.PdfCompare_Classes;

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
        set
        {
            HttpContext.Current.Session["xmlDoc"] = value;
        }
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
        set
        {
            HttpContext.Current.Session["MainCurrPage"] = value;
        }
    }

    public static RHYWManipulation RHYWManFileObj
    {
        get
        {
            if (HttpContext.Current.Session["RHYWFileObj"] != null)
            {
                return (RHYWManipulation)HttpContext.Current.Session["RHYWFileObj"];
            }
            return null;
        }
        set
        {
            HttpContext.Current.Session["RHYWFileObj"] = value;
        }
    }

    public static PDFManipulation PDFManFileObj
    {
        get
        {
            if (HttpContext.Current.Session["PDFManFileObj"] != null)
            {
                return (PDFManipulation)HttpContext.Current.Session["PDFManFileObj"];
            }
            return null;
        }
        set
        {
            HttpContext.Current.Session["PDFManFileObj"] = value;
        }
    }

    public static PDFManipulation PDFPrdManFileObj
    {
        get
        {
            if (HttpContext.Current.Session["PDFManFileObj"] != null)
            {
                return (PDFManipulation)HttpContext.Current.Session["PDFManFileObj"];
            }
            return null;
        }
        set
        {
            HttpContext.Current.Session["PDFManFileObj"] = value;
        }
    }

    public static SourcePDF SourcePDFObj
    {
        get
        {
            if (HttpContext.Current.Session["SourcePDFObj"] != null)
            {
                return (SourcePDF)HttpContext.Current.Session["SourcePDFObj"];
            }
            return null;
        }
        set
        {
            HttpContext.Current.Session["SourcePDFObj"] = value;
        }
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
        set
        {
            HttpContext.Current.Session["ReportListForComments"] = value;
        }

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
        set
        {
            HttpContext.Current.Session["AllSeperatePages"] = value;
        }
    }
    public static String ModifiedWord
    {
        get
        {

            return (String)HttpContext.Current.Session["ModifiedWord"];

        }
        set
        {
            HttpContext.Current.Session["ModifiedWord"] = value;
        }
    }

    public static bool IsPdfLocked
    {
        get
        {
            if (HttpContext.Current.Session["IsPdfLocked"] == null)
                return false;

            return (bool)HttpContext.Current.Session["IsPdfLocked"];

        }
        set
        {
            HttpContext.Current.Session["IsPdfLocked"] = value;
        }
    }

    public static String OutPutStampingPDF
    {
        get
        {

            return (String)HttpContext.Current.Session["OutPutStampingPDF"];

        }
        set
        {
            HttpContext.Current.Session["OutPutStampingPDF"] = value;
        }
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
        set
        {
            HttpContext.Current.Session["CurrentErrorIndex"] = value;
        }
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
        set
        {
            HttpContext.Current.Session["CurrentMistakeIndex"] = value;
        }
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
        set
        {
            HttpContext.Current.Session["TotalMistakesCount"] = value;
        }
    }

    /// <summary>
    /// Contains the object to reference Error List
    /// </summary>
    public static ErrorListHandler errorHl
    {
        get
        {
            if (HttpContext.Current.Session["errorHl"] != null)
            {
                return (ErrorListHandler)HttpContext.Current.Session["errorHl"];
            }
            return null;
        }
        set
        {
            HttpContext.Current.Session["errorHl"] = value;
        }
    }

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
        set
        {
            HttpContext.Current.Session["PDFViewerComments"] = value;
        }
    }

    public static String setDefaultXSL
    {
        get
        {
            if (HttpContext.Current.Session["setDefaultXSL"] != null)
            {
                return (String)HttpContext.Current.Session["setDefaultXSL"];
            }
            return null;
        }
        set
        {
            HttpContext.Current.Session["setDefaultXSL"] = value;
        }
    }

    public static String MainXMLFilePath_PDF
    {
        get
        {
            if (HttpContext.Current.Session["MainXMLFilePath_PDF"] != null)
            {
                return (String)HttpContext.Current.Session["MainXMLFilePath_PDF"];
            }
            return null;
        }
        set
        {
            HttpContext.Current.Session["MainXMLFilePath_PDF"] = value;
        }
    }

    ////public static int CurrentPdfPage
    ////{
    ////    get
    ////    {
    ////        if (HttpContext.Current.Session["currentPdfPage"] != null)
    ////        {
    ////            return (int)HttpContext.Current.Session["currentPdfPage"];
    ////        }
    ////        return -1;
    ////    }
    ////    set
    ////    {
    ////        HttpContext.Current.Session["currentPdfPage"] = value;
    ////    }
    ////}

    //public static string CurrentXSLs
    //{
    //    get
    //    {
    //        if (HttpContext.Current.Session["CurrentXSLs"] != null)
    //        {
    //            return int.Parse(HttpContext.Current.Session["CurrentXSLs"].ToString());
    //        }
    //        return null;
    //    }
    //    set
    //    {
    //        HttpContext.Current.Session["CurrentXSLs"] = value;
    //    }
    //}

}

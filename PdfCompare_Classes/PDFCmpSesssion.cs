using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;


/// <summary>
/// Summary description for CurrSesssion
/// </summary>
public class PDFCmpSesssion
{
    // private constructor
    private PDFCmpSesssion()
    {
    }
    //ArrayList MisMatchArrayList;
    // Gets the current session.
    public static PDFCmpSesssion Current
    {
        get
        {
            PDFCmpSesssion session = (PDFCmpSesssion)HttpContext.Current.Session["__MySession__"];
            if (session == null)
            {
                session = new PDFCmpSesssion();
                HttpContext.Current.Session["__MySession__"] = session;
            }
            return session;
        }
    }
    public static bool LoadPDFAsImage
    {
        get
        {
            if (HttpContext.Current.Session["LoadPDFAsImage"] != null)
            {
                return (bool)HttpContext.Current.Session["LoadPDFAsImage"];
            }
            return false;
        }
        set
        {
            HttpContext.Current.Session["LoadPDFAsImage"] = value;
        }
    }
    // **** add your session properties here, e.g like this:
    //public array
    //public string Property1 { get; set; }
    //public DateTime MyDate { get; set; }
    public static int list1PrevMatchIndex
    {
        get
        {
            if (HttpContext.Current.Session["list1PrevMatchIndex"] != null)
            {
                return int.Parse(HttpContext.Current.Session["list1PrevMatchIndex"].ToString());
            }
            return -1;
        }
        set
        {
            HttpContext.Current.Session["list1PrevMatchIndex"] = value;
        }
    }

    public static int List1CurrPageNo
    {
        get
        {
            if (HttpContext.Current.Session["List1CurrPageNo"] != null)
            {
                return int.Parse(HttpContext.Current.Session["List1CurrPageNo"].ToString());
            }
            return -1;
        }
        set
        {
            HttpContext.Current.Session["List1CurrPageNo"] = value;
        }
    }

    public static int List2CurrPageNo
    {
        get
        {
            if (HttpContext.Current.Session["List2CurrPageNo"] != null)
            {
                return int.Parse(HttpContext.Current.Session["List2CurrPageNo"].ToString());
            }
            return -1;
        }
        set
        {
            HttpContext.Current.Session["List2CurrPageNo"] = value;
        }
    }

    public static int list2PrevMatchIndex
    {
        get
        {
            if (HttpContext.Current.Session["list2PrevMatchIndex"] != null)
            {
                return int.Parse(HttpContext.Current.Session["list2PrevMatchIndex"].ToString());
            }
            return -1;
        }
        set
        {
            HttpContext.Current.Session["list2PrevMatchIndex"] = value;
        }
    }

    public static int list1ShowingIndex
    {
        get
        {
            if (HttpContext.Current.Session["list1ShowingIndex"] != null)
            {
                return int.Parse(HttpContext.Current.Session["list1ShowingIndex"].ToString());
            }
            return -1;
        }
        set
        {
            HttpContext.Current.Session["list1ShowingIndex"] = value;
        }
    }

    public static int list2ShowingIndex
    {
        get
        {
            if (HttpContext.Current.Session["list2ShowingIndex"] != null)
            {
                return int.Parse(HttpContext.Current.Session["list2ShowingIndex"].ToString());
            }
            return -1;
        }
        set
        {
            HttpContext.Current.Session["list2ShowingIndex"] = value;
        }
    }

    public static ArrayList misMatchList
    {
        get
        {
            if (HttpContext.Current.Session["misMatchList"] != null)
            {
                return (ArrayList)HttpContext.Current.Session["misMatchList"];
            }
            return null;
        }
        set
        {
            HttpContext.Current.Session["misMatchList"] = value;
        }
    }

    public static ArrayList wrdList1
    {
        get
        {
            if (HttpContext.Current.Session["wrdList1"] != null)
            {
                return (ArrayList)HttpContext.Current.Session["wrdList1"];
            }
            return null;
        }
        set
        {
            HttpContext.Current.Session["wrdList1"] = value;
        }
    }

    public static ArrayList wrdList2
    {
        get
        {
            if (HttpContext.Current.Session["wrdList2"] != null)
            {
                return (ArrayList)HttpContext.Current.Session["wrdList2"];
            }
            return null;
        }
        set
        {
            HttpContext.Current.Session["wrdList2"] = value;
        }
    }

    public static CompareFiles cf
    {
        get
        {
            if (HttpContext.Current.Session["cf"] != null)
            {
                return (CompareFiles)HttpContext.Current.Session["cf"];
            }
            return null;
        }
        set
        {
            HttpContext.Current.Session["cf"] = value;
        }
    }

    public static string FirstPDFPath
    {
        get
        {
            if (HttpContext.Current.Session["FirstPDFPath"] != null)
            {
                return HttpContext.Current.Session["FirstPDFPath"].ToString();
            }
            return "";
        }
        set
        {
            HttpContext.Current.Session["FirstPDFPath"] = value;
        }
    }

    public static string SecondPDFPath
    {
        get
        {
            if (HttpContext.Current.Session["SecondPDFPath"] != null)
            {
                return HttpContext.Current.Session["SecondPDFPath"].ToString();
            }
            return "";
        }
        set
        {
            HttpContext.Current.Session["SecondPDFPath"] = value;
        }
    }

    public static string MainXMLFilePath
    {
        get
        {
            if (HttpContext.Current.Session["MainXMLFilePath"] != null)
            {
                return HttpContext.Current.Session["MainXMLFilePath"].ToString();
            }
            return "";
        }
        set
        {
            HttpContext.Current.Session["MainXMLFilePath"] = value;
        }
    }

    public static string XMLFilePath
    {
        get
        {
            if (HttpContext.Current.Session["XMLFilePath"] != null)
            {
                return HttpContext.Current.Session["XMLFilePath"].ToString();
            }
            return "";
        }
        set
        {
            HttpContext.Current.Session["XMLFilePath"] = value;
        }
    }

    public static string ModifiedRHYWFilePath
    {
        get
        {
            if (HttpContext.Current.Session["ModifiedRHYWFilePath"] != null)
            {
                return HttpContext.Current.Session["ModifiedRHYWFilePath"].ToString();
            }
            return "";
        }
        set
        {
            HttpContext.Current.Session["ModifiedRHYWFilePath"] = value;
        }
    }

    public static string PDF1PrevPath
    {
        get
        {
            if (HttpContext.Current.Session["PDF1PrevPath"] != null)
            {
                return HttpContext.Current.Session["PDF1PrevPath"].ToString();
            }
            return "";
        }
        set
        {
            HttpContext.Current.Session["PDF1PrevPath"] = value;
        }
    }

    public static string PDF2PrevPath
    {
        get
        {
            if (HttpContext.Current.Session["PDF2PrevPath"] != null)
            {
                return HttpContext.Current.Session["PDF2PrevPath"].ToString();
            }
            return "";
        }
        set
        {
            HttpContext.Current.Session["PDF2PrevPath"] = value;
        }
    }

    public static MisMatch CurrentPageMisMatch
    {
        get
        {
            if (HttpContext.Current.Session["CurrentPageMisMatch"] != null)
            {
                return (MisMatch)HttpContext.Current.Session["CurrentPageMisMatch"];
            }
            return new MisMatch();
        }
        set
        {
            HttpContext.Current.Session["CurrentPageMisMatch"] = value;
        }
    }
}
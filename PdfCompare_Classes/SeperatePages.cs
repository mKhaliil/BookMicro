using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SeperatePages
/// </summary>
/// 
public class SeperatePages
{
    private String prodPDFPath;

    public String ProdPDFPath
    {
        get
        {
            return prodPDFPath;
        }
        set
        {
            this.prodPDFPath = value;
        }
    }
    private String srcPDFPath;

    public String SrcPDFPath
    {
        get
        {
            return srcPDFPath;
        }
        set
        {
            this.srcPDFPath = value;
        }
    } 
    private String xmlPath;

    public String XMLPath
    {
        get
        {
            return xmlPath;
        }
        set
        {
            this.xmlPath = value;
        }
    }

    public SeperatePages()
    {

    }
}
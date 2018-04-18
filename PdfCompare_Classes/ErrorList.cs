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
using Outsourcing_System;

/// <summary>
/// Summary description for ErrorList
/// </summary>
public class MisMatchError
{
    public PdfWord list1Word;
    public PdfWord list2Word;
    public int PageNumber;
    public string pdfSourcePath;
    public string pdfProducedPath;
    public string xmlPath;
    public MisMatch misMatch;
    /// <summary>
    /// 
    /// </summary>
    public string commetns;
    public MisMatchError()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    
}

public class Mistakes
{
    public int page;
    public int mistakeNum;
    public string pdfSourcePath;
    public string pdfProducedPath;
    public string xmlPath;
    public string comments;
    public string mistakeId;
    public string innerText;
}



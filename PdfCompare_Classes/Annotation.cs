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

/// <summary>
/// Summary description for Annotation
/// </summary>
public class RHYWAnnotation
{
    private AnnotType _annotType;
    public AnnotType annotType
    {
        get
        {
            return this._annotType;
        }
    }

    public float llx;
    public float lly;
    public float urx;
    public float ury;
    private string description;
    public string Description
    {
        get
        {
            return this.description;
        }
        set
        {
            this.description = value;
        }
    }
    public RHYWAnnotation(AnnotType annotType)
    {
        this._annotType = annotType;
        llx = lly = urx = ury = 0;
    }

    public string GetHeading()
    {
        return this._annotType.ToString();
    }

    public string GetMessage()
    {
        return "RHYW entry";
    }
}
public enum AnnotType { Part, Chapter, Upara, Npara, Spara, Image, Box}

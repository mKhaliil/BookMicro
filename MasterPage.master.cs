using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class MasterPage : System.Web.UI.MasterPage
{
    MyDBClass objMyDBClass = new MyDBClass();
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (Request.Url.OriginalString.Contains("http://192.168.0.200:91/"))
        {
            objMyDBClass.MainDirectory = ConfigurationManager.AppSettings["MainDirectory"].ToString();
        }
        else
        {
            objMyDBClass.MainDirectory = ConfigurationManager.AppSettings["LiveMainDirectory"].ToString();
        }
        Session["MainDirectory"] = objMyDBClass.MainDirectory;
    }
}

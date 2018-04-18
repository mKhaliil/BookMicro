using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Outsourcing_System.CustomControls
{
    public partial class LoadControl : System.Web.UI.UserControl
    {
        public event EventHandler ButtonPostBack;
        public string ImagePath
        {
            set
            {
                //this.
            }
        }
        public string DisplayText
        {
            set
            {
                this.btnSubmit.Text = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void OnPostBack(EventArgs e)
        {
            if (ButtonPostBack != null)
            {
                ButtonPostBack(this, e);
            }
        }
        protected void lnkBtn_Click(object sender, EventArgs e)
        {
            //System.Threading.Thread.Sleep(5000);
            Response.Write("Control Event");
            OnPostBack(e);
            Response.Write("Control Event End");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outsourcing_System.MasterPages
{
    public partial class AdminMaster_Hiring : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public bool SetLogOut
        {
            set
            {
                divLogout.Visible = value;
                aloginLink.Attributes.Add("style", "display:none");
            }
        }

        public string SetMenuLocation
        {
            set
            {
                divMenu.Attributes.Add("style", "margin-left:" + value);
            }
        }

        public void ShowMessageBox(string MessageText, string MessageType)
        {
            switch (MessageType)
            {
                case "Info":
                    divInfo.Visible = true;
                    divInfo.InnerText = MessageText;
                    break;
                case "Succ":
                    divSuccess.Visible = true;
                    divSuccess.InnerText = MessageText;
                    break;
                case "error":
                    divError.Visible = true;
                    divError.InnerText = MessageText;
                    break;
                case "NoMessage":
                    divError.Visible = false;
                    divInfo.Visible = false;
                    divSuccess.Visible = false;
                    divError.InnerText = MessageText;
                    break;
                default:
                    break;
            }
        }
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();            
            Response.Redirect("BookMicro.aspx");
        }
    }
}

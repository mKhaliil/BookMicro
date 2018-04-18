using System.Web.UI.HtmlControls;
using Outsourcing_System.PdfCompare_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outsourcing_System.UserControls.CommonControl
{
    public partial class ucShowMessage : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ShowMessage(MessageTypes messageType, string message)
        {
            switch (messageType)
            {
                case MessageTypes.Info:
                    lblInfoMessage.Text = message;
                    SlideToggleMessage(divInfo);
                    break;
                case MessageTypes.Success:
                    lblSuccessMessage.Text = message;
                    SlideToggleSuccessMessage(divSuccess);
                    break;
                case MessageTypes.Error:
                    lblErrorMessage.Text = message;
                    SlideToggleMessage(divError);
                    break;
                default:
                    break;
            }
        }

        private void SlideToggleSuccessMessage(HtmlControl div)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "ScriptName", "$(\"#" + div.ClientID + "\").display='block';" +
                ";$(\"#" + div.ClientID + "\").slideDown(\"slow\");" + "setTimeout(function(){$(\"#" +
                div.ClientID + "\").slideUp(\"slow\");},25000);", true);
        }

        private void SlideToggleMessage(HtmlControl div)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "ScriptName", "$(\"#" + div.ClientID + "\").display='block';" +
                ";$(\"#" + div.ClientID + "\").slideDown(\"slow\");" + "setTimeout(function(){$(\"#" +
                div.ClientID + "\").slideUp(\"slow\");},15000);", true);
        }
    }
}
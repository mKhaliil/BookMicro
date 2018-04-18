using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Configuration;

namespace Outsourcing_System
{
    public partial class OnlineTestMasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Url.OriginalString.Contains("http://192.168.0.200:91/"))
                {
                    Session["WebSiteIPAdress"] = Convert.ToString(ConfigurationManager.AppSettings["MainDirectory"]);
                }
                else if (Request.Url.OriginalString.Contains("localhost"))
                {
                    Session["WebSiteIPAdress"] = Convert.ToString(Request.Url.Authority);
                }
                else if (Request.Url.OriginalString.Contains("http://182.191.87.77:91/"))
                {
                    Session["WebSiteIPAdress"] = Convert.ToString(ConfigurationManager.AppSettings["LiveMainDirectory"]);
                }

                if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
                {
                    //tbxLogin.Text = Request.Cookies["UserName"].Value;
                    //tbxPassword.Attributes["value"] = Request.Cookies["Password"].Value;
                    //cbxRememberMe.Checked = true;
                }
            }
        }

        //public string SetLoginStatus
        //{
        //    set
        //    {
        //        lblLogin.Text = value;
        //    }
        //}

        public bool SetLogIn
        {
            set
            {
                lblLogin.Visible = value;
            }
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

        protected void lbtnLogOut_Click(object sender, System.EventArgs e)
        {
            Session.Clear();
            Response.Redirect("BookMicro.aspx", false);
        }

        protected void lbtnHome_Click(object sender, System.EventArgs e)
        {
            if (Session["LoginId"] != null)
            {
                Response.Redirect("OnlineTestUser.aspx");
            }
            else
            {
                Response.Redirect("BookMicro.aspx", false);
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
                default:
                    break;
            }
        }

    }
}
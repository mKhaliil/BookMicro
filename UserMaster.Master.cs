using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Outsourcing_System.PdfCompare_Classes;

namespace Outsourcing_System.MasterPages
{
    public partial class UserMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["LoginId"])))
                {
                    ShowLogOutButton();
                }
                else
                {
                    HideLogOutButton();
                }

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

        //public bool SetLogIn
        //{
        //    set
        //    {
        //        lblLogin.Visible = value;
        //    }
        //}

        //public bool SetLogOut
        //{
        //    set
        //    {
        //        divLogout.Visible = value;
        //        aloginLink.Attributes.Add("style", "display:none");
        //    }
        //}

        //public string SetMenuLocation
        //{
        //    set
        //    {
        //        divMenu.Attributes.Add("style", "margin-left:" + value);
        //    }
        //}

        public void ShowLogOutButton()
        {
            aloginLink.Attributes.Add("style", "display:none");
            lblLogin.Attributes.Add("style", "display:none");
        }

        public void HideLogOutButton()
        {
            lbtnLogOut.Attributes.Add("style", "display:none");
        }

        public void HideLogInButton()
        {
            aloginLink.Attributes.Add("style", "display:none");
            lblLogin.Attributes.Add("style", "display:none");
        }

        protected void lbtnLogOut_Click(object sender, System.EventArgs e)
        {
            Session.Clear();
            Response.Redirect("BookMicro.aspx", true);
        }

        protected void lbtnHome_Click(object sender, System.EventArgs e)
        {
            if (Session["LoginId"] != null)
            {
                Response.Redirect("OnlineTestUser.aspx");
            }
            else
            {
                Response.Redirect("BookMicro.aspx", true);
            }
        }

        protected void lbtnHowItWorks_Click(object sender, System.EventArgs e)
        {
            //if (Session["LoginId"] != null)
            //{
            //Response.Redirect("ContactUs.aspx", true);
            //}
            //else
            //{
            Response.Redirect("HowItWorks.aspx", true);
            //}
        }

        protected void lbtnBooks_Click(object sender, System.EventArgs e)
        {
            //if (Session["LoginId"] != null)
            //{
            //Response.Redirect("ContactUs.aspx", true);
            //}
            //else
            //{
            Response.Redirect("Books.aspx", true);
            //}
        }
        protected void lbtnMessageBox_Click(object sender, System.EventArgs e)
        {
            //if (Session["LoginId"] != null)
            //{
            //Response.Redirect("ContactUs.aspx", true);
            //}
            //else
            //{
            Response.Redirect("MessageBox.aspx", true);
            //}
        }

        protected void lbtnPayments_Click(object sender, System.EventArgs e)
        {
            //if (Session["LoginId"] != null)
            //{
            //Response.Redirect("ContactUs.aspx", true);
            //}
            //else
            //{
            Response.Redirect("PaymentsAndTerms.aspx", true);
            //}
        }

        protected void lbtnAboutUs_Click(object sender, System.EventArgs e)
        {
            //if (Session["LoginId"] != null)
            //{
                Response.Redirect("AboutUs.aspx",true);
            //}
            //else
            //{
            //    Response.Redirect("BookMicro.aspx", true);
            //}
        }

        protected void lbtnContactUs_Click(object sender, System.EventArgs e)
        {
            //if (Session["LoginId"] != null)
            //{
            //Response.Redirect("ContactUs.aspx", true);
            //}
            //else
            //{
            Response.Redirect("ContactUs.aspx", true);
            //}
        }

        public void ShowMessage(MessageTypes messageType, string message)
        {
            //ucShowMessage1.ShowMessage(messageType, message);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outsourcing_System
{
    public partial class OnlineTestRegisterdMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoginId"] == null)
            {
                Response.Redirect("BookMicro.aspx", false);
            }
            if (!IsPostBack)
            {
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

    }
}
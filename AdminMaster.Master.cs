using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outsourcing_System
{
    public partial class AdminMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HideMenuLinks();

                if (!string.IsNullOrEmpty(Convert.ToString(Session["LoginId"])))
                {
                    ShowLogOutButton();
                }
                else
                {
                    HideLogOutButton();
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

        public void HideMenuLinks()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["UserRole"])))
            {
                if (Convert.ToString(Session["UserRole"]) == "7")
                {
                    lbtnNewTask.Attributes.Add("style", "display:none");
                }
                else if (Convert.ToString(Session["UserRole"]) == "5")
                {
                    lbtnApproveTest.Attributes.Add("style", "display:none");
                    lbtnManageUser.Attributes.Add("style", "display:none");
                    lbtnAccount.Attributes.Add("style", "display:none");
                }
            }
        }

        public void ShowMessageBox(string MessageText, string MessageType)
        {
            //switch (MessageType)
            //{
            //    case "Info":
            //        divInfo.Visible = true;
            //        divInfo.InnerText = MessageText;
            //        break;
            //    case "Succ":
            //        divSuccess.Visible = true;
            //        divSuccess.InnerText = MessageText;
            //        break;
            //    case "error":
            //        divError.Visible = true;
            //        divError.InnerText = MessageText;
            //        break;
            //    case "NoMessage":
            //        divError.Visible = false;
            //        divInfo.Visible = false;
            //        divSuccess.Visible = false;
            //        divError.InnerText = MessageText;
            //        break;
            //    default:
            //        break;
            //}
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("BookMicro.aspx");
        }

        protected void lbtnHome_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("AdminPanel.aspx", true);
        }

        protected void lbtnTasksStatus_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("ApproveTasks.aspx", true);
        }

        protected void lbtnLogOut_Click(object sender, System.EventArgs e)
        {
            Session.Clear();

            Response.Redirect("BookMicro.aspx", false);
        }

        protected void lbtnNewTask_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("NewTask.aspx", true);
        }

        protected void lbtnApproveTest_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("ApproveTest.aspx", true);
        }

        protected void lbtnManageUser_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("ManageUser.aspx", true);
        } 
        
        protected void lbtnManageTasks_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("ManageTasks.aspx", true);
        }

        protected void lbtnAccount_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("AccountDetail.aspx", true);
        }
    }
}
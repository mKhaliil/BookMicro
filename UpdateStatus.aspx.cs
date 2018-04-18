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
using System.IO;
using Outsourcing_System;
using BookMicroBeta;

public partial class UpdateStatus : System.Web.UI.Page
{
    MyDBClass objMyDBClass = new MyDBClass();
   protected void Page_Load(object sender, EventArgs e)
    {
        this.lblMessage.Text = "";        
        this.Title = "Outsourcing System :: Update Status";

       if (string.IsNullOrEmpty(Convert.ToString(Session["objUser"])))
           Response.Redirect("BookMicro.aspx"); 

        if ((Session["objUser"] as UserClass).UserType != "admin")
        {
            Response.Redirect("BookMicro.aspx");
        }
        if (!Page.IsPostBack)
        {
            this.ddStatus.SelectedIndex = this.ddStatus.Items.IndexOf(this.ddStatus.Items.FindByText(Request.QueryString["status"].ToString()));
        }        
    }    
    
   

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("BookMicro.aspx");
    }   
   
 
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdminPanel.aspx");
    }
    protected void btnAssign_Click(object sender, EventArgs e)
    {
        string aid = Request.QueryString["aid"].ToString();
        string task = Request.QueryString["task"].ToLower();
        string queryBID = "Select BID from Activity Where AID=" + aid;
        string bid = objMyDBClass.GetID(queryBID);
        string unKnownID = objMyDBClass.GetID("Select UID from [User] Where UserName='unknown'");
        if (Request.QueryString["status"].ToLower() == "working")
        {
            if (ddStatus.SelectedItem.ToString().ToLower() == "pending confirmation")
            {
                if (task.ToLower() == "comparison")
                {
                    objMyDBClass.CreateTask(bid, "Unassigned", "ErrorAdjustment", (Session["objUser"] as UserClass).UserName);
                }
                if (task.ToLower() == "erroradjustment")
                {
                    objMyDBClass.CreateTask(bid, "Unassigned", "Meta", (Session["objUser"] as UserClass).UserName);
                }
            }
            else
            {
                string queryUp = "";
                if (ddStatus.SelectedItem.ToString().ToLower() == "unassigned")
                {
                    queryUp = "Update Activity Set UID='" + unKnownID + "',Status='" + ddStatus.SelectedItem.ToString() + "' Where AID=" + aid;
                }
                else
                {
                    queryUp = "Update Activity Set Status='" + ddStatus.SelectedItem.ToString() + "' Where AID=" + aid;
                }
                int resUp = objMyDBClass.ExecuteCommand(queryUp);
                if (resUp > 0)
                {
                    this.lblMessage.Text = "Task successfully updated";
                }
            }
        }        
    }
}

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
using Outsourcing_System.Controls;
using BookMicroBeta;

namespace Outsourcing_System
{
    public partial class Signup : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "Outsourcing System :: Add User";
            if (Session["objUser"] == null || (Session["objUser"] as UserClass).UserType != "admin")
            {
                Response.Redirect("BookMicro.aspx");
            }
            if (!Page.IsPostBack)
            {
                ProcessControl1.LoadProcesses();
                string qCategory = "Select * From UserCategory";
                DataSet dsCategory = objMyDBClass.GetDataSet(qCategory);
                this.ddCategory.DataTextField = "Category";
                this.ddCategory.DataValueField = "CID";
            }
        }       

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            
            if (txtFullName.Text == "" || txtUserName.Text == "" || txtPassword.Text == "" || txtEmail.Text == "")
            {
                Response.Write("<script type=\"text/javascript\">alert('Please Fill Out All the Fields');</script>");
            }
            else if (txtPassword.Text != txtConfirmPassword.Text)
            {
                Response.Write("<script type=\"text/javascript\">alert('Password and Confirm Password mismatched');</script>");
            }
            else
            {
                string query = "insert into [User](UName,UType,UserName,Password,Email,CID) values('" + txtFullName.Text + "','user','" + txtUserName.Text + "','" + this.txtConfirmPassword.Text + "','" + this.txtEmail.Text + "',"+this.ddCategory.SelectedValue+")";
                int result = objMyDBClass.ExecuteCommand(query);
                if (result > 0)
                {
                    string queryLastId = "Select TOP 1 UID from [User] Order By UID Desc";
                    string uid = objMyDBClass.GetID(queryLastId);
                    string processIDs=ProcessControl1.getSelectedIValues();
                    foreach (string val in processIDs.Split(new char[] { ':' }))
                    {
                        string queryInsert = "Insert into UserCanPerform(UID,PID) Values('" + uid + "','" + val + "')";
                        int inResult = objMyDBClass.ExecuteCommand(queryInsert);
                    }
                    Response.Write("<script type=\"text/javascript\">alert('Congratulation! Account created Succssfully');</script>");
                    Server.Transfer("BookMicro.aspx");
                }
            }
        }

        

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Server.Transfer("BookMicro.aspx");
        }
    }
}

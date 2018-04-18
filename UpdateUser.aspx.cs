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
using BookMicroBeta;
using Outsourcing_System.MasterPages;

namespace Outsourcing_System
{
    public partial class UpdateUser : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "Outsourcing System :: Update User";
            if (Session["objUser"]==null || (Session["objUser"] as UserClass).UserType != "admin")
            {
                Response.Redirect("BookMicro.aspx");
            }
            else if (!Page.IsPostBack)
            {
                ((AdminMaster_Hiring)this.Page.Master).SetLogOut = true;
                ((AdminMaster_Hiring)this.Page.Master).SetMenuLocation = "-20px";
                string queryUser = "Select * from [User] Where UType='user' Order By UserName";
                DataSet dsUser = objMyDBClass.GetDataSet(queryUser);
                this.ddUser.DataSource = dsUser.Tables[0];
                this.ddUser.DataTextField = "UserName";
                this.ddUser.DataValueField = "UID";
                this.ddUser.DataBind();
                this.ddUser.SelectedIndex = 0;
                ProcessControl1.LoadProcesses();

                string qCategory = "Select * From UserCategory";
                DataSet dsCategory = objMyDBClass.GetDataSet(qCategory);
                this.ddCategory.DataTextField = "Category";
                this.ddCategory.DataValueField = "CID";
            }
        }      

       

        protected void btnUpdateUser_Click(object sender, EventArgs e)
        {
            if (txtFullName.Text == "" ||  txtPassword.Text == "" || txtEmail.Text == "")
            {
                Response.Write("<script type=\"text/javascript\">alert('Please Fill Out All the Fields');</script>");
            }
            else if (txtPassword.Text != txtConfirmPassword.Text)
            {
                Response.Write("<script type=\"text/javascript\">alert('Password and Confirm Password mismatched');</script>");
            }
            else
            {
                string query = "Update [User] set CID="+this.ddCategory.SelectedValue+", UName='"+this.txtFullName.Text+"',Password='"+this.txtConfirmPassword.Text+"',Email='"+this.txtEmail.Text+"',IsActive='"+this.ddUserStatus.SelectedItem.Value+"' Where UID="+this.ddUser.SelectedItem.Value;
                int result = objMyDBClass.ExecuteCommand(query);
                if (result > 0)
                {
                    string queryDel = "Delete from UserCanPerform Where UID=" + this.ddUser.SelectedItem.Value;
                    result = objMyDBClass.ExecuteCommand(queryDel);

                    string processIDs = ProcessControl1.getSelectedIValues();
                    foreach (string val in processIDs.Split(new char[] { ':' }))
                    {
                        string queryUserCanPerform = "Insert into [UserCanPerform](PID,UID) Values("+val+","+this.ddUser.SelectedItem.Value+")";
                        result = objMyDBClass.ExecuteCommand(queryUserCanPerform);
                    }                    
                    Response.Write("<script type=\"text/javascript\">alert('User Record Updated Succssfully');</script>");                    
                }
            }
        }

        protected void btnAdminPanel_Click(object sender, EventArgs e)
        {
            Server.Transfer("AdminPanel.aspx");
        }

        protected void ddUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtConfirmPassword.Text = "";
            this.txtPassword.Text = "";
            this.txtFullName.Text = "";
            this.txtEmail.Text = "";
            this.ddUserStatus.SelectedIndex = -1;

            string query = "Select * from [User] where UID=" + this.ddUser.SelectedItem.Value;
            DataSet ds = objMyDBClass.GetDataSet(query);
            DataRow dr = ds.Tables[0].Rows[0];
            this.txtFullName.Text = dr["UName"].ToString();
            this.txtPassword.Text = dr["Password"].ToString();
            this.txtConfirmPassword.Text = dr["Password"].ToString();
            this.txtEmail.Text = dr["Email"].ToString();
            this.ddUserStatus.SelectedValue = dr["isActive"].ToString();
            this.ddCategory.SelectedIndex = this.ddCategory.Items.IndexOf(this.ddCategory.Items.FindByValue(dr["CID"].ToString()));

            string queryProcess = "SELECT UserCanPerform.UID, Process.PID, Process.PName FROM UserCanPerform INNER JOIN ";
            queryProcess += "Process ON Process.PID = UserCanPerform.PID WHERE UserCanPerform.UID =" + this.ddUser.SelectedValue;
            DataSet dsProcess = objMyDBClass.GetDataSet(queryProcess);
            ProcessControl1.UncheckedSelectedBoxes();
            for (int i = 0; i < dsProcess.Tables[0].Rows.Count; i++)
            {
                ProcessControl1.setSelectedBoxes(dsProcess, dsProcess.Tables[0].Rows[i]["PName"].ToString(), "");
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Server.Transfer("BookMicro.aspx");
        }
    }
}

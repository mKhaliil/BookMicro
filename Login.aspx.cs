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
using Outsourcing_System;
using System.IO;
using BookMicroBeta;

public partial class Login : System.Web.UI.Page
{
    MyDBClass objMyDBClass = new MyDBClass(); //DB Object for current session

    protected void Page_Load(object sender, EventArgs e)
    {

        

        if (Session["objUser"] != null)
        {
            if (((UserClass)Session["objUser"]).UserType.Equals("admin"))
                Response.Redirect("AdminPanel.aspx");
            else
                Response.Redirect("UserPanel.aspx");
        }
        this.Title = "Login to Outsourcing System";
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string type = (rdAdmin.Checked == true) ? "admin" : "user";
        string query = "Select * from [User] where UserName='" + this.txtUserName.Text + "' AND Password='" + this.txtPassword.Text + "' AND UType='" + type + "' AND IsActive='1'";
        query = query.Replace(" or ", " and ");
        DataSet ds = objMyDBClass.GetDataSet(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            UserClass objUser = new UserClass();
            objUser.UserID = ds.Tables[0].Rows[0]["UID"].ToString();
            objUser.UserName = this.txtUserName.Text;
            objUser.UserType = ds.Tables[0].Rows[0]["UType"].ToString();
            objUser.UserFullName = ds.Tables[0].Rows[0]["UNAME"].ToString();
            objUser.UserEmail = ds.Tables[0].Rows[0]["Email"].ToString();
            Session["objUser"] = objUser;
            Session["DBObj"] = objMyDBClass;

            if (objUser.UserType == "admin")
            {
                Response.Redirect("AdminPanel.aspx");
            }
            else
            {
                string qAccount = "Select * from AccountInformation Where UID=" + objUser.UserID;
                if (objMyDBClass.GetDataSet(qAccount).Tables[0].Rows.Count > 0)
                {
                    Response.Redirect("UserPanel.aspx");
                }
                else
                {
                    Response.Redirect("AccountInformation.aspx");
                }
            }
        }
        else
        {
            //this.lblMessage.Text = "Incorrect Login Information";
        }
    }

}

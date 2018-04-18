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
using BookMicroBeta;
using Outsourcing_System.MasterPages;

public partial class ApprovedTask : System.Web.UI.Page
{
    MyDBClass objMyDBClass = new MyDBClass();
    protected void Page_Load(object sender, EventArgs e)
    {
       
        this.Title = "Outsourcing System :: Approved Tasks";
        this.lblMessage.Text = "";
        if (Session["objUser"] == null || (Session["objUser"] as UserClass).UserType != "admin")
        {
            Response.Redirect("BookMicro.aspx");
        }
        else 
        ShowDataInGridView();

        //((AdminMaster)this.Page.Master).ShowLogOutButton();

        //((AdminMaster)this.Page.Master).SetLogOut = true;
        //((AdminMaster)this.Page.Master).SetMenuLocation = "-20px";
    }
    
    public void ShowDataInGridView()
    {
        string query = "Select Book.BID,Book.BIdentityNo, Book.BTitle,Book.UploadedBy,Book.[UpDate],Activity.AID, Activity.Status,Activity.CompletionDate,Activity.Deadline,Activity.Task,[User].UName as AssignedTo from Book INNER Join Activity on Book.BID=Activity.BID INNER JOIN [User] on Activity.UID=[User].UID Where Activity.Status='Approved'  AND Activity.AssignedBy='" + (Session["objUser"] as UserClass).UserName + "' ";
        
        DataSet ds = objMyDBClass.GetDataSet(query);
        this.Repeater1.DataSource = ds.Tables[0];
        this.Repeater1.DataBind();
        if (ds.Tables[0].Rows.Count <= 0)
        {
            //this.Master.ShowMessageBox("Sorry! No Record Found", "error");
        }
    }

    protected void lnkAdminPanel_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdminPanel.aspx");
    }
    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("BookMicro.aspx");
    }
    public string DownloadLink(string bookID, string Process,string completionDate)
    {
        string ext = "";
        if (Process == "image" || Process == "table")
        {
            ext = ".zip";
        }
        else if (Process == "index")
        {
            ext = ".xls";
        }
        else if (Process == "mapping")
        {
            if (completionDate != "")
            {
                ext = ".rhyw";
            }
            else
            {
                ext = ".pdf";
            }
        }
        else if (Process == "finalizing")
        {
            ext = ".rhyw";
        }
        string link = "<a class='normaltext' href='" + Session["MainDirectory"].ToString() + "/" + bookID + "/" + Process + "/" + bookID + ext + "' >" + bookID + "</a>";
        return link;
    }
    protected void lnkAddUser_Click(object sender, EventArgs e)
    {
        Response.Redirect("Signup.aspx");
    }
   

   
}

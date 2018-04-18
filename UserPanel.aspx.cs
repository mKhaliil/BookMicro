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
using System.Collections.Generic;
using BookMicroBeta;

public partial class UserPanel : System.Web.UI.Page
{
    MyDBClass objMyDBClass = new MyDBClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = "Outsourcing System :: User Panel";
        this.lblMessage.Text = "";
        if (Session["objUser"] == null)
        {
            Response.Redirect("BookMicro.aspx");
        }

        if (!Page.IsPostBack)
        {
            this.ddTask.Items.Add("All");
            this.ddTask.Items.Add("Today");
            this.ddTask.Items.Add("Working");
            this.ddTask.Items.Add("Pending");
            this.ddTask.Items.Add("Approved");
            this.ddTask.Items.Add("Rejected");
            this.ddTask.SelectedIndex = 0;
            ShowDataInRepeator("All");
            string qBalance = "Select TotalAmount From AccountInformation Where UID=" + (Session["objUser"] as UserClass).UserID;
            string balance = objMyDBClass.GetID(qBalance);
            string htmlVal = "Available Balance :&nbsp;<b>Rs " + Math.Round(double.Parse(balance), 2) + "</b>&nbsp;&nbsp;&nbsp;<br />Click for <a href='AccountDetail.aspx'>Account Detail</a>&nbsp;&nbsp;&nbsp;";
            this.balance.InnerHtml = htmlVal;

            string qDispute = "Select * from Dispute Where Status='User' and UID=" + (Session["objUser"] as UserClass).UserID;
            if (objMyDBClass.GetDataSet(qDispute).Tables[0].Rows.Count > 0)
            {
                string dispHTML = "You have some pending amount<br />Click for <a href='AmountConfirmation.aspx'>Pending Amount</a>";
                this.dispute.InnerHtml = dispHTML;
            }
        }
        if (Session["condition"] != null)
        {
            this.ddTask.SelectedIndex = 2;
            ShowDataInRepeator(Session["condition"].ToString());
            Session["condition"] = null;
        }
    }
    public void ShowDataInRepeator(string condition)
    {
        //string query = "SELECT Activity.AID,Book.BID, Book.BIdentityNo, Book.BTitle, Activity.AID, Activity.AssignedBy, Activity.DeadLine,Activity.CompletionDate, Activity.Status, Activity.Task ";
        //query += "FROM Activity INNER JOIN Book ON Book.BID=Activity.BID  WHERE Activity.UID = " + (Session["objUser"] as UserClass).UserID;

        string query1 = "SELECT distinct Activity.AID,Activity.BID, Book.BIdentityNo, Book.BTitle,  Activity.AssignedBy, Activity.DeadLine,Activity.CompletionDate, Activity.Status,Activity.Comments, Activity.Task,Activity.Cost,Activity.Count FROM Activity INNER JOIN Book ON Book.BID=Activity.BID  WHERE Activity.UID=" + (Session["objUser"] as UserClass).UserID;
        string query2 = "SELECT distinct Activity.AID,Activity.BID, " +
        "cast(Activity.BID as varchar(100))" +
            //"Activity.BID " +
        " as BIdentityNo, Book.BTitle,  Activity.AssignedBy, Activity.DeadLine,Activity.CompletionDate, Activity.Status,Activity.Comments, Activity.Task,Activity.Cost,Activity.Count FROM Activity INNER JOIN Book ON Book.MainBook=cast(Activity.BID as varchar(100))  WHERE Activity.UID=" + (Session["objUser"] as UserClass).UserID;

        string whereCondition = "";
        if (condition == "All")
        {
            whereCondition += "";
        }
        else if (condition == "Today")
        {
            whereCondition += " AND Activity.DeadLine='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "'";
        }
        else if (condition == "Pending")
        {
            whereCondition += " AND Activity.Status='Pending Confirmation'";
        }
        else if (condition == "Assigned")
        {
            whereCondition += " AND Activity.Status='Assigned'";
        }
        else if (condition == "Working")
        {
            whereCondition += " AND Activity.Status='Working'";
        }
        else if (condition == "Rejected")
        {
            whereCondition += " AND Activity.Status='Rejected'";
        }
        else if (condition == "Approved")
        {
            whereCondition += " AND Activity.Status='Approved'";
        }
        string query = query1 + whereCondition + " union " + query2 + whereCondition + " order by status desc";
        DataSet ds = objMyDBClass.GetDataSet(query);
        this.Repeater1.DataSource = ds.Tables[0];
        this.Repeater1.DataBind();
        if (ds.Tables[0].Rows.Count <= 0)
        {
            ((AdminMaster)this.Master).ShowMessageBox("Sorry! No Record Found", "Error");
        }
    }
    public string GetNullValue(string count)
    {
        string retVal = count;
        if (int.Parse(count) == 0)
        {
            retVal = "N/A";
        }
        return retVal;
    }
    public string DownloadLink(string bookID, string Process, string status)
    {
        string link = "<a class='bbw' href='" + Session["MainDirectory"].ToString() + "\\" + bookID.Split(new char[] { '-' })[0] + "\\" + bookID + "\\";
        string firstLink = "";
        string secondLink = "";
        if (status.ToLower() == "pending confirmation" || status.ToLower() == "approved" || status.ToLower() == "rejected")
        {
            if (Process.ToLower() == "image" || Process.ToLower() == "table")
            {
                secondLink = link + Process + "\\" + bookID + ".zip'>ZIP</a>";
            }
            else if (Process.ToLower() == "index")
            {
                secondLink = link + Process + "\\" + bookID + ".xls'>XLS</a>";
            }
            else if (Process.ToLower() == "tagginguntagged")
            {
                secondLink = link + Process + "\\" + bookID + ".rhyw'>RHYW</a>";
            }
        }
        if (status.ToLower() != "pending confirmation" && status.ToLower() != "approved" && status.ToLower() != "rejected")
        {
            if (Process.ToLower() == "image" || Process.ToLower() == "table" || Process.ToLower() == "tagginguntagged")
            {
                firstLink = link + "TaggingUntagged\\" + bookID + ".pdf'>PDF</a>";
            }
            else if (Process.ToLower() == "index")
            {
                firstLink = link + "Index\\" + bookID + ".pdf'>PDF</a>";
            }
        }

        if (Process.ToLower() == "comparison" || Process.ToLower() == "erroradjustment")
        {
            firstLink = "<a class='bbw' href='Files/" + bookID + "/" + bookID + ".pdf'>PDF</a>";
        }
        if (Process.ToLower() == "erroradjustment" || Process.ToLower() == "meta")
        {
            secondLink += "<a class='bbw' href='" + Session["MainDirectory"].ToString() + "/" + bookID + "/" + bookID + ".rhyw' >RHYW</a>";
        }
        else if (Process.ToLower() == "table")
        {
            link += "&nbsp;<a class='bbw' href='" + Session["MainDirectory"].ToString() + "/" + bookID.Split(new char[] { '-' })[0] + "/Tables.zip'>Zip</a>";
        }
        //string extraDir = Server.MapPath("~/" + objMyDBClass.MainDirectory + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID + "/Extra");
        string extraDir = objMyDBClass.MainDirPhyPath + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID + "/Extra";

        if (Directory.Exists(extraDir))
        {
            string[] files = Directory.GetFiles(extraDir);
            foreach (string file in files)
            {
                secondLink += ",<a class='bbw' href='" + Session["MainDirectory"].ToString() + "/" + bookID.Split(new char[] { '-' })[0] + "\\" + bookID + "\\extra\\" + Path.GetFileName(file) + "'>" + Path.GetFileName(file) + "</a>";
            }
        }
        //if(Process.ToLower() == "image" && File.Exists(Server.MapPath("~/" + objMyDBClass.MainDirectory + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID + "/Image/images.txt")))
        if (Process.ToLower() == "image" && File.Exists(objMyDBClass.MainDirPhyPath + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID + "/Image/images.txt"))
        {
            secondLink += ",<a class='bbw' href='" + Session["MainDirectory"].ToString() + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID + "/Image/images.txt'>Images List</a>";
        }
        link = firstLink + " " + secondLink;
        return link.Trim();
    }

    public string SubmitTaskLink(string AID, string BIDNo, string status, string task)
    {
        string link = "";
        if (status.ToLower() == "approved" || status.ToLower() == "pending confirmation")
        {
            //link = "<a class=\"link\" href='SubmitTask.aspx?aid=" + AID + "&bid=" + BIDNo + "'>Submit Task</a>";
        }
        else if (task.ToLower() == "meta")
        {
            link = "<a class=\"link\" href='MetaInformation.aspx?bid=" + BIDNo + "'>Submit Task</a>";
        }
        else if (task == "Comparison")
        {

            link = "<a class=\"link\" href='ComparisonTask.aspx?bid=" + BIDNo + "'>Submit Task</a>";
            //link = "<a class=\"link\" href='BookPreview.aspx?bid=" + BIDNo + "'>Submit Task</a>";
        }
        else if (task == "ErrorAdjustment")
        {
            link = "<a class=\"link\" href='ErrorAdjustment.aspx?bid=" + BIDNo + "'>Submit Task</a>";
        }
        else if (task == "TaggingUntagged")
        {
            link = "<a class=\"link\" href='TagUntag.aspx?bid=" + BIDNo + "'>Submit Task</a>";
        }
        else if (task == "SpellCheck")
        {
            BIDNo = BIDNo.Split('-')[0];
            link = "<a class=\"link\" href='SpellChecker.aspx?aid=" + AID + "&bid=" + BIDNo + "'>Submit Task</a>";
        }
        else if (task == "MistakeInjection")
        {
            BIDNo = BIDNo.Split('-')[0];
            link = "<a class=\"link\" href='MistakesInsertion.aspx?aid=" + AID + "&bid=" + BIDNo + "'>Submit Task</a>";
        }
        else
        {
            link = "<a class=\"link\" href='SubmitTask.aspx?aid=" + AID + "&bid=" + BIDNo + "'>Submit Task</a>";
        }
        return link;
    }


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("BookMicro.aspx");
    }
    protected void ddTask_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowDataInRepeator(this.ddTask.SelectedItem.Text);
    }

    protected void lnkInbox_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/MessageBoard.aspx?act=inbox");
    }

    protected void lnkComposeMail_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Mail.aspx?act=mail");
    }

    protected void lnkOutbox_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/MessageBoard.aspx?act=outbox");
    }

    protected void lnkSentMail_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/MessageBoard.aspx?act=sent");
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("BookMicro.aspx");
    }

    public string ToolTip(string status, string comments)
    {
        string retText = status;
        if (status.ToLower() == "rejected")
        {
            retText = "<span title='" + comments + "'>" + status + "</span";
        }
        return retText;
    }
}

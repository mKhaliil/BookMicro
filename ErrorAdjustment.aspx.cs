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
using System.IO;
using System.Text.RegularExpressions;
using BookMicroBeta;
namespace Outsourcing_System
{
    
    public partial class ErrorAdjustment : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblMessage.Text = "";
            this.Title = "Outsourcing System :: Upload New Task";
            if (Session["objUser"] == null)
            {
                Response.Redirect("BookMicro.aspx");
            }
            if(Request.QueryString["bid"]!=null)
            {
                try
                {
                    string issueFileName = Server.MapPath("~/Files/" + Request.QueryString["bid"].ToString() + "/IssueLog.txt");
                    string contents = "<table width='100%'><tr style='background-color:#507CD1; color:White;'><th align='left' style='padding-left:5pt;'>File ID</th><th align='left' style='padding-left:5pt;'>Page No</th><th align='left' style='padding-left:5pt;'>Error Description</th></tr>";
                    StreamReader tr = new StreamReader(issueFileName);
                    string singleLine = tr.ReadLine();
                    while (singleLine != "")
                    {
                        string[] text = singleLine.Split(new char[] { '}' });
                        contents += "<tr style='background-color:#003366;'><td align='left' class='bbw' style='padding-left:5pt;'>" + text[0].Replace("{", "") + "</td><td align='left' class='bbw' style='padding-left:5pt;'>" + text[2].Replace("{", "") + "</td><td align='left' class='bbw' style='padding-left:5pt;'>" + text[3].Replace("{", "") + "</td></tr>";
                        singleLine = tr.ReadLine();
                    }
                    contents += "</table>";
                    this.errors.InnerHtml = contents;
                }
                catch
                {
                    this.lblMessage.Text = "Issue Log file not found";
                }
            }
            else
            {
                this.lblMessage.Text="Book ID not found";
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            if (File1.PostedFile.FileName != "")
            {
                try
                {
                    string bookID = Request.QueryString["bid"].ToString();
                    //string filName = Server.MapPath("~/Files/" + bookID + "/" + bookID + ".rhyw");
                    string filName = objMyDBClass.MainDirPhyPath + "\\" + bookID + "\\" + bookID + ".rhyw";
                    if (File.Exists(filName))
                    {
                        File.Delete(filName);
                    }
                    File1.PostedFile.SaveAs(filName);

                    string queryUpdate = "Update ACTIVITY Set Status='Pending Confirmation', CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "' Where UID=" + (Session["objUser"] as UserClass).UserID + " AND BID=" + bookID + " AND Task='ErrorAdjustment'";
                    int upRes = objMyDBClass.ExecuteCommand(queryUpdate);
                    if (upRes > 0)
                    {
                        Response.Redirect("AdminPanel.aspx", false);
                    }                    
                }
                catch(Exception ex)
                {
                    this.lblMessage.Text = ex.Message;
                }
            }
        } 
    }
}

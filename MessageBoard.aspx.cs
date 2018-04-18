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

namespace Outsourcing_System
{
    public partial class MessageBoard : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "Outsourcing System :: Message Board";
        if (Session["objUser"]  == null)
        {
            Response.Redirect("BookMicro.aspx");
        }
        else
        {
            string action = Request.QueryString["act"];
            string user = (Session["objUser"] as UserClass).UserName.ToString();
            SqlDataSource1.ConnectionString =objMyDBClass.ConnectionString();
            SqlDataSource1.ProviderName = "System.Data.SqlClient";               

            if (action == "inbox" || action == "")
            {
                string query = "SELECT MailID, Subject, Message, Dat, Sender, Reciever, Stat, DelFrom, DelTo FROM Mail WHERE Reciever='" + user + "' AND DelTo='Null' order by MailID Desc";
                SqlDataSource1.SelectCommand = query;
                this.GridView1.DataSourceID = SqlDataSource1.ID;
               
            }
            else if (action == "outbox")
            {
                string query = "SELECT MailID, Subject, Message, Dat, Sender, Reciever, Stat, DelFrom, DelTo FROM Mail WHERE Sender = '" + user + "' AND Stat='No' order by MailID Desc";
                SqlDataSource1.SelectCommand = query;
                this.GridView1.DataSourceID = SqlDataSource1.ID;
            }
            else if (action == "sent")
            {
                string query = "SELECT MailID, Subject, Message, Dat, Sender, Reciever, Stat, DelFrom, DelTo FROM Mail WHERE Sender = '" + user + "' AND Stat='Yes' AND DelFrom='Null' order by MailID Desc";
                SqlDataSource1.SelectCommand = query;
                this.GridView1.DataSourceID = SqlDataSource1.ID;
            }
            else if (action == "inbdel" || action == "sentdel")
            {
                string query = "UPDATE MAIL set ";
                if (action == "inbdel")
                {
                    query += "DelTo='" + user + "' where MailID=" + Request.QueryString["mid"].ToString();
                }
                else if (action == "sentdel")
                {
                    query += "DelFrom='" + user + "' where MailID=" + Request.QueryString["mid"].ToString();
                }
                int res = objMyDBClass.ExecuteCommand(query);
                if (res > 0)
                {
                    Response.Write("<script language='javascript'>\nalert('Message deleted successfully');\n</script>\n");
                    Response.Redirect("~/MessageBoard.aspx?act=inbox");
                }
            }            
        }
        
    }
   
    
    
    
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        SqlDataSource1.ConnectionString = objMyDBClass.ConnectionString();
        SqlDataSource1.ProviderName = "System.Data.SqlClient";            

        string id = this.GridView1.SelectedDataKey.Value.ToString();
        string action = Request.QueryString["act"].ToString();
        string query = "SELECT MailID,Sender,Message from Mail where MailID=" + id;
        DataSet ds = objMyDBClass.GetDataSet(query);
        string table = "<Table width=\"100%\">";
        table += "<Tr><Td class=\"bbw\" align=\"left\">" + ds.Tables[0].Rows[0]["Message"].ToString() + "</Td></Tr>";
        table += "<br />";
        if (action == "inbox")
        {
            table += "<Tr><Td align=\"center\" class=\"bbw\"><a href=\"MessageBoard.aspx?act=inbdel&mid=" + ds.Tables[0].Rows[0]["MailID"].ToString() + "\" class=\"link\"><b>Delete</b></a>&nbsp;&nbsp;<a href=\"Mail.aspx?act=rep&to=" + ds.Tables[0].Rows[0]["Sender"].ToString() + "\" class=\"link\"><b>Reply</b></a></Td></Tr>";
        }
        else if (action == "sent")
        {
            table += "<Tr><Td align=\"center\" class=\"bbw\"><a href=\"MessageBoard.aspx?act=sentdel&mid=" + ds.Tables[0].Rows[0]["MailID"].ToString() + "\" class=\"link\"><b>Delete</b></a></Td></Tr>";
        }
        table += "</Table>";
        this.message.InnerHtml = table;
        objMyDBClass.UpdateMailStatus(id, (Session["objUser"] as UserClass).UserName.ToString());
    }
    public string CheckImage(string statg)
    {
        string url = "";
        if (statg == "No")
        {
            url = "scripts/unread.gif";
        }
        else
        {
            url = "scripts/read.gif";
        }
        return url;
    }

        protected void lnkUserlPanel_Click(object sender, EventArgs e)
        {
            if ((Session["objUser"] as UserClass).UserType == "user")
            {
                Response.Redirect("~/UserPanel.aspx");
            }
            else if ((Session["objUser"] as UserClass).UserType == "admin")
            {
                Response.Redirect("~/AdminPanel.aspx");
            }
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
    
    }
}

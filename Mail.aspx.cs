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
    public partial class Mail : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            this.Title = "Outsourcing System :: Compose Mail";
            if (Session["objUser"] == null)
            {
                Response.Redirect("BookMicro.aspx");
            }
            else
            {
                string action = Request.QueryString["act"].ToString();                
                if (action == "rep")
                {
                    this.To.Items.Clear();
                    this.To.Items.Add(Request.QueryString["to"].ToString());
                    this.To.SelectedIndex = 0;
                }
                else if (action == "mail")
                {
                    if (!Page.IsPostBack)
                    {
                        string query = "Select UserName from [User] where UserName <> '" + (Session["objUser"] as UserClass).UserName.ToString() + "' AND UType<>'" + (Session["objUser"] as UserClass).UserType.ToString() + "' AND UType<>'unknown' Order By UserName";
                        DataSet ds = objMyDBClass.GetDataSet(query);
                        this.To.DataSource = ds.Tables[0];
                        this.To.DataTextField = "UserName";
                        this.To.DataValueField = "UserName";
                        this.To.DataBind();
                    }
                }

            }
        }
        protected void Send_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string date = dt.Year + "-" + dt.Month + "-" + dt.Day;

                string query = "insert into mail(Subject,Message,Dat,Sender,Reciever,Stat,DelFrom,DelTo) values('" + this.txtSubject.Text + "','" + this.txtMessage.Text + "','" + date + "','" + (Session["objUser"] as UserClass).UserName.ToString() + "','" + this.To.SelectedValue.ToString() + "','No','Null','Null')";
                query = query.Replace("\r\n", "<br />");
                int res = objMyDBClass.ExecuteCommand(query);
                if (res > 0)
                {
                   Response.Redirect("~/MessageBoard.aspx?act=inbox");
                }
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = ex.Message;
            }
        }

        protected void lnkUserlPanel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UserPanel.aspx");
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

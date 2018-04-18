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

public partial class AssignTask : System.Web.UI.Page
{
    MyDBClass objMyDBClass = new MyDBClass();
   protected void Page_Load(object sender, EventArgs e)
    {
        this.lblMessage.Text = "";        
        this.Title = "Outsourcing System :: Assign Task";
        if (Session["objUser"] == null || (Session["objUser"] as UserClass).UserType != "admin")
        {
            Response.Redirect("BookMicro.aspx");
        }
        
        if (!Page.IsPostBack)
        {
            ProcessControl1.LoadProcesses();
            string bookID = Request.QueryString["bid"] != null ? Request.QueryString["bid"].ToString() : "";
            string pName = Request.QueryString["pname"] != null ? Request.QueryString["pname"].ToString() : "";
            string queryActivity = "Select Task,Status from Activity where BID=" + bookID;
            DataSet dsActivity = objMyDBClass.GetDataSet(queryActivity);
            ProcessControl1.setSelectedBoxes(dsActivity, pName,"AssignTask");
        }        
    }    
    
  

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("BookMicro.aspx");
    }
    protected void btnAssign_Click(object sender, EventArgs e)
    {        
        string bookID = Request.QueryString["bid"] != null ? Request.QueryString["bid"].ToString() : "";
        int count = 0;
        string processes = ProcessControl1.getSelectedItems();
        string process = this.lblStatus.Text.Trim().Split(new char[] { ' ' })[4];
        foreach (string item in processes.Split(new char[] { ':' }))
        {
            foreach (ListItem UID in lstUser.Items)
            {
                if (UID.Selected == true && item.ToUpper() == process.ToUpper())
                {
                    //Mail
                    objMyDBClass.SendMail(item.ToUpper(), UID.Text, (Session["objUser"] as UserClass));
                    //End Mail
                    string queryInsert = "Update ACTIVITY Set Cost=" + this.lblRate.Text.Split(new char[] { ' ' })[3] + ",UID=" + UID.Value + ",AssignedBy='" + (Session["objUser"] as UserClass).UserName + "',AssigmentDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "',DeadLine='" + this.Calendar1.Value + "',Status='Working',Comments='" + this.txtComments.Text + "' Where AID=" + Request.QueryString["aid"].ToString();
                    int inResult = objMyDBClass.ExecuteCommand(queryInsert);
                    /**********************/
                    //string qTaskPayment = "Select C.[" + process + "] From UserCategory C inner join [User] U on C.CID=U.CID Where U.UID=" + UID.Value;
                    //string taskPayment = objMyDBClass.GetID(qTaskPayment);

                    //string earningQuery = "Insert into Earnings(UID,AID,ActualEarning,PayableEarning,Bonus,Paid) Values(" + UID.Value + "," + Request.QueryString["aid"].ToString() + "," + taskPayment + "," + taskPayment + ",0.00,'N')";
                    //objMyDBClass.ExecuteCommand(earningQuery);
                    /**********************/                    
                    count=1;
                    break;
                }
            }
            if (count == 1)
            {
                break;
            }
        }
        if (count > 0)
        {
            if (this.File1.PostedFile!=null)
            {
                string ext = Path.GetExtension(this.File1.PostedFile.FileName);
                string querySelBook = "Select BIdentityNo from BOOK Where BID=" + bookID;
                string bIdentityNo = objMyDBClass.GetID(querySelBook);
                string dirPath = Server.MapPath("~/" + Session["MainDirectory"].ToString() + "\\" + bIdentityNo + "\\extra\\" + bIdentityNo + DateTime.Now.Minute + ext);
                File1.PostedFile.SaveAs(dirPath);
            }
        }           
        //***********************************//
        this.lstUser.DataSource = null;
        this.lblStatus.Text = "";
        if (Session["ProcessIndex"] != null)
        {
            Session["ProcessIndex"] = int.Parse(Session["ProcessIndex"].ToString()) + 1;
        }
        btnShowUser_Click(null, null);
    }
    protected void btnShowUser_Click(object sender, EventArgs e)
    {
        this.Panel1.Visible = true;
        this.btnShowUser.Visible = false;
        this.instruction.Visible = false;

        Session["ProcessIndex"]= Session["ProcessIndex"] == null ? "0" : Session["ProcessIndex"].ToString();
        string[] processesValues = ProcessControl1.getSelectedIValues().Split(new char[]{':'});
        string[] processesNames = ProcessControl1.getSelectedItems().Split(new char[]{':'});
        this.lblRate.Text = "";
        if (int.Parse(Session["ProcessIndex"].ToString()) == processesValues.Length - 1)
        {
            this.btnAssign.Text = "Finish";
        }
        else
        {            
            this.btnAssign.Text = "Next";
        }
        if (int.Parse(Session["ProcessIndex"].ToString()) == processesValues.Length)
        {
            Session.Remove("ProcessIndex");
            Session["condition"] = "Unassigned";
            Response.Redirect("AdminPanel.aspx");       
        }
        else
        {
            this.lblStatus.Text = "User available for process " + processesNames[int.Parse(Session["ProcessIndex"].ToString())].ToUpper();
            this.lstUser.Visible = true;
            string pid = processesValues[int.Parse(Session["ProcessIndex"].ToString())];
            string queryUser = "Select * from [User] Inner Join UserCanPerform on  [User].UID=UserCanPerform.UID Where [User].UType='user' AND [User].IsActive='1' AND  UserCanPerform.PID = " + pid + " Order By UserName";
            DataSet dsUser = objMyDBClass.GetDataSet(queryUser);
            if (dsUser.Tables[0].Rows.Count > 0)
            {
                this.lstUser.DataSource = dsUser.Tables[0];
                this.lstUser.DataTextField = "UserName";
                this.lstUser.DataValueField = "UID";
                this.lstUser.DataBind();
            }
            else 
            {
                this.lstUser.Visible = false;
                this.lblStatus.Text = "Sorry! No User Exist for the Process '" + processesNames[int.Parse(Session["ProcessIndex"].ToString())]+"'";
            }
            this.UpdatePanel1.Update();
        }        
    }
    protected void lstUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        string task = this.lblStatus.Text.Split(new char[] { ' ' })[4];
        string uQuery = "SELECT UserCategory.[" + task + "] FROM [User] INNER JOIN UserCategory ON [User].CID = UserCategory.CID WHERE [User].UID=" + lstUser.SelectedValue;
        string rate = objMyDBClass.GetID(uQuery);
        string qCount = "Select [Count] From Activity Where AID=" + Request.QueryString["aid"].ToString();
        double count = double.Parse(objMyDBClass.GetID(qCount));
        count = (count == 0 ? 1 : count);
        this.lblRate.Text = "Estimated Amount : " + rate + " x " + count + "=" + (double.Parse(rate) * count) + " <sup><font color='red'>*</font></sup>";
        this.instruction.Visible = true;
        this.UpdatePanel1.Update();
    }
}

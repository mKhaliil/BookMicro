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

public partial class LogStatus : System.Web.UI.Page
{
    MyDBClass objMyDBClass = new MyDBClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = "Outsourcing System :: Log Status";
        this.lblMessage.Text = "";
        if (Session["objUser"] == null)
        {
            Response.Redirect("BookMicro.aspx");
        }
        if (Request.QueryString["logid"] != null)
        {
            string queryDel = "Delete From LogBook where LogID=" + Request.QueryString["logid"].ToString();
            objMyDBClass.ExecuteCommand(queryDel);
        }
        if (Request.QueryString["act"] != null)
        {
            if (Request.QueryString["act"].ToString() == "view")
            {
                ShowDataInGridView(Request.QueryString["book"].ToString());
            }
        }
        //else if (!Page.IsPostBack)
        //{
        //    ShowDataInGridView("");
        //}       
    }
    
    public void ShowDataInGridView(string BookID)
    {
        string query = "Select LogID,MainBook,Process,Detail,Status,StartTime from LogBook ";
        if (BookID.Trim() != "")
        {
            query += "Where MainBook='" + BookID + "'";
        }
        DataSet ds = objMyDBClass.GetDataSet(query);
        this.GridView1.DataSource = ds.Tables[0];
        this.GridView1.DataBind();
        if (ds.Tables[0].Rows.Count <= 0)
        {
            this.Master.ShowMessageBox("Sorry! No Record Found","Error");
        }
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
       ShowDataInGridView(txtBookID.Text.Trim());
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;         
    }
   
}

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
using BookMicroBeta;

namespace Outsourcing_System
{
    public partial class UserTransactions : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PendingTransaction();
            }
        }

        protected void lnkPendingTransactions_Click(object sender, EventArgs e)
        {
            PendingTransaction();
        }
        public void PendingTransaction()
        {
            //this.GridView1.Columns[0].Visible = false;
            string queryPending = "Select T.WID,U.UID,U.UserName,T.WithdrawAmount,T.TransactionDate From Transactions T Inner Join [User] U on T.UID=U.UID Where T.Status='Pending' and T.UID="+(Session["objUser"] as UserClass).UserID;
            SqlDataSource1.SelectCommand = queryPending;
            GridView1.DataSourceID = SqlDataSource1.ID;
        }

        protected void lnkApprovedTransactions_Click(object sender, EventArgs e)
        {
            //this.GridView1.Columns[0].Visible = false;
            string queryPending = "Select T.WID,U.UID,U.UserName,T.WithdrawAmount,T.TransactionDate From Transactions T Inner Join [User] U on T.UID=U.UID Where T.Status='Approved' and T.UID=" + (Session["objUser"] as UserClass).UserID;
            SqlDataSource1.SelectCommand = queryPending;
            GridView1.DataSourceID = SqlDataSource1.ID;
        }
        protected void lnkRejectedTransactions_Click(object sender, EventArgs e)
        {
            //this.GridView1.Columns[0].Visible = true;
            string queryPending = "Select T.WID,U.UID,U.UserName,T.WithdrawAmount,T.TransactionDate From Transactions T Inner Join [User] U on T.UID=U.UID Where T.Status='Rejected' and T.UID=" + (Session["objUser"] as UserClass).UserID;
            SqlDataSource1.SelectCommand = queryPending;
            GridView1.DataSourceID = SqlDataSource1.ID;
        }
        
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string wid=GridView1.DataKeys[GridView1.SelectedIndex].Values["WID"].ToString();
            //string uid = GridView1.DataKeys[GridView1.SelectedIndex].Values["UID"].ToString();

            //string queyApprove = "Update Transactions Set Status='Approved' Where WID=" + wid;
            //MyDBClass.ExecuteCommand(queyApprove);

            //string queryAccount = "Select UnPaidAmount,TotalAmount From AccountInformation Where UID=" + uid;
            //DataSet dsAccount = MyDBClass.GetDataSet(queryAccount);
            //DataRow dr = dsAccount.Tables[0].Rows[0];
            //string unPaidAmount = dr["UnPaidAmount"].ToString();
            //string totalAmount = dr["TotalAmount"].ToString();
            //string transactAmount = GridView1.SelectedRow.Cells[2].Text;
            //unPaidAmount = (double.Parse(unPaidAmount) - double.Parse(transactAmount)).ToString();
            //string queryUpdateAccount = "Update AccountInformation Set UnPaidAmount=" + unPaidAmount + ", TotalAmount=" + (double.Parse(totalAmount) - double.Parse(transactAmount)) + " Where UID=" + uid;
            //MyDBClass.ExecuteCommand(queryUpdateAccount);

            //Response.Redirect("Transactions.aspx?type=app", false);
        }
    }
}

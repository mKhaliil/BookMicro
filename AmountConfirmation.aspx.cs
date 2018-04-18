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
    public partial class AmountConfirmation : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["objUser"] == null)
            {
                Response.Redirect("BookMicro.aspx");
            }
            if (Request.QueryString["did"] != null)
            {
                ShowCommentsSection();
            }
            AmountConfirmations();
        }
        /// <summary>
        /// Shows the panel to get user comments
        /// </summary>
        public void ShowCommentsSection()
        {
           // this.pnlAmount.Visible = true;
        }
        public void AmountConfirmations()
        {
            //string queryPending = "Select D.DID,D.AID,U.UID,U.UserName,D.AcAmount,D.PpAmount, D.Bonus,D.DDate From Dispute D Inner Join [User] U on D.UID=U.UID Where D.Status='User' and U.UID=" + (Session["objUser"] as UserClass).UserID;
            string queryPending = "Select D.DID,D.AID,U.UID,U.UserName,D.AcAmount,D.PpAmount, dbo.Activity.Task, dbo.Book.BIdentityNo, D.Bonus,D.DDate From Dispute D Inner Join [User] U on D.UID=U.UID INNER JOIN dbo.Activity ON D.AID = dbo.Activity.AID INNER JOIN dbo.Book ON dbo.Activity.BID = dbo.Book.BID Where D.Status='User' and U.UID=" + (Session["objUser"] as UserClass).UserID; //Shoaib here
            SqlDataSource1.SelectCommand = queryPending;
            GridView1.DataSourceID = SqlDataSource1.ID;
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string did = GridView1.DataKeys[GridView1.SelectedIndex].Values["DID"].ToString();
            string aid = GridView1.DataKeys[GridView1.SelectedIndex].Values["AID"].ToString();

            string queryPayableEarning = "Select E.UID,A.TotalAmount,E.Task From Activity E Inner Join AccountInformation A on E.UID=A.UID where E.AID=" + aid;
            DataSet dsPayableEarning = objMyDBClass.GetDataSet(queryPayableEarning);
            DataRow dr = dsPayableEarning.Tables[0].Rows[0];
            string userID = dr["UID"].ToString();
            string task = dr["Task"].ToString();

            string prevAmount = dr["TotalAmount"].ToString() == "" ? "0" : dr["TotalAmount"].ToString();
            //string totalAmount = (double.Parse(prevAmount) + double.Parse(this.GridView1.SelectedRow.Cells[3].Text.Trim()) + double.Parse(this.GridView1.SelectedRow.Cells[4].Text.Trim())).ToString();
            string totalAmount = (double.Parse(prevAmount) + double.Parse(this.GridView1.SelectedRow.Cells[4].Text.Trim())).ToString(); //Shoaib here, fixing the bugg of double amount addition

            //Acount Detail
            string qAccountDetail = "Insert into AccountDetail(UID,Deposit,Withdraw,Balance,Description,[Date]) Values(" + userID + "," + (double.Parse(this.GridView1.SelectedRow.Cells[3].Text.Trim()) + double.Parse(this.GridView1.SelectedRow.Cells[4].Text.Trim())) + ",0.00," + totalAmount + ",'Amount is confirmed by User. Amount against task " + task.ToUpper() + " is deposited'," + DateTime.Now.Date.GetDateTimeFormats('d')[5] + ")";
            objMyDBClass.ExecuteCommand(qAccountDetail);
            //End Account Detail 

            string queryUpdateAccount = "Update AccountInformation Set TotalAmount=" + totalAmount + " Where UID=" + userID;
            objMyDBClass.ExecuteCommand(queryUpdateAccount);

            string qUpdate = "Update Dispute set Status='Close' Where DID=" + did;
            objMyDBClass.ExecuteCommand(qUpdate);

            Response.Redirect("AmountConfirmation.aspx", false);
        }

        protected void btnFileDispute_Click(object sender, EventArgs e)
        {
            string qUpdate = "Update Dispute set Status='Pending' Where DID=" + Request.QueryString["did"].ToString();
            long did = long.Parse(Request.QueryString["did"].ToString());//Shoaib here
            long userID = long.Parse(((UserClass)Session["objUser"]).UserID);//Shoaib here
            string comments = "";//this.txtRemarks.Text.Trim();
            objMyDBClass.InsertDisputeHistory(did, userID, comments);
            objMyDBClass.ExecuteCommand(qUpdate);
        }
    }
}

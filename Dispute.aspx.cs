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
    public partial class Dispute : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["objUser"] == null || (Session["objUser"] as UserClass).UserType != "admin")
            {
                Response.Redirect("BookMicro.aspx");
            }
            if (!Page.IsPostBack)
            {
                Disputes();
            }
        }
       
        public void Disputes()
        {
            //this.GridView1.Columns[0].Visible = true;
            string queryPending = "Select D.DID,D.AID,U.UID,U.UserName,D.AcAmount,D.PpAmount,D.DDate From Dispute D Inner Join [User] U on D.UID=U.UID Where D.Status='Pending'";
            SqlDataSource1.SelectCommand = queryPending;
            GridView1.DataSourceID = SqlDataSource1.ID;
        }

        

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string did = GridView1.DataKeys[GridView1.SelectedIndex].Values["DID"].ToString();
            string qRemarks = "Select Remarks from Dispute where did=" + did;
            string remarks = objMyDBClass.GetID(qRemarks);
            this.txtRemarks.Text = remarks;
            this.pnlDetail.Visible = true;
            this.uplDetail.Update();
        }


        protected void btnResolve_Click(object sender, EventArgs e)
        {
            string did = GridView1.DataKeys[GridView1.SelectedIndex].Values["DID"].ToString();
            string aid = GridView1.DataKeys[GridView1.SelectedIndex].Values["AID"].ToString();

            string qDispute = "Update Dispute Set PpAmount="+this.txtAmount.Text.Trim()+",Remarks='"+this.txtRemarks.Text.Trim()+"',Status='Close',Winner='"+this.ddWinner.SelectedValue+"' Where DID="+did;
            int res=objMyDBClass.ExecuteCommand(qDispute);
            if (res > 0)
            {

                string queryPayableEarning = "Select E.UID,A.TotalAmount,E.Task From Earnings E Inner Join AccountInformation A on E.UID=A.UID where E.AID=" + aid;
                DataSet dsPayableEarning = objMyDBClass.GetDataSet(queryPayableEarning);
                DataRow dr = dsPayableEarning.Tables[0].Rows[0];
                string userID = dr["UID"].ToString();
                string task = dr["Task"].ToString();

                string totalAmount = dr["TotalAmount"].ToString() == "" ? "0" : dr["TotalAmount"].ToString();
                totalAmount = (double.Parse(totalAmount) + double.Parse(this.txtAmount.Text.Trim())).ToString();

                //Acount Detail
                string qAccountDetail = "Insert into AccountDetail(UID,Deposit,Withdraw,Balance,Description,[Date]) Values(" + userID + "," + this.txtAmount.Text.Trim() + ",0.00," + totalAmount + ",'Dispute is compromized. Amount against task " + task.ToUpper() + " is deposited'," + DateTime.Now.Date.GetDateTimeFormats('d')[5] + ")";
                objMyDBClass.ExecuteCommand(qAccountDetail);
                //End Account Detail    

                string queryUpdateAccount = "Update AccountInformation Set TotalAmount=" + totalAmount + " Where UID=" + userID;
                objMyDBClass.ExecuteCommand(queryUpdateAccount);

                string upRemarks = "Update Earnings Set Remarks='" + txtRemarks.Text.Trim() + "' Where AID=" + aid;
                objMyDBClass.ExecuteCommand(upRemarks);

                //DateTime dt = DateTime.Now;
                //string date = dt.Year + "-" + dt.Month + "-" + dt.Day;
                //string queryMail = "insert into mail(Subject,Message,Dat,Reciever,Sender,Stat,DelFrom,DelTo) values('Disputed Amount Resolved','Dear User<br />Yo','" + date + "','" + toUser + "','" + fromUser.UserID + "','No','Null','Null')";
                //queryMail = queryMail.Replace("\r\n", "<br />");
                //int res = ExecuteCommand(queryMail);

            }

            this.txtRemarks.Text = "";
            this.txtAmount.Text = "";

            this.pnlDetail.Visible = false;
            this.uplDetail.Update();
            Response.Redirect("Dispute.aspx",false);
        }
  }
}

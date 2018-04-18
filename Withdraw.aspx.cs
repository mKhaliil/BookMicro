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
    public partial class Withdraw : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            string queryAmount = "Select TotalAmount,UnPaidAmount from AccountInformation where UID=" + (Session["objUser"] as UserClass).UserID;
            DataSet dsAmount = objMyDBClass.GetDataSet(queryAmount);
            if (dsAmount.Tables[0].Rows.Count > 0)
            {
                double tAmount = double.Parse(dsAmount.Tables[0].Rows[0]["TotalAmount"].ToString());
                double tUnpaid = double.Parse(dsAmount.Tables[0].Rows[0]["UnPaidAmount"].ToString());
                tAmount = tAmount - tUnpaid;
                this.lblAmount.Text = tAmount == 0 ? "0.00" : Math.Round(tAmount,2).ToString();
            }
            else
            {
                this.lblAmount.Text = "0.00";
            }            
        }

        protected void btnWithdraw_Click(object sender, EventArgs e)
        {
            if (Math.Round(double.Parse(this.txtWithdrawAmount.Text.Trim()),2) > Math.Round(double.Parse(this.lblAmount.Text.Trim()),2))
            {
                this.lblMessage.Text = "Your request amount exceeds the available balance";
            }
            else
            {
                string queryUpaid = "Select UnPaidAmount From AccountInformation  Where UID=" + (Session["objUser"] as UserClass).UserID;
                string unPaidAmount = objMyDBClass.GetID(queryUpaid);
                unPaidAmount = (double.Parse(unPaidAmount) + double.Parse(this.txtWithdrawAmount.Text.Trim())).ToString();
                string queryUpdateUnpaid = "Update AccountInformation Set UnPaidAmount=" + Math.Round(double.Parse(unPaidAmount), 2) + " Where UID=" + (Session["objUser"] as UserClass).UserID;
                int res=objMyDBClass.ExecuteCommand(queryUpdateUnpaid);

                if (res > 0)
                {
                    string queryInsert = "Insert into Transactions(WithdrawAmount,TransactionDate,Status,UID) Values(" + Math.Round(double.Parse(this.txtWithdrawAmount.Text.Trim()),2).ToString() + ",'" + DateTime.Now.Date.GetDateTimeFormats()[5] + "','Pending'," + (Session["objUser"] as UserClass).UserID + ")";
                    objMyDBClass.ExecuteCommand(queryInsert);
                }
                this.lblMessage.Text = "your request submitted and soon you will be informed";
                Response.Redirect(Request.UrlReferrer.ToString(),false);
            }       
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.lblMessage.Text = "";
            this.txtWithdrawAmount.Text = "";
        }

        

        
    }
}

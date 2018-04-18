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
    public partial class AccountInformation : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblMessage.Text = "";
            if (!Page.IsPostBack)
            {
                string querySel = "Select * From AccountInformation Where UID="+(Session["objUser"] as UserClass).UserID;
                DataSet dsSel = objMyDBClass.GetDataSet(querySel);
                if (dsSel.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsSel.Tables[0].Rows[0];
                    this.txtAcNo.Text = dr["AccountNo"].ToString();
                    this.txtAcTitle.Text = dr["AccountTitle"].ToString();
                    ddAcType.SelectedIndex = dr["AccountType"].ToString() == "Saving" ? 0 : 1;
                    txtBankBranch.Text = dr["BankBranch"].ToString();
                    txtBankCode.Text = dr["BankCode"].ToString();
                    txtCity.Text = dr["City"].ToString();
                    txtCountry.Text = dr["Country"].ToString();
                    this.lblUnpaidAmount.Text = dr["UnPaidAmount"].ToString() == "" ? "0.00" : dr["UnPaidAmount"].ToString();
                    this.lblTotalAmount.Text = dr["TotalAmount"].ToString() == "" ? "0.00" : dr["TotalAmount"].ToString();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string queryInsert = "Insert into AccountInformation(UID,AccountNo,AccountTitle,AccountType,BankBranch,BankCode,City,Country) Values("+(Session["objUser"] as UserClass).UserID+",'" + txtAcNo.Text.Trim() + "','" + txtAcTitle.Text.Trim() + "','" + ddAcType.SelectedValue + "','" + txtBankBranch.Text.Trim() + "','" + txtBankCode.Text.Trim() + "','" + txtCity.Text + "','" + txtCountry.Text + "')";
            int res=objMyDBClass.ExecuteCommand(queryInsert);
            if (res > 0)
            {
                this.lblMessage.Text = "Account Information Saved";
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string queryUp = "Update AccountInformation Set ";
            queryUp += "AccountNo = '" + this.txtAcNo.Text.Trim() + "'";
            queryUp += ",AccountTitle = '" + this.txtAcTitle.Text.Trim() + "'";
            queryUp += ",AccountType = '" + ddAcType.SelectedValue + "'";
            queryUp += ",BankBranch = '" + txtBankBranch.Text.Trim() + "'";
            queryUp += ",BankCode = "+txtBankCode.Text.Trim();
            queryUp += ",City = '" + txtCity.Text.Trim() + "'";
            queryUp += ",Country = '" + txtCountry.Text.Trim()+"'";
            queryUp += " Where UID="+(Session["objUser"] as UserClass).UserID;
            int res=objMyDBClass.ExecuteCommand(queryUp);
            if (res > 0)
            {
                lblMessage.Text = "Record updated successfully";
            }   
        }        
    }
}

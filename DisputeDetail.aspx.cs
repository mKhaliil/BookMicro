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

namespace Outsourcing_System
{
    public partial class DisputeDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["objUser"] == null)
            {
                Response.Redirect("BookMicro.aspx");
            }
            if(Request.QueryString["aid"]!=null)
            {
                string qDetail = "SELECT [User].UserName, Book.BIdentityNo, Activity.Task, Activity.AssignedBy, Activity.AssigmentDate, Activity.DeadLine, Earnings.Remarks,Activity.AID FROM Book INNER JOIN Activity ON Book.BID = Activity.BID LEFT JOIN  Earnings ON Activity.AID = Earnings.AID INNER JOIN [User] ON Activity.UID = [User].UID  WHERE  Activity.AID =" + Request.QueryString["aid"].ToString();
                this.SqlDataSource1.SelectCommand = qDetail;
                this.DetailsView1.DataSourceID = this.SqlDataSource1.ID;
            }
        }
    }
}

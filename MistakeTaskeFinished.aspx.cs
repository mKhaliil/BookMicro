using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class web_MistakeTaskeFinished : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Result"] != null)
        {
            string result = Request.QueryString["Result"].ToString();
            divDialogue.Visible = true;
            this.lblResultShow.Text = result;
        }
    }
    protected void btnGoToHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("Index.aspx");
    }
}
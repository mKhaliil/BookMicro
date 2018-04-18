using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outsourcing_System.MasterPages;

namespace Outsourcing_System
{
    public partial class ValueAddSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((AdminMaster_Hiring)this.Page.Master).SetLogOut = true;
            ((AdminMaster_Hiring)this.Page.Master).SetMenuLocation = "-20px";
        }
    }
}
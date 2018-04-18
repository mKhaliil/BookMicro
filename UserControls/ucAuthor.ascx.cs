using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outsourcing_System.UserControls
{
    public partial class ucAuthor : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string fullName
        {
            get { return this.txtAFulName.Text; }
            set { this.txtAFulName.Text = value; }
        }
        public string prenominal
        {
            get { return this.txtAPrenominal.Text; }
            set { this.txtAPrenominal.Text = value; }
        }
        public string firstName
        {
            get { return this.txtAFName.Text; }
            set { this.txtAFName.Text = value; }
        }
        public string lastName
        {
            get { return this.txtALName.Text; }
            set { this.txtALName.Text = value; }
        }
    }
}
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

namespace Outsourcing_System.Controls
{
    public partial class ProcessControl : System.Web.UI.UserControl
    {
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {      
        }
        public void LoadProcesses()
        {
            DataSet dsProcesses = objMyDBClass.getAllProcesses();
            this.chkProcessList.DataSource = dsProcesses.Tables[0];
            this.chkProcessList.DataTextField = "PName";
            this.chkProcessList.DataValueField = "PID";
            this.chkProcessList.DataBind();
        }

        //Getting Selected Boxes
        public string getSelectedItems()
        {
            string selectedBoxes = "";
            int count = 0;
            foreach (ListItem item in chkProcessList.Items)
            {
                if (item.Enabled==true && item.Selected==true)
                {
                    if (count == 0)
                    {
                        selectedBoxes = item.Text;
                        ++count;
                    }
                    else
                    {
                        selectedBoxes += ":" + item.Text;
                    }
                }
            }
            return selectedBoxes;
        }

        public string getSelectedIValues()
        {
            string selectedBoxes = "";
            int count = 0;
            foreach (ListItem item in chkProcessList.Items)
            {
                if (item.Enabled == true && item.Selected == true)
                {
                    if (count == 0)
                    {
                        selectedBoxes = item.Value;
                        ++count;
                    }
                    else
                    {
                        selectedBoxes += ":" + item.Value;
                    }
                }
            }
            return selectedBoxes;
        }
        public void UncheckedSelectedBoxes()
        {
            foreach (ListItem item in chkProcessList.Items)
            {
                item.Selected = false;
            }
        }
        public void setSelectedBoxes(DataSet dsProcess,string processName,string forForm)
        {
            foreach (ListItem item in chkProcessList.Items)
            {                
                if (item.Text.ToLower() == processName.ToLower())
                {
                    item.Selected = true;
                }
                if (forForm == "AssignTask")
                {
                    foreach (DataRow dr in dsProcess.Tables[0].Rows)
                    {
                        if (item.Selected == false && dr["Task"].ToString() == item.Text && dr["Status"].ToString() != "Unassigned")
                        {
                            item.Enabled = false;
                        }
                    }
                }
            }
        }
    }
}
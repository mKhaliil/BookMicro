using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BookMicroBeta;
using Outsourcing_System.PdfCompare_Classes;

namespace Outsourcing_System
{
    public partial class ResetTask : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearh_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["LoginId"])))
                Response.Redirect("BookMicro.aspx", true);

            if (!string.IsNullOrEmpty(tbxSearchId.Text.Trim()))
            {
                string existRecQuery = "SELECT * FROM MainBook Where MainBook='" + tbxSearchId.Text.Trim() + "'";
                DataSet ds = objMyDBClass.GetDataSet(existRecQuery);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    divResetOptions.Visible = true;
                }
            }
            else
            {
                ucShowMessage1.ShowMessage(MessageTypes.Error, "BookId does not exists.");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["LoginId"])))
                Response.Redirect("BookMicro.aspx", true);

            List<string> checkedValues = cbxlResetTask.Items.Cast<ListItem>().Where(li => li.Selected).Select(li => li.Value).ToList();

            if (checkedValues.Count > 0)
                ResetTaggingUnTagging(checkedValues);
            else
                ucShowMessage1.ShowMessage(MessageTypes.Error, "Please select an item before reset.");
        }

        private void ResetTaggingUnTagging(List<string> checkedValues)
        {
            if (!string.IsNullOrEmpty(tbxSearchId.Text.Trim()) && checkedValues.Count > 0)
            {
                string rhywFilePath = objMyDBClass.MainDirPhyPath + "\\" + tbxSearchId.Text.Trim() + "\\" + tbxSearchId.Text.Trim() + "-1\\TaggingUntagged\\" +
                    tbxSearchId.Text.Trim() + "-1.rhyw";

                string tempXmlFilePath = objMyDBClass.MainDirPhyPath + "\\" + tbxSearchId.Text.Trim() + "\\" + "temp.xml";

                if (File.Exists(rhywFilePath) && File.Exists(tempXmlFilePath))
                {
                    bool isSpliltMerge = false;
                    bool isIndex = false;
                    int indexStart = 0;
                    int indexEnd = 0;
                    bool isImage = false;
                    bool isTable = false;
                    bool tableAlgo1 = true;
                    bool tableAlgo2 = true;
                    bool tableAlgo3 = true;
                    bool isNPara = false;
                    bool nParaAlgo1 = true;
                    bool nParaAlgo2 = false;
                    bool isSPara = false;
                    bool isFootNotes = false;

                    foreach (string checkValue in checkedValues)
                    {
                        if (checkValue.Trim().Equals("Splitting/Merging")) isSpliltMerge = true;

                        else if (checkValue.Trim().Equals("Images")) isImage = true;

                        else if (checkValue.Trim().Equals("Tables")) isTable = true;

                        else if (checkValue.Trim().Equals("NPara")) isNPara = true;

                        else if (checkValue.Trim().Equals("SPara")) isSPara = true;

                        else if (checkValue.Trim().Equals("FootNotes")) isFootNotes = true;
                    }

                    try
                    {
                        File.Delete(rhywFilePath);
                        File.Delete(tempXmlFilePath);

                        string pdfPath = objMyDBClass.MainDirPhyPath + "\\" + tbxSearchId.Text.Trim() + "\\" + tbxSearchId.Text.Trim() + ".pdf";
                        AutoMapService.AutoMappService autoMapSvc = new AutoMapService.AutoMappService();
                        autoMapSvc.AllowAutoRedirect = true;
                        autoMapSvc.ResetTaskAsync(pdfPath, isSpliltMerge, isIndex, indexStart, indexEnd, isImage, isTable, tableAlgo1, tableAlgo2, tableAlgo3,
                            isNPara, nParaAlgo1, nParaAlgo2, isSPara, isFootNotes);

                        //wait for tagginguntag completion
                        while (!File.Exists(rhywFilePath))
                        {

                        }

                        Thread.Sleep(2000);
                        autoMapSvc.CancelAsync(null);
                        autoMapSvc.Dispose();

                        ucShowMessage1.ShowMessage(MessageTypes.Success, "BookId resets successfully.");
                        divResetOptions.Visible = false;
                    }
                    catch (Exception)
                    {
                        ucShowMessage1.ShowMessage(MessageTypes.Error, "Some error has occured.");
                    }
                }
                else
                {
                    ucShowMessage1.ShowMessage(MessageTypes.Error, "Rhyw File do not exists.");
                }
            }
        }
    }
}
using System;

namespace Outsourcing_System
{
    public partial class ComparisonPage2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PDFViewerSource.FilePath = "http://192.168.0.45/Files/380/Comparison/Page1-1.pdf";
            FileLoadPath.Value = "PdftoHtml5Scripts/Page1.pdf";
        }
    }
}
using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

/// <summary>
/// Summary description for SourcePDF
/// </summary>
public class SourcePDF
{
    private string pdfPath;
	public SourcePDF(string pdfPath)
	{
        this.pdfPath = pdfPath;
	}

    public string GetPage(int pageNum)
    {
        string outputPDFPath = pdfPath.Replace(".pdf","_out.pdf");
        ExtractPages(pdfPath, outputPDFPath, pageNum, pageNum);
        return outputPDFPath;
    }

    private static void ExtractPages(string inputFile, string outputFile, int start, int end)
    {
        // get input document
        PdfReader inputPdf = new PdfReader(inputFile);
        // retrieve the total number of pages
        int pageCount = inputPdf.NumberOfPages;
        if (end < start || end > pageCount)
        {
            end = pageCount;
        }
        // load the input document
        Document inputDoc = new Document(inputPdf.GetPageSizeWithRotation(1));

        // create the filestream
        using (FileStream fs = new FileStream(outputFile, FileMode.Create))
        {
            // create the output writer
            PdfWriter outputWriter = PdfWriter.GetInstance(inputDoc, fs);
            inputDoc.Open();
            PdfContentByte cb1 = outputWriter.DirectContent;

            // copy pages from input to output document
            for (int i = start; i <= end; i++)
            {
                inputDoc.SetPageSize(inputPdf.GetPageSizeWithRotation(i));
                inputDoc.NewPage();

                PdfImportedPage page = outputWriter.GetImportedPage(inputPdf, i);
                int rotation = inputPdf.GetPageRotation(i);

                if (rotation == 90 || rotation == 270)
                {
                    cb1.AddTemplate(page, 0, -1f, 1f, 0, 0,
                        inputPdf.GetPageSizeWithRotation(i).Height);
                }
                else
                {
                    cb1.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                }
            }
            inputDoc.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.WebControls;
using BookMicroBeta;

namespace Outsourcing_System
{
    public partial class AddVolumeBreaks : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        CommonClass objCommonClass = new CommonClass();

        protected void Page_Load(object sender, EventArgs e)
        {


        }
        private void PopulateFields()
        {
            string Query = "Select distinct Activity.BID,cast(Activity.BID as varchar(100)) as BIdentityNo," +
                " Book.BTitle,Book.UploadedBy,Book.[UpDate],Activity.AID,Activity.Status,Activity.CompletionDate," +
                "Activity.Cost,Activity.[Count],Activity.Deadline,Activity.Task,[User].UName as AssignedTo " +
                "from Book INNER Join Activity on Book.MainBook=cast(Activity.BID as varchar(100)) INNER JOIN [User] " +
                "on Activity.UID=[User].UID Where Book.BStatus='Complete' AND Activity.Status like'Approved' " +
                "and Activity.Task like 'Meta'   AND Activity.AssignedBy='" + (Session["objUser"] as UserClass).UserName + "' ";
            DataSet dsBooks = objMyDBClass.GetDataSet(Query);
            //ddlBookIds.DataSource = dsBooks.Tables[0];
            //ddlBookIds.DataValueField = dsBooks.Tables[0].Columns[0].ToString();
            //ddlBookIds.DataTextField = dsBooks.Tables[0].Columns[2].ToString();
            //ddlBookIds.DataBind();

        }

        protected void btnInsertVolumeBreak_Click(object sender, EventArgs e)
        {
            string bookID = Request.QueryString["BID"].ToString();
            string rhywPath = System.Configuration.ConfigurationManager.AppSettings["MainDirPhyPath"] + "\\" + bookID + "\\" + bookID + ".rhyw";
            try
            {
                string FileName = rhywPath;
                RemovePriorVolumeBreaks(FileName);

                XmlDocument xmlDoc = new XmlDocument();

                string xml = objCommonClass.LoadEncryptedXmlFile(FileName);
                xmlDoc.LoadXml(xml);
                XmlNodeList UparasList = xmlDoc.SelectNodes("//upara");

                int PageSpan = Convert.ToInt32(ddlPages.SelectedValue.ToString());
                int pageNo = PageSpan;
                string pdfPath = @"D:\Khalil Work\Documents_From_Saqib\ValueAdd_Testing\Final_Version.pdf";
                string outPdfPath = "";
                PdfReader inputPdf = new PdfReader(pdfPath);


                XmlNodeList ChapsList = xmlDoc.SelectNodes("//section[@type=\"chapter\"]");
                List<XmlNode> ChapEndsList = new List<XmlNode>();
                foreach (XmlNode item in ChapsList)
                {
                    XmlNodeList uparas = item.SelectNodes(".//upara");
                    for (int i = uparas.Count - 1; i > 0; i--)
                    {
                        if (uparas[i].InnerText.Length > 20)
                        {
                            ChapEndsList.Add(uparas[i]);
                            break;
                        }
                    }

                }
                int counter = 0;
                List<int> pages = new List<int>();
                for (int i = 1; i <= inputPdf.NumberOfPages; i++)
                {
                    string pageText = PdfTextExtractor.GetTextFromPage(inputPdf, i).Replace("\n", " ");
                    if (!pageText.Equals(""))
                    {
                        Match obj = Regex.Match(pageText, @"\s[0-9]{0,}$");
                        if (!obj.Value.Equals(""))
                        {
                            pageText = pageText.Replace(obj.Value, "").Replace(" ", "");
                        }
                    }
                    bool txtOccures = false;
                    for (int j = 0; j < ChapEndsList.Count; j++)
                    {
                        if (pageText.Contains(ChapEndsList[j].InnerText.Replace(" ", "")) || (ChapEndsList[j].InnerText.Replace(" ", "").Contains(pageText)))
                        {
                            txtOccures = true;
                            break;
                        }
                        else
                        {
                            txtOccures = false;
                        }
                    }
                    if (!txtOccures)
                    {
                        counter = counter + 1;
                    }
                    else
                    {
                        counter = 0;
                    }
                    if (counter.Equals(Convert.ToInt32(ddlPages.SelectedValue)))
                    {
                        pages.Add(i);
                        counter = 0;
                    }
                }

                List<string> generatedPages = new List<string>();
                if (!Directory.Exists(@"C:\ValueAdd_Testing\"))
                {
                    Directory.CreateDirectory(@"C:\ValueAdd_Testing\");
                }
                for (int i = 0; i < pages.Count; i++)
                {
                    outPdfPath = @"C:\ValueAdd_Testing\" + pages[i] + ".pdf";
                    generatedPages.Add(outPdfPath);
                    ExtractPages(pdfPath, outPdfPath, pages[i], pages[i]);
                }
                for (int i = 0; i < generatedPages.Count; i++)
                {
                    XmlNode tempNode = ReadText(generatedPages[i]);
                    XmlNodeList paras = tempNode.SelectNodes("//Para");
                    for (int j = paras.Count - 1; j > 0; j--)
                    {
                        if (paras[j].ChildNodes.Count > 1 && (paras[j].InnerText.Length > 5))
                        {
                            string textTomatch = paras[j].InnerText;
                            for (int k = 0; k < UparasList.Count; k++)
                            {
                                if (UparasList[k].InnerText.Replace(" ", "").Contains(textTomatch.Replace(" ", "")))
                                {
                                    XmlNode parentNode = UparasList[k].SelectSingleNode(".//ancestor::pbp-front|.//ancestor::pbp-end|.//ancestor::pbp-body");
                                    if (parentNode.Name.Equals("pbp-front") || parentNode.Name.Equals("pbp-end"))
                                    {
                                        j = 0;
                                        break;
                                    }
                                    else
                                        if (UparasList[k].SelectSingleNode(".//break") != null)
                                        {
                                            XmlNode sectionVolumeNode = createSection(xmlDoc);
                                            UparasList[k].PreviousSibling.AppendChild(sectionVolumeNode);

                                            objCommonClass.SaveEncryptedXmlFile(xmlDoc.OuterXml, FileName);
                                            j = 0;
                                            break;
                                        }
                                        else
                                        {
                                            XmlNode sectionVolumeNode = createSection(xmlDoc);
                                            UparasList[k].PreviousSibling.AppendChild(sectionVolumeNode);
                                            objCommonClass.SaveEncryptedXmlFile(xmlDoc.OuterXml, FileName);
                                            j = 0;
                                            break;
                                        }
                                }
                            }
                        }
                    }
                }

                InsertAtTheEndOfChapter(FileName);

                if (Directory.Exists(@"C:\ValueAdd_Testing\"))
                {
                    for (int i = 0; i < generatedPages.Count; i++)
                    {
                        File.Delete(generatedPages[i]);
                        File.Delete(generatedPages[i].Replace(".pdf", ".tetml"));
                    }
                    Directory.Delete(@"C:\ValueAdd_Testing\");

                }
                //  messageBox.ShowMessage("Volume Breaks inserted successfully");

            }
            catch (Exception ex)
            {
                this.Master.ShowMessageBox(ex.Message,"error");
            }
        }
        #region |Mehtods|
        #region void RemovePriorVolumeBreaks()
        public void RemovePriorVolumeBreaks(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.Load(filePath);
            string xml = objCommonClass.LoadEncryptedXmlFile(path);
            xmlDoc.LoadXml(xml);
            XmlNodeList volumeBreaksList = xmlDoc.SelectNodes("//break[@type=\"volume\"]");
            foreach (XmlNode vNode in volumeBreaksList)
            {
                vNode.ParentNode.RemoveChild(vNode);
            }
            //xmlDoc.Save(this.filePath);
            objCommonClass.SaveEncryptedXmlFile(xmlDoc.OuterXml, path);
        }
        #endregion

        #region void InsertAtTheEndOfChapter()

        //Khalil Code Here
        private bool EmptyChapter(XmlNode objNode)
        {

            if (objNode.SelectSingleNode("descendant::section-title").InnerText.Equals("-"))
            {
                return true;
            }
            return false;
        }
        //Khalil Code Ends Here
        public void InsertAtTheEndOfChapter(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string xml = objCommonClass.LoadEncryptedXmlFile(path);
            xmlDoc.LoadXml(xml);

            XmlNodeList sectionList = xmlDoc.SelectNodes("//section[@type=\"chapter\"]|//post-section");
            foreach (XmlNode sectionNode in sectionList)
            {

                try
                {

                    //if (cbxEmptyChapters.Checked)
                    //{
                    //    #region creation of break node
                    //    XmlNode breakNode = xmlDoc.CreateElement("break");
                    //    XmlNode idNode = xmlDoc.CreateAttribute("id");
                    //    idNode.Value = "200";
                    //    breakNode.Attributes.Append((XmlAttribute)idNode);
                    //    XmlNode typeNode = xmlDoc.CreateAttribute("type");
                    //    breakNode.Attributes.Append((XmlAttribute)typeNode);
                    //    typeNode.Value = "volume";
                    //    //Adding Volume Num
                    //    XmlNode volumeNumNode = xmlDoc.CreateAttribute("num");
                    //    breakNode.Attributes.Append((XmlAttribute)volumeNumNode);
                    //    volumeNumNode.Value = "V";
                    //    #endregion end of creation of break node

                    //    XmlNodeList bodyList = sectionNode.SelectNodes(".//body");
                    //    if (bodyList.Count > 0)
                    //    {
                    //        XmlNode lastChild = bodyList[bodyList.Count - 1].LastChild;
                    //        if (lastChild != null)
                    //        {
                    //            //Khalil Code Here
                    //            if (lastChild.Name == "spara" || lastChild.Name == "npara" || lastChild.Name == "upara")
                    //            {
                    //                if (lastChild.LastChild != null)
                    //                    lastChild = lastChild.LastChild;
                    //            }
                    //            if (lastChild.Name == "box")
                    //            {
                    //                lastChild = lastChild.ParentNode;
                    //            }
                    //            if (lastChild.Name.Equals("break") && (lastChild.Attributes["type"].Value.Equals("page")))
                    //            {
                    //                //to delete
                    //                // lastChild.ParentNode.InnerText = lastChild.ParentNode.InnerText + "chapter Break";
                    //                lastChild.ParentNode.InsertBefore(breakNode, lastChild);
                    //            }
                    //            else
                    //            {
                    //                //to delete
                    //                //lastChild.InnerText = lastChild.InnerText + "chapter Break";
                    //                lastChild.AppendChild(breakNode);


                    //            }
                    //            //Khalil Code Ends here
                    //            //if (lastChild.Name == "spara")
                    //            //{
                    //            //    if (lastChild.LastChild != null)
                    //            //        lastChild = lastChild.LastChild;
                    //            //}
                    //            //if (lastChild.Name == "box")
                    //            //{
                    //            //    lastChild = lastChild.ParentNode;
                    //            //}
                    //            //lastChild.AppendChild(breakNode);
                    //        }
                    //    }
                    //}
                    //else if (EmptyChapter(sectionNode))
                    //{
                    #region creation of break node
                    XmlNode breakNode = xmlDoc.CreateElement("break");
                    XmlNode idNode = xmlDoc.CreateAttribute("id");
                    idNode.Value = "200";
                    breakNode.Attributes.Append((XmlAttribute)idNode);
                    XmlNode typeNode = xmlDoc.CreateAttribute("type");
                    breakNode.Attributes.Append((XmlAttribute)typeNode);
                    typeNode.Value = "volume";
                    //Adding Volume Num
                    XmlNode volumeNumNode = xmlDoc.CreateAttribute("num");
                    breakNode.Attributes.Append((XmlAttribute)volumeNumNode);
                    volumeNumNode.Value = "V";
                    #endregion end of creation of break node

                    XmlNodeList bodyList = sectionNode.SelectNodes(".//body");
                    if (bodyList.Count > 0)
                    {
                        XmlNode lastChild = bodyList[bodyList.Count - 1].LastChild;
                        if (lastChild != null)
                        {
                            //Khalil Code Here
                            if (lastChild.Name == "spara" || lastChild.Name == "npara" || lastChild.Name == "upara")
                            {
                                if (lastChild.LastChild != null)
                                    lastChild = lastChild.LastChild;
                            }
                            if (lastChild.Name == "box")
                            {
                                lastChild = lastChild.ParentNode;
                            }
                            if (lastChild.Name.Equals("break") && (lastChild.Attributes["type"].Value.Equals("page")))
                            {
                                //to delete
                                // lastChild.ParentNode.InnerText = lastChild.ParentNode.InnerText + "chapter Break";
                                lastChild.ParentNode.InsertAfter(breakNode, lastChild);
                            }
                            else
                            {
                                lastChild.AppendChild(breakNode);
                            }
                        }
                    }
                    // }
                }
                catch
                {

                }
            }
            objCommonClass.SaveEncryptedXmlFile(xmlDoc.OuterXml, path);
        }
        #endregion
        public XmlNode ReadText(string pdfPath)
        {
            string[] directory = pdfPath.Split('.');
            string tetmlFilePath = directory[0] + ".tetml";
            string pdffilePath = pdfPath;
            //tetFile = XmlFile;
            string strParameter = "-m line --pageopt \"tetml={glyphdetails={geometry=false font=false}} contentanalysis={dehyphenate=false}\" -o \"" + tetmlFilePath + "\" \"" + pdffilePath + "\"";
            //string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks}\" -o \"" + XmlFile + "\" \"" + PDFFilePath + "\"";
            //string Img_Conversion_bat = @"D:\work\tet.exe";
            string Img_Conversion_bat = @"C:\XSL\tet.exe";
            Process pConvertTetml = new Process();
            pConvertTetml.StartInfo.UseShellExecute = false;
            pConvertTetml.StartInfo.RedirectStandardError = true;
            pConvertTetml.StartInfo.RedirectStandardOutput = true;
            pConvertTetml.StartInfo.CreateNoWindow = true;
            pConvertTetml.StartInfo.Arguments = strParameter;
            pConvertTetml.StartInfo.FileName = Img_Conversion_bat;
            pConvertTetml.Start();
            pConvertTetml.WaitForExit();

            string OutputFile = directory[0] + ".xml";

            TextReader tr = new StreamReader(tetmlFilePath);
            string comp = tr.ReadToEnd();
            tr.Close();
            string Temp2 = "<?xml version=\"1.0\" encoding=\"utf-8\"?><book>";
            MatchCollection pageCol = Regex.Matches(comp, "<Page[ ].*?>.*?</Page>", RegexOptions.Singleline);
            for (int m = 0; m < pageCol.Count; m++)
            {
                Temp2 += "\n" + Regex.Match(pageCol[m].Value.ToString(), "<Page[ ].*?>").Value;
                MatchCollection paraColl = Regex.Matches(pageCol[m].Value.ToString(), "<Para>.*?</Para>", RegexOptions.Singleline);
                MatchCollection imageCol1 = Regex.Matches(pageCol[m].Value.ToString(), "<PlacedImage[ ].*? />", RegexOptions.Singleline);
                foreach (Match para in paraColl)
                {
                    Temp2 += "\n<Para>";
                    MatchCollection matCol = Regex.Matches(para.Value.ToString(), "<Text>.*?</Text>");
                    foreach (Match mat in matCol)
                    {
                        Temp2 += "\n" + mat.Value;
                    }
                    Temp2 += "\n</Para>";
                }
                foreach (Match image in imageCol1)
                {
                    Temp2 += image.Value;
                }
                Temp2 += "\n</Page>";
            }
            Temp2 = Temp2 + "</book>";
            XmlDocument tempXmlDoc = new XmlDocument();
            tempXmlDoc.LoadXml(Temp2);
            return tempXmlDoc;
        }
        public void ExtractPages(string inputFile, string outputFile, int start, int end)
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

                    PdfImportedPage page =
                        outputWriter.GetImportedPage(inputPdf, i);
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
        public XmlNode createSection(XmlDocument xmlDoc)
        {
            XmlNode breakNode = xmlDoc.CreateElement("break");
            XmlNode idNode = xmlDoc.CreateAttribute("id");
            idNode.Value = "200";
            breakNode.Attributes.Append((XmlAttribute)idNode);
            XmlNode typeNode = xmlDoc.CreateAttribute("type");
            breakNode.Attributes.Append((XmlAttribute)typeNode);
            typeNode.Value = "volume";
            //Adding Volume Num
            XmlNode volumeNumNode = xmlDoc.CreateAttribute("num");
            breakNode.Attributes.Append((XmlAttribute)volumeNumNode);
            volumeNumNode.Value = "V";
            return breakNode;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                FileUpload1.SaveAs(@"D:\Khalil Work\Documents_From_Saqib\ValueAdd_Testing\Final_Version.pdf");
            }
        }

    }
        #endregion
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
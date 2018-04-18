using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.IO;
using System.Net;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Drawing;
using System.Data;
using BookMicroBeta;
using System.Drawing.Imaging;
using Outsourcing_System.BookMicroOcrService;
using Outsourcing_System.MasterPages;
using Outsourcing_System.OcrService;
using Outsourcing_System.OcrService11;
using Outsourcing_System.PdfCompare_Classes;
using ListItem = System.Web.UI.WebControls.ListItem;
using iTextSharp.text.html.simpleparser;
using OfficeOpenXml;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Globalization;

namespace Outsourcing_System
{
    public partial class OnlineTest_New : System.Web.UI.Page
    {
        #region |Fields and Properties|

        GlobalVar objGlobal = new GlobalVar();
        MyDBClass objMyDBClass = new MyDBClass();

        #endregion

        //public byte[] ManipulatePdf(byte[] src)
        //{
        //    PdfReader reader = new PdfReader(src);
        //    int n = reader.NumberOfPages;
        //    PdfDictionary pageDict;
        //    PdfRectangle rect = new PdfRectangle(55, 76, 560, 816);
        //    for (int i = 1; i <= n; i++)
        //    {
        //        pageDict = reader.GetPageN(i);
        //        pageDict.Put(PdfName.CROPBOX, rect);
        //    }
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (PdfStamper stamper = new PdfStamper(reader, ms))
        //        {
        //        }
        //        return ms.ToArray();
        //    }
        //}

        public void TrimLeftandRightFoall(string sourceFilePath, string outputFilePath, float cropwidth)
        {

            PdfReader pdfReader = new PdfReader(sourceFilePath);
            float width = 18;
            float height = 15;
            float widthTo_Trim = iTextSharp.text.Utilities.MillimetersToPoints(cropwidth);

            PdfRectangle rectLeftside = new PdfRectangle(widthTo_Trim, widthTo_Trim, width - widthTo_Trim, height - widthTo_Trim);

            using (var output = new FileStream(outputFilePath, FileMode.CreateNew, FileAccess.Write))
            {
                // Create a new document
                Document doc = new Document();

                // Make a copy of the document
                PdfSmartCopy smartCopy = new PdfSmartCopy(doc, output);

                // Open the newly created document
                doc.Open();

                // Loop through all pages of the source document
                for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                {
                    // Get a page
                    var page = pdfReader.GetPageN(i);
                    page.Put(PdfName.TRIMBOX, rectLeftside);

                    var copiedPage = smartCopy.GetImportedPage(pdfReader, i);
                    smartCopy.AddPage(copiedPage);
                }

                doc.Close();

            }
        }

        //private string HighLightSelectedParas(string inFilePath, string outputFilePath, BaseColor color, List<string> lstcoordinates)
        //{
        //    try
        //    {
        //        using (Stream inputPdfStream = new FileStream(inFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        //        using (Stream outputPdfStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
        //        {
        //            //Opens the unmodified PDF for reading
        //            PdfReader reader = new PdfReader(inputPdfStream);

        //            var stamper = new PdfStamper(reader, outputPdfStream) { FormFlattening = true, FreeTextFlattening = true };
        //            foreach (string coordinates in lstcoordinates)
        //            {
        //                string[] dimensions = coordinates.Split(' ');
        //                float x = 0;
        //                float y = 0;
        //                float.TryParse(dimensions[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
        //                float.TryParse(dimensions[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
        //                int width = Convert.ToInt32(Math.Round(Convert.ToDouble(dimensions[3]))) == 0 ? 1 : Convert.ToInt32(Math.Round(Convert.ToDouble(dimensions[3])));
        //                int height = Convert.ToInt32(Math.Round(Convert.ToDouble(dimensions[2]))) == 0 ? 1 : Convert.ToInt32(Math.Round(Convert.ToDouble(dimensions[2])));
        //                iTextSharp.text.Image objImage1 = iTextSharp.text.Image.GetInstance(new Bitmap(width, height), color);

        //                objImage1.SetAbsolutePosition(x, y - 2);
        //                PdfGState _state = new PdfGState()
        //                {
        //                    FillOpacity = 0.7F,
        //                    StrokeOpacity = 0.7F
        //                };

        //                //var yy = dimensions[4];
        //                //var tt = Convert.ToInt32(dimensions[4]);

        //                //stamper.GetOverContent(Convert.ToInt32(dimensions[4])).SetGState(_state);
        //                //stamper.GetOverContent(Convert.ToInt32(dimensions[4])).AddImage(objImage1, true);

        //                stamper.GetOverContent(1).SetGState(_state);
        //                stamper.GetOverContent(1).AddImage(objImage1, true);
        //            }
        //            stamper.Close();
        //        }
        //        lstcoordinates.Clear();
        //        File.Delete(inFilePath);
        //        File.Copy(outputFilePath, inFilePath);
        //        File.Delete(outputFilePath);
        //        return outputFilePath;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            //Below code is for OCs Service testing No use in BookMicro, it is temporary code           
            //BookMicroOcrService1.GetOcrText obj = new BookMicroOcrService1.GetOcrText();
            //string imgPath = @"D:\testing\OCR\Input\2018-02-22\2018-02-22.docx";

            //byte[] bytesImg = File.ReadAllBytes(imgPath);
            //var res = obj.GetTextandXMLFromImage(bytesImg, "2018-02-22", 1, "docx");



            //// List<string> lstcoordinates = new List<string>();
            ////   double llx = 40;
            ////    double lly = 100;
            ////    double urx = 0;
            ////    double ury = 0;
            ////    lstcoordinates.Add((Convert.ToDouble(llx)) + " " + (Convert.ToDouble(lly)) + " " + "50" + " " + "50" + " " + "1");

            //// string inFilePath = "";
            ////string outputFilePath = "";

            ////HighLightSelectedParas(inFilePath, outputFilePath, BaseColor.ORANGE, lstcoordinates);


           // string src = @"C:\Users\Aamir\Desktop\Cropped Error Pdf\35818.pdf";

            //string src = @"C:\Users\Aamir\Desktop\Testing\9781925495478-text.pdf";
            //PdfReader reader = new PdfReader(src);
            //int n = reader.NumberOfPages;

            //TrimLeftandRightFoall(@"C:\Users\Aamir\Desktop\PDF BOX\23.pdf", @"C:\Users\Aamir\Desktop\PDF BOX\23__.pdf", 200);

            if (!IsPostBack)
            {
                if (Request.Cookies["Email"] != null && Request.Cookies["Password"] != null)
                {
                    tbxEmail.Text = Request.Cookies["Email"].Value;
                    tbxPassword.Attributes["value"] = Request.Cookies["Password"].Value;
                    cbxRememberMe.Checked = true;
                }
                else
                {
                    tbxEmail.Text = "";
                    tbxPassword.Text = "";
                    tbxPassword.Attributes["value"] = "";
                    cbxRememberMe.Checked = false;
                }

                //if (Request.QueryString["testType"] != null && 
                //    Request.QueryString["testType"].Equals("upgraded") && 
                //    !string.IsNullOrEmpty(Convert.ToString(Session["LoginId"])))
                //{
                //    Session["TestType"] = "upgraded";
                //}

                Page.MaintainScrollPositionOnPostBack = true;

                if (Convert.ToString(Session["CountryName"]).Equals("pakistan"))
                {
                    mvTrainingVideos.ActiveViewIndex = 1;
                    lbtnUrdu.Attributes.Add("Style", "color:#CCCCCC");
                    lbtnEnglish.Attributes.Add("Style", "color:#0099ff");
                    lbtnUrdu.Enabled = false;
                    lbtnEnglish.Enabled = true;
                }
                else if (Convert.ToString(Session["CountryName"]).Equals("other"))
                {
                    mvTrainingVideos.ActiveViewIndex = 1;
                    lbtnUrdu.Attributes.Add("Style", "color:#0099ff");
                    lbtnEnglish.Attributes.Add("Style", "color:#CCCCCC");
                    //lbtnUrdu.Enabled = true;

                    lbtnUrdu.Enabled = false;
                    lbtnEnglish.Enabled = false;
                }
            }

            //((UserMaster)this.Master).HideLogOutButton();
            //((UserMaster)this.Page.Master).SetLogIn = true;
            //((UserMaster)this.Page.Master).SetMenuLocation = "0px";

            //Response.Redirect("http://localhost:13609/PdfWebCompare/web/Index.aspx");

            //Response.Redirect("http://localhost:36022/Login.aspx");

            //var aa = Convert.ToString(hfSubmit.Value);

            //if (Request.Cookies["StudentCookies"] != null)
            //{
            //    string username = Request.Cookies["UserId"].Value;
            //}
            //HttpCookie cookie = new HttpCookie("mybigcookie");
            //cookie.Values.Add("name", name);
            //cookie.Values.Add("address", address);

            ////get the values out
            //string name = response.Cookies["mybigcookie"]["name"];
            //string address = response.Cookies["mybigcookie"]["address"];

            //HttpCookie StudentCookies = Request.Cookies["StudentCookies"];
            //if (StudentCookies != null)
            //{
            //    string username = StudentCookies.Value;
            //    im.Attributes.Add("style", "color: white;");
            //}
        }


        //private const string ContentTypeNamespace =
        //  @"http://schemas.openxmlformats.org/package/2006/content-types";

        //private const string WordprocessingMlNamespace =
        //    @"http://schemas.openxmlformats.org/wordprocessingml/2006/main";

        //private const string DocumentXmlXPath =
        //    "/t:Types/t:Override[@ContentType=\"application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml\"]";

        //private const string BodyXPath = "/w:document/w:body";

        ////private string docxFile = @"F:\TestDoc.docx";
        ////private string docxFileLocation = "";

        //public void GetTextFromDocx(string docxFilePath)
        //{
        //    if (!string.IsNullOrEmpty(docxFilePath))
        //    {
        //        string documentXmlLocation = FindDocumentXmlLocation(docxFilePath);

        //        if (!string.IsNullOrEmpty(documentXmlLocation))
        //        {
        //            string textWithImgIds = ReadDocumentXml(documentXmlLocation, docxFilePath);
        //        }
        //    }
        //}

        //private string FindDocumentXmlLocation(string docxFilePath)
        //{
        //    ZipFile zip = new ZipFile(docxFilePath);
        //    foreach (ZipEntry entry in zip)
        //    {
        //        // Find "[Content_Types].xml" zip entry

        //        if (string.Compare(entry.Name, "[Content_Types].xml", true) == 0)
        //        {
        //            Stream contentTypes = zip.GetInputStream(entry);

        //            XmlDocument xmlDoc = new XmlDocument();
        //            xmlDoc.PreserveWhitespace = true;
        //            xmlDoc.Load(contentTypes);
        //            contentTypes.Close();

        //            //Create an XmlNamespaceManager for resolving namespaces

        //            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
        //            nsmgr.AddNamespace("t", ContentTypeNamespace);

        //            // Find location of "document.xml"

        //            XmlNode node = xmlDoc.DocumentElement.SelectSingleNode(DocumentXmlXPath, nsmgr);

        //            if (node != null)
        //            {
        //                string location = ((XmlElement)node).GetAttribute("PartName");
        //                return location.TrimStart(new char[] { '/' });
        //            }
        //            break;
        //        }
        //    }
        //    zip.Close();
        //    return null;
        //}

        //private string ReadDocumentXml(string documentXmlLocation, string docxFile)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    ZipFile zip = new ZipFile(docxFile);
        //    foreach (ZipEntry entry in zip)
        //    {
        //        if (string.Compare(entry.Name, documentXmlLocation, true) == 0)
        //        {
        //            Stream documentXml = zip.GetInputStream(entry);

        //            XmlDocument xmlDoc = new XmlDocument();
        //            xmlDoc.PreserveWhitespace = true;
        //            xmlDoc.Load(documentXml);
        //            documentXml.Close();

        //            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
        //            nsmgr.AddNamespace("w", WordprocessingMlNamespace);

        //            XmlNode node = xmlDoc.DocumentElement.SelectSingleNode(BodyXPath, nsmgr);

        //            if (node == null)
        //                return string.Empty;

        //            sb.Append(ReadNode(node, docxFile));

        //            break;
        //        }
        //    }
        //    zip.Close();
        //    return sb.ToString();
        //}

        //private string ReadNode(XmlNode node, string docxFile)
        //{
        //    if (node == null || node.NodeType != XmlNodeType.Element)
        //        return string.Empty;

        //    StringBuilder sb = new StringBuilder();
        //    foreach (XmlNode child in node.ChildNodes)
        //    {
        //        if (child.NodeType != XmlNodeType.Element) continue;

        //        switch (child.LocalName)
        //        {
        //            case "t":                           // Text
        //                sb.Append(child.InnerText.TrimEnd());

        //                string space = ((XmlElement)child).GetAttribute("xml:space");
        //                if (!string.IsNullOrEmpty(space) && space == "preserve")
        //                    sb.Append(' ');

        //                break;

        //            case "cr":                          // Carriage return
        //            case "br":                          // Page break
        //                sb.Append(Environment.NewLine);
        //                break;

        //            case "tab":                         // Tab
        //                sb.Append("\t");
        //                break;

        //            case "p":                           // Paragraph
        //                sb.Append(ReadNode(child, docxFile));
        //                sb.Append(Environment.NewLine);
        //                sb.Append(Environment.NewLine);
        //                break;

        //            case "imagedata":                         // Image
        //                if (child.Attributes != null && child.Attributes.Count > 0)
        //                {
        //                    ZipFile zip = new ZipFile(docxFile);
        //                    foreach (ZipEntry entry in zip)
        //                    {
        //                        if (entry.Name.Equals("word/_rels/document.xml.rels"))
        //                        {
        //                            using (Stream documentXml = zip.GetInputStream(entry))
        //                            {
        //                                XmlDocument xmlDoc = new XmlDocument();
        //                                xmlDoc.PreserveWhitespace = true;
        //                                xmlDoc.Load(documentXml);

        //                                var relationNodes = xmlDoc.SelectNodes("//@Id/..");

        //                                foreach (XmlNode node1 in relationNodes)
        //                                {
        //                                    if (node1.Attributes["Id"].Value.Equals(child.Attributes[0].Value))
        //                                    {
        //                                        sb.Append("<image src='" + node1.Attributes["Target"].Value + "'> ");
        //                                        break;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                break;

        //            default:
        //                sb.Append(ReadNode(child, docxFile));
        //                break;
        //        }
        //    }
        //    return sb.ToString();
        //}


        //        public void TestMethod()
        //        {
        //            string docxFile = @"F:\TestDoc.docx";


        //            GetTextFromDocx(docxFile);

        //            //OcrService.GetText obj = new OcrService.GetText();

        //            //string imgPath = @"F:\0264.pdf";

        //            //string testImg = @"F:\media";

        //            //// zip up the files
        //            //try
        //            //{
        //            //    string[] filenames = Directory.GetFiles(testImg);

        //            //    // Zip up the files - From SharpZipLib Demo Code
        //            //    using (ZipOutputStream s = new ZipOutputStream(File.Create(testImg + "\\test.zip")))
        //            //    {
        //            //        s.SetLevel(9); // 0-9, 9 being the highest level of compression

        //            //        byte[] buffer = new byte[4096];

        //            //        foreach (string file in filenames)
        //            //        {

        //            //            ZipEntry entry = new ZipEntry(System.IO.Path.GetFileName(file));

        //            //            entry.DateTime = DateTime.Now;
        //            //            s.PutNextEntry(entry);

        //            //            using (FileStream fs = File.OpenRead(file))
        //            //            {
        //            //                int sourceBytes;
        //            //                do
        //            //                {
        //            //                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
        //            //                    s.Write(buffer, 0, sourceBytes);

        //            //                } while (sourceBytes > 0);
        //            //            }
        //            //        }
        //            //        s.Finish();
        //            //        s.Close();
        //            //    }
        //            //}
        //            //catch (Exception ex)
        //            //{
        //            //}
        //            //var bytesImg = File.ReadAllBytes(imgPath);
        //            //obj.GetTextandXMLFromImage(bytesImg, "TestDoc", "F:/Files_OcrXml/TestDoc", 1, 1, "", "", "", "");



        //            string xmlPath = @"F:\document.xml";
        //            XmlDocument xDoc = new XmlDocument();
        //            xDoc.Load(xmlPath);

        //            xDoc.InnerXml = xDoc.InnerXml.Replace("w:", "");

        //            StringBuilder pdfText = new StringBuilder();

        //            //var imageRowList11 = xDoc.SelectNodes("//r");

        //            //foreach (XmlNode node in imageRowList11)
        //            //{
        //            //    //var childNodes = node.Cast<XmlNode>().Where(x => x.ChildNodes != null).Select(y => y.ChildNodes).ToList();

        //            //    foreach (var chNode in node.ChildNodes)
        //            //    {
        //            //        foreach (var chNode in node.ChildNodes)
        //            //    {

        //            //    }

        //            //    //if (node.Cast<XmlNode>().Any(x => x.ChildNodes != null && x.ChildNodes[0].Name.Equals("pict")))
        //            //    //{
        //            //    //    pdfText.Append(node.InnerText.Trim() + " ");
        //            //    //}
        //            //    //else
        //            //    //{
        //            //    //    pdfText.Append(node.InnerText.Trim() + " ");
        //            //    //}
        //            //}


        ////            var imageRowList = xDoc.SelectNodes("//p/descendant::pict/../..");

        ////            if (imageRowList != null && imageRowList.Count > 0)
        ////            {
        ////                foreach (XmlNode node in imageRowList)
        ////                {
        ////                    var yy = node.SelectNodes("descendant::t");

        ////                      foreach (XmlNode node in imageRowList)
        ////                {

        ////}
        ////                }
        ////            }

        //            string xmlText = "";


        //            //pdfText.Append("<img src=''>");
        //            //XmlDocument xDoc = new XmlDocument();
        //            //try
        //            //{
        //            //    using (StreamReader sr = new StreamReader(xmlPath))
        //            //    {
        //            //        xmlText = sr.ReadToEnd().Replace("w:", "");
        //            //    }

        //            //    xDoc.Load(xmlText);
        //            //}
        //            //catch
        //            //{

        //            //}


        //            //var imageRowList1 = xDoc.SelectNodes("//w:p");

        //            //var imageRowList2 = xDoc.SelectNodes("//w");

        //            //var imageRowList3 = xDoc.SelectNodes("//p");




        //        }

        //public List<Table> GetTableFromDocx(string docxPath)
        //{
        //    try
        //    {
        //        XmlDocument xDoc = new XmlDocument();
        //        xDoc.Load(docxPath);
        //        xDoc.InnerXml = xDoc.InnerXml.Replace("w:", "");

        //        StringBuilder pdfText = new StringBuilder();

        //        List<Table> tblObjList = new List<Table>();
        //        Table tblObj = null;
        //        List<Line> lineObj = null;

        //        var tblList = xDoc.SelectNodes("//tbl");

        //        if (tblList != null && tblList.Count > 0)
        //        {
        //            for (int i = 0; i < tblList.Count; i++)
        //            {
        //                tblObj = new Table();
        //                lineObj = new List<Line>();

        //                var rows = tblList[i].SelectNodes("descendant::tr");

        //                if (rows != null && rows.Count > 0)
        //                {
        //                    tblObj.RowCount = rows.Count;

        //                    for (int j = 0; j < rows.Count; j++)
        //                    {
        //                        var cols = rows[j].SelectNodes("descendant::tc");

        //                        if (cols != null && cols.Count > 0)
        //                        {
        //                            tblObj.ColumnCount = cols.Count;

        //                            for (int k = 0; k < cols.Count; k++)
        //                            {
        //                                var textList = cols[k].SelectNodes("descendant::r");

        //                                for (int l = 0; l < textList.Count; l++)
        //                                {
        //                                    lineObj.Add(new Line
        //                                    {
        //                                        LineText = textList[l].InnerText.Trim(),
        //                                        RowNum = j + 1,
        //                                        ColumnNum = k + 1
        //                                    });
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                tblObj.Lines = lineObj;
        //                tblObj.TableNum = i + 1;
        //                tblObj.PageNum = 1;
        //                tblObjList.Add(tblObj);
        //            }
        //        }

        //        return tblObjList;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //public void ExporttoExcel(Table tblList, string fileName)
        //{
        //    FileInfo objInfo = new FileInfo(fileName);
        //    if (!File.Exists(fileName))
        //    {
        //        using (ExcelPackage p = new ExcelPackage(objInfo))
        //        {
        //            p.Workbook.Worksheets.Add("Sample WorkSheet");
        //            ExcelWorksheet ws = p.Workbook.Worksheets[1];
        //            ws.Name = "Sample Worksheet"; 

        //            StringBuilder finalText = new StringBuilder();

        //            for (int i = 0; i < tblList.RowCount; i++)
        //            {
        //                var rowLines = tblList.Lines.Where(x => x.RowNum.Equals(i + 1)).ToList();

        //                for (int j = 0; j < tblList.ColumnCount; j++)
        //                {
        //                    var lineCols = rowLines.Where(x => x.ColumnNum.Equals(j + 1)).ToList();

        //                    if (lineCols.Count > 0)
        //                    {
        //                        for (int k = 0; k < lineCols.Count; k++)
        //                        {
        //                            finalText.Append(lineCols[k].LineText.Trim() + " ");
        //                        }
        //                        var innerCell = ws.Cells[i + 2, j + 1];
        //                        innerCell.Value = Convert.ToString(finalText);
        //                        finalText.Length = 0;
        //                    }
        //                }
        //            }
        //            p.Save();
        //        }
        //    }
        //}

        //public bool CreateImagesZip(string extractedFilesPath, string fileName)
        //{
        //    bool status = true;

        //    // zip up the files
        //    try
        //    {
        //        //\word\media

        //        string imageFolderPath = extractedFilesPath;

        //        string[] filenames = Directory.GetFiles(imageFolderPath);

        //        // Zip up the files - From SharpZipLib Demo Code
        //        using (ZipOutputStream s = new ZipOutputStream(File.Create(extractedFilesPath + "\\" + fileName + ".zip")))
        //        {
        //            s.SetLevel(9); // 0-9, 9 being the highest level of compression

        //            byte[] buffer = new byte[4096];

        //            foreach (string file in filenames)
        //            {
        //                ZipEntry entry = new ZipEntry(System.IO.Path.GetFileName(file));

        //                entry.DateTime = DateTime.Now;
        //                s.PutNextEntry(entry);

        //                using (FileStream fs = File.OpenRead(file))
        //                {
        //                    int sourceBytes;
        //                    do
        //                    {
        //                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
        //                        s.Write(buffer, 0, sourceBytes);

        //                    } while (sourceBytes > 0);
        //                }
        //            }
        //            s.Finish();
        //            s.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = false;
        //    }

        //    return status;
        //}

        public void Createtetml(string filePath)
        {
            if (filePath == null)
                return;

            //WriteLog("Generating tetml File............ Please Wait");
            //WriteLog("This Will Take Time Depending upon PDF Pages");

            string DirectoryPath = Directory.GetParent(filePath).ToString();
            string wordTETMLPath = DirectoryPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(filePath) + ".tetml";
            //tetFile = XmlFile;
            //string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks}\" -o \"" + XmlFile + "\" \"" + PDFFilePath + "\"";

            string strParameter =
                 "-m word --pageopt \"tetml={glyphdetails={geometry=true font=true}} contentanalysis={nopunctuationbreaks} clippingarea={cropbox}\" -o \"" +
                wordTETMLPath + "\" \"" + filePath + "\"";
            //string Img_Conversion_bat = @"D:\work\tet.exe";

            string Img_Conversion_bat = System.Configuration.ConfigurationSettings.AppSettings["TetPath"].ToString();
            Process pConvertTetml = new Process();
            pConvertTetml.StartInfo.UseShellExecute = false;
            pConvertTetml.StartInfo.RedirectStandardError = true;
            pConvertTetml.StartInfo.RedirectStandardOutput = true;
            pConvertTetml.StartInfo.CreateNoWindow = true;
            pConvertTetml.StartInfo.Arguments = strParameter;
            pConvertTetml.StartInfo.FileName = Img_Conversion_bat;
            pConvertTetml.Start();
            pConvertTetml.WaitForExit();
        }

        public void CreateNParatetml(string filePath)
        {
            if (filePath == null)
                return;

            //WriteLog("Generating tetml File............ Please Wait");
            //WriteLog("This Will Take Time Depending upon PDF Pages");

            string DirectoryPath = Directory.GetParent(filePath).ToString();
            string wordTETMLPath = DirectoryPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(filePath) + ".tetml";
            //tetFile = XmlFile;
            //string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks}\" -o \"" + XmlFile + "\" \"" + PDFFilePath + "\"";

            string strParameter =
                 "-m word --pageopt \"tetml={glyphdetails={geometry=true font=true}} structureanalysis={list=true} contentanalysis={nopunctuationbreaks} clippingarea={cropbox}\" -o \"" +
                wordTETMLPath + "\" \"" + filePath + "\"";
            //string Img_Conversion_bat = @"D:\work\tet.exe";

            string Img_Conversion_bat = System.Configuration.ConfigurationSettings.AppSettings["TetPath"].ToString();
            Process pConvertTetml = new Process();
            pConvertTetml.StartInfo.UseShellExecute = false;
            pConvertTetml.StartInfo.RedirectStandardError = true;
            pConvertTetml.StartInfo.RedirectStandardOutput = true;
            pConvertTetml.StartInfo.CreateNoWindow = true;
            pConvertTetml.StartInfo.Arguments = strParameter;
            pConvertTetml.StartInfo.FileName = Img_Conversion_bat;
            pConvertTetml.Start();
            pConvertTetml.WaitForExit();
        }

        
        public XmlDocument LoadXmlDocument(string xmlPath)
        {
            if ((xmlPath == "") || (xmlPath == null) || (!File.Exists(xmlPath)))
                return null;

            StreamReader strreader = new StreamReader(xmlPath);
            string xmlInnerText = strreader.ReadToEnd();
            strreader.Close();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlInnerText);

            return xmlDoc;
        }

        private XmlDocument LoadTetmlXmlDocument(string tetFilePath)
        {
            XmlDocument tetDoc = new XmlDocument();
            try
            {
                StreamReader sr = new StreamReader(tetFilePath);
                string xmlText = sr.ReadToEnd();
                sr.Close();
                string documentXML = Regex.Match(xmlText, "<Document.*?</Document>", RegexOptions.Singleline).ToString();
                tetDoc.LoadXml(documentXML);
            }
            catch
            {
                return null;
            }
            return tetDoc;
        }

        public string Get8Digits()
        {
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            return String.Format("{0:D8}", random);
        }

       

        private void ExtractImages(string pdfPath)
        {
            string dirPath = Directory.GetParent(pdfPath).ToString();

            string bookId = System.IO.Path.GetFileNameWithoutExtension(pdfPath);
            string outfilebase = dirPath + "\\" + bookId + "-1\\Image\\";

            if (!Directory.Exists(outfilebase))
                Directory.CreateDirectory(outfilebase);

            string strParameter = "--targetdir " + outfilebase + " --image " + pdfPath;
            //string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks}\" -o \"" + XmlFile + "\" \"" + PDFFilePath + "\"";
            //string Img_Conversion_bat = @"D:\work\tet.exe";
            string Img_Conversion_bat = "C:\\XSL\\tet.exe";
            Process pConvertTetml = new Process();
            pConvertTetml.StartInfo.UseShellExecute = false;
            pConvertTetml.StartInfo.RedirectStandardError = true;
            pConvertTetml.StartInfo.RedirectStandardOutput = true;
            pConvertTetml.StartInfo.CreateNoWindow = true;
            pConvertTetml.StartInfo.Arguments = strParameter;
            pConvertTetml.StartInfo.FileName = Img_Conversion_bat;
            pConvertTetml.Start();
            pConvertTetml.WaitForExit();
        }

        //public void Createtetml(string tetFile, string pdfPath)
        //{
        //    string tetmlExePath = @"C:\XSLBookMicro\tet.exe";

        //    string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} " +
        //        "contentanalysis={nopunctuationbreaks}\" -o \"" +
        //        tetFile.Replace(".pdf", ".tetml") + "\" \"" + pdfPath + "\"";
        //    Process pConvertTetml = new Process();
        //    pConvertTetml.StartInfo.UseShellExecute = false;
        //    pConvertTetml.StartInfo.RedirectStandardError = true;
        //    pConvertTetml.StartInfo.RedirectStandardOutput = true;
        //    pConvertTetml.StartInfo.CreateNoWindow = true;
        //    pConvertTetml.StartInfo.Arguments = strParameter;
        //    pConvertTetml.StartInfo.FileName = tetmlExePath;
        //    pConvertTetml.Start();
        //    pConvertTetml.WaitForExit();
        //}

        public void ReadTetml()
        {
            
        }

        

        #region |Events|

        //private void ExtractPages(string path, string fileName)
        //{
        //    string src = path + @"\" + fileName + "_pre.pdf";
        //    string dest = path + @"\" + fileName + ".pdf";

        //    File.Move(dest, src);

        //    PdfReader pdf = new PdfReader(src);

        //    PdfDictionary pageDict;
        //    PdfArray cropBox;
        //    PdfArray mediaBox;

        //    float letterWidth = PageSize.LETTER.Width;
        //    float letterHeight = PageSize.LETTER.Height;

        //    int pageCount = pdf.NumberOfPages;

        //    //for (int i = 1; i <= pageCount; i++)
        //    //{
        //    pageDict = pdf.GetPageN(1);
        //    cropBox = pageDict.GetAsArray(PdfName.CROPBOX);
        //    mediaBox = pageDict.GetAsArray(PdfName.MEDIABOX);

        //    cropBox[0] = new PdfNumber(30);
        //    cropBox[1] = new PdfNumber(40);
        //    cropBox[2] = new PdfNumber(letterWidth + 30);
        //    cropBox[3] = new PdfNumber(letterHeight + 40);

        //    mediaBox[0] = new PdfNumber(30);
        //    mediaBox[1] = new PdfNumber(40);
        //    mediaBox[2] = new PdfNumber(letterWidth + 30);
        //    mediaBox[3] = new PdfNumber(letterHeight + 40);

        //    pageDict.Put(PdfName.CROPBOX, cropBox);
        //    pageDict.Put(PdfName.MEDIABOX, mediaBox);
        //    //}

        //    PdfStamper stamper = new PdfStamper(pdf, new FileStream(dest, FileMode.Create));
        //    stamper.Close();

        //}

        //private void ExtractPages(string inputFile, string outputFile, int start, int end)
        //{
        //    // get input document
        //    PdfReader inputPdf = new PdfReader(inputFile);
        //    // retrieve the total number of pages
        //    int pageCount = inputPdf.NumberOfPages;
        //    if (end < start || end > pageCount)
        //    {
        //        end = pageCount;
        //    }

        //    //Pdf width and height is in points
        //    var pdfPage1 = inputPdf.GetPageSize(1);

        //    //Convert point to mm for use in xsl file
        //    double width = (pdfPage1.Width * 0.352777778);
        //    double height = pdfPage1.Height * 0.352777778;

        //    iTextSharp.text.Rectangle cropbox2 = inputPdf.GetCropBox(1);
        //    var box1 = inputPdf.GetPageSizeWithRotation(1);

        //    double top = Math.Round((box1.Top - cropbox2.Top) * 0.352777778, 3);
        //    double bottom = Math.Round(cropbox2.Bottom * 0.352777778, 3);
        //    double right = Math.Round((box1.Right - cropbox2.Right) * 0.352777778, 3);
        //    double left = Math.Round(cropbox2.Left * 0.352777778, 3);

        //    //var pgSize = new iTextSharp.text.Rectangle(myWidth, myHeight);
        //    //var doc = new iTextSharp.text.Document(pgSize, leftMargin, rightMargin, topMargin, bottomMargin);

        //    // load the input document
        //    Document inputDoc = new Document(inputPdf.GetCropBox(1));




        //    //PdfDictionary pdfDictionary = inputPdf.GetPageN(1);
        //    //PdfArray cropArray = new PdfArray();
        //    //iTextSharp.text.Rectangle cropbox = inputPdf.GetCropBox(1);

        //    //var box = inputPdf.GetPageSizeWithRotation(1);

        //    //cropArray.Add(new PdfNumber(top));
        //    //cropArray.Add(new PdfNumber(bottom));
        //    //cropArray.Add(new PdfNumber(right));
        //    //cropArray.Add(new PdfNumber(left));
        //    //pdfDictionary.Put(PdfName.CROPBOX, cropArray);
        //    //pdfDictionary.Put(PdfName.MEDIABOX, cropArray);
        //    //pdfDictionary.Put(PdfName.TRIMBOX, cropArray);
        //    //pdfDictionary.Put(PdfName.BLEEDBOX, cropArray);
        //    //var pdfPage = inputPdf.GetPageSize(pdfDictionary);

        //    //PdfArray mediabox = pdfDictionary.GetAsArray(PdfName.MEDIABOX);
        //    //PdfArray cropbox1 = pdfDictionary.GetAsArray(PdfName.CROPBOX);

        //    // create the filestream
        //    using (FileStream fs = new FileStream(outputFile, FileMode.Create))
        //    {
        //        // create the output writer
        //        PdfWriter outputWriter = PdfWriter.GetInstance(inputDoc, fs);
        //        inputDoc.Open();
        //        PdfContentByte cb1 = outputWriter.DirectContent;

        //        // copy pages from input to output document
        //        for (int i = start; i <= end; i++)
        //        {
        //            inputDoc.SetPageSize(box1);
        //            inputDoc.NewPage();

        //            PdfImportedPage page = outputWriter.GetImportedPage(inputPdf, i);

        //            int rotation = inputPdf.GetPageRotation(i);

        //            if (rotation == 90 || rotation == 270)
        //            {
        //                cb1.AddTemplate(page, 0, -1f, 1f, 0, 0, inputPdf.GetPageSizeWithRotation(i).Height);
        //            }
        //            else
        //            {
        //                cb1.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
        //            }
        //        }
        //        inputDoc.Close();
        //    }
        //}

        //private void ExtractPages(string path, string fileName)
        //{
        //    float width = 8.5f * 72;
        //    float height = 11f * 72;
        //    float tolerance = 1f;

        //    PdfReader reader = new PdfReader("source.pdf");

        //    Rectangle cropBox = reader.getCropBox(i);
        //    float widthToAdd = width - cropBox.getWidth();
        //    float heightToAdd = height - cropBox.getHeight();
        //    if (Math.abs(widthToAdd) > tolerance || Math.abs(heightToAdd) > tolerance)
        //    {
        //    float[] newBoxValues = new float[] { 
        //    cropBox.getLeft() - widthToAdd / 2,
        //    cropBox.getBottom() - heightToAdd / 2,
        //    cropBox.getRight() + widthToAdd / 2,
        //    cropBox.getTop() + heightToAdd / 2
        //    };
        //    PdfArray newBox = new PdfArray(newBoxValues);

        //    PdfDictionary pageDict = reader.getPageN(i);
        //    pageDict.put(PdfName.CROPBOX, newBox);
        //    pageDict.put(PdfName.MEDIABOX, newBox);
        //    }


        //    OutputStream out = new ByteArrayOutputStream();
        //    PdfStamper stamper = new PdfStamper(reader, out);
        //    stamper.close();

        //    byte byteArray[] = (((ByteArrayOutputStream)out).toByteArray()); 





        //    ByteArrayOutputStream outputStream = new ByteArrayOutputStream( );
        //    for (int i = 1; i <= reader.getNumberOfPages(); i++)
        //    {
        //    outputStream.write(reader.getPageContent(i));
        //    }
        //    PDDocument pdDocument = new PDDocument().load(outputStream.toByteArray( );)  



        //    PdfReader reader = new PdfReader("source.pdf");
        //    byte byteArray[] = reader.getPageContent(1); // page 1



        //    PdfReader reader = new PdfReader("source.pdf");
        //    int n = reader.getNumberOfPages();
        //    reader close();
        //    ByteArrayOutputStream boas;
        //    PdfStamper stamper;
        //    for (int i = 0; i < n; ) {
        //        reader = new PdfReader("source.pdf");
        //        reader.selectPages(String.valueOf(++i));
        //        baos = new ByteArrayOutputStream();
        //        stamper = new PdfStamper(reader, baos);
        //        stamper.close();
        //        doSomethingWithBytes(baos.toByteArray);
        //    }



        //}

        //private void ExtractPages(string inputFile, string outputFile, int start, int end)
        //{

        //    PdfReader reader = null;
        //    Document document = null;
        //    PdfCopy pdfCopyProvider = null;
        //    PdfImportedPage importedPage = null;

        //    try
        //    {
        //        // Intialize a new PdfReader instance with the contents of the source Pdf file:
        //        reader = new PdfReader(inputFile);

        //        // Capture the correct size and orientation for the page:
        //        document = new Document(reader.GetPageSizeWithRotation(1));

        //        // Initialize an instance of the PdfCopyClass with the source 
        //        // document and an output file stream:
        //        pdfCopyProvider = new PdfCopy(document,
        //            new System.IO.FileStream(outputFile, System.IO.FileMode.Create));

        //        document.Open();

        //        // Extract the desired page number:
        //        importedPage = pdfCopyProvider.GetImportedPage(reader, start);
        //        pdfCopyProvider.AddPage(importedPage);
        //        document.Close();
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        //public void test()
        //{

        //}

        //public void RenderText(iTextSharp.text.pdf.parser.TextRenderInfo renderInfo)
        //{
        //    var tt  = renderInfo.GetCharacterRenderInfos();
        //    //int g = renderInfo.GetColorNonStroke().G;
        //    //int b = renderInfo.GetColorNonStroke().B;

        //}

        protected void Page_Init(object sender, EventArgs e)
        {

            //PdfReader reader_FirstPdf = new PdfReader(pdf_of_FirstFile);





            string filePath = "";
            //Common.Createtetml(filePath);

            //PDFManipulation obj = new PDFManipulation(filePath);
            //ExtractPages(filePath, filePath, 1, 1);

            if (!Page.IsPostBack)
            {
                string countryName = "";
                HttpCookie myCookie = new HttpCookie("Region");
                myCookie = Request.Cookies["Region"];

                bool ServerStat = true;

                try
                {
                    // Read the cookie information and display it.
                    if (myCookie == null && (!Page.IsPostBack))
                    {
                        string sURL;
                        //lblTestIp.Text = GetIPAddress();
                        sURL = "https://freegeoip.net/xml/" + GetIPAddress();

                        WebRequest wrGETURL;
                        wrGETURL = WebRequest.Create(sURL);

                        WebProxy myProxy = new WebProxy("myproxy", 80);
                        myProxy.BypassProxyOnLocal = true;

                        wrGETURL.Proxy = WebProxy.GetDefaultProxy();

                        Stream objStream;
                        objStream = wrGETURL.GetResponse().GetResponseStream();

                        StreamReader objReader = new StreamReader(objStream);
                        string xmlText = objReader.ReadToEnd();
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(xmlText);
                        XmlNode country = xmlDoc.SelectSingleNode("//CountryName");

                        if (country.InnerText.Trim() != "")
                        {
                            ListItem newItem = new ListItem(country.InnerText, country.InnerText);
                            if (!ddlRegion.Items.Contains(newItem))
                            {
                                ddlRegion.Items.Add(newItem);
                                divRegionInfo.Visible = true;
                            }
                            ddlRegion.SelectedValue = country.InnerText;
                            countryName = country.InnerText.Trim().ToLower();
                        }
                        else
                        {
                            countryName = "other";
                        }

                        if (countryName.Equals("pakistan"))
                        {
                            //If Pakistan then show urdu videos
                            mvTrainingVideos.ActiveViewIndex = 0;
                            //lbtnUrdu.Visible = true;
                            //lbtnEnglish.Visible = true;

                            Session["CountryName"] = "pakistan";
                        }
                        else
                        {
                            mvTrainingVideos.ActiveViewIndex = 0;
                            Session["CountryName"] = "other";
                            lbtnEnglish.Visible = true;
                        }
                    }
                    else
                    {
                        if (myCookie != null)
                        {
                            string region = myCookie.Value;
                            divRegionInfo.Visible = false;
                            ddlRegion.SelectedValue = region;
                        }
                        if (ddlRegion.SelectedValue.ToLower().Equals("pakistan"))
                        {
                            //If Pakistan then show urdu videos
                            mvTrainingVideos.ActiveViewIndex = 0;
                            Session["CountryName"] = "pakistan";
                        }
                        else
                        {
                            mvTrainingVideos.ActiveViewIndex = 0;
                            Session["CountryName"] = "other";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ServerStat = false;
                }

                //If ip location fails then select english video
                if (!ServerStat)
                {
                    mvTrainingVideos.ActiveViewIndex = 0;
                    Session["CountryName"] = "other";
                }
            }//end isPostback
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                HttpCookie myCookie = new HttpCookie("Region");

                // Set the cookie value.
                myCookie.Value = ddlRegion.SelectedItem.Value;
                // Set the cookie expiration date.
                myCookie.Expires = DateTime.Now.AddYears(50); // For a cookie to effectively never expire

                // Add the cookie.
                Response.Cookies.Add(myCookie);

                divRegionInfo.Visible = false;

                //if (ddlRegion.SelectedValue.Equals("Asia") || ddlRegion.SelectedValue.ToLower().Equals("pakistan") || ddlRegion.SelectedValue.ToLower().Equals("india"))
                //{

                //    Response.Redirect("http://58.65.163.243:91/BookMicroTest/BookMicro.aspx");
                //}
                //else
                //{
                //    Response.Redirect("http://175.41.132.19/BookMicroTest/BookMicro.aspx");
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Reading Lines From OCR XML

        //public void StartOcrByService(byte[] fileBytes, string bookId, string fileSavingPath, string fileType)
        //{
        //    try
        //    {
        //        using (ocrService.GetText client = new ocrService.GetText())
        //        {
        //            client.Timeout = -1;
        //            string text = client.GetTextandXMLFromImage(fileBytes, bookId, fileSavingPath, fileType);

        //            string producedPdfPath = fileSavingPath + bookId + ".pdf";
        //            while (!File.Exists(producedPdfPath)) { }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //InsertExceptionMessage("Ocr is not started because of invalid length of file maller then 0", bookId, fileType, ex);
        //        //ShowMessage(MessageTypes.Error, Common.CommonApplicationErrorMessage());
        //    }
        //}

        private List<Para> GetLinesByPara_OcrXml(string xmlPath)
        {
            int pageNumber = 0;
            int lineNum = 0;
            int wordNum = 0;
            int para = 0;

            List<CharParams> list_Chars_CurrentLine = null;
            List<CharParams> list_Chars_NextLine = null;
            List<CharParams> list_Chars_PrevLine = null;

            List<OcrLine> list_Lines = null;
            List<OcrWord> list_LineWords = null;
            List<Para> list_Paras = new List<Para>();

            OcrLine ln = null;

            double top = 0;
            double bottom = 0;

            try
            {
                XmlDocument xmlDoc = LoadXml(xmlPath);
                XmlNodeList pages = xmlDoc.SelectNodes("//page");

                foreach (XmlNode page in pages)
                {
                    //list_Lines.Clear();
                    para = 0;
                    lineNum = 0;

                    pageNumber++;

                    XmlNodeList paras = page.SelectNodes("descendant::par");

                    if (paras != null)
                    {
                        if (paras.Count > 0)
                        {
                            //Iterate through all paras
                            for (int i = 0; i < paras.Count; i++)
                            {
                                para++;

                                list_Lines = new List<OcrLine>();

                                //Iterate through lines in a para
                                for (int line = 0; line < paras[i].ChildNodes.Count; line++)
                                {
                                    list_LineWords = new List<OcrWord>();
                                    ln = new OcrLine();
                                    lineNum++;

                                    list_Chars_CurrentLine = GetCharParamsOfLine(paras[i].ChildNodes[line]);
                                    list_LineWords = GetWordsFromLine(paras[i].ChildNodes[line], pageNumber, lineNum);

                                    if ((line + 1) < paras[i].ChildNodes.Count)
                                    {
                                        list_Chars_NextLine = GetCharParamsOfLine(paras[i].ChildNodes[line + 1]);
                                        GetCharParams_Bottom(list_Chars_CurrentLine, list_Chars_NextLine, out bottom);
                                    }
                                    else
                                    {
                                        list_Chars_NextLine = new List<CharParams>();
                                    }

                                    if ((i == 0) && (line == 0))
                                    {
                                        ln.DistanceTopLine = 0;
                                        ln.DistanceBottomLine = Math.Abs(bottom);
                                    }
                                    else if ((i == paras.Count - 1) && (line == paras[i].ChildNodes.Count - 1))
                                    {
                                        ln.DistanceTopLine = Math.Abs(top);
                                        ln.DistanceBottomLine = 0;
                                    }
                                    else
                                    {
                                        ln.DistanceTopLine = Math.Abs(top);
                                        ln.DistanceBottomLine = Math.Abs(bottom);
                                    }

                                    ln.Left = Convert.ToDouble(paras[i].ChildNodes[line].Attributes["l"].Value);
                                    ln.Right = Convert.ToDouble(paras[i].ChildNodes[line].Attributes["r"].Value);
                                    ln.Top = Convert.ToDouble(paras[i].ChildNodes[line].Attributes["t"].Value);
                                    ln.Bottom = Convert.ToDouble(paras[i].ChildNodes[line].Attributes["b"].Value);
                                    ln.Characters = list_Chars_CurrentLine;
                                    ln.Words = list_LineWords;
                                    ln.Line = GetLineFromChars(paras[i].ChildNodes[line]);
                                    ln.Para = para;
                                    ln.ConatinsEmphasisWords = false;

                                    list_Chars_PrevLine = list_Chars_CurrentLine;
                                    GetCharParams_Top(list_Chars_CurrentLine, list_Chars_PrevLine, out top);

                                    ln.LineWidth = Convert.ToDouble(paras[i].ChildNodes[line].Attributes["r"].Value) - Convert.ToDouble(paras[i].ChildNodes[line].Attributes["l"].Value);
                                    ln.LineNumber = lineNum;

                                    if (paras[i].ParentNode.ParentNode.ParentNode.ParentNode.Attributes.Count > 0)
                                    {
                                        if (paras[i].ParentNode.ParentNode.ParentNode.ParentNode.Attributes["blockType"].Value.Equals("Table"))
                                        {
                                            ln.Type = "table";
                                            ln.TableColumns = paras[i].ParentNode.ParentNode.ParentNode.ChildNodes.Count;
                                        }
                                    }

                                    list_Lines.Add(ln);
                                }
                                //end line iteration

                                list_Paras.Add(new Para
                                {
                                    Line = list_Lines,
                                    Page = pageNumber,
                                    PageHeight = Convert.ToDouble(page.Attributes["height"].Value),
                                    PageWidth = Convert.ToDouble(page.Attributes["width"].Value)
                                });
                            }//end all para iteration
                        }
                    }
                }

                return list_Paras;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public XmlDocument LoadXml(string xmlPath)
        {
            XmlDocument tetDoc = new XmlDocument();
            StreamReader sr = new StreamReader(xmlPath);
            string xmlText = sr.ReadToEnd();
            sr.Close();
            string documentXML = System.Text.RegularExpressions.Regex.Match(xmlText, "<document.*?</document>", System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
            string finalDocument = documentXML.Replace(" xmlns=\"http://www.abbyy.com/FineReader_xml/FineReader10-schema-v1.xml\" version=\"1.0\" producer=\"ABBYY FineReader Engine 11\" languages=\"\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.abbyy.com/FineReader_xml/FineReader10-schema-v1.xml http://www.abbyy.com/FineReader_xml/FineReader10-schema-v1.xml\"", "");
            tetDoc.LoadXml(finalDocument);
            return tetDoc;
        }

        public List<CharParams> GetCharParamsOfLine(XmlNode line)
        {
            List<CharParams> wordChars = new List<CharParams>();
            XmlNodeList charParam = line.SelectNodes("descendant::charParams");

            foreach (XmlNode chParams in charParam)
            {
                if (!(chParams.InnerText.Equals("")))
                {
                    wordChars.Add(new CharParams
                    {
                        Char = chParams.InnerText,
                        Top = Convert.ToDouble(chParams.Attributes["t"].Value),
                        Bottom = Convert.ToDouble(chParams.Attributes["b"].Value),
                        Left = Convert.ToDouble(chParams.Attributes["l"].Value),
                        Right = Convert.ToDouble(chParams.Attributes["r"].Value),
                        Height = (Convert.ToDouble(chParams.Attributes["b"].Value) - Convert.ToDouble(chParams.Attributes["t"].Value))
                    });
                }
            }

            return wordChars;
        }

        public void GetCharParams_Bottom(List<CharParams> list_Chars_CurrentLine, List<CharParams> list_Chars_NextLine, out double bottom)
        {
            bottom = 0;
            bool endLoop = false;
            bool charMatched = false;
            string[] matchingChars = { "a", "e", "o", "n", "s" };
            string selectedChar = null;

            if ((list_Chars_CurrentLine.Count > 0) && (list_Chars_NextLine.Count > 0))
            {
                //foreach (var character in matchingChars)
                //{
                //    if ((list_Chars_CurrentLine.Any(x => (x.Char == character))) && (list_Chars_NextLine.Any(x => (x.Char == character))))
                //    {
                //        selectedChar = character;
                //        charMatched = true;
                //        break;
                //    }
                //}

                //if (charMatched)
                //{
                //    var currentLine = list_Chars_CurrentLine.Where(x => x.Char == selectedChar).ToList();
                //    var nextLine = list_Chars_NextLine.Where(x => x.Char == selectedChar).ToList();

                //    bottom = Math.Abs(currentLine[0].Bottom - nextLine[0].Top);
                //}

                //else
                //{
                foreach (var currentLinechar in list_Chars_CurrentLine)
                {
                    //Only match alphabets
                    if (Regex.IsMatch(currentLinechar.Char, @"^[a-zA-Z]+$"))
                    {
                        if (list_Chars_NextLine.Any(x => (x.Char == currentLinechar.Char)))
                        {
                            foreach (var nextLineChar in list_Chars_NextLine)
                            {
                                if (currentLinechar.Char == nextLineChar.Char)
                                {
                                    bottom = Math.Abs(currentLinechar.Bottom - nextLineChar.Top);
                                    endLoop = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (endLoop)
                        break;
                }
                //}
            }
        }

        public void GetCharParams_Top(List<CharParams> list_Chars_CurrentLine, List<CharParams> list_Chars_PrevLine, out double top)
        {
            top = 0;
            bool endLoop = false;
            bool charMatched = false;
            string[] matchingChars = { "a", "e", "o", "n", "s" };
            string selectedChar = null;


            if ((list_Chars_CurrentLine.Count > 0) && (list_Chars_PrevLine.Count > 0))
            {
                //foreach (var character in matchingChars)
                //{
                //    if ((list_Chars_CurrentLine.Any(x => (x.Char == character))) && (list_Chars_PrevLine.Any(x => (x.Char == character))))
                //    {
                //        selectedChar = character;
                //        charMatched = true;
                //        break;
                //    }
                //}

                //if (charMatched)
                //{
                //    var currentLine = list_Chars_CurrentLine.Where(x => x.Char == selectedChar).ToList();
                //    var nextLine = list_Chars_PrevLine.Where(x => x.Char == selectedChar).ToList();

                //    top = Math.Abs(currentLine[0].Top - nextLine[0].Bottom);
                //}
                //else
                //{
                foreach (var currentLinechar in list_Chars_CurrentLine)
                {
                    //Only match alphabets
                    if (Regex.IsMatch(currentLinechar.Char, @"^[a-zA-Z]+$"))
                    {
                        if (list_Chars_PrevLine.Any(x => (x.Char == currentLinechar.Char)))
                        {
                            foreach (var prevLineChar in list_Chars_PrevLine)
                            {
                                if (currentLinechar.Char == prevLineChar.Char)
                                {
                                    top = Math.Abs(currentLinechar.Top - prevLineChar.Bottom);
                                    endLoop = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (endLoop)
                        break;
                }
                //}
            }
        }

        public string GetLineFromChars(XmlNode line)
        {
            StringBuilder sb = new StringBuilder();
            XmlNodeList charParam = line.SelectNodes("descendant::charParams");

            foreach (XmlNode chParams in charParam)
            {
                if (chParams.InnerText.Equals(""))
                {
                    sb.Append(chParams.InnerText + " ");
                }
                else
                {
                    sb.Append(chParams.InnerText);
                }
            }

            return Convert.ToString(sb);
        }

        public List<OcrWord> GetWordsFromLine(XmlNode line, int pageNum, int lineNum)
        {
            List<OcrWord> lineWords = new List<OcrWord>();
            List<CharParams> wordChars = new List<CharParams>();
            XmlNodeList charParam = line.SelectNodes("descendant::charParams");
            StringBuilder characters = new StringBuilder();

            foreach (XmlNode chParams in charParam)
            {
                if (!chParams.InnerText.Equals(""))
                {
                    characters.Append(chParams.InnerText);
                }
                else
                {
                    lineWords.Add(new OcrWord
                    {
                        Word = Convert.ToString(characters),
                        Page = pageNum,
                        LineNumber = lineNum
                    });

                    characters.Length = 0;
                }
            }

            return lineWords;
        }

        #endregion
        //private XmlDocument MissingFontsValidating()
        //{
        //    try
        //    {
        //        string matchedText = "";
        //        string xmlPath1 = @"D:/Cid Book1/2/981.xml";
        //        Common obj = new Common();
        //        XmlDocument xmldoc = obj.LoadXmlDocument(xmlPath1);
        //        XmlNodeList uparas = xmldoc.SelectNodes("//upara");
        //        //string directory = Path.GetDirectoryName(PDFFile);
        //        //CommonClass objCommon = new CommonClass();
        //        System.Text.StringBuilder stBuilder = new System.Text.StringBuilder();
        //        //string NotEmbededDirectory = directory + "\\NotEmbededPages\\";
        //        //string matchedText = "";
        //        string pageno = "";
        //        foreach (XmlNode upara in uparas)
        //        {
        //            if (upara.OuterXml.Contains("NotEmbeded") && !upara.OuterXml.Contains("TextManuplated"))
        //            {
        //                //if (!Directory.Exists(NotEmbededDirectory))
        //                //{
        //                //    Directory.CreateDirectory(NotEmbededDirectory);
        //                //}

        //                for (int i = 0; i < upara.ChildNodes.Count; i++)
        //                {
        //                    if (upara.ChildNodes[i].Name.Equals("ln"))
        //                    {
        //                        pageno = upara.ChildNodes[i].Attributes["page"].Value;
        //                        break;
        //                    }
        //                }
        //                stBuilder = new StringBuilder();
        //                //if (!File.Exists(NotEmbededDirectory + "Page-" + pageno + ".txt"))
        //                //{
        //                //objCommon.ExtractPages(PDFFile, NotEmbededDirectory + "Page-" + pageno + ".pdf", Convert.ToInt32(pageno), Convert.ToInt32(pageno));

        //                string extractedPdfPath = "Page-" + pageno + ".pdf";

        //                //byte[] pdfBytes = File.ReadAllBytes(extractedPdfPath);
        //                //StartOcrByService(pdfBytes, "Page-" + pageno, NotEmbededDirectory, "");
        //                string xmlPath = @"D:/Cid Book1/2/Page-1.xml";
        //                if (File.Exists(xmlPath))
        //                {
        //                    //Get paras list from whole ocr xml
        //                    List<Para> list_Lines_ByPara = GetLinesByPara_OcrXml(xmlPath);

        //                    if (list_Lines_ByPara != null)
        //                    {
        //                        if (list_Lines_ByPara.Count > 0)
        //                        {
        //                            foreach (var para in list_Lines_ByPara)
        //                            {
        //                                foreach (var line in para.Line)
        //                                {
        //                                    stBuilder.Append(" </br>" + line.Line + " ");
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                XmlNodeList nodeLines = xmldoc.SelectNodes("//ln[@page='" + pageno + "']");

        //                var list = nodeLines.Cast<XmlNode>().Where(x => x.Attributes["fonttype"].Value.Equals("NotEmbeded")).ToList();

        //                for (int line = 0; line < list.Count; line++)
        //                {
        //                    //foreach (XmlNode line in list)
        //                    //{
        //                    //if (line.Attributes["page"]!= null)
        //                    //{
        //                    //    pageno = line.Attributes["page"].Value;
        //                    //}

        //                    var allNodes = nodeLines.Cast<XmlNode>().ToList();
        //                    int index = 0;
        //                    XmlNode previousNode = null;
        //                    XmlNode nextNode = null;
        //                    int temp = 0;

        //                    for (int i = 0; i < allNodes.Count; i++)
        //                    {
        //                        if (allNodes[i].InnerText.Trim().Equals(list[line].InnerText.Trim()))
        //                        {
        //                            temp = i;

        //                            while (allNodes[temp].Attributes["fonttype"].Value.Equals("NotEmbeded"))
        //                            {
        //                                temp--;
        //                            }
        //                            if (temp > 0)
        //                                previousNode = allNodes[temp];

        //                            temp = i;

        //                            while (allNodes[temp].Attributes["fonttype"].Value.Equals("NotEmbeded"))
        //                            {
        //                                temp++;
        //                            }
        //                            if (temp < allNodes.Count)
        //                                nextNode = allNodes[temp];

        //                            break;
        //                        }
        //                    }

        //                    string previousLineText = previousNode == null ? "" : previousNode.InnerText;
        //                    string nextLineText = nextNode == null ? "" : nextNode.InnerText;

        //                    string preLine = "";
        //                    string postLine = "";

        //                    if (previousLineText != null)
        //                    {
        //                        List<string> prevLineWords = Regex.Split(previousLineText, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
        //                        List<string> nextLineWords = Regex.Split(nextLineText, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

        //                        if (prevLineWords != null)
        //                        {
        //                            if (prevLineWords.Count > 0)
        //                            {
        //                                for (int k = 1; k < prevLineWords.Count; k++)
        //                                {
        //                                    if (k == 4) break;
        //                                    preLine = prevLineWords[prevLineWords.Count - k] + " " + preLine;
        //                                }
        //                                preLine = preLine.Remove(preLine.Length - 1);
        //                            }
        //                        }
        //                        if (nextLineWords != null)
        //                        {
        //                            if (nextLineWords.Count > 0)
        //                            {
        //                                for (int k = 0; k < nextLineWords.Count; k++)
        //                                {
        //                                    if (k == 3) break;
        //                                    postLine = postLine + nextLineWords[k] + " ";
        //                                }
        //                                postLine = postLine.Remove(postLine.Length - 1);
        //                            }
        //                        }
        //                    }

        //                    string RegexPattren = preLine.Replace("(", "\\(").Replace(")", "\\)") + "[\\w\\W\\S\\s\\s\\D':;\"<>,.?]*" +
        //                                                      postLine.Replace("(", "\\(").Replace(")", "\\)");
        //                    matchedText = Regex.Match(stBuilder.ToString(), RegexPattren).Value;
        //                    matchedText = string.IsNullOrEmpty(preLine) ? matchedText : matchedText.Replace(preLine, "");
        //                    matchedText = string.IsNullOrEmpty(postLine) ? matchedText : matchedText.Replace(postLine, "");
        //                    List<string> matchedList = Regex.Split(matchedText, "</br>").Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();

        //                    //if ("¬")¬  ¬

        //                    List<string> finalLines = new List<string>();
        //                    int hyphenLineNum = 0;

        //                    //Combine hyphen's word with previous line text
        //                    for (int m = 0; m < matchedList.Count; m++)
        //                    {
        //                        string matchedLn = matchedList[m].Trim();

        //                        if (Convert.ToString(matchedLn[matchedLn.Length - 1]) == "¬")
        //                            hyphenLineNum = m;

        //                        else if (hyphenLineNum > 0 && m - 1 >= 0)
        //                        {
        //                            finalLines.Add(matchedList[m - 1].Trim().Replace("¬", "") + matchedList[m].Trim().Replace("¬", ""));
        //                            hyphenLineNum = 0;
        //                        }
        //                        else 
        //                            finalLines.Add(matchedLn);
        //                    }
        //                    //end

        //                    for (int k = 0; k < finalLines.Count; k++)
        //                    {
        //                        XmlElement linetoInsert = xmldoc.CreateElement("ln");
        //                        foreach (XmlAttribute attr in list[line].Attributes)
        //                        {
        //                            linetoInsert.SetAttribute(attr.Name, attr.Value);
        //                        }
        //                        linetoInsert.Attributes["page"].Value = pageno;
        //                        XmlAttribute AttribTextManuplated = xmldoc.CreateAttribute("TextManuplated");
        //                        linetoInsert.Attributes.Append(AttribTextManuplated);
        //                        linetoInsert.InnerText = finalLines[k];
        //                        if (list[line].ParentNode != null)
        //                        {
        //                            list[line].ParentNode.InsertBefore(linetoInsert, list[line]);
        //                        }

        //                        if (k > 0) 
        //                            line++;
        //                    }
        //                }
        //                XmlNodeList nodeLinestoDel = xmldoc.SelectNodes("//ln[@page=\"" + pageno + "\" and @ fonttype=\"NotEmbeded\"]");
        //                for (int l = 0; l < nodeLinestoDel.Count; l++)
        //                {
        //                    if (nodeLinestoDel[l].Attributes["TextManuplated"] == null)
        //                    {
        //                        nodeLinestoDel[l].ParentNode.RemoveChild(nodeLinestoDel[l]);
        //                    }
        //                }
        //            }
        //        }

        //        return xmldoc;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private XmlDocument MissingFontsValidating()
        {
            try
            {
                string matchedText = "";
                string autoMapServiceXml = @"D:/Cid Book1/2/981.xml";
                Common obj = new Common();
                XmlDocument xmldoc = obj.LoadXmlDocument(autoMapServiceXml);

                List<string> pageNumList = xmldoc.Cast<XmlNode>().Where(x => x.Attributes["page"] != null)
                                                                 .Select(y => Convert.ToString(y.Attributes["page"].Value)).ToList();

                if (pageNumList != null)
                {
                    if (pageNumList.Count > 0)
                    {
                        foreach (var pageNum in pageNumList)
                        {
                            //Read all lines from ocr xml
                            string ocrXmlLines = GetOcrXmlLines(pageNum);

                            XmlNodeList nodeLines = xmldoc.SelectNodes("//ln[@page='" + pageNum + "']");

                            var cidFontLines = nodeLines.Cast<XmlNode>().Where(x => x.Attributes["fonttype"].Value.Equals("NotEmbeded")).ToList();

                            for (int line = 0; line < cidFontLines.Count; line++)
                            {
                                var allNodes = nodeLines.Cast<XmlNode>().ToList();
                                XmlNode previousNode = null;
                                XmlNode nextNode = null;
                                int temp = 0;

                                for (int i = 0; i < allNodes.Count; i++)
                                {
                                    if (allNodes[i].InnerText.Trim().Equals(cidFontLines[line].InnerText.Trim()))
                                    {
                                        temp = i;

                                        while (allNodes[temp].Attributes["fonttype"].Value.Equals("NotEmbeded"))
                                        {
                                            if (temp == 0)
                                                break;

                                            if (temp > 0)
                                                temp--;
                                        }
                                        if (temp > 0)
                                            previousNode = allNodes[temp];

                                        temp = i;

                                        while (allNodes[temp].Attributes["fonttype"].Value.Equals("NotEmbeded"))
                                        {
                                            if (temp == allNodes.Count - 1)
                                                break;

                                            if (temp < allNodes.Count)
                                                temp++;
                                        }
                                        if (temp < allNodes.Count - 1)
                                            nextNode = allNodes[temp];

                                        break;
                                    }
                                }

                                string previousLineText = previousNode == null ? "" : previousNode.InnerText;
                                string nextLineText = nextNode == null ? "" : nextNode.InnerText;

                                string preLine = "";
                                string postLine = "";

                                if (previousLineText != null)
                                {
                                    List<string> prevLineWords = Regex.Split(previousLineText, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
                                    List<string> nextLineWords = Regex.Split(nextLineText, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

                                    if (prevLineWords != null)
                                    {
                                        if (prevLineWords.Count > 0)
                                        {
                                            if (prevLineWords.Count == 1)
                                                preLine = prevLineWords[0];

                                            else if (prevLineWords.Count > 1)
                                            {
                                                for (int k = 1; k < prevLineWords.Count; k++)
                                                {
                                                    if (k == 4) break;
                                                    preLine = prevLineWords[prevLineWords.Count - k] + " " + preLine;
                                                }
                                                preLine = preLine.Remove(preLine.Length - 1);
                                            }
                                        }
                                    }
                                    if (nextLineWords != null)
                                    {
                                        if (nextLineWords.Count > 0)
                                        {
                                            if (nextLineWords.Count == 1)
                                                postLine = nextLineWords[0];

                                            else
                                            {
                                                for (int k = 0; k < nextLineWords.Count; k++)
                                                {
                                                    if (k == 3) break;
                                                    postLine = postLine + nextLineWords[k] + " ";
                                                }
                                                postLine = postLine.Remove(postLine.Length - 1);
                                            }
                                        }
                                    }
                                }

                                string RegexPattren = preLine.Replace("(", "\\(").Replace(")", "\\)") +
                                                      "[\\w\\W\\S\\s\\s\\D':;\"<>,.?]*" +
                                                      postLine.Replace("(", "\\(").Replace(")", "\\)");
                                matchedText = Regex.Match(ocrXmlLines, RegexPattren).Value;
                                matchedText = string.IsNullOrEmpty(preLine) ? matchedText : matchedText.Replace(preLine, "");
                                matchedText = string.IsNullOrEmpty(postLine) ? matchedText : matchedText.Replace(postLine, "");
                                List<string> matchedList = Regex.Split(matchedText, "</br>").Where(x => !string.IsNullOrEmpty(x.Trim())).ToList();

                                //if ("¬")¬  ¬

                                List<string> finalLines = new List<string>();
                                int hyphenLineNum = 0;

                                //Combine hyphen's word with previous line text
                                for (int m = 0; m < matchedList.Count; m++)
                                {
                                    string matchedLn = matchedList[m].Trim();

                                    if (Convert.ToString(matchedLn[matchedLn.Length - 1]) == "¬")
                                        hyphenLineNum = m;

                                    else if (hyphenLineNum > 0 && m - 1 >= 0)
                                    {
                                        finalLines.Add(matchedList[m - 1].Trim().Replace("¬", "") +
                                                       matchedList[m].Trim().Replace("¬", ""));
                                        hyphenLineNum = 0;
                                    }
                                    else
                                        finalLines.Add(matchedLn);
                                }
                                //end

                                for (int k = 0; k < finalLines.Count; k++)
                                {
                                    XmlElement linetoInsert = xmldoc.CreateElement("ln");
                                    foreach (XmlAttribute attr in cidFontLines[line].Attributes)
                                    {
                                        linetoInsert.SetAttribute(attr.Name, attr.Value);
                                    }
                                    linetoInsert.Attributes["page"].Value = pageNum;
                                    XmlAttribute AttribTextManuplated = xmldoc.CreateAttribute("TextManuplated");
                                    linetoInsert.Attributes.Append(AttribTextManuplated);
                                    linetoInsert.InnerText = finalLines[k];
                                    if (cidFontLines[line].ParentNode != null)
                                    {
                                        cidFontLines[line].ParentNode.InsertBefore(linetoInsert, cidFontLines[line]);
                                    }

                                    if (k > 0)
                                        line++;
                                }
                            }

                            //Delete old lines with NotEmbeded font from current page
                            XmlNodeList nodeLinestoDel = xmldoc.SelectNodes("//ln[@page=\"" + pageNum + "\" and @ fonttype=\"NotEmbeded\"]");
                            for (int l = 0; l < nodeLinestoDel.Count; l++)
                            {
                                if (nodeLinestoDel[l].Attributes["TextManuplated"] == null)
                                {
                                    nodeLinestoDel[l].ParentNode.RemoveChild(nodeLinestoDel[l]);
                                }
                            }
                            //end
                        }//end pageNum foreach
                    }
                }
                return xmldoc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetOcrXmlLines(string pageno)
        {
            StringBuilder stBuilder = new StringBuilder();

            //string extractedPdfPath = "Page-" + pageno + ".pdf";
            //byte[] pdfBytes = File.ReadAllBytes(extractedPdfPath);
            //StartOcrByService(pdfBytes, "Page-" + pageno, NotEmbededDirectory, "");
            string xmlPath = @"D:/Cid Book1/2/Page-1.xml";

            if (File.Exists(xmlPath))
            {
                //Get paras list from whole ocr xml
                List<Para> list_Lines_ByPara = GetLinesByPara_OcrXml(xmlPath);

                if (list_Lines_ByPara != null)
                {
                    if (list_Lines_ByPara.Count > 0)
                    {
                        foreach (var para in list_Lines_ByPara)
                        {
                            foreach (var line in para.Line)
                            {
                                stBuilder.Append(" </br>" + line.Line + " ");
                            }
                        }
                    }
                }
            }
            return Convert.ToString(stBuilder);
        }



        protected void lbtnUrdu_Click(object sender, EventArgs e)
        {
            mvTrainingVideos.ActiveViewIndex = 0;
            lbtnUrdu.Attributes.Add("Style", "color:#CCCCCC");
            lbtnEnglish.Attributes.Add("Style", "color:#0099ff");
            lbtnEnglish.Enabled = true;
            lbtnUrdu.Enabled = false;
        }

        protected void lblEnglish_Click(object sender, EventArgs e)
        {
            mvTrainingVideos.ActiveViewIndex = 0;
            lbtnUrdu.Attributes.Add("Style", "color:#0099ff");
            lbtnEnglish.Attributes.Add("Style", "color:#CCCCCC");
            lbtnUrdu.Enabled = true;
            lbtnEnglish.Enabled = false;
        }

        protected void lbtnForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForgotPassword.aspx");
        }

        public string GetIPAddress()
        {
            //var aa = Request.ServerVariables["REMOTE_ADDR"].ToString();
            //var bb = Request.ServerVariables["http_user_agent"].ToString();
            //var cc = Request.ServerVariables["request_method"].ToString();
            //var server_name = Request.ServerVariables["server_name"].ToString();
            //var dd = Request.ServerVariables["server_port"].ToString();
            //var ee = Request.ServerVariables["server_software"].ToString();
            //var ff= Request.ServerVariables["REMOTE_HOST"].ToString();

            string ipaddress;

            try
            {
                ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (ipaddress == "" || ipaddress == null)
                    ipaddress = Request.ServerVariables["REMOTE_ADDR"];

                return ipaddress;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public string GetCountryFromIp()
        //{
        //    string userHostIpAddress = "58.65.163.243";
        //    IPAddress ip;
        //    string country = null;
        //    if (IPAddress.TryParse(userHostIpAddress, out ip))
        //    {
        //        country = ip.Country(); // return value: UNITED STATES
        //        string iso3166TwoLetterCode = ip.Iso3166TwoLetterCode(); // return value: US

        //        //if ((country.Trim().ToLower().Equals("pakistan")) || (country.Trim().ToLower().Equals("india")))
        //        //{
        //        //    videos.Visible = false;
        //        //}
        //    }
        //    return country;
        //}

        private void loadLatestXml_New(string email, string rndNumber)
        {

            string xmlFile = "";
            if (email != "")
            {
                //xmlFile = objMyDBClass.MainDirPhyPath + "/Tests/" + email + "/" + rndNumber + "/" + rndNumber + ".rhyw";
                xmlFile = objMyDBClass.MainDirPhyPath + "\\Tests\\" + email + "/ComparisonTests/" + rndNumber + "/" + rndNumber + "-1/Comparison/" + rndNumber + "-1.rhyw";
            }
            else
            {
                xmlFile = objMyDBClass.MainDirPhyPath + "/" + email + "/" + rndNumber + ".rhyw";
            }
            objGlobal.XMLPath = xmlFile;
            Session["XMLPath"] = objGlobal.XMLPath;
            objGlobal.PBPDocument = new System.Xml.XmlDocument();
            objGlobal.LoadXml();
            Session["PBPDocument"] = objGlobal.PBPDocument;
        }

        private int GetTotalMistakes(string email, string rndNumber)
        {
            loadLatestXml_New(email, rndNumber);
            XmlNodeList correctedNodes = objGlobal.PBPDocument.SelectNodes(@"//*[@PDFmistake]");
            int totalOccurences = correctedNodes.Count;
            return totalOccurences;
        }

        //private void showSuccessMessage(string message)
        //{
        //    if (message != "")
        //    {
        //        divSuccess.Visible = true;
        //        lblSuccess.Text = message;
        //        lblSuccess.ForeColor = Color.Blue;
        //    }
        //    else
        //    {
        //        divSuccess.Visible = false;
        //    }
        //}

        protected void imgbtnTest_Click(object sender, EventArgs e)
        {
            //string email = string.Empty;
            //string username = string.Empty; 
            StringBuilder notDeletedTest = new StringBuilder();
            string[] temp = null;
            //string[] allTestNumbers = { "120", "121", "122", "123" };
            
            List<string> allTestNumbers = new List<string>();

            if (!string.IsNullOrEmpty(Convert.ToString(Session["TestType"])))
            {
                //allTestNumbers.Add("124");

                //username = Convert.ToString(Session["OnlineTestUser"]);
                //email = Convert.ToString(Session["Email"]);

                //imgbtnStartTest.Src = "~/img/button_startTest-upgraded.jpg";
                
            }
            else
            {
                allTestNumbers.Add("120");
                allTestNumbers.Add("121");
                allTestNumbers.Add("122");
                allTestNumbers.Add("123");
                //username = txtName.Text.Trim();
                //email = txtEmail.Text.Trim();
            }

            
            int newTestNumber = 0;
            string username = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string testType = "Comparison";
            int totalOnlineUsers = Convert.ToInt32(objMyDBClass.OnlineUsers());
            if (totalOnlineUsers < 500)
            {
                //Passed users cannot give tests of same type again
                string userDetails = objMyDBClass.CheckTestStatus(username, email, testType);

                if ((userDetails != null) && (userDetails != ""))
                {
                    ucShowMessage1.ShowMessage(MessageTypes.Error, "Test can't be started because you have already passed the test.");
                    //imgbtnStartTest.Enabled = false;
                    return;
                }
                //end

                //Passed users cannot give tests of different types in next 24 hours
                var testDates = objMyDBClass.GetTestDate(username, email);

                if ((testDates != null) && (testDates.Count > 0))
                {
                    if (DateTime.Now <= Convert.ToDateTime(testDates[0]).AddHours(24))
                    {
                        ucShowMessage1.ShowMessage(MessageTypes.Error, "Test can't be started because only one attempt per day is allowed.");
                        return;
                    }
                }
                //end

                if (((username != null) && (username != "")) && ((email != null) && (email != "")))
                {
                    objMyDBClass.InsertOnlineUsers(email);
                    Session["OnlineTestUser"] = username;
                    Session["email"] = email;

                    string path = objMyDBClass.MainDirPhyPath + @"\\Tests\\Original\\";
                    string ipAddress = GetIPAddress();

                    //Resriction on using same IP Address for different email addresses
                    List<TestUser> list = objMyDBClass.GetUserDetails_ByIPAdress(username, email, ipAddress);
                    bool emailCheck = false;

                    if ((list != null) && (list.Count > 0))
                    {
                        foreach (var item in list)
                        {
                            if ((item.Email != email))
                            {
                                emailCheck = true;
                            }
                        }

                        if (emailCheck)
                        {
                            string value1 = objMyDBClass.InsertOnlineTest_IPAddress(username, email, ipAddress);
                        }
                    }
                    //end

                    List<string> emailId_List = objMyDBClass.GetEmailId_ByName(username);
                    bool check = false;

                    if ((emailId_List != null) && (emailId_List.Count > 0))
                    {
                        foreach (var item in emailId_List)
                        {
                            if (item.Trim().Equals(email))
                            {
                                check = true;
                                break;
                            }
                        }
                    }

                    Random rnd = new Random();
                    int rndNumber = 0;

                    rndNumber = rnd.Next(120, 123);

                    //If user has previously take a test then select next test number
                    if (check)
                    {
                        int oldTestName = objMyDBClass.GetTestName(username, email);

                        if (oldTestName == rndNumber)
                        {
                            oldTestName++;

                            if (oldTestName >= 123)
                                rndNumber = 120;

                            else
                                rndNumber = oldTestName;
                        }
                    }
                    //end


                    string userDir_Path = objMyDBClass.MainDirPhyPath + "\\Tests\\" + email;

                    //If user directory not exists then create it
                    if (!Directory.Exists(userDir_Path))
                    {
                        Directory.CreateDirectory(userDir_Path);
                    }

                    //Delete all previous test in comparisonTest Folder.If any test can't be deleted and current test number is 
                    //same as that test then select some different test
                    string userCompTestsDir = objMyDBClass.MainDirPhyPath + "\\Tests\\" + email + "/ComparisonTests/";

                    DirectoryInfo di = new DirectoryInfo(userCompTestsDir);

                    //If old test directories exists
                    if (di.Exists)
                    {
                        DirectoryInfo[] oldTests = di.GetDirectories();

                        foreach (var test in oldTests)
                        {
                            notDeletedTest.Append(DeleteDirectories(test.FullName) + ",");
                        }

                        temp = notDeletedTest.ToString().Split(',');

                        if ((temp != null) && (temp.Length > 0))
                        {
                            temp = temp.Where(x => (!string.IsNullOrEmpty(x))).ToArray();
                        }

                        bool contains = temp.Any(s => s.Contains(Convert.ToString(rndNumber)));
                        if (contains)
                        {
                            var unMatched = (from p in allTestNumbers
                                             where !(from test in temp
                                                     select test).Contains(p)
                                             select p);

                            newTestNumber = Convert.ToInt32(unMatched.First());
                        }
                        else
                        {
                            newTestNumber = rndNumber;
                        }
                    }
                    //No old test directory
                    else
                    {
                        newTestNumber = rndNumber;
                    }

                    string userDir = objMyDBClass.MainDirPhyPath + "\\Tests\\" + email + "/ComparisonTests/" + newTestNumber;
                    string orignalDir = Common.GetComparisonEntryTestFiles_InputFilesPath() + "\\" + Convert.ToString(newTestNumber);

                    Directory.CreateDirectory(userDir);
                    DirectoryInfo dInfo = CopyTo(new DirectoryInfo(orignalDir), userDir);
                    //File.Delete(userDir + "\\" + newTestNumber + ".rhyw");
                    //File.Copy(userDir + "\\" + rndNumber + "-1" + "\\TaggingUntagged\\" + rndNumber + "-1.rhyw", userDir + "\\" + rndNumber + ".rhyw");
                    File.Copy(userDir + "\\" + newTestNumber + "-1" + "\\TaggingUntagged\\" + newTestNumber + "-1.rhyw", userDir + "\\" + newTestNumber + "-1" + "\\Comparison\\" + newTestNumber + "-1.rhyw");
                    File.Copy(userDir + "\\" + newTestNumber + "-1" + "\\TaggingUntagged\\" + newTestNumber + "-1.pdf", userDir + "\\" + newTestNumber + "-1" + "\\Comparison\\" + newTestNumber + "-1.pdf");

                    Session["TotalMarks"] = GetTotalMistakes(email, Convert.ToString(newTestNumber));
                    Session["TestName"] = Convert.ToString(newTestNumber);

                    Session["rhywFile"] = newTestNumber + "-1.rhyw";
                    Session["SrcPDF"] = newTestNumber + "-1.pdf";
                    Session["MainXMLFilePath"] = userDir + "\\" + newTestNumber + "-1" + "\\Comparison\\" + newTestNumber + "-1.rhyw";

                    string value = objMyDBClass.InsertOnlineTest(username, email, Convert.ToString(newTestNumber), ipAddress, Convert.ToInt32(Session["TotalMarks"]), testType);

                    if (Convert.ToInt32(value) >= 0)
                    {
                        //Moving xsl files from C to UserDirectory if not exists 
                        try
                        {
                            int requireUpdation = objMyDBClass.CheckOperationalFiles_Updation();

                            int insertedRow = 0;

                            string userDir_StartTest = Common.GetDirectoryPath() + "User Files/Tests/" + email.Trim() + "/XSL";

                            if (!Directory.Exists(userDir_StartTest))
                            {
                                insertedRow = objMyDBClass.InsertOperationalFiles_StartTest(email.Trim());
                            }

                            string orignalXSLFiles = Common.GetXSLSourcePath();

                            if (insertedRow > 0)
                            {
                                if (!Directory.Exists(userDir_StartTest))
                                    Directory.CreateDirectory(userDir_StartTest);

                                DirectoryInfo dInfo_StartTest = CopyTo(new DirectoryInfo(orignalXSLFiles), userDir_StartTest, true);
                            }
                            else if (requireUpdation > 0)
                            {
                                if (Directory.Exists(userDir_StartTest))
                                    Directory.Delete(userDir_StartTest, true);

                                Directory.CreateDirectory(userDir_StartTest);

                                DirectoryInfo dInfo_StartTest = CopyTo(new DirectoryInfo(orignalXSLFiles), userDir_StartTest, true);
                            }
                        }
                        catch (Exception)
                        {
                            ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured.");
                            return;
                        }
                        //end moving files

                        //Response.Redirect("Step2.aspx?username=" + username + "&bid=" + rndNumber, false);
                        Response.Redirect("ComparisonPreProcess.aspx?type=comparisonEntryTest", true);
                    }
                    else if (Convert.ToInt32(value) < 0)
                    {
                        ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured.");
                    }
                }
            }
            else
            {
                ucShowMessage1.ShowMessage(MessageTypes.Error, "There is big traffic, please try later.");
            }
        }

        public static string DeleteDirectories(string userDir_Path)
        {
            try
            {
                if (Directory.Exists(userDir_Path))
                    Directory.Delete(userDir_Path, true);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return System.IO.Path.GetFileName(userDir_Path);
            }

            //file is deleted
            return "";
        }

        //private void showMessage(string message)
        //{
        //    if (message != "")
        //    {
        //        DivError.Visible = true;
        //        lblError.Text = message;
        //    }
        //    else
        //    {
        //        DivError.Visible = false;
        //    }
        //}

        //public int GenerateTestName(string userName, string email, int totalFiles)
        //{
        //    int testName = Convert.ToInt32(objMyDBClass.GetTestName(userName, email));

        //    Random rnd = new Random();
        //    int newRndNumber;
        //    int rndNumber = rnd.Next(224, 234);

        //    if (rndNumber != testName)
        //    {
        //        rndNumber = testName;
        //    }

        //    else if (rndNumber == testName)
        //    {
        //        for (int i = 0; i < totalFiles; ++i)
        //        {
        //            do
        //            {
        //                newRndNumber = rnd.Next(224, 234);
        //            } while (testName != newRndNumber);
        //        }
        //    }

        //    return rndNumber;
        //}

        public DirectoryInfo CopyTo(DirectoryInfo sourceDir, string destinationPath, bool overwrite = false)
        {
            var sourcePath = sourceDir.FullName;

            var destination = new DirectoryInfo(destinationPath);

            destination.Create();

            foreach (var sourceSubDirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(sourceSubDirPath.Replace(sourcePath, destinationPath));

            foreach (var file in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
                File.Copy(file, file.Replace(sourcePath, destinationPath), overwrite);

            return destination;
        }

        //private void showMessage(string message)
        //{
        //    if (message != "")
        //    {
        //        DivError.Visible = true;
        //        lblError.Text = message;
        //    }
        //    else
        //    {
        //        DivError.Visible = false;
        //    }
        //}

        protected void imgbtnLogin_Click(object sender, System.EventArgs e)
        {
            MyDBClass objMyDBClass = new MyDBClass();

            Session.Clear();

            Session["CountryName"] = "other";

            if (Convert.ToString(Session["LoginId"]) == "")
            {
                if (cbxRememberMe.Checked) //If user checks Stay signed in
                {
                    Response.Cookies["email"].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
                }
                else //If next time user don't want to save password
                {
                    Response.Cookies["email"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
                }

                TestUser user_Details = objMyDBClass.Validate_User(Convert.ToString(tbxEmail.Text), Convert.ToString(tbxPassword.Text));

                if (user_Details == null)
                {
                    //showMessage("The email or password you entered is incorrect. Please try again (make sure your caps lock is off).");
                    ucShowMessage1.ShowMessage(MessageTypes.Error, "The email or password you entered is incorrect. Please try again (make sure your caps lock is off).");
                    return;
                }

                Response.Cookies["email"].Value = tbxEmail.Text.Trim();
                Response.Cookies["Password"].Value = tbxPassword.Text.Trim();

                if (user_Details != null)
                {
                    if ((user_Details.UserId != null) && (user_Details.UserId != ""))
                    {
                        Session["LoginId"] = user_Details.UserId;
                        Session["UserDetail"] = user_Details;
                        Session["Email"] = tbxEmail.Text.Trim();
                        Session["UserRole"] = user_Details.UserType.ToLower().Trim();
                        Session["OnlineTestUser"] = user_Details.FullName;

                        if (!objMyDBClass.GetUserIsActiveStatus(user_Details.UserId))
                        {
                            //Session["OnlineTestUser"] = user_Details.FullName;
                            Session["email"] = tbxEmail.Text.Trim();

                            Response.Redirect("UserDetails.aspx", true);
                        }

                        //Moving xsl files from C to UserDirectory if not exists 
                        try
                        {
                            int requireUpdation = objMyDBClass.CheckOperationalFiles_Updation();

                            int insertedRow = 0;

                            string userDir = Common.GetDirectoryPath() + "User Files/" + tbxEmail.Text.Trim() + "/XSL";

                            if (!Directory.Exists(userDir))
                            {
                                insertedRow = objMyDBClass.InsertOperationalFiles(user_Details.UserId);
                            }

                            string orignalDir = Common.GetXSLSourcePath();

                            if (insertedRow > 0)
                            {
                                if (!Directory.Exists(userDir))
                                    Directory.CreateDirectory(userDir);

                                DirectoryInfo dInfo = CopyTo(new DirectoryInfo(orignalDir), userDir, true);
                            }
                            else if (requireUpdation > 0)
                            {
                                if (Directory.Exists(userDir))
                                    Directory.Delete(userDir, true);

                                Directory.CreateDirectory(userDir);

                                DirectoryInfo dInfo = CopyTo(new DirectoryInfo(orignalDir), userDir, true);
                            }
                        }
                        catch (Exception)
                        {
                            ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured.");
                            return;
                        }
                        //end moving files

                        if ((user_Details.UserType.ToLower().Trim() == "1") || (user_Details.UserType.ToLower().Trim() == "2"))
                        {
                            Response.Redirect("OnlineTestUser.aspx", true);
                        }
                        //else if (user_Details.UserType.ToLower().Trim() == "7")
                        //{
                        //    Response.Redirect("OnlineTestAdmin.aspx?UserId=" + user_Details.UserId);
                        //}

                        //Admin user create tagging/untagging tasks
                        else if (user_Details.UserType.ToLower().Trim() == "5")
                        {
                            string id = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxEmail.Text.Trim()));
                            string pass = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxPassword.Text.Trim()));
                            string type = HttpUtility.UrlEncode(CommonClass.Encrypt(user_Details.UserType.Trim()));

                            Response.Redirect(string.Format("AdminPanel.aspx?id={1}&p={2}&t={3}", Request.Url.Host, id, pass, type), true);
                        }

                        //teamlead user perform mapping
                        else if (user_Details.UserType.ToLower().Trim() == "6" || user_Details.UserType.ToLower().Trim() == "7")
                        {
                            string id = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxEmail.Text.Trim()));
                            string pass = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxPassword.Text.Trim()));
                            string type = HttpUtility.UrlEncode(CommonClass.Encrypt(user_Details.UserType.Trim()));

                            Response.Redirect(string.Format("AdminPanel.aspx?id={1}&p={2}&t={3}", Request.Url.Host, id, pass, type), true);
                        }
                    }
                }
                else
                {
                    Response.Redirect("BookMicro.aspx");
                }
            }
        }

        #endregion

        protected void imgbtnStartTest_Click(object sender, ImageClickEventArgs e)
        {
            imgbtnTest_Click(sender, e);
            //showSuccessMessage("Test for current batch is Closed. Please try again once next Batch test is active.");
            //return;
        }
    }

    public class Para
    {
        public List<OcrLine> Line { get; set; }
        public int Page { get; set; }
        public double PageWidth { get; set; }
        public double PageHeight { get; set; }
    }

    public class OcrLine
    {
        public int Page { get; set; }
        public int Para { get; set; }
        public string Line { get; set; }
        public double Left { get; set; }
        public double Right { get; set; }
        public double Top { get; set; }
        public double Bottom { get; set; }
        public List<CharParams> Characters { get; set; }
        public List<OcrWord> Words { get; set; }
        public double DistanceTopLine { get; set; }
        public double DistanceBottomLine { get; set; }
        public double LineWidth { get; set; }
        public string FontSize { get; set; }
        public string Type { get; set; }
        public int TableColumns { get; set; }
        public bool ConatinsEmphasisWords { get; set; }
        public string Contained { get; set; }
        public string CommonChar { get; set; }
        public string CommonHeightChar { get; set; }
        public double CommonHeight { get; set; }
        public int LineNumber { get; set; }
        public double PageHeight { get; set; }
    }
    public class CharParams
    {
        public string Char { get; set; }
        public double Left { get; set; }
        public double Right { get; set; }
        public double Top { get; set; }
        public double Bottom { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public int Count { get; set; }
        public double Margin { get; set; }
        public string Type { get; set; }
    }

    public class OcrWord
    {
        public string Word { get; set; }
        public string EmphasisType { get; set; }
        public int LineNumber { get; set; }
        public int Page { get; set; }
    }


    //public class Table
    //{
    //    public int PageNum { get; set; }

    //    public int TableNum { get; set; }

    //    public int RowCount { get; set; }

    //    public int ColumnCount { get; set; }

    //    public List<Line> Lines { get; set; }
    //}

    //public class Line
    //{
    //    public int RowNum { get; set; }

    //    public int ColumnNum { get; set; }

    //    public string LineText { get; set; }
    //}
}
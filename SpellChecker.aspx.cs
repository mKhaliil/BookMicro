using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.IO;
using System.Xml;
using Keyoti.RapidSpell;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace Outsourcing_System
{
    public partial class SpellChecker1 : System.Web.UI.Page
    {
        #region |Fields and Properties|

        //  Microsoft.Office.Interop.Word.Application WordApp = new Microsoft.Office.Interop.Word.Application();
        RapidSpellChecker objSpell = new RapidSpellChecker("797D72717D7A23282A334C5047505035393F3D393E41418");
        ConversionClass objConversionClass = new ConversionClass();
        MyDBClass objMyDBClass = new MyDBClass();
        GlobalVar objGlobal = new GlobalVar();

        #endregion

        #region |Methods|

        private string HighLightMisSpelled(string path, string word)
        {

            //Path to where you want the file to output
            string[] splited = path.Split('/');
            String FileName = splited[splited.Length - 1];
            string outputFilePath = path.Replace(".pdf", "_Highlighted.pdf");
            string inputFilePath = path;
            //Path to where the pdf you want to modify is
             bool found = false;
            using (Stream inputPdfStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream outputPdfStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                //Opens the unmodified PDF for reading
                PdfReader reader = new PdfReader(inputPdfStream);
                //Creates a stamper to put an image on the original pdf
                reader.GetPageContent(4);
               
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    string pageText = PdfTextExtractor.GetTextFromPage(reader, i);
                    pageText = pageText.Replace("-", "").Replace("\r\n","").Replace("\n","");
                    if (pageText.ToLower().Contains(word.ToLower().Replace("-","")))
                    {
                        found = true;
                        List<string> lstcoordinates = getCoordinates(path, word);
                        var stamper = new PdfStamper(reader, outputPdfStream) { FormFlattening = true, FreeTextFlattening = true };
                        foreach (string coordinates in lstcoordinates)
                        {
                            string[] dimensions = coordinates.Split(' ');
                            float x = 0;
                            float y = 0;
                            float.TryParse(dimensions[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                            float.TryParse(dimensions[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y);

                            iTextSharp.text.Image objImage1 = iTextSharp.text.Image.GetInstance(new Bitmap(Convert.ToInt32(Math.Round(Convert.ToDouble(dimensions[2]))), 1), BaseColor.RED);

                            objImage1.SetAbsolutePosition(x, y - 2);

                            stamper.GetOverContent(i).AddImage(objImage1, true);

                        }
                        stamper.Close();
                    }
                    

                }


            }
            if (!found)
            {
                if (File.Exists(outputFilePath))
                {
                    File.Delete(outputFilePath);
                }
                File.Copy(inputFilePath, outputFilePath);
            }

            return outputFilePath;



        }
        private void ExtractPages(string inputFile, string outputFile, int start, int end)
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
                        cb1.AddTemplate(page, 0, -1f, 1f, 0, 0, inputPdf.GetPageSizeWithRotation(i).Height);
                    }
                    else
                    {
                        cb1.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                    }
                }
                inputDoc.Close();
            }
        }

        private List<string> getCoordinates(string inputFilePath, string text)
        {

            string tetmlPath = inputFilePath.Replace(".pdf", ".tetml");
            string llx = "";
            string lly = "";
            string urx = "";
            string ury = "";

            string coordinates = "";
            string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks} clippingarea={cropbox}\" -o \"" + tetmlPath + "\" \"" + inputFilePath + "\"";
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

            XmlDocument tetDoc = new XmlDocument();
            StreamReader sr = new StreamReader(tetmlPath);
            string xmlText = sr.ReadToEnd();
            sr.Close();
            string documentXML = System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>", System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
            tetDoc.LoadXml(documentXML);
            XmlNodeList pages = tetDoc.SelectNodes("//Page");
            List<string> lstcoordiants = new List<string>();

            foreach (XmlNode page in pages)
            {
                string pageText = "";
                string[] textToCheck = text.Split(' ');
                XmlNodeList words = page.SelectNodes("//Text");

                for (int i = 0; i < words.Count; i++)
                {
                    string word = words[i].InnerText.Replace(",", "").Replace(".", "").Replace("'", "").Replace(")", "").Replace("(", "").Replace("’", "");
                    string spellMistakeText = textToCheck[0].ToLower().Replace(",", "").Replace(".", "").Replace("'", "").Replace(")", "").Replace("(", "").Replace("’", "");
                    if (word.ToLower().Equals(spellMistakeText))
                    {

                        XmlNode boxNode = words[i].NextSibling;
                        llx = boxNode.Attributes["llx"].Value;
                        lly = boxNode.Attributes["lly"].Value;
                        urx = boxNode.Attributes["urx"].Value;
                        ury = boxNode.Attributes["ury"].Value;
                        double width = Convert.ToDouble(urx) - Convert.ToDouble(llx);
                        coordinates = llx + " " + lly + " " + width;
                        lstcoordiants.Add(coordinates);
                    }
                }

            }
            File.Delete(tetmlPath);
            return lstcoordiants;
        }

        private void getPageFile(string filePath, string pdfPagefile, string page)
        {

            if (!File.Exists(pdfPagefile))
            {
                objConversionClass.ExtractPages(filePath, pdfPagefile, int.Parse(page), int.Parse(page));
            }
        }
        private void populateMistakes()
        {

        }
        private void spellChecker()
        {
            string MainBook = Session["MainBook"].ToString();
            if (Session["BID"].ToString() != "")
            {
               List<SpellError> listErrors= objMyDBClass.getSpellErrors(Session["BID"].ToString());
               foreach (SpellError error in listErrors)
               {
                   lstBxMistakes.Items.Add(error.Word);
               }
            }
            string rhywFilePath = objMyDBClass.MainDirPhyPath + "/" + MainBook + "/" + MainBook + "-1/TaggingUntagged/" + MainBook + "-1.rhyw";
            objGlobal.XMLPath = rhywFilePath;
            Session["XMLPath"] = objGlobal.XMLPath;
            objGlobal.PBPDocument = new System.Xml.XmlDocument();
            objGlobal.LoadXml();
            Session["PBPDocument"] = objGlobal.PBPDocument;
            objGlobal.LoadXml();

            #region commented portion
            //XmlDocument xmldoc = new XmlDocument();

            //xmldoc.LoadXml(objGlobal.PBPDocument.OuterXml);
            //XmlNodeList lines = xmldoc.SelectNodes("//ln");
            //foreach (XmlNode line in lines)
            //{
            //    line.InnerText = " " + line.InnerText;
            //}
            //txtContent.Text = xmldoc.InnerText;

            #region |Ms workd Logic|
            //Microsoft.Office.Interop.Word.Range DRange;
            ////this.Text = "Creating Word...";

            ////adding new document to word application

            //WordApp.Documents.Add();
            ////set title
            //// this.Text = "Genarating error list...";
            ////get the range of activer document
            //DRange = WordApp.ActiveDocument.Range();
            ////insert textbox data after the content of range of active document
            //DRange.InsertAfter(txtContent.Text);
            ////createing object for error collection and store the all errors
            //Microsoft.Office.Interop.Word.ProofreadingErrors SpellCollection = DRange.SpellingErrors;
            #endregion

          
            //    List<SpellError> Errors = new List<SpellError>();
        //    BadWord badWord;

        //    //check some text.
        //    objSpell.Check(txtContent.Text);

        //    //iterate through all bad words in the text.
        //    List<string> MisSplled = new List<string>();
        //    while ((badWord = objSpell.NextBadWord()) != null)
        //    {
        //        MisSplled.Add(badWord.GetWord());

        //    }
        //removeHere:
        //    for (int i = 0; i < MisSplled.Count; i++)
        //    {
        //        objSpell.Check(MisSplled[i]);
        //        if (objSpell.NextBadWord() == null)
        //        {
        //            MisSplled.Remove(MisSplled[i]);
        //            goto removeHere;
        //        }
        //    }


        //    if (MisSplled.Count > 0)
        //    {
        //        lstBxMistakes.Items.Clear();
        //        lstBxSuggestions.Items.Clear();
        //        int iword = 0;
        //        string newWord = null;

        //        for (iword = 0; iword < MisSplled.Count; iword++)
        //        {

        //            if (lstBxMistakes.Items.FindByText(MisSplled[iword]) != null)
        //            {
        //                var AlreadyExisting = from n in Errors where (n.Word.Equals(MisSplled[iword])) select n;
        //                foreach (var item in AlreadyExisting)
        //                {
        //                    if (item != null)
        //                    {
        //                        // item.Occurences = item.Occurences + 1;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                SpellError sError = new SpellError();
        //                sError.Word = MisSplled[iword];

        //                // sError.Regex = @"[\w\W\d\D]{5}" + SpellCollection[iword].Text + @"[\w\W\d\D]{5}";                        
        //                foreach (XmlNode line in lines)
        //                {
        //                    if (line.InnerText.Contains(sError.Word))
        //                    {
        //                        if (sError.PageNo != null)
        //                        {
        //                            if (!sError.PageNo.Contains(line.Attributes["page"].Value))
        //                            {
        //                                sError.PageNo = sError.PageNo + "," + line.Attributes["page"].Value;
        //                            }
        //                            sError.Occurences = sError.Occurences + 1;
        //                        }
        //                        else
        //                        {
        //                            sError.PageNo = line.Attributes["page"].Value;
        //                            sError.Occurences = 1;
        //                        }

        //                    }
        //                }
        //                //Match matchedtext = Regex.Match(text, @"<ln(.*?)" + SpellCollection[iword].Text + "(.*?)<");
        //                //XmlDocument tempDoc = new XmlDocument();
        //                //tempDoc.LoadXml("<main>" + matchedtext.Value + "/ln></main>");
        //                //XmlNode lnNode = tempDoc.SelectSingleNode("//ln");
        //                //if (lnNode != null)
        //                //{
        //                //    sError.PageNo = Convert.ToInt32(lnNode.Attributes["page"].Value);
        //                //}
        //                newWord = MisSplled[iword];
        //                Errors.Add(sError);
        //                string insertRecQuery = "Insert Into SPELL_MISTAKES Values('" + sError.Word + "','" + sError.PageNo + "','" + sError.Occurences + "','','" + Session["BID"].ToString() + "')";
        //                int insRes = objMyDBClass.ExecuteCommand(insertRecQuery);
        //                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(sError.Word);
        //                //System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(sError.Word, sError.PageNo + "@" + sError.Occurences);

        //                lstBxMistakes.Items.Add(item);
        //            }
        //        }

        //    }
        //    XmlNodeList spellInserted = objGlobal.PBPDocument.SelectNodes("//SpellMistake");

            //for (int i = 0; i < lstBxMistakes.Items.Count; i++)
            //{

            //    XmlNodeList pageContents = objGlobal.PBPDocument.SelectNodes("//*[@page]/..");
            //    string[] punctuation = { "?", "&", "!", "*", "-", "_", "+", "#", "@", "!", "(", ")", "\"", ";", ":", "'", "'", ",", "." };

            //    var abc = spellInserted.Cast<XmlNode>().Where(node => node.InnerText.ToLower().Contains(lstBxMistakes.Items[i].Text.ToLower())).FirstOrDefault();
            //    if (abc == null)
            //    {
            //        //if (item.InnerText.Contains(lstBxMistakes.Items[i].Text))
            //        //{
            //        objGlobal.PBPDocument.InnerXml = objGlobal.PBPDocument.InnerXml.Replace(" " + lstBxMistakes.Items[i].Text + " ", "<SpellMistake edited=\"false\"> " + lstBxMistakes.Items[i].Text + " </SpellMistake> ");
            //        //for (int j = 0; j < punctuation.Length; j++)
            //        //{
            //        //    if (item.InnerText.Contains(lstBxMistakes.Items[i].Text + punctuation[j]))
            //        //    {
            //        //        item.InnerXml = item.InnerXml.Replace(" " + lstBxMistakes.Items[i].Text + punctuation[j], "<SpellMistake edited=\"false\"> " + lstBxMistakes.Items[i].Text + punctuation[j] + " </SpellMistake>");
            //        //    }
            //        //}


            //    }


            //}
            #endregion

            objGlobal.XMLPath = Session["XMLPath"].ToString();
            objGlobal.SaveXml();

            // Session["WordApp"] = WordApp;
        }

        public void EndProcess()
        {
            System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcesses();
            for (int l = 0; l < p.Length; l++)
            {
                if (p[l].ProcessName.ToLower() == "winword")
                {
                    p[l].Kill();
                }
            }
        }

        #endregion

        #region |Events|

        protected void Page_Load(object sender, EventArgs e)
        {
            //((UserMaster)this.Page.Master).ShowLogOutButton();

            string directoryPath = objMyDBClass.MainDirPhyPath + "/" + Session["MainBook"].ToString() + "/SpellCheck";
            Session["SpellDir"] = directoryPath;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (!Page.IsPostBack)
            {
                spellChecker();

            }
        }

        protected void btnReplace_Click(object sender, EventArgs e)
        {
            if (txtManualSpell.Text != "" || lstBxSuggestions.SelectedItem != null)
            {
                System.Web.UI.WebControls.ListItem Mistakeitem = lstBxMistakes.Items[lstBxMistakes.SelectedIndex];
                string txtToReplace = Mistakeitem.Text;
                string replaceWith = "";
                if (txtManualSpell.Text.Equals(""))
                {
                    System.Web.UI.WebControls.ListItem suggestitem = lstBxSuggestions.Items[lstBxSuggestions.SelectedIndex];
                    replaceWith = suggestitem.Text;
                }
                else
                {
                    replaceWith = txtManualSpell.Text;
                }
                SpellError error = objMyDBClass.getSpellError(Mistakeitem.Text, Session["BID"].ToString());


                string pages = error.PageNo;
                string occurences = error.Occurences.ToString();
                string currPage = pages.Split(',')[0];
                objGlobal.PBPDocument = (XmlDocument)Session["PBPDocument"];
                bool edited = false;
                XmlNodeList pageContents = objGlobal.PBPDocument.SelectNodes("//SpellMistake");
                var spellNode = pageContents.Cast<XmlNode>().Where(node => node.InnerText.ToLower().Trim().StartsWith(txtToReplace.ToLower())).FirstOrDefault();
                if (spellNode != null)
                {
                    spellNode.Attributes["edited"].Value = "true";
                    spellNode.InnerText = spellNode.InnerText.Replace(txtToReplace, replaceWith);
                    edited = true;
                }


                occurences = Convert.ToString(Convert.ToInt32(occurences) - 1);
                int index = lstBxMistakes.SelectedIndex;
                if (pages.Contains(","))
                {
                    pages = pages.Remove(0, pages.IndexOf(',') + 1);
                    string updateQuery = "UPDATE SPELL_MISTAKES SET PAGE_NO='" + pages + "',OCCURENCES='" + occurences + "' WHERE WORD like '" + txtToReplace + "' AND BOOK_ID='" + Session["BID"].ToString() + "'";
                    updateWorddetail(updateQuery);
                }

                else
                {
                    string updateQuery = "delete from SPELL_MISTAKES WHERE WORD like '" + txtToReplace + "' AND BOOK_ID='" + Session["BID"].ToString() + "'";
                    updateWorddetail(updateQuery);                    
                    lstBxMistakes.Items.RemoveAt(lstBxMistakes.SelectedIndex);
                    lstBxSuggestions.Items.Clear();

                }
                if (index < lstBxMistakes.Items.Count)
                {
                    lstBxMistakes.Items[index].Selected = true;
                    lstBxMistakes_SelectedIndexChanged(lstBxMistakes, e);
                }
                
                else if (lstBxMistakes.Items.Count > 0)
                {
                    lstBxMistakes.Items[0].Selected = true;
                    lstBxMistakes_SelectedIndexChanged(lstBxMistakes, e);
                }
                objGlobal.XMLPath = Session["XMLPath"].ToString();
                objGlobal.SaveXml();
                objGlobal.LoadXml();
                Session["PBPDocument"] = objGlobal.PBPDocument;

            }
            else
            {
                lblSuggestion.Visible = true;
                lblManualyText.Visible=true;
            }
        }

        private void updateWorddetail(string updatquery)
        {
            int insRes = objMyDBClass.ExecuteCommand(updatquery);
        }

        protected void btnReplaceAll_Click(object sender, EventArgs e)
        {
             if (txtManualSpell.Text != "" || lstBxSuggestions.SelectedItem != null)
            {
            System.Web.UI.WebControls.ListItem Mistakeitem = lstBxMistakes.Items[lstBxMistakes.SelectedIndex];            
            string txtToReplace = Mistakeitem.Text;
            string replaceWith = "";
            if (txtManualSpell.Text.Equals(""))
            {
                System.Web.UI.WebControls.ListItem suggestitem = lstBxSuggestions.Items[lstBxSuggestions.SelectedIndex];
                replaceWith = suggestitem.Text;
            }
            else
            {
                replaceWith = txtManualSpell.Text;
            }
            SpellError error = objMyDBClass.getSpellError(Mistakeitem.Text, Session["BID"].ToString());
            string pages = error.PageNo;
            string occurences = error.Occurences.ToString();
            string currPage = pages.Split(',')[0];
            objGlobal.PBPDocument = (XmlDocument)Session["PBPDocument"];
            bool edited = false;
            XmlNodeList pageContents = objGlobal.PBPDocument.SelectNodes("//SpellMistake");

            foreach (XmlNode item in pageContents)
            {
                if (item.InnerText.Contains(txtToReplace))
                {
                    item.Attributes["edited"].Value = "true";
                    item.InnerText = item.InnerText.Replace(txtToReplace, replaceWith);
                    edited = true;
                }
            }
            string updateQuery = "delete from SPELL_MISTAKES WHERE WORD like '" + txtToReplace + "' AND BOOK_ID='" + Session["BID"].ToString() + "'";
            updateWorddetail(updateQuery);
            lstBxMistakes.Items.RemoveAt(lstBxMistakes.SelectedIndex);
            lstBxSuggestions.Items.Clear();
            if (lstBxMistakes.Items.Count > 0)
            {
                lstBxMistakes.Items[0].Selected = true;
                lstBxMistakes_SelectedIndexChanged(lstBxMistakes, e);
            }
            objGlobal.XMLPath = Session["XMLPath"].ToString();
            objGlobal.SaveXml();
            objGlobal.LoadXml();
            Session["PBPDocument"] = objGlobal.PBPDocument;
            }
             else
             {
                 lblSuggestion.Visible = true;
                 lblManualyText.Visible = true;
             }
        }

        protected void btnIgnore_Click(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.ListItem Mistakeitem = lstBxMistakes.Items[lstBxMistakes.SelectedIndex];
            //System.Web.UI.WebControls.ListItem suggestitem = lstBxSuggestions.Items[lstBxSuggestions.SelectedIndex];
            string txtToReplace = Mistakeitem.Text;

            SpellError error = objMyDBClass.getSpellError(Mistakeitem.Text, Session["BID"].ToString());
            string pages = error.PageNo;
            string occurences = error.Occurences.ToString();
            objGlobal.PBPDocument = (XmlDocument)Session["PBPDocument"];
            bool edited = false;
            XmlNodeList pageContents = objGlobal.PBPDocument.SelectNodes("//SpellMistake");
            foreach (XmlNode item in pageContents)
            {
                if (item.InnerText.Contains(txtToReplace))
                {
                    item.Attributes["edited"].Value = "true";
                    edited = true;
                }
            }
            string updateQuery = "delete from SPELL_MISTAKES WHERE WORD like '" + txtToReplace + "' AND BOOK_ID='" + Session["BID"].ToString() + "'";
            updateWorddetail(updateQuery);
            lstBxMistakes.Items.RemoveAt(lstBxMistakes.SelectedIndex);
            lstBxSuggestions.Items.Clear();
            if (lstBxMistakes.Items.Count > 0)
            {
                lstBxMistakes.Items[0].Selected = true;
                lstBxMistakes_SelectedIndexChanged(lstBxMistakes, e);
            }
            objGlobal.XMLPath = Session["XMLPath"].ToString();
            objGlobal.SaveXml();
            objGlobal.LoadXml();
            Session["PBPDocument"] = objGlobal.PBPDocument;

        }
        //protected void btnFinish_Click(object sender, EventArgs e)
        //{
        //    for (int i = 0; i < lstBxMistakes.Items.Count; i++)
        //    {


        //        System.Web.UI.WebControls.ListItem Mistakeitem = lstBxMistakes.Items[i];

        //        string txtToReplace = Mistakeitem.Text;

        //        DataSet ds = objMyDBClass.GetDataSet("select * from SPELL_MISTAKES where word like '" + Mistakeitem.Text + "'");
        //        string pages = ds.Tables[0].Rows[0][2].ToString();
        //        string occurences = ds.Tables[0].Rows[0][3].ToString();
        //        objGlobal.PBPDocument = (XmlDocument)Session["PBPDocument"];
        //        bool edited = false;
        //        XmlNodeList pageContents = objGlobal.PBPDocument.SelectNodes("//SpellMistake");
        //        foreach (XmlNode item in pageContents)
        //        {
        //            if (item.InnerText.Contains(txtToReplace))
        //            {
        //                item.Attributes["edited"].Value = "true";
        //                edited = true;
        //            }
        //        }
        //        occurences = Convert.ToString(Convert.ToInt32(occurences) - 1);
        //        if (pages.Contains(","))
        //        {
        //            pages = pages.Remove(0, pages.IndexOf(',') + 1);
        //            string updateQuery = "UPDATE SPELL_MISTAKES SET PAGE_NO='" + pages + "',OCCURENCES='" + occurences + "' WHERE WORD like '" + txtToReplace + "' AND BOOK_ID='" + Request.QueryString["bid"].ToString() + "'";
        //            updateWorddetail(updateQuery);
        //        }
        //        else
        //        {
        //            lstBxMistakes.Items.RemoveAt(lstBxMistakes.SelectedIndex);
        //            lstBxSuggestions.Items.Clear();

        //        }
        //        if (lstBxMistakes.Items.Count > 0)
        //        {
        //            lstBxMistakes.Items[0].Selected = true;
        //            lstBxMistakes_SelectedIndexChanged(lstBxMistakes, e);
        //        }
        //        objGlobal.XMLPath = Session["XMLPath"].ToString();
        //        objGlobal.SaveXml();
        //        objGlobal.LoadXml();
        //        Session["PBPDocument"] = objGlobal.PBPDocument;
        //    }









        //    string bid = Request.QueryString["bid"].ToString();
        //    if (bid != "")
        //    {
        //        string deleteQuery = "Delete from SPELL_MISTAKES where BOOK_ID='" + bid + "'";
        //        updateWorddetail(deleteQuery);
        //    }
        //    string dbBookId = objMyDBClass.ExecuteSelectCom("select bid from book where mainbook like '" + bid + "%'");
        //    string queryUpdate = "Update ACTIVITY Set Status='Pending Confirmation' Where BID=" + dbBookId + " AND Task='SpellCheck'";
        //    objMyDBClass.ExecuteCommand(queryUpdate);
        //    Response.Redirect("AdminPanel.aspx", false);
        //    //CreateComparisonTask();

        //}

        protected void btnFinish_Click(object sender, EventArgs e)
        {
            objGlobal.XMLPath = Session["XMLPath"].ToString();
            objGlobal.LoadXml();

            XmlNodeList pageContents = objGlobal.PBPDocument.SelectNodes("//SpellMistake");
            foreach (XmlNode item in pageContents)
            {
                item.Attributes["edited"].Value = "true";
            }
            objGlobal.XMLPath = Session["XMLPath"].ToString();
            objGlobal.SaveXml();
            objGlobal.LoadXml();
            Session["PBPDocument"] = objGlobal.PBPDocument;
            if (Session["BID"] != null)
            {
                string deleteQuery = "Delete from SPELL_MISTAKES where BOOK_ID='" + Session["BID"].ToString() + "'";
                updateWorddetail(deleteQuery);
            }
            string queryUpdate = "Update ACTIVITY Set Status='Approved' Where BID=" + Session["BID"].ToString() + " AND Task='SpellCheck'";
            objMyDBClass.ExecuteCommand(queryUpdate);
            CreateMistakeInjectionTask(Session["AID"].ToString(), Session["LoginId"].ToString());
            Response.Redirect("OnlineTestUser.aspx");

        }

        public void CreateMistakeInjectionTask(string aid, string userId)
        {
            int inResult = 0;
            string queryBookID = "Select BID From Activity Where AID=" + aid;
            string bookID = objMyDBClass.GetID(queryBookID);
            inResult = objMyDBClass.CreateTask(bookID, "Unassigned", "MistakeInjection", userId);

            CreateNewXml();
        }

        public void CreateNewXml()
        {
            string xmlPath = Common.GetDirectoryPath() + Convert.ToString(Session["MainBook"]) + "/" + Convert.ToString(Session["MainBook"]) +
                             "-1/TaggingUntagged" + "/" + Convert.ToString(Session["MainBook"]) + "-1.rhyw";
            string newXmlPath = Common.GetDirectoryPath() + Convert.ToString(Session["MainBook"]) + "/" + Convert.ToString(Session["MainBook"]) +
                                "-1/TaggingUntagged" + "/" + Convert.ToString(Session["MainBook"]) + "-1_actual.rhyw";

            Common commObj=new Common();

            commObj.LoadXml(xmlPath);
            XmlDocument doc = commObj.xmlDoc;

            GlobalVar objGlobal = new GlobalVar();
            objGlobal.PBPDocument = doc;
            objGlobal.XMLPath = newXmlPath;
            objGlobal.SaveXml();

            //Session["xmlFile_BeforeMistakeInj"] = newXmlPath;
        }

        protected void lstBxMistakes_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.ListItem item = lstBxMistakes.Items[lstBxMistakes.SelectedIndex];
            //Microsoft.Office.Interop.Word.Application WordApp = new Microsoft.Office.Interop.Word.Application();
            //Microsoft.Office.Interop.Word.SpellingSuggestions CorrectionsCollection;
            //WordApp.Documents.Add();
            txtManualSpell.Text = "";
            ArrayList CorrectionsCollection = objSpell.FindSuggestions(item.Text);
            lstBxSuggestions.Items.Clear();
            List<SpellError> listErrors = objMyDBClass.getSpellErrors(Session["BID"].ToString());
            SpellError objError = (from n in listErrors where (n.Word.Equals(item.Text)) select n).FirstOrDefault();
            if (objError != null)
            {
                string pages = objError.PageNo;
                string occurences = objError.Occurences.ToString();
                lblPages.Text = pages;
                lblOccurences.Text = occurences;
                string page = "";
                if (pages.Contains(','))
                {
                    page = pages.Split(',')[0];
                }
                else
                {
                    page = pages;
                }
                lblSourcePageNo.Text = page;
                string pageFilePath = Session["SpellDir"].ToString() + "/Page" + page + ".pdf";
                string MainBook = Session["MainBook"].ToString();
                getPageFile(objMyDBClass.MainDirPhyPath + "/" + MainBook + "/" + MainBook + ".pdf", pageFilePath, page);
                HighLightMisSpelled(pageFilePath, item.Text);
                PDFViewerSource.FilePath = System.Configuration.ConfigurationManager.AppSettings["MainDirectory"] + "/" + MainBook + "/SpellCheck/Page" + page + "_Highlighted.pdf";
                if (CorrectionsCollection.Count > 0)
                {

                    for (int iWord = 0; iWord < CorrectionsCollection.Count; iWord++)
                    {
                        lstBxSuggestions.Items.Add(CorrectionsCollection[iWord].ToString());
                    }
                }
                else
                {
                    lstBxSuggestions.Items.Add("No suggestions!");
                }
            }
        }

        #endregion

        ASPNetSpell.SpellChecker sc = new ASPNetSpell.SpellChecker();

    }
}
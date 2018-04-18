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
using System.Collections;
using System.IO;
using Outsourcing_System;

namespace Outsourcing_System.PdfCompare_Classes
{
    public class ErrorListHandler
    {
        private ArrayList errorList;

        private string srcPDFPath;
        private string prdPDFPath;
        private string xmlPath;
        //public string SrcPDFPath
        //{
        //    set
        //    {
        //        //PDFCmpSesssion.FirstPDFPath = value;
        //        this.srcPDFPath = value;
        //    }
        //    get
        //    {
        //        //return PDFCmpSesssion.FirstPDFPath;
        //        return this.srcPDFPath;
        //    }
        //}

        ///// <summary>
        ///// Second PDF to compare with
        ///// </summary>
        //public string ProdPDFPath
        //{
        //    set
        //    {
        //        this.prdPDFPath = value;
        //        //PDFCmpSesssion.SecondPDFPath = value;
        //    }
        //    get
        //    {
        //        //return PDFCmpSesssion.SecondPDFPath;
        //        return this.prdPDFPath;
        //    }
        //}

        /// <summary>
        /// The respective xml file path which had been used to produce ProdPDFPath
        /// </summary>
        //public string XMLPath
        //{
        //    set
        //    {
        //        this.xmlPath = value;
        //    }
        //    get
        //    {
        //        return this.xmlPath;
        //    }
        //}
        private int currentPageNumber;

        //public int CurrentPageNumber
        //{
        //    set
        //    {
        //        this.currentPageNumber = value;
        //    }
        //    get
        //    {
        //        return this.currentPageNumber;
        //    }
        //}

        private int list1PrevMatchIndex;
        private int list2PrevMatchIndex;
        private int list1ShowingIndex;
        private int list2ShowingIndex;

        private int List1CurrPageNo;
        private int List2CurrPageNo;

        private CompareFiles cf;

        public void removeError(int index)
        {
            if (errorList != null)
            {
                errorList.RemoveAt(index);
            }
        }

        public ArrayList CurrentErrors
        {
            get { return errorList; }
        }

        public int ErrorCount
        {
            get
            {
                if (errorList == null)
                    return 0;
                else
                    return errorList.Count;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MisMatchError GetErrorAtIndex(int index)
        {
            if (index < errorList.Count)
            {
                return (MisMatchError) errorList[index];
            }
            else
                return null;
        }

        public String GetProducedPDFPathAt(int Index)
        {
            if (Index < errorList.Count)
            {
                return ((MisMatchError) errorList[Index]).pdfProducedPath;
            }
            else
                return null;
        }

        public String GetSourcePDFPathAt(int Index)
        {
            if (Index < errorList.Count)
            {
                return ((MisMatchError) errorList[Index]).pdfSourcePath;
            }
            else
                return null;
        }

        public String GetCorrespondingXMLPathAt(int Index)
        {
            if (Index < errorList.Count)
            {
                return ((MisMatchError) errorList[Index]).xmlPath;
            }
            else
                return null;
        }


        public ErrorListHandler()
        {
            errorList = new ArrayList();
            //
            // TODO: Add constructor logic here
            //
        }

        public ErrorListHandler(ArrayList list)
        {

            errorList = list;
        }

        private ArrayList wrdList1;
        private ArrayList wrdList2;

        public void FindError(String SourcePDFPath, String ProducedPDFPath, String XMLPath, int PageNumber)
        {
            this.srcPDFPath = SourcePDFPath;
            this.prdPDFPath = ProducedPDFPath;
            this.xmlPath = XMLPath;
            this.currentPageNumber = PageNumber;
            PreProcessMatch();
            //showLists();

            if (ProducedPDFPath != "")
                MatchAndAddInList();
        }

        /// <summary>
        /// Re-Finds the error, removes previous error and inserts at Index
        /// </summary>
        /// <param name="ProducedPDFPath"></param>
        /// <param name="Index"></param>
        public void FindErrorAt(String ProducedPDFPath, int Index)
        {
            this.prdPDFPath = ProducedPDFPath;
            MisMatchError wrdError = ((MisMatchError) errorList[Index]);
            this.srcPDFPath = wrdError.pdfSourcePath;
            PreProcessMatch();
            //showLists();
            MatchAndReAddInListAt(Index);
        }


        private void PreProcessMatch()
        {
            //PDFFile xmlPDF = new PDFFile(ProducedPDFPath);
            //PDFFile pdfPDF = new PDFFile(SourcePDFPath);

            PDFFile xmlPDF = new PDFFile(this.prdPDFPath);
            PDFFile pdfPDF = new PDFFile(this.srcPDFPath);


            //LogWritter.WriteLineInLog("Source File: " + this.srcPDFPath + "| Prod File: " + this.prdPDFPath);

            cf = new CompareFiles();
            cf.firstFile = pdfPDF;
            cf.secondFile = xmlPDF;
            list1PrevMatchIndex = -1;
            list2PrevMatchIndex = -1;
            List1CurrPageNo = 1;
            List2CurrPageNo = 1;
            list1ShowingIndex = 0;
            list2ShowingIndex = 0;

            wrdList1 = cf.firstFile.GenerateAndGetAllWordsInFile();
            wrdList2 = cf.secondFile.GenerateAndGetAllWordsInFile();
        }

        private void MatchAndAddInList()
        {
            int wrdList1Count = wrdList1.Count;
            int wrdList2Count = wrdList2.Count;
            if (wrdList1Count == 0 && wrdList2Count != 0)
            {
                PdfWord wrd1 = new PdfWord(this.currentPageNumber, 0, "", "");
                PdfWord wrd2 = (PdfWord) (wrdList2[0]);
                AddErrorInList(wrd1, wrd2, srcPDFPath, prdPDFPath, xmlPath);
            }
            else if (wrdList1Count != 0 && wrdList2Count == 0)
            {
                PdfWord wrd1 = (PdfWord)(wrdList1[0]);
                PdfWord wrd2 = new PdfWord(this.currentPageNumber, 0, "", "");
                AddErrorInList(wrd1, wrd2, srcPDFPath, prdPDFPath, xmlPath);
            }
            else
            {
                if (wrdList1.Count == wrdList2.Count)
                {
                    for (int i = 0; i < wrdList1.Count; i++)
                    {
                        PdfWord wrd1 = (PdfWord)(wrdList1[i]);
                        PdfWord wrd2 = (PdfWord)(wrdList2[i]);
                        if (!wrd1.Text.Equals(wrd2.Text))
                        {
                            wrd1.PageNumber = this.currentPageNumber;
                            wrd2.PageNumber = this.currentPageNumber;
                            AddErrorInList(wrd1, wrd2, srcPDFPath, prdPDFPath, xmlPath);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes the previous entry from error list and adds a new entry if found at index location
        /// </summary>
        private void MatchAndReAddInListAt(int Index)
        {
            errorList.RemoveAt(Index);
            int wrdList1Count = wrdList1.Count;
            int wrdList2Count = wrdList2.Count;
            if (wrdList1Count == 0 && wrdList2Count != 0)
            {
                PdfWord wrd1 = new PdfWord(this.currentPageNumber, 0, "", "");
                PdfWord wrd2 = (PdfWord)(wrdList2[0]);
                AddErrorInListAt(wrd1, wrd2, srcPDFPath, prdPDFPath, prdPDFPath.Split('.')[0] + ".xml", Index);
            }
            else if (wrdList1Count != 0 && wrdList2Count == 0)
            {
                PdfWord wrd1 = (PdfWord)(wrdList1[0]);
                PdfWord wrd2 = new PdfWord(this.currentPageNumber, 0, "", "");
                AddErrorInListAt(wrd1, wrd2, srcPDFPath, prdPDFPath, prdPDFPath.Split('.')[0] + ".xml", Index);
            }
            else
            {
                if (wrdList1.Count == wrdList2.Count)
                {
                    for (int i = 0; i < wrdList1.Count; i++)
                    {
                        PdfWord wrd1 = (PdfWord)(wrdList1[i]);
                        PdfWord wrd2 = (PdfWord)(wrdList2[i]);
                        if (!wrd1.Text.Equals(wrd2.Text))
                        {
                            if (wrd1.PageNumber == null)
                            {
                                wrd1.PageNumber = this.currentPageNumber;
                            }
                            if (wrd2.PageNumber == null)
                            {
                                wrd2.PageNumber = this.currentPageNumber;
                            }
                            AddErrorInListAt(wrd1, wrd2, srcPDFPath, prdPDFPath, prdPDFPath.Split('.')[0] + ".xml",
                                Index);
                            break;
                        }
                    }
                }
            }
        }

        private void AddErrorInList(PdfWord wrd1, PdfWord wrd2, string SourcePDFPath, string ProducedPDFPath,
            string XMLPath)
        {
            MisMatchError wrdError = new MisMatchError();
            wrdError.list1Word = wrd1;
            wrdError.list2Word = wrd2;
            wrdError.pdfSourcePath = SourcePDFPath;
            wrdError.pdfProducedPath = ProducedPDFPath;
            wrdError.xmlPath = XMLPath;
            //SiteSession.ErrorList.Add(wrdError);
            errorList.Add(wrdError);
        }

        private void AddErrorInListAt(PdfWord wrd1, PdfWord wrd2, string SourcePDFPath, string ProducedPDFPath,
            string XMLPath, int Index)
        {
            MisMatchError wrdError = new MisMatchError();
            wrdError.list1Word = wrd1;
            wrdError.list2Word = wrd2;
            wrdError.pdfSourcePath = SourcePDFPath;
            wrdError.pdfProducedPath = ProducedPDFPath;
            wrdError.xmlPath = XMLPath;
            errorList.Insert(Index, wrdError);
        }

        public void WriteErrorReportToFile(string filePath)
        {
            //StreamWriter sw = new StreamWriter(filePath);
            ////int totalErrorCount = SiteSession.ErrorList.Count;
            //int totalErrorCount = SiteSession.errorHl.ErrorCount;
            //for (int i = 0; i < totalErrorCount; i++)
            //{
            //    //MisMatchError mmError = (MisMatchError)SiteSession.ErrorList[i];
            //    MisMatchError mmError = (MisMatchError) SiteSession.errorHl.GetErrorAtIndex(i);
            //    PdfWord wrd1 = mmError.list1Word;
            //    PdfWord wrd2 = mmError.list2Word;
            //    string errorLine = wrd1.Text + '\t' + wrd1.PageNumber + '\t' + wrd1.LineNumber + '\t' + wrd2.Text + '\t' +
            //                       wrd2.PageNumber + '\t' + wrd2.LineNumber + '\t';
            //    sw.WriteLine(errorLine);
            //}
            //sw.Close();
        }
    }
}
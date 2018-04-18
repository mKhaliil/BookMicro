using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using OfficeOpenXml;
using System.IO;
using System.Collections;
using System.Diagnostics;

public class ExcelHandler
{
    public static string FileWritePath;
    public static void GenerateExcelFile(DataSet dataSet)
    {
        ////DataSet dataSet = feedsGenerationController.GetFeedData(Books, GetSelectedChannelName(), Convert.ToInt64(ddlChannelNames.SelectedValue), IsEbookChannel(GetSelectedChannelName()));
        //Hashtable currExcelProceses = ExcelHandler.CheckExcellProcesses();
        //if (dataSet != null)
        //{
        //    DataTable toCsv = dataSet.Tables[0].Copy();
        //    using (ExcelPackage p = new ExcelPackage())
        //    {
        //        //Create a sheet
        //        p.Workbook.Worksheets.Add("Sample WorkSheet");
        //        ExcelWorksheet ws = p.Workbook.Worksheets[1];
        //        ws.Name = "Sheet1"; //Setting Sheet's name
        //        int colIndex = 1;
        //        int rowIndex = 1;

        //        foreach (DataColumn dc in toCsv.Columns) //Creating Headings
        //        {
        //            /////var cell = ws.Cells[rowIndex, colIndex];
        //            //Setting Top/left,right/bottom borders.
        //            //Setting Value in cell
        //            /////cell.Value = dc.ColumnName;
        //            //OfficeOpenXml.Style.ExcelColor ec = new OfficeOpenXml.Style.ExcelColor();
        //            //range.Style.Font.Color = System.Drawing.Color.Yellow;
        //            //  cell. = ec.SetColor(System.Drawing.Color.Red);
        //            //OfficeOpenXml.Style.ExcelTextFont fnt = new OfficeOpenXml.Style.ExcelTextFont();
        //            //cell.Style = fnt.Bold;
        //            colIndex++;
        //        }
        //        bool isDifferent = false;
        //        bool ishighlighted = false;
        //        foreach (DataRow dr in toCsv.Rows) // Adding Data into rows
        //        {
        //            colIndex = 1;
        //            rowIndex++;
        //            if (!dr[0].ToString().ToUpper().Equals(dr[1].ToString().ToUpper()))
        //            {
        //                isDifferent = true;
        //            }
        //            foreach (DataColumn dc in toCsv.Columns)
        //            {
        //                var cell = ws.Cells[rowIndex, colIndex];
        //                //Setting Value in cell
        //                if (isDifferent & !ishighlighted)
        //                {
        //                    ExcelRange range = ws.Cells[rowIndex, colIndex];
        //                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
        //                    isDifferent = false;
        //                    ishighlighted = true;
        //                }
        //                cell.Value = Convert.ToString(dr[dc.ColumnName]);

        //                colIndex++;
        //            }
        //        }
        //        Byte[] bin = p.GetAsByteArray();
        //        using (BinaryWriter binWriter = new BinaryWriter(File.Open(FileWritePath, FileMode.Create)))
        //        {
        //            binWriter.Write(bin);
        //        }

        //        //p.SaveAs(fi);
        //        //string file = Server.MapPath("~/ReportRepository/") + "ChannelReport" + ".xlsx";
        //        //File.WriteAllBytes(file, bin);
        //        //Response.AddHeader("Content-disposition", "attachment; filename=Report_" + GetSelectedChannelName().Replace(" ", "").Trim() + ".xlsx");
        //        //Response.ContentType = "application/vnd.ms-excel";
        //        //Response.BinaryWrite(bin);
        //        //Response.End();
        //    }
            
        //    ExcelHandler.KillExcel(currExcelProceses);//Kill the current Excel PRocesses

        //}
    }

    /// <summary>
    /// Gets all currently running excel Processes
    /// </summary>
    /// <returns></returns>
    public static Hashtable CheckExcellProcesses()
    {
        Process[] AllProcesses = Process.GetProcessesByName("excel");
        Hashtable htExcelProc = new Hashtable();
        int iCount = 0;

        foreach (Process ExcelProcess in AllProcesses)
        {
            htExcelProc.Add(ExcelProcess.Id, iCount);
            iCount = iCount + 1;
        }
        return htExcelProc;
    }

    /// <summary>
    /// Kills all excel processes not in the hash table
    /// </summary>
    public static void KillExcel(Hashtable htExcelProc)
    {
        Process[] AllProcesses = Process.GetProcessesByName("excel");

        // check to kill the right process
        foreach (Process ExcelProcess in AllProcesses)
        {
            if (htExcelProc.ContainsKey(ExcelProcess.Id) == false)
                ExcelProcess.Kill();
        }

        AllProcesses = null;
    }
}

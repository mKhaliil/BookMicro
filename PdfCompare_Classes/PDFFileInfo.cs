using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;


public class RHYWFile
{
    public ArrayList words;
    public int pageCount;
    public RHYWFile()
    {
        words = new ArrayList();
        pageCount = -1;
    }
}
/// <summary>
/// Summary description for PDFFile
/// </summary>
public class PDFFileInfo
{
    public PDFFileInfo()
    {
        format = "";
        title = "";
        fontSize = -1;
        fontName = "";
        productionEditionTypeID = -1;
        bookID = -1;
        filePath = "";
        vol = -1;
        psid = -1;
        isbn13 = "";
        fileName = "";
    }

    public string format;
    public string title;
    public string fontName;
    public int pages;
    public int fontSize;
    public long productionEditionTypeID;
    public long bookID;
    public string isbn13;
    public int wordCount;
    public int WordCount
    {
        set
        {
            this.wordCount = value;
        }
        get
        {

            return this.wordCount;
        }
    }

    /// <summary>
    /// File's Complete Physical path
    /// </summary>
    public string filePath;

    /// <summary>
    /// File's Name
    /// </summary>
    public string fileName;
    public int vol;
    public int psid;


    /// <summary>
    /// Gets the format of the PDF file based on the fontSize property
    /// </summary>
    public string ExtractedFormat
    {
        get
        {
            if (fontSize == -1)
                return "";
            else
                return format;
        }
    }

    public static bool compareTextFile(PDFFileInfo fileObj, PDFFileInfo dbObj)
    {
        string fileTitle = fileObj.title.Trim().Replace(" ", "");
        string dbTitle = dbObj.title.Trim().Replace(" ", "");
        if (!dbTitle.ToLower().StartsWith(fileTitle.ToLower()))
        {
            return false;
        }
        //if (firstObj.title == secondObj.title & firstObj.format == secondObj.format & firstObj.pages == secondObj.pages
        //    & firstObj.fontSize == secondObj.fontSize)
        //{
        //    return true;
        //}
        else
            return true;
    }

    /// <summary>
    /// Compares the filename with its info in db
    /// </summary>
    /// <param name="fileObj"></param>
    /// <param name="dbObj"></param>
    /// <returns></returns>
    public static bool compareCoverFile(PDFFileInfo fileObj, PDFFileInfo dbObj)
    {
        string trimmedFileName = fileObj.getTrimmedFileName();
        string isbn13 = dbObj.isbn13;
        if (fileObj.pages <= 1) // Cover file is greater than 1 pages
        {
            if (trimmedFileName.Trim().Contains(isbn13)) //File's name matches with the ISBN in the db
            {
                //if (dbObj.title.Trim().StartsWith(fileObj.title.Trim()))
                if (fileObj.title.Trim().StartsWith(dbObj.title.Trim()))
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        else
            return false;
    }

    /// <summary>
    /// Compares Text file name with cover file name
    /// </summary>
    /// <param name="textFileObj"></param>
    /// <param name="coverFileObj"></param>
    /// <returns></returns>
    public static bool compareTextWithCover(PDFFileInfo textFileObj, PDFFileInfo coverFileObj)
    {
        string textFileName = textFileObj.fileName;
        string coverFileName = coverFileObj.fileName;

        textFileName = textFileName.TrimEnd(".pdf".ToCharArray());
        textFileName = Regex.Replace(textFileName, "(?:)cover|[-_]", "");

        coverFileName = coverFileName.TrimEnd(".pdf".ToCharArray());
        coverFileName = Regex.Replace(coverFileName, "(?:)text|[-_]", "");

        if (!coverFileName.Trim().Equals(textFileName.Trim()))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Gets the file name after removing the pdf extension and other cover/text labels
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private string getTrimmedFileName()
    {
        if (fileName.Trim().Equals(string.Empty))
            return "";
        string trimmedFileName = fileName;
        trimmedFileName = fileName.TrimEnd(".pdf".ToCharArray());
        trimmedFileName = Regex.Replace(trimmedFileName, "(?:)cover|[-_]", "");
        return trimmedFileName;
    }
}


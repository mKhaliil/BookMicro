using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Diagnostics;


public class OCROperations
{

    //private static MODI.Document objDoc;
    //public MODI.Document getSignleInstance()
    //{
    //    if (objDoc != null)
    //    {
    //        return objDoc;
    //    }
    //    objDoc = new MODI.Document();
    //    return objDoc;
    //}
    //public void pdfToImage(string pdfPath)
    //{
    //    string command = "convert -colorspace rgb -density 400 \"" + pdfPath + "\" -profile \"D:\\USWebCoatedSWOP.icc\" \"" + pdfPath.Replace(".pdf", ".tiff") + "\" ";
    //    ProcessStartInfo pInfo = new ProcessStartInfo("cmd", "/c " + command);
    //    pInfo.CreateNoWindow = true;
    //    pInfo.RedirectStandardOutput = true;
    //    pInfo.UseShellExecute = false;
    //    Process p = new Process();
    //    p.StartInfo = pInfo;
    //    p.Start();
    //    p.Close();

    //}
    //public string readtext(string fileToOCR)
    //{
    //    pdfToImage(fileToOCR);
    //    System.Threading.Thread.Sleep(20000);
    //    MODI.Document md = getSignleInstance();

    //    md.Create(fileToOCR.Replace(".pdf", ".tiff"));
        
    //    md.OCR(MODI.MiLANGUAGES.miLANG_ENGLISH, true, true);

    //    MODI.Image img = (MODI.Image)md.Images[0];

    //    MODI.Layout layout = img.Layout;

    //    layout = img.Layout;

    //    string result = layout.Text;

    //    md.Close(false);
        

    //    return result;

    //}
    ////DocumentClass myDoc = new DocumentClass();
    ////myDoc.Create(@"C:\Documents and Settings\mkhalil\Desktop\Testing_Folder\DummyTiffwithText.tif"); //we work with the .tiff extension
    ////myDoc.OCR(MiLANGUAGES.miLANG_ENGLISH, true, true);
    ////string text = "";
    ////foreach (Image anImage in myDoc.Images)
    ////{
    ////    Console.WriteLine(anImage.Layout.Text); //here we cout to the console.
    ////}
    ////return text;
}

// Load Image from File

//Bitmap BWImage = new Bitmap(@"C:\Documents and Settings\mkhalil\Desktop\Testing_Folder\29937");
//// Lock destination bitmap in memory
//BitmapData BWLockImage = BWImage.LockBits(new Rectangle(0, 0, BWImage.Width, BWImage.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

//// Copy image data to binary array
//int imageSize = BWLockImage.Stride * BWLockImage.Height;
//byte[] BWImageBuffer = new byte[imageSize];
//Marshal.Copy(BWLockImage.Scan0, BWImageBuffer, 0, imageSize);
//DoOCR(BWLockImage, BWImageBuffer, tmpPosRect, false);



//// Do the OCR with this function
//public string DoOCR(System.Drawing.Imaging.BitmapData BWLockImage, byte[] BWImageBuffer, Rectangle iAusschnitt, bool isNumber)
//{
//    Bitmap tmpImage = Bildausschnitt1bpp(BWLockImage, BWImageBuffer, iAusschnitt);
//    string file = Path.GetTempFileName();
//    string tmpResult = "";
//    try
//    {
//        tmpImage.Save(file, ImageFormat.Tiff);
//        _MODIDocument.Create(file);
//        // Modi parameter erstellen
//        _MODIDocument.OCR(MODI.MiLANGUAGES.miLANG_ENGLISH, false, false);

//        MODI.IImage myImage = (MODI.IImage)_MODIDocument.Images[0]; //first page in file
//        MODI.ILayout myLayout = (MODI.ILayout)myImage.Layout;
//        tmpResult = myLayout.Text;
//    }
//    catch
//    {
//        if (_MODIDocument != null)
//        {
//            _MODIDocument.Close(false); //Closes the document and deallocates the memory.
//            _MODIDocument = null;
//        }
//        // Bild freigeben
//        tmpImage.Dispose();
//        tmpImage = null;
//        // Garbage Collector ausführen
//        GC.Collect();
//        // Bilddatei löschen
//        File.Delete(file);
//    }
//    return tmpResult;
//}


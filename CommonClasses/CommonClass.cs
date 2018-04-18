using System;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using Outsourcing_System;
using System.Security.Cryptography;


public class CommonClass
{
    public static string xmlFilePath;
    ImageIndex objImageIndex = new ImageIndex();
    public CommonClass()
    {
    }

    public static void ShowMessage(string message)
    {

    }

    //=======================================================
    //Reconversion from XML to HTML Methods
    //=======================================================

    #region string TableReconversion(string stringTable)
    public static string TableReconversion(string stringTable)
    {
        stringTable = Regex.Replace(stringTable, "border=\"on\"", "border=\"1\" width=\"100%\"");
        stringTable = Regex.Replace(stringTable, "border=\"off\"", "border=\"1\" width=\"100%\"");
        stringTable = Regex.Replace(stringTable, "<header />", "<caption>&nbsp;</caption>");
        stringTable = Regex.Replace(stringTable, "<header", "<caption");
        stringTable = Regex.Replace(stringTable, "</header", "</caption");
        stringTable = Regex.Replace(stringTable, "<row>", "<TR>");
        stringTable = Regex.Replace(stringTable, "</row>", "</TR>");
        stringTable = Regex.Replace(stringTable, "<head-row>", "<TR>");
        stringTable = Regex.Replace(stringTable, "</head-row>", "</TR>");
        stringTable = Regex.Replace(stringTable, "head-col", "TH");
        stringTable = Regex.Replace(stringTable, "<col", "<TD");
        stringTable = Regex.Replace(stringTable, "</col>", "</TD>");
        stringTable = BoldItalicReconversion(stringTable);
        return stringTable;
    }
    #endregion

    #region BoldItalicReconversion(string tempString)
    public static string BoldItalicReconversion(string tempString)
    {
        //Converting Keywords of Index
        MatchCollection keyCol = Regex.Matches(tempString, "<keywrd.*?/>");
        for (int i = 0; i < keyCol.Count; i++)
        {
            string st1 = Regex.Match(keyCol[i].Value, "key=.*?/>").Value.Replace("/>", "").Replace("key=", "");
            st1 = "<BR><FONT color=gray>" + st1.Trim() + "</FONT>";
            tempString = tempString.Replace(keyCol[i].Value, st1);
        }

        Regex breakError = new Regex("></break>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        MatchCollection regbreakError = breakError.Matches(tempString);
        for (int i = 0; i < regbreakError.Count; i++)
        {
            string str1 = regbreakError[i].Value;
            string str2 = " />";
            tempString = tempString.Replace(regbreakError[i].Value, str2);
        }

        while (Regex.Match(tempString, "<emphasis type=\"bold-italic\">.*?</emphasis>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success)
        {
            string oldbolditalic = Regex.Match(tempString, "<emphasis type=\"bold-italic\">.*?</emphasis>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Value;
            string newbolditalic = oldbolditalic.Replace("<emphasis type=\"bold-italic\">", "<B><I>").Replace("</emphasis>", "</I></B>");
            if (oldbolditalic != "" && newbolditalic != "")
            {
                tempString = tempString.Replace(oldbolditalic, newbolditalic);
            }
        }

        while (Regex.Match(tempString, "<emphasis type=\"bold\">.*?</emphasis>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success)
        {
            string oldbold = Regex.Match(tempString, "<emphasis type=\"bold\">.*?</emphasis>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Value;
            string newbold = oldbold.Replace("<emphasis type=\"bold\">", "<B>").Replace("</emphasis>", "</B>");
            if (oldbold != "" && newbold != "")
            {
                tempString = tempString.Replace(oldbold, newbold);
            }
        }

        while (Regex.Match(tempString, "<emphasis type=\"italic\">.*?</emphasis>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success)
        {
            string olditalic = Regex.Match(tempString, "<emphasis type=\"italic\">.*?</emphasis>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Value;
            string newitalic = olditalic.Replace("<emphasis type=\"italic\">", "<I>").Replace("</emphasis>", "</I>");
            if (olditalic != "" && newitalic != "")
            {
                tempString = tempString.Replace(olditalic, newitalic);
            }
        }

        while (Regex.Match(tempString, "<inline-markup type=\"url\">.*?</inline-markup>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Success)
        {
            string inlinemarkup = Regex.Match(tempString, "<inline-markup type=\"url\">.*?</inline-markup>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Value;
            string newinlinemarkup = inlinemarkup.Replace("<inline-markup type=\"url\">", "<FONT color=blue>").Replace("</inline-markup>", "</FONT>");
            if (inlinemarkup != "" && newinlinemarkup != "")
            {
                tempString = tempString.Replace(inlinemarkup, newinlinemarkup);
            }
        }

        MatchCollection footColl = Regex.Matches(tempString, "<footnote id=.*?>.*?</footnote>");
        for (int r = 0; r < footColl.Count; r++)
        {
            string footnote = Regex.Match(tempString, "<footnote id=.*?>.*?</footnote>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Value;
            tempString = tempString.Replace(footnote, "<FONT color=green><SUP>" + Regex.Match(footColl[r].Value, "[0-9]+").Value + "</SUP></FONT>");

            string newFootNode = footnote.Replace("<footnote id=\"" + Regex.Match(footColl[r].Value, "[0-9]+").Value + "\">", "<BR><FONT color=green><SUB>").Replace("</footnote>", "</SUB></FONT>");

            tempString = tempString + newFootNode;

        }
        Regex Num = new Regex(@"<NUM>.*?</NUM>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        MatchCollection regNum = Num.Matches(tempString);
        for (int i = 0; i < regNum.Count; i++)
        {
            string str1 = regNum[i].Value;
            string str2 = str1.Replace("<num>", "<PRE>").Replace("</num>", "</PRE>");
            tempString = tempString.Replace(regNum[i].Value, str2);
        }

        Regex Span = new Regex(@"<PARA>.*?</PARA>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        MatchCollection regSpan = Span.Matches(tempString);
        for (int i = 0; i < regSpan.Count; i++)
        {
            string str1 = regSpan[i].Value;
            string str2 = str1.Replace("<para>", "<SPAN>").Replace("</para>", "</SPAN>");
            tempString = tempString.Replace(regSpan[i].Value, str2);
        }

        Regex Div = new Regex(@"<LINE>.*?</LINE>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        MatchCollection regDiv = Div.Matches(tempString);
        for (int i = 0; i < regDiv.Count; i++)
        {
            string str1 = regDiv[i].Value;
            string str2 = str1.Replace("<line>", "<DIV>").Replace("</line>", "</DIV>");
            tempString = tempString.Replace(regDiv[i].Value, str2);
        }


        MatchCollection breakCol = Regex.Matches(tempString, "<break.*?>");
        for (int g = 0; g < breakCol.Count; g++)
        {
            if (breakCol[g].Value.Contains("volume"))
            {
                string st1 = Regex.Match(breakCol[g].Value, "[0-9]+").Value;
                tempString = tempString.Replace(breakCol[g].Value, "<FONT color=red>" + st1 + "</FONT>");
            }
            else if (breakCol[g].Value.Contains("page"))
            {
                string st1 = Regex.Match(breakCol[g].Value, "[0-9]+").Value;
                tempString = tempString.Replace(breakCol[g].Value, "<FONT color=orange>" + st1 + "</FONT>");
            }
        }

        Regex Markup = new Regex("<inline-markup.*?</inline-markup>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        MatchCollection regMarkup = Markup.Matches(tempString);
        for (int i = 0; i < regMarkup.Count; i++)
        {
            string str1 = regMarkup[i].Value;
            string str2 = str1.Replace("<inline-markup type=\"url\">", "<FONT color=blue>").Replace("</inline-markup>", "</FONT>");
            tempString = tempString.Replace(regMarkup[i].Value, str2);
        }
        return tempString;
    }
    #endregion


    //=======================================================
    //Conversion from HTML to XML Methods
    //=======================================================

    #region BoldItalic(string textFile)
    public static string BoldItalic(string textFile)
    {
        //Converting <B><I> to <emphasis>
        Regex reg1 = new Regex("<EM>[ ]?<B>.*?</B>[ ]?</EM>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Regex reg2 = new Regex("<EM>[ ]?<STRONG>.*?</STRONG>[ ]?</EM>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Regex reg3 = new Regex("<I>[ ]?<B>.*?</B>[ ]?</I>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Regex reg4 = new Regex("<I>[ ]?<STRONG>.*?</STRONG>[ ]?</I>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        Regex reg5 = new Regex("<B>[ ]?<EM>.*?</EM>[ ]?</B>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Regex reg6 = new Regex("<STRONG>[ ]?<EM>.*?</EM>[ ]?</STRONG>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Regex reg7 = new Regex("<B>[ ]?<I>.*?</I>[ ]?</B>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Regex reg8 = new Regex("<STRONG>[ ]?<I>.*?</I>[ ]?</STRONG>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        MatchCollection colreg1 = reg1.Matches(textFile);
        for (int i = 0; i < colreg1.Count; i++)
        {
            string text1 = colreg1[i].Value;
            string text2 = text1.Replace("<EM> <B>", "<emphasis type=\"bold-italic\">").Replace("<EM><B>", "<emphasis type=\"bold-italic\">");
            text2 = text2.Replace("</B> </EM>", "</emphasis>").Replace("</B></EM>", "</emphasis>");
            textFile = textFile.Replace(colreg1[i].Value, text2);
        }
        MatchCollection colreg2 = reg2.Matches(textFile);
        for (int i = 0; i < colreg2.Count; i++)
        {
            string text1 = colreg2[i].Value;
            string text2 = text1.Replace("<EM> <STRONG>", "<emphasis type=\"bold-italic\">").Replace("<EM><STRONG>", "<emphasis type=\"bold-italic\">");
            text2 = text2.Replace("</STRONG> </EM>", "</emphasis>").Replace("</STRONG></EM>", "</emphasis>");
            textFile = textFile.Replace(colreg2[i].Value, text2);
        }
        MatchCollection colreg3 = reg3.Matches(textFile);
        for (int i = 0; i < colreg3.Count; i++)
        {
            string text1 = colreg3[i].Value;
            string text2 = text1.Replace("<I> <B>", "<emphasis type=\"bold-italic\">").Replace("<I><B>", "<emphasis type=\"bold-italic\">");
            text2 = text2.Replace("</B> </I>", "</emphasis>").Replace("</B></I>", "</emphasis>");
            textFile = textFile.Replace(colreg3[i].Value, text2);
        }
        MatchCollection colreg4 = reg4.Matches(textFile);
        for (int i = 0; i < colreg4.Count; i++)
        {
            string text1 = colreg4[i].Value;
            string text2 = text1.Replace("<I> <STRONG>", "<emphasis type=\"bold-italic\">").Replace("<I><STRONG>", "<emphasis type=\"bold-italic\">");
            text2 = text2.Replace("</STRONG> </I>", "</emphasis>").Replace("</STRONG></I>", "</emphasis>");
            textFile = textFile.Replace(colreg4[i].Value, text2);
        }


        MatchCollection colreg5 = reg5.Matches(textFile);
        for (int i = 0; i < colreg5.Count; i++)
        {
            string text1 = colreg5[i].Value;
            string text2 = text1.Replace("<B> <EM>", "<emphasis type=\"bold-italic\">").Replace("<B><EM>", "<emphasis type=\"bold-italic\">");
            text2 = text2.Replace("</EM> </B>", "</emphasis>").Replace("</EM></B>", "</emphasis>");
            textFile = textFile.Replace(colreg5[i].Value, text2);
        }
        MatchCollection colreg6 = reg6.Matches(textFile);
        for (int i = 0; i < colreg6.Count; i++)
        {
            string text1 = colreg6[i].Value;
            string text2 = text1.Replace("<STRONG> <EM>", "<emphasis type=\"bold-italic\">").Replace("<STRONG><EM>", "<emphasis type=\"bold-italic\">");
            text2 = text2.Replace("</EM> </STRONG>", "</emphasis>").Replace("</EM></STRONG>", "</emphasis>");
            textFile = textFile.Replace(colreg6[i].Value, text2);
        }
        MatchCollection colreg7 = reg7.Matches(textFile);
        for (int i = 0; i < colreg7.Count; i++)
        {
            string text1 = colreg7[i].Value;
            string text2 = text1.Replace("<B> <I>", "<emphasis type=\"bold-italic\">").Replace("<B><I>", "<emphasis type=\"bold-italic\">");
            text2 = text2.Replace("</I> </B>", "</emphasis>").Replace("</I></B>", "</emphasis>");
            textFile = textFile.Replace(colreg7[i].Value, text2);
        }
        MatchCollection colreg8 = reg8.Matches(textFile);
        for (int i = 0; i < colreg8.Count; i++)
        {
            string text1 = colreg8[i].Value;
            string text2 = text1.Replace("<STRONG> <I>", "<emphasis type=\"bold-italic\">").Replace("<STRONG><I>", "<emphasis type=\"bold-italic\">");
            text2 = text2.Replace("</I> </STRONG>", "</emphasis>").Replace("</I></STRONG>", "</emphasis>");
            textFile = textFile.Replace(colreg8[i].Value, text2);
        }

        Regex italicOPen = new Regex(@"<I>|<EM>", RegexOptions.IgnoreCase);
        MatchCollection regitalicOPen = italicOPen.Matches(textFile);
        for (int i = 0; i < regitalicOPen.Count; i++)
        {
            textFile = textFile.Replace(regitalicOPen[i].Value, "<emphasis type=\"italic\">");
        }
        Regex italicClose = new Regex(@"</I>|</EM>", RegexOptions.IgnoreCase);
        MatchCollection regitalicClose = italicClose.Matches(textFile);
        for (int i = 0; i < regitalicClose.Count; i++)
        {
            textFile = textFile.Replace(regitalicClose[i].Value, "</emphasis>");
        }
        Regex boldOpen = new Regex(@"<B>|<STRONG>", RegexOptions.IgnoreCase);
        MatchCollection regboldOpen = boldOpen.Matches(textFile);
        for (int i = 0; i < regboldOpen.Count; i++)
        {
            textFile = textFile.Replace(regboldOpen[i].Value, "<emphasis type=\"bold\">");
        }
        Regex boldClose = new Regex(@"</B>|</STRONG>", RegexOptions.IgnoreCase);
        MatchCollection regboldClose = boldClose.Matches(textFile);
        for (int i = 0; i < regboldClose.Count; i++)
        {
            textFile = textFile.Replace(regboldClose[i].Value, "</emphasis>");
        }
        textFile = Regex.Replace(textFile, "<P>[ ]*</P>", "");
        Regex Num = new Regex(@"<PRE>.*?</PRE>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        MatchCollection regNum = Num.Matches(textFile);
        for (int i = 0; i < regNum.Count; i++)
        {
            string str1 = regNum[i].Value;
            string str2 = str1.Replace("<PRE>", "<num>").Replace("</PRE>", "</num>");
            textFile = textFile.Replace(regNum[i].Value, str2);
        }
        //Removing unwanted background spans
        MatchCollection unWantedSpans = Regex.Matches(textFile, "<SPAN[ ]style.*?>.*?</SPAN>");
        for (int i = 0; i < unWantedSpans.Count; i++)
        {
            string str1 = unWantedSpans[i].Value;
            str1 = Regex.Replace(str1, "</?SPAN.*?>", "");
            textFile = textFile.Replace(unWantedSpans[i].Value, str1);
        }
        Regex Span = new Regex(@"<SPAN>.*?</SPAN>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        MatchCollection regSpan = Span.Matches(textFile);
        for (int i = 0; i < regSpan.Count; i++)
        {
            string str1 = regSpan[i].Value;
            string str2 = str1.Replace("<SPAN>", "<para>").Replace("</SPAN>", "</para>");
            textFile = textFile.Replace(regSpan[i].Value, str2);
        }
    Come:
        Regex Div = new Regex(@"<DIV>.*?</DIV>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        MatchCollection regDiv = Div.Matches(textFile);
        for (int i = 0; i < regDiv.Count; i++)
        {
            if (Regex.IsMatch(regDiv[i].Value, "<BR.*?>"))
            {
                string newVal = Regex.Replace(regDiv[i].Value, "<BR.*?>", "</DIV><DIV>");
                textFile = textFile.Replace(regDiv[i].Value, newVal);
                goto Come;
            }
            string str1 = regDiv[i].Value;
            string str2 = str1.Replace("<DIV>", "<line>").Replace("</DIV>", "</line>");
            textFile = textFile.Replace(regDiv[i].Value, str2);
        }
        return textFile;
    }
    #endregion

    #region CommonConversion(string textFile)
    public static string CommonConversion(string textFile)
    {
        //Converting Keywords of Index 
        MatchCollection keyCol = Regex.Matches(textFile, "<BR><FONT color=gray>.*?</FONT>");
        for (int g = 0; g < keyCol.Count; g++)
        {
            string st1 = keyCol[g].Value.Replace("<FONT color=gray>", "").Replace("</FONT>", "").Replace("<BR>", "");
            string st2 = "<keywrd logical=\"true\" key=" + st1 + " />";
            textFile = textFile.Replace(keyCol[g].Value, st2);
        }

        //textFile = textFile.Replace("<FONT color=red>V</FONT>", "<break type=\"volume\" id=\"0\" num=\"v\" />");
        MatchCollection volumeCol = Regex.Matches(textFile, "<FONT color=red>.*?</FONT>");
        for (int g = 0; g < volumeCol.Count; g++)
        {
            string st1 = volumeCol[g].Value.Replace("<FONT color=red>", "").Replace("</FONT>", "");
            string st2 = "<break id=\"" + st1 + "\" type=\"volume\" num=\"V\" />";
            textFile = textFile.Replace(volumeCol[g].Value, st2);
        }
        //textFile = textFile.Replace("<FONT color=orange>P</FONT>", "<break type=\"page\" id=\"0\" num=\"0\" />");

        MatchCollection pageCol = Regex.Matches(textFile, "<FONT color=orange>.*?</FONT>");
        for (int g = 0; g < pageCol.Count; g++)
        {
            string st1 = pageCol[g].Value.Replace("<FONT color=orange>", "").Replace("</FONT>", "");
            string st2 = "<break id=\"" + st1 + "\" type=\"page\" num=\"" + st1 + "\" />";
            textFile = textFile.Replace(pageCol[g].Value, st2);
        }

        Regex Markup = new Regex("<FONT color=blue.*?</FONT>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        MatchCollection regMarkup = Markup.Matches(textFile);
        for (int i = 0; i < regMarkup.Count; i++)
        {
            string str1 = regMarkup[i].Value;
            string str2 = str1.Replace("<FONT color=blue>", "<inline-markup type=\"url\">").Replace("</FONT>", "</inline-markup>");
            str2 = Regex.Replace(str2, "</?A.*?>", "");//Removing <A> tag from inline markup
            textFile = textFile.Replace(regMarkup[i].Value, str2);
        }
        Regex Footnote = new Regex("<FONT color=green><SUP>.*?</SUP></FONT>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        MatchCollection regFootnote = Footnote.Matches(textFile);
        for (int i = 0; i < regFootnote.Count; i++)
        {
            string footDef = Regex.Match(textFile, "<BR>.*?</FONT>").Value;
            textFile = textFile.Replace(footDef, "");
            footDef = footDef.Replace("<FONT color=green><SUB>", "<footnote id=\"" + Regex.Match(regFootnote[i].Value, "[0-9]+").Value + "\">").Replace("</SUB></FONT>", "</footnote>");
            footDef = Regex.Replace(footDef, "<BR.*?>", "");
            textFile = textFile.Replace(regFootnote[i].Value, footDef);
        }

        textFile = textFile.Replace("&nbsp;", " ");
        /*Regex spaces = new Regex(@"[ ][ ]+", RegexOptions.IgnoreCase);
        MatchCollection regspaces = spaces.Matches(textFile);
        for (int i = 0; i < regspaces.Count; i++)
        {
            textFile = textFile.Replace(regspaces[i].Value, " ");
        }*/
        return textFile;
    }
    #endregion

    //=======================================================
    //Content Correction Methods
    //=======================================================

    //Create Options For Replacement
    #region static string[] CreateOptionsList(System.Xml.XmlNode OptionList, string Text)
    public static string[] CreateOptionsList(System.Xml.XmlNode OptionList, string Text)
    {
        string[] OptionsList = new string[OptionList.SelectNodes("option").Count + 2];
        OptionsList[0] = "Others";
        OptionsList[1] = Text;
        int OptionIndex = 1;
        foreach (System.Xml.XmlNode Option in OptionList.SelectNodes("option"))
        {
            string OptionText = Text;
            string TargetPattern = Option.SelectSingleNode("pattern").InnerText;
            string Replacement = Option.SelectSingleNode("replacement").InnerText;
            OptionText = System.Text.RegularExpressions.Regex.Replace(Text, TargetPattern, Replacement);
            OptionsList[++OptionIndex] = OptionText;
        }
        return OptionsList;
    }
    #endregion

    ///Loading Xml encrypted file
    public string LoadEncryptedXmlFile()
    {

        Stream xmlStream = objImageIndex.GetHeader(CommonClass.xmlFilePath, true);
        if (xmlStream.Length == 0)
            throw new FileLoadException();
        StreamReader reader = new StreamReader(xmlStream);
        byte[] bytes1 = new byte[xmlStream.Length];
        xmlStream.Position = 0;
        xmlStream.Read(bytes1, 0, (int)xmlStream.Length);
        string xml = System.Text.Encoding.Unicode.GetString(bytes1);

        return xml;
    }

    public string LoadEncryptedXmlFile(string filePath)
    {

        Stream xmlStream = objImageIndex.GetHeader(filePath, true);
        StreamReader reader = new StreamReader(xmlStream);
        byte[] bytes1 = new byte[xmlStream.Length];
        xmlStream.Position = 0;
        xmlStream.Read(bytes1, 0, (int)xmlStream.Length);
        string xml = System.Text.Encoding.Unicode.GetString(bytes1);
        return xml;
    }

    /// <summary>
    /// Saves the xml doc in encrypted fashion, using the outer xml of the document element, Saves the file in xmlFilePath location
    /// </summary>
    public void SaveEncryptedXmlFile(string RootOuterXml)
    {
        objImageIndex.SetHeader(RootOuterXml, CommonClass.xmlFilePath);
    }
    /// <summary>
    /// Saves the xml doc in encrypted fashion, using the outer xml of the document element, Saves the file in xmlFilePath location
    /// </summary>

    public void SaveEncryptedXmlFile(string RootOuterXml, string filePath)
    {
        objImageIndex.SetHeader(RootOuterXml, filePath);
    }
    //private void ConvertNpara(XmlNode origNodyCopy)
    //{

    //    convertedNode = GlobalVar.PBPDocument.CreateElement("npara");
    //    string origNodeXml = origNodyCopy.InnerXml;
    //    origNodeXml = Regex.Replace(origNodeXml, "</?num.*?>", "");
    //    origNodeXml = Regex.Replace(origNodeXml, "</?para.*?>", "");
    //    origNodeXml = Regex.Replace(origNodeXml, "</?line.*?>", "");

    //    ///Checking extra npara parameters
    //    if (npara.hasNumbers)
    //    {
    //        XmlNodeList lnNodes = origNodyCopy.SelectNodes("descendant::ln|descendant::break");
    //        int iterat = 0;
    //        foreach (XmlNode lnNode in lnNodes)
    //        {
    //            string xml = lnNode.InnerXml;
    //            if (iterat == 0)
    //            {
    //                ++iterat;
    //                string num = xml.Split(' ')[0];
    //                int r = 0;
    //                string empTag = "";
    //                if (Regex.Match(num, "<emphasis").Success)
    //                {
    //                    r = 1;
    //                    empTag = Regex.Match(xml, "<emphasis.*?>").Value.ToString();
    //                    xml = Regex.Replace(xml, "<emphasis.*?>", "");
    //                    num = xml.Split(' ')[0];
    //                }
    //                if (Regex.Match(num, "</emphasis>").Success)
    //                {
    //                    r = 2;
    //                    num = Regex.Replace(num, "</emphasis>", "");
    //                }

    //                string sentence = xml.Substring(xml.IndexOf(' '));
    //                if (r == 0 || r == 2)
    //                {
    //                    xml = "<num>" + num + "</num>" + sentence;
    //                }
    //                else if (r == 1)
    //                {
    //                    xml = "<num>" + num + "</num>" + empTag + sentence;
    //                }
    //            }
    //            lnNode.InnerXml = xml;
    //            convertedNode.InnerXml += lnNode.OuterXml;
    //        }
    //    }
    //    else
    //    {
    //        XmlNodeList lnNodes = origNodyCopy.SelectNodes("descendant::ln|descendant::break");
    //        if (npara.StartNum == NparaStartNum.interger)
    //        {
    //            int num = 1;
    //            if (Regex.Match(cbNparaStartNum.Text, "[0-9]+").Success)
    //            {
    //                num = int.Parse(cbNparaStartNum.Text);
    //            }
    //            foreach (XmlNode lnNode in lnNodes)
    //            {
    //                string xml = lnNode.InnerXml;
    //                xml = "<num>" + num++ + frmConvertParaDlg.NparaSeparator + "</num>" + xml;
    //                lnNode.InnerXml = xml;
    //                convertedNode.InnerXml += lnNode.OuterXml;
    //            }
    //        }else if (npara.StartNum == NparaStartNum.alphabet)
    //        {
    //            char num = 'a';
    //            foreach (XmlNode lnNode in lnNodes)
    //            {
    //                string xml = lnNode.InnerXml;
    //                xml = "<num>" + num++ + frmConvertParaDlg.NparaSeparator + "</num>" + xml;
    //                lnNode.InnerXml = xml;
    //                convertedNode.InnerXml += lnNode.OuterXml;
    //            }
    //        }
    //        else if (npara.StartNum == NparaStartNum.roman)
    //        {
    //            string[] romans = { "i", "ii", "iii", "iv", "v", "vi", "vii", "viii", "ix", "x", "xi", "xii", "xiii", "xiv", "xv", "xvi", "xvii", "xviii", "xix", "xxi", "xxii", "xxiii", "xxiv", "xxv", "xxvi", "xxvii", "xxviii", "xxix", "xxx" };
    //            for (int i = 0; i < lnNodes.Count; i++)//each (XmlNode lnNode in lnNodes)
    //            {
    //                XmlNode lnNode = lnNodes[i];
    //                string xml = lnNode.InnerXml;
    //                xml = "<num>" + romans[i] + frmConvertParaDlg.NparaSeparator + "</num>" + xml;
    //                lnNode.InnerXml = xml;
    //                convertedNode.InnerXml += lnNode.OuterXml;
    //            }
    //        }
    //    }
    //    #region Copy original node attributes

    //    foreach (XmlAttribute attr in origNodyCopy.Attributes)
    //    {
    //        if (attr.Name == "id" | attr.Name == "pnum" | attr.Name == "text-indent" | attr.Name == "padding-bottom")
    //            ((XmlElement)this.convertedNode).SetAttribute(attr.Name, attr.Value);
    //    }
    //    #endregion
    //}

    public static string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    public static string Decrypt(string cipherText)
    {
        if (cipherText == null)
            return null;

        string EncryptionKey = "MAKV2";
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
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
using System.Text.RegularExpressions;
using System.Xml;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
namespace Outsourcing_System
{
    public class ConversionClass
    {

        #region Method For Filter HTML

        public string filterHtml(string stringToFilter)
        {
            stringToFilter = stringToFilter.Replace(@"font-size: 10.0pt;", "").Replace(@"font-size: 12.0pt;", "").Replace(@"font-size:10.0pt;", "").Replace(@"font-size:12.0pt;", "").Replace(@"FONT-SIZE:10.0pt;", "").Replace(@"FONT-SIZE:12.0pt;", "").Replace(@"FONT-SIZE: 10.0pt;", "").Replace(@"FONT-SIZE: 12.0pt;", "").Replace(@"FONT-SIZE: 12pt;", "").Replace(@"&amp;", "&").Replace(@"<strong>", "<b>").Replace(@"Garamond", @"Verdana").Replace(@"garamond", @"Verdana").Replace(@"</strong>", @"</b>").Replace("<STRONG>", "<b>").Replace(@"</STRONG>", @"</b>").Replace(@"<em>", @"<i>").Replace(@"</em>", @"</i>").Replace(@"<EM>", @"<i>").Replace(@"</EM>", @"</i>").Replace(@"<BR>", @"<br/>").Replace(@"&nbsp;", @" ").ToString();
            stringToFilter = stringToFilter.Replace("\r\n", " ");
            string filteredString = stringToFilter.Replace("&gt;", ")").Replace("&lt;", "(");
            Regex regEx = new System.Text.RegularExpressions.Regex("<!--.+?-->");
            MatchCollection matchCol = regEx.Matches(filteredString);

            int ind = 0;
            for (int i = 0; i < matchCol.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine("------------------------------------");
                filteredString = filteredString.Remove(matchCol[i].Index + ind, matchCol[i].Length);
                ind -= matchCol[i].Length;
                System.Diagnostics.Debug.WriteLine("Comment Replaced");
                System.Diagnostics.Debug.WriteLine("------------------------------------");
                continue;
            }

            regEx = new System.Text.RegularExpressions.Regex("<.+?>");
            matchCol = regEx.Matches(filteredString);

            ind = 0;
            for (int i = 0; i < matchCol.Count; i++)
            {

                string textToReplace = filteredString.Substring(matchCol[i].Index + ind, matchCol[i].Length);
                string tagName = textToReplace.Split(' ')[0];
                if (tagName.StartsWith("<?"))
                {
                    System.Diagnostics.Debug.WriteLine("------------------------------------");
                    filteredString = filteredString.Remove(matchCol[i].Index + ind, matchCol[i].Length);
                    ind -= matchCol[i].Length;
                    System.Diagnostics.Debug.WriteLine("Processing Instruction Replaced");
                    System.Diagnostics.Debug.WriteLine("------------------------------------");
                    continue;
                }
                if (tagName.StartsWith("<!"))
                {
                    System.Diagnostics.Debug.WriteLine("------------------------------------");
                    filteredString = filteredString.Remove(matchCol[i].Index + ind, matchCol[i].Length);
                    ind -= matchCol[i].Length;
                    System.Diagnostics.Debug.WriteLine("comment Replaced");
                    System.Diagnostics.Debug.WriteLine("------------------------------------");
                    continue;
                }
                if (tagName.IndexOf(":") > 0)
                {
                    if (tagName.IndexOf("/") < 0)
                    {
                        System.Diagnostics.Debug.WriteLine("------------------------------------");
                        filteredString = filteredString.Remove(matchCol[i].Index + 1 + ind, matchCol[i].Length - 2);
                        filteredString = filteredString.Insert(matchCol[i].Index + 1 + ind, "x");
                        ind -= matchCol[i].Length - 3;
                        System.Diagnostics.Debug.WriteLine(filteredString.Substring(matchCol[i].Index + 1 + ind, 3));
                        System.Diagnostics.Debug.WriteLine("------------------------------------");
                    }
                    else
                    {
                        if (tagName.StartsWith("</"))
                        {
                            System.Diagnostics.Debug.WriteLine("------------------------------------");
                            filteredString = filteredString.Remove(matchCol[i].Index + 2 + ind, matchCol[i].Length - 3);
                            filteredString = filteredString.Insert(matchCol[i].Index + 2 + ind, "x");
                            ind -= matchCol[i].Length - 4;
                            System.Diagnostics.Debug.WriteLine(filteredString.Substring(matchCol[i].Index + 1 + ind, 4));
                            System.Diagnostics.Debug.WriteLine("------------------------------------");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("------------------------------------");
                            filteredString = filteredString.Remove(matchCol[i].Index + ind, matchCol[i].Length);
                            ind -= matchCol[i].Length;
                            System.Diagnostics.Debug.WriteLine("Empty tag removed");
                            System.Diagnostics.Debug.WriteLine("------------------------------------");
                            continue;
                        }
                    }
                }
            }
            regEx = null;
            regEx = new Regex("&");
            matchCol = regEx.Matches(filteredString);

            ind = 0;
            int iTempIndex;
            for (int i = 0; i < matchCol.Count; i++)
            {
                if (filteredString.IndexOf("&quot;", matchCol[i].Index + ind) > 0)
                {
                    filteredString = filteredString.Remove(matchCol[i].Index + ind, 6);
                    filteredString = filteredString.Insert(matchCol[i].Index + ind, "'");
                    ind -= 5;
                }
                else
                    if (filteredString.IndexOf("&amp;", matchCol[i].Index + ind) < 0)
                    {
                        iTempIndex = matchCol[i].Index;
                        filteredString = filteredString.Insert(iTempIndex + ind + 1, "amp;");
                        ind += 4;
                    }
                    else
                        if (filteredString.IndexOf("&amp;amp;", matchCol[i].Index + ind) > 0)
                        {
                            filteredString = filteredString.Remove(matchCol[i].Index + ind + 1, 4);
                            ind -= 4;
                        }
            }

            XmlDocument xmlFilterString = new XmlDocument();
            xmlFilterString.LoadXml("<FilterString>" + filteredString + "</FilterString>");
            XmlNodeList xmlAllNodes = xmlFilterString.DocumentElement.SelectNodes("//*");
            // remove attributes to all elements
            foreach (XmlNode xmlNodeToClean in xmlAllNodes)
            {
                if (xmlNodeToClean.Name != "FONT" && xmlNodeToClean.Name != "break" && (xmlNodeToClean.Attributes != null))
                    xmlNodeToClean.Attributes.RemoveAll();
            }

            filteredString = xmlFilterString.DocumentElement.InnerXml;
            regEx = new Regex("<x>");
            filteredString = regEx.Replace(filteredString, "");
            regEx = new Regex("</x>");
            filteredString = regEx.Replace(filteredString, "");
            regEx = new Regex("<span>");
            filteredString = regEx.Replace(filteredString, "");
            regEx = new Regex("</span>");
            filteredString = regEx.Replace(filteredString, "");
            regEx = new Regex("<meta />");
            filteredString = regEx.Replace(filteredString, "");
            regEx = new Regex("<style></style>");
            filteredString = regEx.Replace(filteredString, "");
            regEx = new Regex("<link />");
            filteredString = regEx.Replace(filteredString, "");
            regEx = new Regex("·         ");
            filteredString = regEx.Replace(filteredString, "• ");
            if (filteredString.Replace(@"<p>", "").Replace(@"<P>", "").Replace(@"</p>", "").Replace(@"</P>", "").Replace(@"<br/>", "").Replace(@"<BR/>", "").Replace(@"<br />", "") == "")
            {
                filteredString = "";
            }
            return filteredString;
        }
        #endregion

        #region HTML conversion methods
        public string Xml2HtmlBoldItalic(string tempString)
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
        /// <summary>
        /// Converts html to a refined form, xhtml
        public string Html2XmlCommonConversion(string textFile)
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
                string st2 = "<break type=\"volume\" id=\"" + st1 + "\" num=\"v\" />";
                textFile = textFile.Replace(volumeCol[g].Value, st2);
            }
            //textFile = textFile.Replace("<FONT color=orange>P</FONT>", "<break type=\"page\" id=\"0\" num=\"0\" />");

            MatchCollection pageCol = Regex.Matches(textFile, "<FONT color=orange>.*?</FONT>");
            for (int g = 0; g < pageCol.Count; g++)
            {
                string st1 = pageCol[g].Value.Replace("<FONT color=orange>", "").Replace("</FONT>", "");
                string st2 = "<break type=\"page\" id=\"" + st1 + "\" num=\"" + st1 + "\" />";
                textFile = textFile.Replace(pageCol[g].Value, st2);
            }

            //Regex Markup = new Regex("<FONT color=blue.*?</FONT>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            //MatchCollection regMarkup = Markup.Matches(textFile);
            //for (int i = 0; i < regMarkup.Count; i++)
            //{
            //    string str1 = regMarkup[i].Value;
            //    string str2 = str1.Replace("<FONT color=\"blue\">", "<inline-markup type=\"url\">").Replace("</FONT>", "</inline-markup>");
            //    str2 = Regex.Replace(str2, "</?A.*?>", "");//Removing <A> tag from inline markup
            //    textFile = textFile.Replace(regMarkup[i].Value, str2);
            //}

            Regex Markup = new Regex("<a.*?>.*?</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MatchCollection regMarkup = Markup.Matches(textFile);
            for (int i = 0; i < regMarkup.Count; i++)
            {
                string str1 = regMarkup[i].Value;
                str1=str1.Replace("<a>", "<inline-markup type=\"url\">");
                string str2 = str1.Replace("</a>", "</inline-markup>");
                
                str2 = Regex.Replace(str2, "</?A.*?>", "");//Removing <A> tag from inline markup
                textFile = textFile.Replace(regMarkup[i].Value, str2);
            }
            Regex Footnote = new Regex("<FONT color=\"green\"><SUP>.*?</SUP></FONT>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MatchCollection regFootnote = Footnote.Matches(textFile);
            for (int i = 0; i < regFootnote.Count; i++)
            {
                string footDef = Regex.Match(textFile, "<br />.*?</FONT>").Value;
                textFile = textFile.Replace(footDef, "");
                footDef = footDef.Replace("<FONT color=\"green\"><sub>", "<footnote id=\"" + Regex.Match(regFootnote[i].Value, "[0-9]+").Value + "\">").Replace("</sub></FONT>", "</footnote>");
                footDef = Regex.Replace(footDef, "<br.*?>", "");
                textFile = textFile.Replace(regFootnote[i].Value, footDef);
            }

            textFile = textFile.Replace("&nbsp;", " ");
            Regex spaces = new Regex(@"[ ][ ]+", RegexOptions.IgnoreCase);
            MatchCollection regspaces = spaces.Matches(textFile);
            for (int i = 0; i < regspaces.Count; i++)
            {
                textFile = textFile.Replace(regspaces[i].Value, " ");
            }
            return textFile;
        }
        /// <summary>
        /// Converts the refined xhtml to xml
        public string Html2XmlBoldItalic(string textFile)
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

        #region  void ExtractPages(string inputFile, string outputFile, int start, int end)
        //public void ExtractPages(string inputFile, string outputFile, int start, int end)
        //{
        //    // get input document
        //    PdfReader inputPdf = new PdfReader(inputFile);
        //    // retrieve the total number of pages
        //    int pageCount = inputPdf.NumberOfPages;
        //    if (end < start || end > pageCount)
        //    {
        //        end = pageCount;
        //    }
        //    // load the input document
        //    Document inputDoc = new Document(inputPdf.GetPageSizeWithRotation(1));

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
        //            inputDoc.SetPageSize(inputPdf.GetPageSizeWithRotation(i));
        //            inputDoc.NewPage();

        //            PdfImportedPage page =
        //                outputWriter.GetImportedPage(inputPdf, i);
        //            int rotation = inputPdf.GetPageRotation(i);

        //            if (rotation == 90 || rotation == 270)
        //            {
        //                cb1.AddTemplate(page, 0, -1f, 1f, 0, 0,
        //                    inputPdf.GetPageSizeWithRotation(i).Height);
        //            }
        //            else
        //            {
        //                cb1.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
        //            }
        //        }
        //        inputDoc.Close();
        //    }
        //}

        public void ExtractPages(string inputFile, string outputFile, int start, int end)
        {
            PdfReader reader = null;
            Document document = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage = null;

            try
            {
                using (Stream outPutStream = new FileStream(outputFile, FileMode.Create))
                {
                    // Intialize a new PdfReader instance with the contents of the source Pdf file:
                    reader = new PdfReader(inputFile);

                    // Capture the correct size and orientation for the page:
                    document = new Document(reader.GetPageSizeWithRotation(1));

                    // Initialize an instance of the PdfCopyClass with the source 
                    // document and an output file stream:
                    //pdfCopyProvider = new PdfCopy(document, new FileStream(outputFile, FileMode.Create));

                    pdfCopyProvider = new PdfCopy(document, outPutStream);
                    document.Open();

                    // Extract the desired page number:
                    importedPage = pdfCopyProvider.GetImportedPage(reader, start);
                    pdfCopyProvider.AddPage(importedPage);

                    document.Close();
                    pdfCopyProvider.Close();
                    importedPage.ClosePath();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
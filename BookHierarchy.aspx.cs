using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;

namespace Outsourcing_System
{
    public partial class BookHierarchy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PDFViewerSource.FilePath = System.Configuration.ConfigurationManager.AppSettings["MainDirectory"] + "/1200/1200.pdf";
            LoadPBPDocument(rhywFilePath());
            //PopulatePBPBook();
        }
        #region |Fields and Properties|

        ConversionClass objConversionClass = new ConversionClass();
        MyDBClass objMyDBClass = new MyDBClass();
        GlobalVar objGlobal = new GlobalVar();
        private bool ConstructionModel = false;
        private bool OpenFlag = true;
        private bool ValidationFlag;

        #endregion

        #region |Methods|

        public string rhywFilePath()
        {
            string bookid = "1200";
            return objMyDBClass.MainDirPhyPath + "/" + bookid + "/" + bookid + "-1/TaggingUntagged/" + bookid + "-1.rhyw";
        }
        private void addingCoverChild(TreeNode coverTreenode, System.Xml.XmlNode coverXMLNode)
        {
            if (ConstructionModel == false) // If image-model is selected
            {
                System.Xml.XmlNode im = objGlobal.PBPDocument.CreateElement("image-model");
                coverXMLNode.AppendChild(im);
                System.Xml.XmlNode n = objGlobal.PBPDocument.CreateElement("front");
                n.Attributes.Append(objGlobal.PBPDocument.CreateAttribute("image-url"));
                im.AppendChild(n);
                n = objGlobal.PBPDocument.CreateElement("spine");
                n.Attributes.Append(objGlobal.PBPDocument.CreateAttribute("image-url"));
                im.AppendChild(n);
                n = objGlobal.PBPDocument.CreateElement("back");
                n.Attributes.Append(objGlobal.PBPDocument.CreateAttribute("image-url"));
                im.AppendChild(n);
                //Shoaib here
                TreeNode imTreenode = new TreeNode("image-model");
                imTreenode.Text = im.Name;
                coverTreenode.ChildNodes.Add(imTreenode);
                //~Shoaib here
                //Shoaib here 
                System.Xml.XmlNode colorTone = objGlobal.PBPDocument.CreateElement("color-code");
                colorTone.InnerText = "1";
                im.AppendChild(colorTone);
                //~Shoaib here
            }
            else
            {
                System.Xml.XmlNode im = objGlobal.PBPDocument.CreateElement("construction-model");
                TreeNode imTreenode = new TreeNode("construction-model");

                coverXMLNode.AppendChild(im);
                #region Removed Cover attributes for new schema
                //				System.Xml.XmlNode n = GlobalVar.PBPDocument.CreateElement("c-title");
                //				n.AppendChild(GlobalVar.PBPDocument.CreateElement("c-main-title"));
                //				n.AppendChild(GlobalVar.PBPDocument.CreateElement("c-sub-title"));
                //				im.AppendChild(n);
                //				n = GlobalVar.PBPDocument.CreateElement("c-author-list");
                //				n.AppendChild(GlobalVar.PBPDocument.CreateElement("c-author"));
                //				im.AppendChild(n);
                //				n = GlobalVar.PBPDocument.CreateElement("c-category");
                //				im.AppendChild(n);
                #endregion
                System.Xml.XmlNode sec = objGlobal.PBPDocument.CreateElement("c-section");
                XmlAttribute type = sec.Attributes.Append(objGlobal.PBPDocument.CreateAttribute("type"));
                type.Value = "book-info"; //Asad:Update changed from Book-Info -> book-info  (26-04-2007) :: New Schema, Imran Malik, 18/11/2006

                XmlNode head = objGlobal.PBPDocument.CreateElement("cs-head");

                head.AppendChild(objGlobal.PBPDocument.CreateElement("cs-title"));
                sec.AppendChild(head);

                XmlNode body = objGlobal.PBPDocument.CreateElement("body");
                #region Inserting Id attribute in Body initialized by 1 as default By Rizwan Rashid Date:2006-12-08
                //XmlAttribute ID_body = objGlobal.PBPDocument.CreateAttribute("id");
                //ID_body.Value = objGlobal.maxId.ToString();
                //body.Attributes.Append(ID_body);
                #endregion
                sec.AppendChild(body);
                im.AppendChild(sec);
                TreeNode desc = new TreeNode("Book-Description");
                //coverNode.Nodes.Add(desc);
                imTreenode.ChildNodes.Add(desc);
                desc.Text = sec.Name;
                System.Xml.XmlNode cimage = objGlobal.PBPDocument.CreateElement("c-image");
                cimage.Attributes.Append(objGlobal.PBPDocument.CreateAttribute("image-url"));
                XmlNode voiceDescrp = objGlobal.PBPDocument.CreateElement("voice-description");
                //voiceDescrp.AppendChild(objGlobal.PBPDocument.CreateElement("para"));
                cimage.AppendChild(voiceDescrp);
                im.AppendChild(cimage);
                TreeNode img = new TreeNode("Cover-Image");
                //coverNode.Nodes.Add(img);
                imTreenode.ChildNodes.Add(img);
                img.Text = cimage.Name;
                //Shoaib here
                imTreenode.Text = im.Name;
                coverTreenode.ChildNodes.Add(imTreenode);
                //~Shoaib here
                //Shoaib here 
                System.Xml.XmlNode colorTone = objGlobal.PBPDocument.CreateElement("color-code");
                colorTone.InnerText = "1";
                im.AppendChild(colorTone);
                //~Shoaib here
                #region Removed cover attributes for new schema
                //				n = GlobalVar.PBPDocument.CreateElement("c-ISBN");
                //				n.Attributes.Append(GlobalVar.PBPDocument.CreateAttribute("edition-value"));
                //				im.AppendChild(n);
                #endregion
            }
        }

        private bool PopulatePBPBook()
        {
            _pbpbook.Nodes.Clear();
            _pbpbook.ShowExpandCollapse = true;
            TreeNode rootNode = new TreeNode("Book");
            TreeNode metaNode = new TreeNode("Meta");
            TreeNode bookrep_Info = new TreeNode("book-info"); //Asad:Update changed from Book-Info -> book-info  (26-04-2007) :: new added by rizwan Dated 2006-11-09
            //TreeNode book_genre=new TreeNode("Book-Genre");//by rizwan Dated 2006-11-10
            TreeNode bisac = new TreeNode("BISAC");//Asad:Update for Standard BISAC code change in schema 23-04-2007
            TreeNode frontNode = new TreeNode("Front");
            TreeNode coverNode = new TreeNode("Cover");
            // New Schema pbp-front->title-block by Imran Malik 20-11-2006
            TreeNode titleNode = new TreeNode("Title");
            TreeNode translatorNode = new TreeNode("Translator");
            TreeNode book_titleNode = new TreeNode("Book-Title");
            TreeNode authorNode = new TreeNode("Author");
            // End title-block
            TreeNode bookNoticesNode = new TreeNode("Book-Notices");
            TreeNode bodyNode = new TreeNode("Body");
            TreeNode endNode = new TreeNode("End");
            TreeNode postNode = new TreeNode("Post-Section");
            TreeNode preNode = new TreeNode("Pre-Section");
            _pbpbook.Nodes.Add(rootNode);
            rootNode.ChildNodes.Add(metaNode);
            rootNode.ChildNodes.Add(frontNode);
            rootNode.ChildNodes.Add(bodyNode);
            rootNode.ChildNodes.Add(endNode);
            metaNode.ChildNodes.Add(bookrep_Info);//by rizwann Dated 2006-11-09
            frontNode.ChildNodes.Add(coverNode);
            frontNode.ChildNodes.Add(bisac);//by rizwan Dated 2006-11-10:: Asad:Update for Update for Standard BISAC code change in schema 23-04-2007
            // New Schema pbp-front->title-block by Imran Malik 20-11-2006
            titleNode.ChildNodes.Add(book_titleNode);
            titleNode.ChildNodes.Add(authorNode);
            titleNode.ChildNodes.Add(translatorNode);
            frontNode.ChildNodes.Add(titleNode);
            // End title-block
            frontNode.ChildNodes.Add(bookNoticesNode);
            frontNode.ChildNodes.Add(preNode);
            if (OpenFlag)
            {
                //GlobalVar.PBPDocument.Load(PBPBookAbsPath);
                //Stream strm = GlobalVar.Validator.XMLStream;


                try
                {

                    objGlobal.XMLPath = rhywFilePath();
                    Session["XMLPath"] = objGlobal.XMLPath;
                    objGlobal.PBPDocument = new System.Xml.XmlDocument();
                    objGlobal.LoadXml();
                    Session["PBPDocument"] = objGlobal.PBPDocument;

                }
                catch (Exception exp)
                {
                    //MessageBox.Show("Cannot load book, contact support", "Load error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //
                objGlobal.PBPDocument.DocumentElement.Attributes.RemoveAll();
                if (objGlobal.PBPDocument.DocumentElement != null)
                {
                    if (!objGlobal.PBPDocument.DocumentElement.Name.Equals("pbp-book"))
                    {
                        return false;
                    }
                }
                if (objGlobal.PBPDocument.DocumentElement.SelectSingleNode("pbp-front/cover/construction-model") != null)
                {
                    ConstructionModel = true;
                }
                else if (objGlobal.PBPDocument.DocumentElement.SelectSingleNode("pbp-front/cover/image-model") != null)
                {
                    ConstructionModel = false;
                }
                goto j;
            }
            else
            {
                objGlobal.PBPDocument = new System.Xml.XmlDocument();
                objGlobal.PBPDocument.AppendChild(objGlobal.PBPDocument.CreateXmlDeclaration("1.0", "UTF-8", null));
            }
            System.Xml.XmlNode bookNode = objGlobal.PBPDocument.CreateElement("pbp-book");
            objGlobal.PBPDocument.AppendChild(bookNode);
            rootNode.Text = bookNode.Name;
            //do something here for bookrep-info
            System.Xml.XmlNode metaXMLNode = objGlobal.PBPDocument.CreateElement("pbp-meta");
            bookNode.AppendChild(metaXMLNode);
            metaNode.Text = metaXMLNode.Name;
            System.Xml.XmlNode pbpInfoXMLNode = objGlobal.PBPDocument.CreateElement("pbp-info");
            XmlAttribute att = pbpInfoXMLNode.Attributes.Append(objGlobal.PBPDocument.CreateAttribute("schema-name"));
            att.Value = "PBPBook_P02.xsd"; // modify for new schema, Imran Malik 14/12/2006
            pbpInfoXMLNode.Attributes.Append(objGlobal.PBPDocument.CreateAttribute("schema-rev"));
            pbpInfoXMLNode.Attributes.Append(objGlobal.PBPDocument.CreateAttribute("file-name"));
            pbpInfoXMLNode.Attributes.Append(objGlobal.PBPDocument.CreateAttribute("tag-date"));
            att = pbpInfoXMLNode.Attributes.Append(objGlobal.PBPDocument.CreateAttribute("tag-operator"));
            att.Value = "pakistan";
            pbpInfoXMLNode.Attributes.Append(objGlobal.PBPDocument.CreateAttribute("book-title"));
            att = pbpInfoXMLNode.Attributes.Append(objGlobal.PBPDocument.CreateAttribute("book-type"));
            att.Value = "PBPress Novel";
            att = pbpInfoXMLNode.Attributes.Append(objGlobal.PBPDocument.CreateAttribute("publication-status"));
            att.Value = "NOT FOR PUBLICATION";
            att = pbpInfoXMLNode.Attributes.Append(objGlobal.PBPDocument.CreateAttribute("copyright-status"));
            att.Value = "OUT OF COPYRIGHT";
            metaXMLNode.AppendChild(pbpInfoXMLNode);

            #region Doc-Track element to be added and its functionality By Rizwan Rashid Date:2006-11-15
            System.Xml.XmlNode docTrack = objGlobal.PBPDocument.CreateElement("doc-track");
            metaXMLNode.AppendChild(docTrack);
            #endregion

            #region bookrep-info new node in TreeView and its functionality  By Rizwan Rashid Date:2006-11-09
            XmlNode XmlNodebookrep_Info = objGlobal.PBPDocument.CreateElement("bookrep-info");
            bookrep_Info.Text = XmlNodebookrep_Info.Name;
            XmlNode author_ID = objGlobal.PBPDocument.CreateElement("author-id"); //Asad:18-04-2007: updated author_ID to author_id according to the change in the schema
            XmlNodebookrep_Info.AppendChild(author_ID);
            XmlNode book_summary = objGlobal.PBPDocument.CreateElement("book-summary");
            XmlNodebookrep_Info.AppendChild(book_summary);
            XmlNode author_Info = objGlobal.PBPDocument.CreateElement("author-info"); //Asad:Update changed from Author-Info -> author-info  (26-04-2007)
            XmlNodebookrep_Info.AppendChild(author_Info);
            metaXMLNode.AppendChild(XmlNodebookrep_Info);

            #endregion

            System.Xml.XmlNode frontXMLNode = objGlobal.PBPDocument.CreateElement("pbp-front");
            bookNode.AppendChild(frontXMLNode);
            frontNode.Text = frontXMLNode.Name;
            System.Xml.XmlNode coverXMLNode = objGlobal.PBPDocument.CreateElement("cover");
            frontXMLNode.AppendChild(coverXMLNode);
            coverNode.Text = coverXMLNode.Name;
            addingCoverChild(coverNode, coverXMLNode);
            #region Code for adding new functionality of Book-Genre By Rizwan Rashid Date:2006-11-10 :: Asad:Update 23-04-2007
            System.Xml.XmlNode BISAC = objGlobal.PBPDocument.CreateElement("BISAC");
            frontXMLNode.AppendChild(BISAC);
            bisac.Text = BISAC.Name;

            System.Xml.XmlNode BISAC_item = objGlobal.PBPDocument.CreateElement("BISAC-item");
            BISAC.AppendChild(BISAC_item);
            System.Xml.XmlNode BISAC_text = objGlobal.PBPDocument.CreateElement("BISAC-text");
            BISAC_item.AppendChild(BISAC_text);
            System.Xml.XmlNode BISAC_code = objGlobal.PBPDocument.CreateElement("BISAC-code");
            BISAC_item.AppendChild(BISAC_code);
            #endregion

            #region Code for adding ISBN element By Rizwan Rashid Date:2006-11-14
            System.Xml.XmlNode ISBN = objGlobal.PBPDocument.CreateElement("ISBN");
            frontXMLNode.AppendChild(ISBN);
            #endregion

            #region pbp-front-title-block, for new schema, By Imran Malik 20/11/2006
            System.Xml.XmlNode titleXMLNode = objGlobal.PBPDocument.CreateElement("title-block");
            frontXMLNode.AppendChild(titleXMLNode);
            titleNode.Text = titleXMLNode.Name;
            System.Xml.XmlNode bookTitle = objGlobal.PBPDocument.CreateElement("book-title");
            titleXMLNode.AppendChild(bookTitle);
            book_titleNode.Text = bookTitle.Name;
            System.Xml.XmlNode mainTitle = objGlobal.PBPDocument.CreateElement("main-title");
            bookTitle.AppendChild(mainTitle);
            System.Xml.XmlNode subTitle = objGlobal.PBPDocument.CreateElement("sub-title");
            bookTitle.AppendChild(subTitle);
            System.Xml.XmlNode runningheader = objGlobal.PBPDocument.CreateElement("running-header");
            bookTitle.AppendChild(runningheader);
            System.Xml.XmlNode titleAuthor = objGlobal.PBPDocument.CreateElement("author");
            titleXMLNode.AppendChild(titleAuthor);
            authorNode.Text = titleAuthor.Name;
            System.Xml.XmlNode titlefull = objGlobal.PBPDocument.CreateElement("full-name");
            titleAuthor.AppendChild(titlefull);
            /* End Code*/
            System.Xml.XmlNode titlePrenominal = objGlobal.PBPDocument.CreateElement("prenominal");
            titleAuthor.AppendChild(titlePrenominal);
            System.Xml.XmlNode titleFname = objGlobal.PBPDocument.CreateElement("first-name");
            titleAuthor.AppendChild(titleFname);
            System.Xml.XmlNode titleLname = objGlobal.PBPDocument.CreateElement("last-name");
            titleAuthor.AppendChild(titleLname);
            #endregion

            System.Xml.XmlNode booknoticesXMLNode = objGlobal.PBPDocument.CreateElement("book-notices");
            System.Xml.XmlNode copyright = objGlobal.PBPDocument.CreateElement("copyright");
            booknoticesXMLNode.AppendChild(copyright);
            System.Xml.XmlNode disclaimer = objGlobal.PBPDocument.CreateElement("disclaimer");
            booknoticesXMLNode.AppendChild(disclaimer);
            System.Xml.XmlNode reproduction = objGlobal.PBPDocument.CreateElement("reproduction");
            booknoticesXMLNode.AppendChild(reproduction);
            System.Xml.XmlNode pubDate = objGlobal.PBPDocument.CreateElement("publication-date");
            booknoticesXMLNode.AppendChild(pubDate);
            #region Title and Author-Name and Disclaimer is commented here By Rizwan Rashid Date:2006-11-16
            /*System.Xml.XmlNode bnc = objGlobal.PBPDocument.CreateElement("title");
            booknoticesXMLNode.AppendChild(bnc);
            bnc = objGlobal.PBPDocument.CreateElement("author-name");
            booknoticesXMLNode.AppendChild(bnc);
            bnc = objGlobal.PBPDocument.CreateElement("disclaimer");
            booknoticesXMLNode.AppendChild(bnc);*/
            #endregion
            frontXMLNode.AppendChild(booknoticesXMLNode);
            bookNoticesNode.Text = booknoticesXMLNode.Name;
            System.Xml.XmlNode tocXMLNode = objGlobal.PBPDocument.CreateElement("toc");
            frontXMLNode.AppendChild(tocXMLNode);
            System.Xml.XmlNode bodyXMLNode = objGlobal.PBPDocument.CreateElement("pbp-body");
            bookNode.AppendChild(bodyXMLNode);
            bodyNode.Text = bodyXMLNode.Name;
            System.Xml.XmlNode endXMLNode = objGlobal.PBPDocument.CreateElement("pbp-end");
            bookNode.AppendChild(endXMLNode);
            endNode.Text = endXMLNode.Name;
            endNode.ChildNodes.Add(postNode);
        j:
            _pbpbook.ExpandAll();

            //CreatePBPBookMenu();
            // CreateParaMenu();
            // MaximumId(); // call maximum ID for finding greatest id 
            return true;
        }

        private void LoadPBPDocument(string path)
        {
            try
            {
                string strXpath = "";
                if (!PopulatePBPBook())
                {
                    return;
                }
                System.Xml.XmlNode pNode;
                System.Xml.XmlNode pbpRoot = objGlobal.PBPDocument.DocumentElement;
                foreach (TreeNode tNode in _pbpbook.Nodes[0].ChildNodes)
                {
                    if (tNode.Text.Equals("Meta"))
                    {
                        pNode = pbpRoot.SelectSingleNode("pbp-meta");
                        if (pNode != null)
                        {
                            tNode.Text = pNode.Name;
                            strXpath = pNode.OuterXml.Substring(0, pNode.OuterXml.IndexOf('>') + 1);
                            tNode.Value = strXpath;
                            tNode.Text = "<div oncontextmenu=\"ContextShow(event);return false\">" + tNode.Text + "<span style=\" color:White;\">@@" + strXpath + "</span></div>";
                            tNode.NavigateUrl = "javascript:void(0)";
                            #region code for bookrep-info By Rizwan Rashid Date:2006-11-10
                            System.Xml.XmlNode booknode;
                            foreach (TreeNode binfo in tNode.ChildNodes)
                            {
                                if (binfo.Text == "book-info") //Asad:Update changed from Book-Info -> book-info  (26-04-2007)
                                {
                                    booknode = pNode.SelectSingleNode("bookrep-info");
                                    binfo.Text = booknode.Name;
                                    strXpath = booknode.OuterXml.Substring(0, booknode.OuterXml.IndexOf('>') + 1);
                                    binfo.Value = strXpath;
                                    binfo.Text = "<div oncontextmenu=\"ContextShow(event);return false\">" + binfo.Text + "<span style=\" color:White;\">@@" + strXpath + "</span></div>";
                                    binfo.NavigateUrl = "javascript:void(0)";
                                }
                            }
                            #endregion
                        }
                    }
                    else if (tNode.Text.Equals("Front"))
                    {
                        pNode = pbpRoot.SelectSingleNode("pbp-front");
                        if (pNode != null)
                        {
                            tNode.Text = pNode.Name;
                            strXpath = pNode.OuterXml.Substring(0, pNode.OuterXml.IndexOf('>') + 1);
                            tNode.Value = strXpath;
                            tNode.Text = "<div oncontextmenu=\"ContextShow(event);return false\">" + tNode.Text + "<span style=\" color:White;\">@@" + strXpath + "</span></div>";
                            tNode.NavigateUrl = "javascript:void(0)";
                            System.Xml.XmlNode fpn;
                            foreach (TreeNode ftn in tNode.ChildNodes)
                            {
                                if (ftn.Text == "Cover")
                                {
                                    fpn = pNode.SelectSingleNode("cover");
                                    if (fpn != null)
                                    {
                                        ftn.Text = fpn.Name;
                                        strXpath = fpn.OuterXml.Substring(0, fpn.OuterXml.IndexOf('>') + 1);
                                        ftn.Value = strXpath;
                                        ftn.Text = "<div oncontextmenu=\"ContextShow(event);return false\">" + ftn.Text + "<span style=\" color:White;\">@@" + strXpath + "</span></div>";
                                        ftn.NavigateUrl = "javascript:void(0)";
                                    }
                                    //Shoaib here
                                    System.Xml.XmlNode constMdlXmlnode, imgMdlXmlnode;
                                    constMdlXmlnode = fpn.SelectSingleNode("construction-model");
                                    imgMdlXmlnode = fpn.SelectSingleNode("image-model");
                                    if (constMdlXmlnode != null)
                                        this.ConstructionModel = true;
                                    else if (imgMdlXmlnode != null)
                                        this.ConstructionModel = false;
                                    else
                                        this.ConstructionModel = true;
                                    //~Shoaib here
                                    if (ConstructionModel)
                                    {
                                        ftn.ChildNodes.Clear();
                                        //Shoaib here :changes: all nodes that were previosuly appended in cover treenode(ftn) are now appended in construction model treenode(constMdl) which is appended in cover treenode 
                                        TreeNode constMdlTreenode = new TreeNode("construction-model");
                                        ftn.ChildNodes.Add(constMdlTreenode);
                                        constMdlTreenode.Text = constMdlXmlnode.Name;
                                        //~Shoaib here
                                        TreeNode bookdesc = new TreeNode("Book-Description");
                                        bookdesc.Text = fpn.SelectSingleNode("construction-model/c-section[@type='book-info']").Name;  //Asad:Update changed from Book-Info -> book-info  (26-04-2007) :: New Schema changes by Imran Malik, 20/11/200
                                        constMdlTreenode.ChildNodes.Add(bookdesc);
                                        XmlNode authorabtnode = fpn.SelectSingleNode("construction-model/c-section[@type='author-info']"); //Asad:Update changed from Author-Info -> author-info  (26-04-2007) :: New Schema changes by Imran Malik, 20/11/2006
                                        if (authorabtnode != null)
                                        {
                                            TreeNode authorabt = new TreeNode("About-Author");
                                            authorabt.Text = authorabtnode.Name;
                                            constMdlTreenode.ChildNodes.Add(authorabt);
                                        }
                                        TreeNode coverimage = new TreeNode("Cover-Image");
                                        XmlNode imgNode = fpn.SelectSingleNode("construction-model/c-image");
                                        if (imgNode == null)
                                        {
                                            XmlNode cisbnNode = fpn.SelectSingleNode("construction-model/c-ISBN");
                                            System.Xml.XmlNode cimage = objGlobal.PBPDocument.CreateElement("c-image");
                                            fpn.SelectSingleNode("construction-model").InsertBefore(cimage, cisbnNode);
                                            cimage.Attributes.Append(objGlobal.PBPDocument.CreateAttribute("image-url"));
                                            XmlNode vd = objGlobal.PBPDocument.CreateElement("voice-description");
                                            //vd.AppendChild(GlobalVar.PBPDocument.CreateElement("para")); //Asad:Update 31 March,2008, Update in the Schema by removing para
                                            cimage.AppendChild(vd);
                                            coverimage.Text = cimage.Name;
                                        }
                                        else
                                        {
                                            coverimage.Text = imgNode.Name;
                                        }
                                        constMdlTreenode.ChildNodes.Add(coverimage);

                                        constMdlTreenode.ExpandAll();
                                    }
                                    else if (!ConstructionModel)//if the cover model is 'image-model'
                                    {
                                        ftn.ChildNodes.Clear();
                                        //Shoaib here :changes: all nodes that were previosuly appended in cover treenode(ftn) are now appended in construction model treenode(constMdl) which is appended in cover treenode 
                                        TreeNode imgMdlTreenode = new TreeNode("image-model");
                                        ftn.ChildNodes.Add(imgMdlTreenode);
                                        imgMdlTreenode.Text = imgMdlXmlnode.Name;
                                        //ftn.Tag = imgMdlXmlnode;
                                        //~Shoaib here
                                    }
                                }
                                else if (ftn.Text == "Title")
                                {
                                    fpn = pNode.SelectSingleNode("title-block");
                                    if (fpn != null)
                                    {
                                        ftn.Text = fpn.Name;
                                        #region New Schema code for title-block, By Imran Malik Date:2006-11-15
                                        System.Xml.XmlNode transnode;
                                        foreach (TreeNode tnode in ftn.ChildNodes)
                                        {
                                            if (tnode.Text == "Book-Title")
                                            {
                                                transnode = pNode.SelectSingleNode("//title-block/book-title");
                                                tnode.Text = transnode.Name;
                                            }
                                            if (tnode.Text == "Author")
                                            {
                                                transnode = pNode.SelectSingleNode("//title-block/author");
                                                tnode.Text = transnode.Name;
                                            }
                                            if (tnode.Text == "Translator")
                                            {
                                                transnode = pNode.SelectSingleNode("//title-block/translator");
                                                if (transnode != null)
                                                {
                                                    tnode.Text = transnode.Name;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                #region Code Added for book-genre By Rizwan Rashid Dated:2006-11-10 :: Asad:Update for standard BISAC code 23-04-2007
                                else if (ftn.Text == "BISAC") //Asad:Update
                                {
                                    fpn = pNode.SelectSingleNode("BISAC");//Asad:Update
                                    if (fpn != null)
                                    {
                                        ftn.Text = fpn.Name;
                                    }
                                }
                                #endregion
                                else if (ftn.Text == "Book-Notices")
                                {
                                    fpn = pNode.SelectSingleNode("book-notices");
                                    if (fpn != null)
                                    {
                                        ftn.Text = fpn.Name;
                                    }
                                }
                                else if (ftn.Text == "Pre-Section")
                                {
                                    XmlNodeList ps = pNode.SelectNodes("pre-section");
                                    for (int i = 0; i <= ps.Count - 1; i++)
                                    {
                                        fpn = ps.Item(i);
                                        if (fpn != null)
                                        {
                                            if (fpn.HasChildNodes)
                                            {
                                                TreeNode preTree;
                                                if (fpn.Attributes.GetNamedItem("type").Value.Equals("other"))
                                                {
                                                    preTree = new TreeNode("Other");
                                                    strXpath = fpn.OuterXml.Substring(0, fpn.OuterXml.IndexOf('>') + 1);
                                                    preTree.Value = strXpath;
                                                    preTree.Text = "<div oncontextmenu=\"ContextShow(event);return false\">other<span style=\" color:White;\">@@" + strXpath + "</span></div>";
                                                    preTree.NavigateUrl = "javascript:void(0)";
                                                }
                                                else if (fpn.Attributes.GetNamedItem("type").Value.Equals("author-books")) //Asad:Update Jan 01, 2008
                                                {
                                                    preTree = new TreeNode("author-books");
                                                    strXpath = fpn.OuterXml.Substring(0, fpn.OuterXml.IndexOf('>') + 1);
                                                    preTree.Value = strXpath;
                                                    preTree.Text = "<div oncontextmenu=\"ContextShow(event);return false\">author-books<span style=\" color:White;\">@@" + strXpath + "</span></div>";
                                                    preTree.NavigateUrl = "javascript:void(0)";
                                                }
                                                else
                                                {
                                                    preTree = new TreeNode(fpn.SelectSingleNode("head/section-title").InnerText);
                                                    strXpath = fpn.OuterXml.Substring(0, fpn.OuterXml.IndexOf('>') + 1);
                                                    preTree.Value = strXpath;
                                                    preTree.Text = "<div oncontextmenu=\"ContextShow(event);return false\">" + preTree.Text + "<span style=\" color:White;\">@@" + strXpath + "</span></div>";
                                                    preTree.NavigateUrl = "javascript:void(0)";
                                                }
                                                TreeNode HeadingTree = generateHeadings(fpn, 0);
                                                if (HeadingTree != null)
                                                {
                                                    if (HeadingTree.Text != "null")
                                                        preTree.ChildNodes.Add(HeadingTree);
                                                    else
                                                        while (HeadingTree.ChildNodes.Count > 0)
                                                        {
                                                            preTree.ChildNodes.Add(HeadingTree.ChildNodes[0]);
                                                        }
                                                }
                                                ftn.ChildNodes.Add(preTree);
                                                // preTree.Text = fpn.Name;
                                                // preTree.EnsureVisible();
                                                preTree.ExpandAll();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (tNode.Text.Equals("Body"))
                    {
                        System.Xml.XmlNode bNode;
                        pNode = pbpRoot.SelectSingleNode("pbp-body");
                        tNode.Text = pNode.Name;
                        strXpath = pNode.OuterXml.Substring(0, pNode.OuterXml.IndexOf('>') + 1);
                        tNode.Value = strXpath;
                        tNode.Text = "<div oncontextmenu=\"ContextShow(event);return false\">" + tNode.Text + "<span style=\" color:White;\">@@" + strXpath + "</span></div>";
                        tNode.NavigateUrl = "javascript:void(0)";
                        bNode = pNode.FirstChild;
                        if (bNode != null)
                        {
                            if (bNode.Name.Equals("body"))
                            {
                                //_pbpbook.Nodes = tNode;
                                //_paras.SelectedNode = null;
                                goto j;
                            }
                            else if (bNode.Name.Equals("section"))
                            {
                                //**Aatif:Update for adding of Section[Book] on upper Section Hierarchy::Date 29 August 2007								
                                //**Aatif:update:for traversal of Books elemenets ( Part ,Chapter ,Heading)::Date 29 August 2007
                                if (bNode.Attributes[1].Value.Equals("book"))  //Asad:Update for ID as first attribute and type as second :: 03-07-2007
                                {
                                    foreach (System.Xml.XmlNode bookXML in pNode.ChildNodes)
                                    {
                                        TreeNode bookTree = new TreeNode("Book");
                                        tNode.ChildNodes.Add(bookTree);
                                        //bookTree.EnsureVisible();
                                        bookTree.Text = bookXML.Name;
                                        strXpath = bookXML.OuterXml.Substring(0, bookXML.OuterXml.IndexOf('>') + 1);
                                        bookTree.Value = strXpath;
                                        bookTree.Text = "<div oncontextmenu=\"ContextShow(event);return false\">" + bookTree.Text + "<span style=\" color:White;\">@@" + strXpath + "</span></div>";
                                        bookTree.NavigateUrl = "javascript:void(0)";
                                        #region Headings in Book
                                        // TODO: populate Headings in Part
                                        #endregion
                                        #region Parts in Book
                                        //**Aatif**Update:Go in depth of part for chapter and heading section
                                        foreach (System.Xml.XmlNode prtXML in bookXML.ChildNodes)
                                        {
                                            if (prtXML.Name.Equals("section"))
                                            {
                                                #region part->Chapter->heading
                                                if (prtXML.Attributes["type"].Value.Equals("part"))
                                                {
                                                    //string prtTitle = prtXML.SelectSingleNode("head/section-title").InnerText; 
                                                    TreeNode prtTree = new TreeNode("Part");
                                                    bookTree.ChildNodes.Add(prtTree);
                                                    //prtTree.EnsureVisible();
                                                    prtTree.Text = prtXML.Name;
                                                    strXpath = prtXML.OuterXml.Substring(0, prtXML.OuterXml.IndexOf('>') + 1);
                                                    prtTree.Value = strXpath;
                                                    prtTree.Text = "<div oncontextmenu=\"ContextShow(event);return false\">" + prtTree.Text + "<span style=\" color:White;\">@@" + strXpath + "</span></div>";
                                                    prtTree.NavigateUrl = "javascript:void(0)";
                                                    #region Chapters in Parts
                                                    foreach (System.Xml.XmlNode chpXML in prtXML.ChildNodes)
                                                    {
                                                        if (chpXML.Name.Equals("section"))
                                                        {
                                                            if (chpXML.Attributes["type"].Value.Equals("chapter"))
                                                            {
                                                                string chpTitle = chpXML.SelectSingleNode("head/section-title").InnerText;
                                                                TreeNode chpTree = new TreeNode(chpTitle);
                                                                prtTree.ChildNodes.Add(chpTree);
                                                                //chpTree.EnsureVisible();
                                                                chpTree.Text = chpXML.Name;
                                                                #region Headings in Chapters in Parts
                                                                // Add Heading Children to the Book Tree - START
                                                                TreeNode HeadingTree = generateHeadings(chpXML, 0);
                                                                if (HeadingTree != null)
                                                                {
                                                                    if (HeadingTree.Text != "null")
                                                                        chpTree.ChildNodes.Add(HeadingTree);
                                                                    else
                                                                        foreach (TreeNode treeNode in HeadingTree.ChildNodes)
                                                                            chpTree.ChildNodes.Add(treeNode);
                                                                }
                                                                chpTree.ExpandAll();
                                                                // Add Heading Children to the Book Tree - END
                                                                #endregion

                                                                //if (tNode.ChildNodes[0].Index == prtTree.Index)
                                                                //{
                                                                //    if (prtTree.ChildNodes[0].Index == chpTree.Index)
                                                                //    {
                                                                //       // _pbpbook.SelectedNode = chpTree;
                                                                //        //_paras.SelectedNode = null;
                                                                //    }
                                                                //}
                                                            }
                                                            else if (chpXML.Attributes["type"].Value.StartsWith("level"))
                                                            {
                                                                #region Headings in Body
                                                                // Add Heading Children to the Book Tree - START
                                                                TreeNode HeadingTree = generateHeadings(chpXML, 0);
                                                                if (HeadingTree != null)
                                                                {
                                                                    if (HeadingTree.Text != "null")
                                                                        prtTree.ChildNodes.Add(HeadingTree);
                                                                    else
                                                                        foreach (TreeNode treeNode in HeadingTree.ChildNodes)
                                                                            prtTree.ChildNodes.Add(treeNode);
                                                                }
                                                                prtTree.ExpandAll();
                                                                // Add Heading Children to the Book Tree - END
                                                                #endregion
                                                                //  _pbpbook.SelectedNode = prtTree.ChildNodes[0];
                                                                //_paras.SelectedNode = null;
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                }
                                                #endregion

                                            }
                                        }
                                        #endregion
                                        //**Aatif**::update::If chapter is direct decendent of Book Node then Traverse it::Date: 29 August 2007
                                        #region Chapters in Book
                                        foreach (System.Xml.XmlNode chpXML in bookXML.ChildNodes)
                                        {
                                            if (chpXML.Name.Equals("section"))
                                            {
                                                if (chpXML.Attributes["type"].Value.Equals("chapter"))
                                                {
                                                    string chpTitle = chpXML.SelectSingleNode("head/section-title").InnerText;
                                                    TreeNode chpTree = new TreeNode(chpTitle);
                                                    bookTree.ChildNodes.Add(chpTree);
                                                    //chpTree.EnsureVisible();
                                                    chpTree.Text = chpXML.Name;
                                                    #region Headings in Chapters in Books
                                                    // Add Heading Children to the Book Tree - START
                                                    TreeNode HeadingTree = generateHeadings(chpXML, 0);
                                                    if (HeadingTree != null)
                                                    {
                                                        if (HeadingTree.Text != "null")
                                                            chpTree.ChildNodes.Add(HeadingTree);
                                                        else
                                                            foreach (TreeNode treeNode in HeadingTree.ChildNodes)
                                                                chpTree.ChildNodes.Add(treeNode);
                                                    }
                                                    chpTree.ExpandAll();
                                                    // Add Heading Children to the Book Tree - END
                                                    #endregion

                                                    //if (tNode.ChildNodes[0].Index == bookTree.Index)
                                                    //{
                                                    //    if (bookTree.ChildNodes[0].Index == chpTree.Index)
                                                    //    {
                                                    //       // _pbpbook.SelectedNode = chpTree;
                                                    //        //_paras.SelectedNode = null;
                                                    //    }
                                                    //}
                                                }
                                                else if (chpXML.Attributes["type"].Value.StartsWith("level"))
                                                {
                                                    #region Headings in Body
                                                    // Add Heading Children to the Book Tree - START
                                                    TreeNode HeadingTree = generateHeadings(chpXML, 0);
                                                    if (HeadingTree != null)
                                                    {
                                                        if (HeadingTree.Text != "null")
                                                            bookTree.ChildNodes.Add(HeadingTree);
                                                        else
                                                            foreach (TreeNode treeNode in HeadingTree.ChildNodes)
                                                                bookTree.ChildNodes.Add(treeNode);
                                                    }
                                                    bookTree.ExpandAll();
                                                    // Add Heading Children to the Book Tree - END
                                                    #endregion
                                                    //_pbpbook.SelectedNode = bookTree.FirstNode;
                                                    //_paras.SelectedNode = null;
                                                }
                                            }
                                        }

                                        #endregion
                                        //~**Aatif::Date: 29 August 2007
                                    }
                                }
                                //~**Aatif::Update Ends here Date: 29 August 2007
                                else if (bNode.Attributes[1].Value.Equals("part"))  //Asad:Update for ID as first attribute and type as second :: 03-07-2007
                                {
                                    foreach (System.Xml.XmlNode prtXML in pNode.ChildNodes)
                                    {
                                        TreeNode prtTree = new TreeNode("Part");
                                        tNode.ChildNodes.Add(prtTree);
                                        //prtTree.EnsureVisible();
                                        prtTree.Text = prtXML.Name;
                                        #region Headings in Part
                                        // TODO: populate Headings in Part
                                        #endregion
                                        #region Chapters in Parts
                                        foreach (System.Xml.XmlNode chpXML in prtXML.ChildNodes)
                                        {
                                            if (chpXML.Name.Equals("section"))
                                            {
                                                if (chpXML.Attributes["type"].Value.Equals("chapter"))
                                                {
                                                    string chpTitle = chpXML.SelectSingleNode("head/section-title").InnerText;
                                                    TreeNode chpTree = new TreeNode(chpTitle);
                                                    prtTree.ChildNodes.Add(chpTree);
                                                    //chpTree.EnsureVisible();
                                                    chpTree.Text = chpXML.Name;
                                                    #region Headings in Chapters in Parts
                                                    // Add Heading Children to the Book Tree - START
                                                    TreeNode HeadingTree = generateHeadings(chpXML, 0);
                                                    if (HeadingTree != null)
                                                    {
                                                        if (HeadingTree.Text != "null")
                                                            chpTree.ChildNodes.Add(HeadingTree);
                                                        else
                                                            foreach (TreeNode treeNode in HeadingTree.ChildNodes)
                                                                chpTree.ChildNodes.Add(treeNode);
                                                    }
                                                    chpTree.ExpandAll();
                                                    // Add Heading Children to the Book Tree - END
                                                    #endregion

                                                    //if (tNode.FirstNode.Index == prtTree.Index)
                                                    //{
                                                    //    if (prtTree.FirstNode.Index == chpTree.Index)
                                                    //    {
                                                    //        _pbpbook.SelectedNode = chpTree;
                                                    //        _paras.SelectedNode = null;
                                                    //    }
                                                    //}
                                                }
                                                else if (chpXML.Attributes["type"].Value.StartsWith("level"))
                                                {
                                                    #region Headings in Body
                                                    // Add Heading Children to the Book Tree - START
                                                    TreeNode HeadingTree = generateHeadings(chpXML, 0);
                                                    if (HeadingTree != null)
                                                    {
                                                        if (HeadingTree.Text != "null")
                                                            prtTree.ChildNodes.Add(HeadingTree);
                                                        else
                                                            foreach (TreeNode treeNode in HeadingTree.ChildNodes)
                                                                prtTree.ChildNodes.Add(treeNode);
                                                    }
                                                    prtTree.ExpandAll();
                                                    // Add Heading Children to the Book Tree - END
                                                    #endregion
                                                    //_pbpbook.SelectedNode = prtTree.FirstNode;
                                                    //_paras.SelectedNode = null;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                else if (bNode.Attributes[1].Value.Equals("chapter"))  //Asad:Update for ID as first attribute and type as second :: 03-07-2007
                                {
                                    foreach (System.Xml.XmlNode chpXML in pNode.ChildNodes)
                                    {
                                        if (chpXML.Name.Equals("section"))
                                        {
                                            string chpTitle = chpXML.SelectSingleNode("head/section-title").InnerText;
                                            TreeNode chpTree = new TreeNode(chpTitle);
                                            tNode.ChildNodes.Add(chpTree);
                                            //chpTree.EnsureVisible();                                            
                                            strXpath = chpXML.OuterXml.Substring(0, chpXML.OuterXml.IndexOf('>') + 1);
                                            chpTree.Value = strXpath;
                                            chpTree.Text = "<div oncontextmenu=\"ContextShow(event);return false\">" + chpTree.Text + "<span style=\" color:White;\">@@" + strXpath + "</span></div>";
                                            chpTree.NavigateUrl = "javascript:void(0)";
                                            #region Headings in Chapters
                                            // Add Heading Children to the Book Tree - START
                                            TreeNode HeadingTree = generateHeadings(chpXML, 0);
                                            if (HeadingTree != null)
                                            {
                                                if (HeadingTree.Text != "null")
                                                    chpTree.ChildNodes.Add(HeadingTree);
                                                else
                                                {
                                                    while (HeadingTree.ChildNodes.Count > 0)
                                                    {
                                                        chpTree.ChildNodes.Add(HeadingTree.ChildNodes[0]);
                                                    }

                                                    //foreach (TreeNode treeNode in HeadingTree.ChildNodes)
                                                    //{
                                                    //    chpTree.ChildNodes.Add(treeNode);

                                                    //}
                                                }
                                            }
                                            chpTree.ExpandAll();
                                            // Add Heading Children to the Book Tree - END
                                            #endregion
                                        }
                                    }
                                    //_pbpbook.SelectedNode = tNode.FirstNode;
                                    //_paras.SelectedNode = null;
                                }
                                else if (bNode.Attributes[0].Value.StartsWith("level"))
                                {
                                    foreach (System.Xml.XmlNode hdnXML in pNode.ChildNodes)
                                    {
                                        if (hdnXML.Name.Equals("section"))
                                        {
                                            #region Headings in Body
                                            // Add Heading Children to the Book Tree - START
                                            TreeNode HeadingTree = generateHeadings(hdnXML, 0);
                                            if (HeadingTree != null)
                                            {
                                                if (HeadingTree.Text != "null")
                                                    tNode.ChildNodes.Add(HeadingTree);
                                                else
                                                    foreach (TreeNode treeNode in HeadingTree.ChildNodes)
                                                        tNode.ChildNodes.Add(treeNode);
                                            }
                                            tNode.ExpandAll();
                                            // Add Heading Children to the Book Tree - END
                                            #endregion
                                        }
                                    }
                                    //_pbpbook.SelectedNode = tNode.FirstNode;
                                    //_paras.SelectedNode = null;
                                }
                            }
                        }
                    }
                    else if (tNode.Text.Equals("End"))
                    {
                        pNode = pbpRoot.SelectSingleNode("pbp-end");
                        tNode.Text = pNode.Name;
                        strXpath = pbpRoot.OuterXml.Substring(0, pbpRoot.OuterXml.IndexOf('>') + 1);
                        tNode.Value = strXpath;
                        tNode.Text = "<div oncontextmenu=\"ContextShow(event);return false\">" + tNode.Text + "<span style=\" color:White;\">@@" + strXpath + "</span></div>";
                        tNode.NavigateUrl = "javascript:void(0)";
                        TreeNode postTree = new TreeNode("Post-Section");
                        strXpath = "<pbp-end>";
                        postTree.Value = strXpath;
                        postTree.Text = "<div oncontextmenu=\"ContextShow(event);return false\">Post-Section<span style=\" color:White;\">@@" + strXpath + "</span></div>";
                        postTree.NavigateUrl = "javascript:void(0)";
                        tNode.ChildNodes.Add(postTree);
                        XmlNodeList ps = pNode.SelectNodes("post-section");
                        System.Xml.XmlNode fpn;
                        for (int i = 0; i <= ps.Count - 1; i++)
                        {
                            fpn = ps[i];
                            if (fpn != null)
                            {
                                if (fpn.HasChildNodes)
                                {
                                    TreeNode pTree;
                                    if (fpn.Attributes.GetNamedItem("type").Value.Equals("other"))
                                    {
                                        pTree = new TreeNode("Other");
                                        strXpath = fpn.OuterXml.Substring(0, fpn.OuterXml.IndexOf('>') + 1);
                                        postTree.Value = strXpath;
                                        postTree.Text = "<div oncontextmenu=\"ContextShow(event);return false\">other<span style=\" color:White;\">@@" + strXpath + "</span></div>";
                                        postTree.NavigateUrl = "javascript:void(0)";
                                    }
                                    else
                                    {
                                        pTree = new TreeNode(fpn.SelectSingleNode("head/section-title").InnerText);
                                    }
                                    //**Aatif:Update for Giving heading facility in Pre/Post section::Date 05 September 2007
                                    TreeNode HeadingTree = generateHeadings(fpn, 0);

                                    if (HeadingTree != null)
                                    {
                                        if (HeadingTree.Text != "null")
                                            pTree.ChildNodes.Add(HeadingTree);
                                        else
                                            while (HeadingTree.ChildNodes.Count > 0)
                                            {
                                                pTree.ChildNodes.Add(HeadingTree.ChildNodes[0]);
                                            }
                                    }
                                    //**Aatif:Update for Giving heading facility in Pre/Post section::Date 05 September 2007
                                    //pTree.Text = fpn.Name;
                                    postTree.ChildNodes.Add(pTree);
                                    //pTree.EnsureVisible();
                                    pTree.ExpandAll();
                                }
                            }
                        }
                    }
                }
            j:
                ValidationFlag = true;

                //this.preProcessBook();
            }
            catch { }
            //catch (XmlException e) 
            //{ 
            //    ValidationFlag = false; 
            //    MessageBox.Show(e.StackTrace, "Open XML Exception (func:LoadPBPDocument)", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            //} 
            //catch (Exception e) 
            //{ 
            //    ValidationFlag = false; 
            //    MessageBox.Show(e.Message, "Open Exception (func:LoadPBPDocument)", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            //} 
            return;
        }

        private TreeNode generateHeadings(XmlNode targetParent, int TreeLevel)
        {
            TreeNode thisNode = null;
            string tabs = "";
            for (int tbIndx = 0; tbIndx < TreeLevel; tbIndx++) tabs += "   ";
            string NodeText = "";

            if (targetParent.Attributes["type"].Value.StartsWith("level"))
            {
                NodeText = targetParent.Attributes["type"].Value;
                thisNode = new TreeNode(NodeText);
                thisNode.Text = NodeText;
                string strXpath = targetParent.OuterXml.Substring(0, targetParent.OuterXml.IndexOf('>') + 1);
                thisNode.Value = strXpath;
                thisNode.Text = "<div oncontextmenu=\"ContextShow(event);return false\">" + thisNode.Text + "<span style=\" color:White;\">@@" + strXpath + "</span></div>";
                thisNode.NavigateUrl = "javascript:void(0)";
                //System.Diagnostics.Debug.WriteLine(tabs + thisNode.Text);
            }
            else
            {
                NodeText = "null";
                thisNode = new TreeNode(NodeText);
                //System.Diagnostics.Debug.WriteLine(tabs + thisNode.Text);
            }

            foreach (XmlNode childNode in targetParent.SelectNodes("section"))
                if (childNode.Attributes["type"].Value.StartsWith("level"))
                    thisNode.ChildNodes.Add(generateHeadings(childNode, (TreeLevel + 1)));

            return thisNode;
        }

        #region |Web Methods|


        public string AddLevel1(string CurrentPage, string SelectedLineHidden, string sectionType, int SectionLevel)
        {
            ComparisonService.ComparisonTasksClient svc = new ComparisonService.ComparisonTasksClient();

            //svc.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential("win 7", "pakistan", "58.65.163.243:");
            //svc.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            try
            {
                svc.Open();
                string result = "";
                if (HttpContext.Current.Session["XMLPath"] != null)
                {
                    string xpath = "section";
                    string tempText = "<?xml version=\"1.0\"?><Book>" + txtSelectedLineHidden.Text + "</Book>";
                    XmlDocument tempDoc = new XmlDocument();
                    tempDoc.LoadXml(tempText);
                    XmlNode tempNode = tempDoc.SelectSingleNode("//section");
                    for (int i = 0; i < tempNode.Attributes.Count; i++)
                    {
                        if (i == 0)
                        {
                            xpath = xpath + "[@" + tempNode.Attributes[i].Name + "='" + tempNode.Attributes[i].Value + "'";
                        }
                        else
                        {
                            xpath = xpath + " and @" + tempNode.Attributes[i].Name + "='" + tempNode.Attributes[i].Value + "'";
                        }
                    }
                    xpath = xpath + "]";
                    result = svc.AddLevels(sectionType, SectionLevel, CurrentPage, xpath, HttpContext.Current.Session["XMLPath"].ToString());
                }
                return result;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (svc.State.Equals(System.ServiceModel.CommunicationState.Opened))
                {
                    svc.Close();
                }
            }
        }

        #endregion

        #endregion

        protected void lnkLevel2_Click(object sender, EventArgs e)
        {
            AddLevel1("6", "", "level2", 3);
        }

        protected void lnkLevel1_Click(object sender, EventArgs e)
        {
            AddLevel1("6", "", "level1", 4);
        }

        protected void lnkLevel3_Click(object sender, EventArgs e)
        {
            AddLevel1("6", "", "level3", 2);
        }

        protected void lnkLevel4_Click(object sender, EventArgs e)
        {
            AddLevel1("6", "", "level4", 1);
        }

        protected void lnkChapter_Click(object sender, EventArgs e)
        {

        }
    }
}
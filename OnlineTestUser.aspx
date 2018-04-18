<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master" AutoEventWireup="true"
    CodeBehind="OnlineTestUser.aspx.cs" Inherits="Outsourcing_System.OnlineTestUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Src="UserControls/ucOnlineTestTasks.ascx" TagName="ucOnlineTestTasks"
    TagPrefix="uc1" %>
<%@ Register Src="CustomControls/ProcessControl.ascx" TagName="ProcessControl" TagPrefix="uc1" %>
<%@ Register Src="UserControls/CommonControl/ucShowMessage.ascx" TagName="ucShowMessage" TagPrefix="uc2" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">

    <style type="text/css">
        .TestClearedFormatting {
            background-color: #FFE4B5;
            border: 1px solid #FC8B01;
            border-radius: 5px;
            float: left;
            /*margin-right: 20px;*/
            padding: 3px;
            text-align: left;
        }
    </style>
    <style type="text/css">
        b, a {
            color: #3b5998;
            text-decoration: none;
        }

            a:hover {
                text-decoration: underline;
            }

        h2 {
            color: #3B5998;
            font-size: 18px;
            line-height: 24px;
            font-weight: bold;
        }

        .dropdown {
            display: inline;
            margin-right: 3px;
            margin-left: 3px;
            position: relative;
        }

            .dropdown .dropdown_button {
                cursor: pointer;
                width: auto;
                display: inline-block;
                padding-left: 2px;
                padding-top: 4px;
                padding-bottom: 4px;
                padding-right: 2px;
                /*border: 1px solid #AAA;*/
                -webkit-border-radius: 2px;
                -moz-border-radius: 2px;
                border-radius: 2px;
                font-weight: bold;
                /*color: #717780;*/
                line-height: 16px;
                text-decoration: none !important;
                /*background: white url("http://ttb.li/dump/buttons/dropdown_arrow.png") no-repeat 100% 0px;*/
            }

            .dropdown.open .dropdown_button {
                border: 1px solid #3B5998;
                color: white;
                background: #6D84B4 url("http://ttb.li/dump/buttons/dropdown_arrow.png") no-repeat 100% -26px;
                -moz-border-radius-topleft: 2px;
                -moz-border-radius-topright: 2px;
                -moz-border-radius-bottomright: 0px;
                -moz-border-radius-bottomleft: 0px;
                -webkit-border-radius: 2px 2px 0px 0px;
                border-radius: 2px 2px 0px 0px;
                border-bottom-color: #6D84B4;
            }

            .dropdown .dropdown_button img {
                height: 14px;
                margin-top: 1px;
                margin-bottom: 1px;
                float: left;
                margin-right: 5px;
            }

            .dropdown .dropdown_content {
                display: none;
                position: absolute;
                top: 100%;
                left: 0;
                margin: 5px 0;
                border: 1px solid #777;
                padding: 0px;
                background: #e9e9e9;
                width: 121px;
                height: 82px;
            }

            .dropdown.open .dropdown_content {
                display: block;
            }

            .dropdown .dropdown_content li {
                list-style: none;
                margin-left: 0px;
                line-height: 22px;
                border-top: 1px solid #FFF;
                border-bottom: 1px solid #FFF;
                margin-top: 2px;
                margin-bottom: 2px;
            }

                .dropdown .dropdown_content li:hover {
                    border-top-color: #3B5998;
                    border-bottom-color: #3B5998;
                    background: #6D84B4;
                }

                .dropdown .dropdown_content li a {
                    display: block;
                    padding: 7px 7px;
                    padding-right: 15px;
                    color: black;
                    text-decoration: none !important;
                }

                .dropdown .dropdown_content li:hover a {
                    color: white;
                    text-decoration: none !important;
                }

                .dropdown .dropdown_content li img {
                    height: 14px;
                    margin-top: 1px;
                    margin-bottom: 1px;
                    float: left;
                    margin-right: 5px;
                    border: none;
                }
    </style>

    <style type="text/css">
        .Star {
            background-image: url(img/Star.gif);
            height: 17px;
            width: 17px;
        }

        .WaitingStar {
            background-image: url(img/WaitingStar.gif);
            height: 17px;
            width: 17px;
        }

        .FilledStar {
            background-image: url(img/FilledStar.gif);
            height: 17px;
            width: 17px;
        }
    </style>
    <script type="text/javascript">



        $(function () {
            $('.dropdown .dropdown_button').click(function (e) {
                var parent = $(this).parent().toggleClass('open');
                $('.dropdown.open').not(parent).removeClass('open');
                e.stopPropagation();
            });
            $(document).click(function (e) {
                $('.dropdown.open').removeClass('open');
            });
        });

        function pageLoad() {
            $(function () {
                var gv = $('#<%=gvArchiveTasks.ClientID%>');
                var thead = $('<thead/>');
                thead.append(gv.find('tr:eq(0)'));
                gv.append(thead);
                gv.dataTable({
                    "sPaginationType": "full_numbers",
                    "iDisplayLength": 10,
                    "aaSorting": [],
                    "bDestroy": true,
                    "bRetrieve": true,
                    'bProcessing': true,
                    'bServerSide': false
                });
            });
        }



        $(function () {
            var gv = $('#<%=gvTaskManager.ClientID%>');
            var thead = $('<thead/>');
            thead.append(gv.find('tr:eq(0)'));
            gv.append(thead);
            gv.dataTable({
                "sPaginationType": "full_numbers",
                "iDisplayLength": 10,
                "aaSorting": [],
                "bDestroy": true,
                "bRetrieve": true,
                'bProcessing': true,
                'bServerSide': false
            });
        });


        $(function () {
            var gv = $('#<%=gvMyTasks.ClientID%>');
            var thead = $('<thead/>');
            thead.append(gv.find('tr:eq(0)'));
            gv.append(thead);
            gv.dataTable({
                "sPaginationType": "full_numbers",
                "iDisplayLength": 10,
                "aaSorting": [],
                "bDestroy": true,
                "bRetrieve": true,
                'bProcessing': true,
                'bServerSide': false
            });
        });



    </script>


</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
    <asp:Panel ID="pnlUserTasks" runat="server">
        <div id="mid" align="center">

            <div id="divStatusMessages" style="width: 42.5%; margin: 0 auto;" runat="server">
                <uc2:ucShowMessage ID="ucShowMessage1" runat="server" />
            </div>

            <div id="profile">
                <div id="notice">
                    <div id="noticeL">
                    </div>
                    <div class="dropdown">
                        <a class="dropdown_button">
                            <input type="image" id="imgbtnEditProfile" src="img/icon_edit.png" onclick="return false;" /></a>
                        <ul class="dropdown_content">
                            <li>
                                <asp:LinkButton ID="lbtnEditProfile" OnClick="lbtnEditProfile_Click" runat="server"> Edit Profile</asp:LinkButton></li>
                            <li>
                                <asp:LinkButton ID="lbtnChangePassword" OnClick="lbtnChangePassword_Click" runat="server">Change Password</asp:LinkButton></li>
                        </ul>
                    </div>
                    Edit your profile

                    <br />
                    <div id="noticeR" style="margin-top: -2%;">
                        Share on
                                <img src="img/icon_fb.png" />&nbsp;<img src="img/icon_vimeo.png" />&nbsp;<img src="img/icon_youtube.png" />
                    </div>
                </div>

                <div>
                    &nbsp;
                </div>

                <div id="profileL">
                    <div style="margin: left:20px; margin-top: -10px; float: right;">
                        <asp:Button ID="btnTestTraining" Style="border: 1px solid #2A4F96; background-color: #2A4F96; color: #FFFFFF; width: 126px; height: 35px;"
                            runat="server" Text="Tests and Trainings"
                            OnClick="btnTestTraining_Click" />
                        <asp:Button ID="btnAccountDetails" Style="border: 1px solid #2A4F96; background-color: #2A4F96; color: #FFFFFF; width: 110px; height: 35px;"
                            runat="server" Text="Account Details"
                            OnClick="btnAccountDetails_Click" />
                    </div>
                    <div id="divNormalProfile" runat="server">
                        <div id="profileHD" style="margin-left: 20%;">
                            <div id="info">
                                <div id="img">
                                    <asp:Image ID="imgUserProfile" Width="100" Height="100" align="left" runat="server" />
                                </div>
                                <span style="display: inline-block; position: absolute; max-width: 100%; height: 100px;">
                                    <h1><strong>
                                        <asp:Label ID="lblProfileName" runat="server"></asp:Label></strong></h1>
                                </span>

                                <div style="margin-top: 0px; clear: both;">
                                    <div id="ql" style="width: 100%; margin-left: 95%;">
                                        <div id="divImagesTest" class="TestClearedFormatting" runat="server" visible="false">
                                            <asp:Label ID="lblImages" runat="server"> </asp:Label>
                                        </div>
                                        <div id="divTablesTest" class="TestClearedFormatting" runat="server" visible="false">
                                            <asp:Label ID="lblTables" runat="server"></asp:Label>
                                        </div>
                                        <div id="divIndexTest" class="TestClearedFormatting" runat="server" visible="false">
                                            <asp:Label ID="lblIndex" runat="server"></asp:Label>
                                        </div>
                                        <div id="divMappingTest" class="TestClearedFormatting" runat="server" visible="false">
                                            <asp:Label ID="lblMapping" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <p>
                            <br />
                            <div style="margin-left: -15%; width: 600px;">
                                <asp:Label ID="lblDescription" runat="server" Style="font-size: 13px; font-family: Open Sans; color: #333333"> </asp:Label>
                            </div>
                            <br />
                    </div>

                    <div id="divMyTasks" style="margin-top: 1px; margin-bottom: 35px">
                        My Tasks
                    </div>
                    <asp:GridView ID="gvMyTasks" runat="server" Style="margin-left: 1px; background-color: #E9E9E9; margin-top: 15px;"
                        AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" EmptyDataText='No Task available in My Tasks.'
                        DataKeyNames="AID,BID,TaskName, TaskStatus, BookId, Complexity, PageCount, OnePageTime_InSeconds"
                        Width="95%" OnRowDataBound="gvMyTasks_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Sr. No" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSrNo" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Task Name" HeaderStyle-Width="9%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestName" Text='<%# Eval("BookId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblType" Text='<%# Eval("Description") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Type" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblIssueCategory" Text='<%# Eval("TaskName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Task Amount" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblAmount" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bonus" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblBonus" />
                                    <%-- <table>
                                        <tr>
                                             <td><asp:Label runat="server" ID="lblAmount" /><td>
                                           <td> <asp:Label runat="server" ID="lblBonus" /></td>
                                        </tr>
                                    </table>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Task Time" HeaderStyle-Width="9%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTaskTime" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="14%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("TaskStatus") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <div style="min-width: 60px; text-align: center; padding: 2px;">
                                        <asp:ImageButton ID="ImgSubmitTask" runat="server" OnClick="lbtnSubmitTask_Click"
                                            ImageUrl="~/img/btn_submitTask.png" Height="30" Width="85" />
                                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="lnkDownloadgvMyTasks_Click" Visible="false"></asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gridHeader" />
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                    </asp:GridView>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <asp:Label runat="server" ID="lblAsterikMyTasks" />

                    <div>
                        &nbsp;
                    </div>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />

                    <div id="hd" style="margin-bottom: 20px; margin-top: 3px">
                        OPEN TASKS
                    </div>
                    <div>
                        &nbsp;
                    </div>
                    <asp:GridView ID="gvTaskManager" runat="server" Style="margin-left: 1px; background-color: #E9E9E9"
                        AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" EmptyDataText='No Task available in Open Tasks.'
                        DataKeyNames="BID, TaskStatus, TaskName, Complexity, PageCount, OnePageTime_InSeconds, aid, Priority"
                        Width="95%" OnRowDataBound="gvTaskManager_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Sr. No" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSrNo" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Task Name" HeaderStyle-Width="9%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestName" Text='<%# Eval("BookId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblType" Text='<%# Eval("Description") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Type" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblIssueCategory" Text='<%# Eval("TaskName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Task Amount" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblAmount" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bonus" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblBonus" />
                                    <%-- <table>
                                        <tr>
                                             <td><asp:Label runat="server" ID="lblAmount" /><td>
                                           <td> <asp:Label runat="server" ID="lblBonus" /></td>
                                        </tr>
                                    </table>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Task Time" HeaderStyle-Width="9%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTaskTime" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="14%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("TaskStatus") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <div style="min-width: 60px; text-align: center; padding: 2px;">
                                        <asp:ImageButton ID="ImgSubmitTask" runat="server" OnClick="imgAssignMe_Click" ImageUrl="~/img/btn_assign.png"
                                            Height="30" Width="85" />
                                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="lnkDownloadgvTaskManager_Click" Visible="false"></asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gridHeader" />
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                    </asp:GridView>
                    <br />
                    <br />
                    <br />
                    <br />
                    <asp:Label runat="server" ID="lblAsterikOpenTasks" />

                    <%--* Approximate bonus amount for time and quality--%>

                    <div>
                        &nbsp;
                    </div>
                    <br />
                    <div style="margin-top: 3.5%" align="right">
                        <asp:LinkButton ID="lbtnArchiveTasks" runat="server" Style="color: #0066cc; font-size: 18px; font-weight: bolder"
                            OnClick="lbtnArchiveTasks_Click">Archive Tasks</asp:LinkButton>
                    </div>
                    <div id="hd" style="margin-top: 4%">
                        <asp:Label ID="lblTestTaken" runat="server"></asp:Label>
                        <%--TESTS (2)--%>
                    </div>
                    <div>
                        &nbsp;
                    </div>
                </div>
                <div id="profileR">
                    <div id="star" style="display: block; margin: 0px auto; text-align: center;">
                        <asp:Label ID="lblRank" runat="server"></asp:Label>
                    </div>
                    <h3 style="font-family: Open Sans; font-size: 22px; color: #0066cc; text-align: center; padding-left: 0px !important; margin-top: 5%;">
                        <asp:Label ID="lblJobRank" runat="server"></asp:Label> 
                    </h3>

                    <h3 style="font-family: Open Sans; font-size: 13px; color: #0066cc; text-align: center; padding-left: 0px !important; margin-top: -4%;">
                        <asp:Label ID="lblNextRank" runat="server"></asp:Label> 
                    </h3>
                    <h3>
                        <asp:Label ID="lblTasksCompleted" runat="server"></asp:Label>
                        <%-- Tasks Completed: 1--%>
                    </h3>
                    <h3>
                        <asp:Label ID="lblTasksInProgress" runat="server"></asp:Label>
                        <%--Tasks In Progress: 2--%>
                    </h3>
                    <h3>
                        <img src="img/icon_stars.png" /></h3>
                    
                    <div style="width:100%; color:#0066cc; font-weight:bold;" runat="server" id="divUpgradeLevel">
                         <div style="text-align: center;">
                            <span style="color:black;">Need More Books?</span> <asp:LinkButton ID="lbtnUpgradeLevel" OnClick="lbtnUpgradeLevel_Click" 
                                runat="server" style ="text-decoration: underline; color:#0066cc !important;">Upgrade Level</asp:LinkButton>
                        </div>
                    </div>
                    
                    <%-- <div style="width:100%; color:#0066cc; font-weight:bold;" runat="server" id="divBooksLegend">
                         <div style="text-align: center;">
                             <div style="display:inline-block;width: 20%; height: 20px; background-color:turquoise !important;">
                                    </div>
                             <span style="color:black;">Upgraded Books</span> 
                        </div>                      
                     </div>--%>
                    
                     <div style="width:61%; height:20px; color:#0066cc; font-weight:bold; margin-left:auto; margin-right:auto;" runat="server" id="divBooksLegend">
                             <div style="float:left;width: 42%; height: 20px; background-color:turquoise !important;">
                                    </div>
                             <span style="margin-left: 3%;color:black;">Upgraded Books</span> 
                     </div>


                    <hr />
                    <h4>Completed Tasks
                    </h4>
                    <asp:GridView ID="gvCompletedTasks" runat="server" Style="margin-left: 11px; background-color: #E9E9E9"
                        AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" EmptyDataText='No data available.'
                        GridLines="None" ShowHeader="false" ShowFooter="false" Width="95%" OnRowDataBound="gvCompletedTasks_RowDataBound"
                        DataKeyNames="BookId, TaskType, TimelyDelivery, Quality, Responsiveness, Comments"
                        CellSpacing="10">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <table cellspacing="8">
                                        <tr align="left">
                                            <td>
                                                <asp:Label runat="server" ID="lblFeedBackDate" Text='<%# Eval("FeedBackDate") %>' /></td>
                                        </tr>
                                        <tr align="left">
                                            <td>
                                                <asp:LinkButton runat="server" ID="lbtnTask" OnClick="lbtnTask_Click" /></td>
                                        </tr>
                                        <tr align="left">
                                            <td>
                                                <cc1:Rating ID="Rating1" runat="server"
                                                    StarCssClass="Star" WaitingStarCssClass="WaitingStar" EmptyStarCssClass="Star"
                                                    FilledStarCssClass="FilledStar" CurrentRating='<%# Eval("Rating") %>'>
                                                </cc1:Rating>
                                                <span style="margin-left: 4%; display: inline-block; margin-top: 2.5%;">4.0</span>
                                            </td>

                                        </tr>
                                        <tr align="left">
                                            <td>
                                                <asp:Label runat="server" ID="lblComments" /></td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gridHeader" />
                        <RowStyle HorizontalAlign="Left"></RowStyle>
                    </asp:GridView>

                </div>
            </div>
        </div>
        <div>
            &nbsp;
        </div>
        <table width="30%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr id="tbline1" runat="server" visible="false">
                <td bgcolor="#CCCCCC" colspan="2" height="1"></td>
            </tr>
            <tr id="tdTableTasks" runat="server" visible="false">
                <td height="30">Table Tasks
                </td>
                <td>
                    <div id="score">
                        <asp:Label ID="lblTableTasks" runat="server"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr id="tbline2" runat="server" visible="false">
                <td bgcolor="#CCCCCC" colspan="2" height="1"></td>
            </tr>
            <tr id="tdImageTasks" runat="server" visible="false">
                <td height="30">Image Tasks
                </td>
                <td>
                    <div id="score">
                        <asp:Label ID="lblImageTasks" runat="server"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr id="tbline3" runat="server" visible="false">
                <td bgcolor="#CCCCCC" colspan="2" height="1"></td>
            </tr>
            <tr id="tdIndexTasks" runat="server" visible="false">
                <td height="30">Index Tasks
                </td>
                <td>
                    <div id="score">
                        <asp:Label ID="lblIndexTasks" runat="server"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr id="tbline4" runat="server" visible="false">
                <td bgcolor="#CCCCCC" colspan="2" height="1"></td>
            </tr>
            <tr id="tdMappingTask" runat="server" visible="false">
                <td height="30">Mapping Tasks
                </td>
                <td>
                    <div id="score">
                        <asp:Label ID="lblMappingTasks" runat="server"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr id="tbline5" runat="server" visible="false">
                <td bgcolor="#CCCCCC" colspan="2" height="1"></td>
            </tr>
            <%-- <tr>
            <td height="30">
                Image Tasks
            </td>
            <td>
                <div id="score">
                    <asp:Label ID="Label2" runat="server"></asp:Label</div>
            </td>
        </tr>
        <tr>
            <td bgcolor="#CCCCCC" colspan="2" height="1">
            </td>
        </tr>--%>
        </table>
        <div id="AssignMe" runat="server" visible="false">
            <div style="position: fixed; left: 0; top: 0; background-color: Black; width: 100%; height: 100%; opacity: 0.5;">
            </div>
            <div style="position: absolute; margin: auto; top: 30%; left: 11%; min-height: 500px; background-color: White; border-radius: 25px; border: 5px solid rgb(66, 66, 146); z-index: 500; width: 75%; padding: 10px;">
                <div style="float: right;">
                    <asp:ImageButton ID="btnClose" runat="server" ImageUrl="~/img/icon_close.png" Width="25"
                        OnClick="btnClose_Click" Height="25" />
                </div>
                <table>
                    <tr>
                        <td class="normaltext" align="right">For Task :
                        </td>
                        <td class="normaltext" align="left">
                            <uc1:ProcessControl ID="ProcessControl1" runat="server" />
                        </td>
                    </tr>
                    <%-- <tr>
                <td align="right" valign="top" class="normaltext">
                    Dead Line :
                </td>
                <td align="left">
                    <input runat="server" class="normaltext" name="Calendar1" id="Calendar1" autocomplete="off"
                        onfocus="showDcsCalendar(this);" type="text" style="width: 220px" /><asp:RequiredFieldValidator
                            ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="Calendar1"></asp:RequiredFieldValidator>
                </td>
            </tr>--%>
                    <tr>
                        <td align="right" valign="top" class="normaltext">Comments :
                            
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtComments" runat="server" Columns="25" Rows="10" TextMode="MultiLine"
                                CssClass="normaltext"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="normaltext">Extra Attachment :
                        </td>
                        <td align="left">
                            <input type="file" id="File1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right"></td>
                        <td align="left">
                            <asp:Label runat="server" ID="lblStatus" Text="" CssClass="message" />
                            <br />
                            <asp:Label runat="server" ID="lblUserName" Text=""></asp:Label>
                            <asp:Label ID="lblRate" runat="server" ForeColor="#009900" />
                            <br />
                            <asp:Button ID="btnAssign" runat="server" Font-Bold="true" Font-Size="Larger" Width="120"
                                Height="30" Text="Assign" CssClass="button" OnClick="btnAssign_Click" />
                            <br />
                            <br />
                            <span runat="server" id="instruction"><font color='red'>*</font><sub> All Tasks except
                                        Image, TaggingUntagged and Meta show amount per unit</sub></span>

                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlArchive" runat="server" Visible="false">
        <div id="divArchiveTasks" style="margin-top: 1px; margin-bottom: 35px">
            Archive Tasks
        </div>
        <asp:GridView ID="gvArchiveTasks" runat="server" Style="margin-left: 2.5%; background-color: #E9E9E9; margin-top: 15px;"
            AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" EmptyDataText='No Task available in archive Tasks.'
            DataKeyNames="AID,BID,TaskName, TaskStatus, BookId, Complexity, PageCount, OnePageTime_InSeconds"
            Width="95%" OnRowDataBound="gvArchiveTasks_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Sr. No#" HeaderStyle-Width="6%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSrNo" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Book Id" HeaderStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTestName" Text='<%# Eval("BookId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description" HeaderStyle-Width="37%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblType" Text='<%# Eval("Description") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Type" HeaderStyle-Width="12%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblIssueCategory" Text='<%# Eval("TaskName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Task Amount" HeaderStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblAmount" Text='<%# Eval("PayableEarning") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Bonus" HeaderStyle-Width="8%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblBonus" Text='<%# Eval("Bonus") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Task Time" HeaderStyle-Width="8%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTaskTime" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" HeaderStyle-Width="9%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("TaskStatus") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="gridHeader" />
            <RowStyle HorizontalAlign="Center"></RowStyle>
        </asp:GridView>
    </asp:Panel>
    <%--</ContentTemplate>--%>
    <%--<Triggers>
        <asp:PostBackTrigger ControlID = "ProcessControl1" />
    </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>



<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AdminMaster_Hiring.Master"
    AutoEventWireup="true" CodeBehind="BookHierarchy.aspx.cs" Inherits="Outsourcing_System.BookHierarchy" %>

<%@ Register Assembly="PdfViewer" Namespace="PdfViewer" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LinkPortion" runat="server">
    <script type="text/javascript" language="javascript">
        function fnAddLevel1() {
            var currentpage = $("#txtCurrentPage").val();
            var SelectedLineHidden = $("#txtSelectedLineHidden").val();


            var Data = '{"CurrentPage": "' + currentpage + '","SelectedLineHidden": "' + SelectedLineHidden + '","sectionType": "level1","SectionLevel": "4"}';
            $.ajax({
                type: "POST",
                url: "BookHierarchy.aspx/AddSectionOperation",
                data: Data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                }

            });
            e.preventDefault();
        }

        $(function () {
            /**************************************************
            * Context-Menu with Sub-Menu
            **************************************************/
            $.contextMenu({
                selector: '.context-menu-sub',
                callback: function (key, options) {
                    var m = "clicked: " + key;
                    window.console && console.log(m) || alert(m);
                },
                items: {



                    "Edit": {
                        "name": "Edit",
                        "items": {
                            "Chapter": { "name": "Chapter" },
                            "Add Level": {
                                "name": "Add Level",
                                "items": {
                                    "Level 1": { "name": "Level 1" },
                                    "Level 2": { "name": "Level 2" },
                                    "Level 3": { "name": "Level 3" },
                                    "Level 4": { "name": "Level 4" }
                                }
                            }

                        }
                    },
                    "Remove": {
                        "name": "Remove"
                    }

                }
            });
        });
    </script>
    <script type="text/javascript">
        function getMousePosition(e) {
            e = e || window.event;
            var position = {
                'x': e.clientX,
                'y': e.clientY
            };

            return position;
        }


        function getScrollPosition() {
            var x = 0;
            var y = 0;

            if (typeof (window.pageYOffset) == 'number') {
                x = window.pageXOffset;
                y = window.pageYOffset;
            } else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
                x = document.documentElement.scrollLeft;
                y = document.documentElement.scrollTop;
            } else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
                x = document.body.scrollLeft;
                y = document.body.scrollTop;
            }

            var position = {
                'x': x,
                'y': y
            };

            return position;
        }

        function ContextShow(event) {

            event = event || window.event;
            var target = event.target;
            var value = target.innerHTML.replace(target.innerHTML.substring(0, target.innerHTML.indexOf("@@") + 2), "").replace("</span>", "");
            EnableDisableMenu(value);
            $('#ctl00_ContentPlaceHolder1_txtSelectedLineHidden').val(value);
            var m = getMousePosition(event);
            var s = getScrollPosition(event);
            var client_height = document.body.clientHeight;
            var display_context = document.getElementById('divMenu');

            if (m.x > (screen.height / 2)) {
                display_context.style.display = "block";
                var context_height = display_context.clientHeight;
                display_context.style.left = m.x + "px";
                display_context.style.top = m.y + s.y + "px";
            }
            else {
                display_context.style.display = "block";
                display_context.style.left = m.x + "px";
                display_context.style.top = m.y + s.y + "px";
            }

        }
        function EnableDisableMenu(value) {
            if (value.substring(0, 8) == "<section") {
                if (value.contains('type="chapter"')) {
                    $("#ctl00_ContentPlaceHolder1_lnkLevel1").css('display', 'block');
                    $("#ctl00_ContentPlaceHolder1_lnkLevel2").css('display', 'none');
                    $("#ctl00_ContentPlaceHolder1_lnkLevel3").css('display', 'none');
                    $("#ctl00_ContentPlaceHolder1_lnkLevel4").css('display', 'none');

                }
                else if (value.contains('type="level1"')) {
                    $("#ctl00_ContentPlaceHolder1_lnkLevel1").css('display', 'block');
                    $("#ctl00_ContentPlaceHolder1_lnkLevel2").css('display', 'block');
                    $("#ctl00_ContentPlaceHolder1_lnkLevel3").css('display', 'none');
                    $("#ctl00_ContentPlaceHolder1_lnkLevel4").css('display', 'none');
                }
                else if (value.contains('type="level2"')) {
                    $("#ctl00_ContentPlaceHolder1_lnkLevel1").css('display', 'none');
                    $("#ctl00_ContentPlaceHolder1_lnkLevel2").css('display', 'block');
                    $("#ctl00_ContentPlaceHolder1_lnkLevel3").css('display', 'block');
                    $("#ctl00_ContentPlaceHolder1_lnkLevel4").css('display', 'none');
                }
                else if (value.contains('type="level3"')) {
                    $("#ctl00_ContentPlaceHolder1_lnkLevel1").css('display', 'none');
                    $("#ctl00_ContentPlaceHolder1_lnkLevel2").css('display', 'none');
                    $("#ctl00_ContentPlaceHolder1_lnkLevel3").css('display', 'block');
                    $("#ctl00_ContentPlaceHolder1_lnkLevel4").css('display', 'block');

                }
                else if (value.contains('type="level4"')) {
                    $("#ctl00_ContentPlaceHolder1_lnkLevel1").css('display', 'none');
                    $("#ctl00_ContentPlaceHolder1_lnkLevel2").css('display', 'none');
                    $("#ctl00_ContentPlaceHolder1_lnkLevel3").css('display', 'none');
                    $("#ctl00_ContentPlaceHolder1_lnkLevel4").css('display', 'block');
                }

            }
        }
        $(document).ready(function () {
            $('body').keydown(function (e) {
                if (e.keyCode == 27) {
                    $('#divMenu').css("display", "none");
                }

            });
            var e = $.Event("keydown", {
                keyCode: 27
            });

            $('body').keypress(function () {
                $("body").trigger(e);
            });
            $('#divMenu').mouseleave(function () {
                if ($('#divMenu').css("display") == "block") {
                    $('#divMenu').css("display", "none");
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div oncontextmenu="ContextShow(event);return false">
        <strong>right click me</strong>
    </div>
    <table id="Maintble" width="100%">
        <tr>
            <td align="left" style="width: 70%;">
                <b>Source Pdf</b>
            </td>
            <td align="left" style="width: 30%;">
                <b>Book Hierarchy</b>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <div style="border: 1px solid gray; padding: 8px;">
                    <cc1:ShowPdf ID="PDFViewerSource" runat="server" BorderWidth="1px" BorderColor="Gray"
                        Height="600px" Width="100%" />
                </div>
            </td>
            <td valign="top" style="padding-left: 15px;">
                <div style="border: 1px solid gray; padding: 8px; min-height: 600px; max-height: 600px;
                    overflow: scroll;">
                    <asp:TreeView ID="_pbpbook" runat="server" ShowLines="true">
                        <Nodes>
                            <asp:TreeNode Text="abc">
                                <asp:TreeNode Text="abc"></asp:TreeNode>
                            </asp:TreeNode>
                            <asp:TreeNode Text="abc">
                                <asp:TreeNode Text="abc"></asp:TreeNode>
                            </asp:TreeNode>
                            <asp:TreeNode Text="abc">
                                <asp:TreeNode Text="abc"></asp:TreeNode>
                            </asp:TreeNode>
                            <asp:TreeNode Text="abc">
                                <asp:TreeNode Text="abc"></asp:TreeNode>
                            </asp:TreeNode>
                        </Nodes>
                    </asp:TreeView>
                </div>
            </td>
        </tr>
    </table>
    <asp:TextBox ID="txtSelectedLineHidden" Text="abcdd" runat="server"></asp:TextBox>
    <div id="divMenu" style="display: none; border: 1px solid #D6D6D6; position: absolute;
        left: 900px; top: 300px; padding: 2px; background-color: #F0F0F0; min-width:200px;">
        <ul class="navMenu" style="list-style-type: none;">
            <li class='last'><span>Edit</span>
                <ul class="navMenu" style="list-style-type: none;">
                    <li>
                        <asp:LinkButton ID="lnkChapter" runat="server" Text="Chapter"></asp:LinkButton></li>
                    <li><span>Add Level</span>
                        <ul class="navMenu" style="list-style-type: none;">
                            <li>
                                <asp:LinkButton ID="lnkLevel1" runat="server" Text="Level 1" OnClick="lnkLevel1_Click"></asp:LinkButton></li>
                            <li>
                                <asp:LinkButton ID="lnkLevel2" runat="server" Text="Level 2" OnClick="lnkLevel2_Click"></asp:LinkButton></li>
                            <li>
                                <asp:LinkButton ID="lnkLevel3" runat="server" Text="Level 3" OnClick="lnkLevel3_Click"></asp:LinkButton></li>
                            <li>
                                <asp:LinkButton ID="lnkLevel4" runat="server" Text="Level 4" OnClick="lnkLevel4_Click"></asp:LinkButton></li>
                        </ul>
                    </li>
                </ul>
            </li>
            <li id="mnuRemove">
                <asp:LinkButton ID="lnkRemove" runat="server" Text="Remove"></asp:LinkButton></li>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

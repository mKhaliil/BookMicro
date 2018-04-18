<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" Async="true" CodeBehind="ResetTask.aspx.cs" Inherits="Outsourcing_System.ResetTask" %>

<%@ Register src="UserControls/CommonControl/ucShowMessage.ascx" tagname="ucShowMessage" tagprefix="uc1" %>

<asp:Content ID="headContents" ContentPlaceHolderID="mainHeadContents" runat="server">
    <style type="text/css">
        .outerDivStyle {
            width: 80%;
            margin: 0 auto;
        }

        .mainDivStyle {
            width: 50%;
            margin: 0 auto;
        }

        .searchDivStyle {
            width: 100%;
            margin-top: 3%;
            margin-left: 0%;
        }
    </style>
</asp:Content>
<asp:Content ID="bodyContents" ContentPlaceHolderID="mainBodyContents" runat="server">
     <div id="divStatusMessages" style="width: 42.5%; margin: 0 auto;" runat="server">
                   <uc1:ucShowMessage ID="ucShowMessage1" runat="server" />
                </div>
    <div class="outerDivStyle">
        <div class="mainDivStyle">
            <span>Enter Book Id :</span>
            <asp:TextBox runat="server" ID="tbxSearchId" Width="166px" Style="display: inline-block;"></asp:TextBox>
            <asp:Button runat="server" ID="btnSearh" OnClick="btnSearh_Click" Text="Search" Style="display: inline-block;" />

            

            <div class="searchDivStyle" runat="server" id="divResetOptions" visible="False">
                <div style="float: left; margin-top:1%; margin-right:3%;">
                    Reset Task :
                </div>
                <div style="float: left; background-color:beige; margin-right:3%;">
                    <asp:CheckBoxList ID="cbxlResetTask" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem>
                        Splitting/Merging
                        </asp:ListItem>
                        <asp:ListItem>
                         Images
                        </asp:ListItem>
                        <asp:ListItem>
                         Tables
                        </asp:ListItem>
                        <asp:ListItem>
                        NPara
                        </asp:ListItem>
                        <asp:ListItem>
                        SPara
                        </asp:ListItem>
                        <asp:ListItem>
                         FootNotes
                        </asp:ListItem>
                    </asp:CheckBoxList>
                </div>
                <div style="float: left">
                    <asp:Button runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text="Reset" Style="margin-left: 3%;" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

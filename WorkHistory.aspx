<%@ Page Title="" Language="C#" MasterPageFile="~/UserMaster.Master" AutoEventWireup="true" CodeBehind="WorkHistory.aspx.cs" Inherits="Outsourcing_System.WorkHistory" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.7.1213, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainHeadContents" runat="server">

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainBodyContents" runat="server">

    <div>
        <h3>Work History and Feedback</h3>
    </div>

    <asp:GridView ID="gvArchiveTasks" runat="server" Style="margin-left: auto; margin-right: auto; background-color: #E9E9E9"
        AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" EmptyDataText='No data available.'
        GridLines="None" ShowHeader="false" ShowFooter="false" Width="65%" OnRowDataBound="gvArchiveTasks_RowDataBound"
        DataKeyNames="BookId, TaskType, TimelyDelivery, Quality, Responsiveness"
        CellSpacing="10">
        <Columns>
            <asp:TemplateField HeaderStyle-Width="100px">
                <ItemTemplate>
                    <table style="padding-left: 5%; padding-top: 5%; padding-bottom: 5%; width: 100%" cellspacing="14">
                        <tr align="left">
                            <td style="width: 83%">
                                <asp:Label runat="server" ID="lblFeedBackDate" Text='<%# Eval("FeedBackDate") %>' /></td>
                            <td style="width: 17%"> </td>
                            <%--$7.00 earned--%>
                        </tr>
                        <tr align="left">
                            <td>
                                <asp:Label runat="server" ID="lblTask" /></td>
                        </tr>
                        <tr align="left">
                            <td>
                                <cc1:Rating ID="Rating1" runat="server"
                                    StarCssClass="Star" WaitingStarCssClass="WaitingStar" EmptyStarCssClass="Star"
                                    FilledStarCssClass="FilledStar" CurrentRating='<%# Eval("Rating") %>'>
                                </cc1:Rating>
                                 <span style="margin-left:1.7%;display: inline-block;margin-top:0.5%;">4.0</span>
                            </td>
                        </tr>
                        <tr align="left">
                            <td>
                                <asp:Label runat="server" ID="lblComments" Text='<%# Eval("Comments") %>' /></td>
                        </tr>
                    </table>
                    <hr />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle CssClass="gridHeader" />
        <RowStyle HorizontalAlign="Left"></RowStyle>
    </asp:GridView>
</asp:Content>

<%@ Page Language="C#" ValidateRequest="false" MasterPageFile="UserMaster.Master"
    AutoEventWireup="True" CodeBehind="MetaInformation.aspx.cs" Inherits="Outsourcing_System.MetaInformation"
    EnableSessionState="True" %>

<%@ Reference Control="~/UserControls/ucAuthor.ascx" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
    <asp:Label ID="lblMessage" CssClass="message" Text="" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="10" style="width: 60%; text-align: center; margin: auto;">
                <tr>
                    <td align="left" style="width: 25%;">Select Information Type :
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddInformation" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddInformation_SelectedIndexChanged">
                            <asp:ListItem Value="meta">Meta</asp:ListItem>
                            <asp:ListItem Value="title">Title</asp:ListItem>
                            <asp:ListItem Value="bookrepinfo">Bookrep Info</asp:ListItem>
                            <asp:ListItem Value="bisac">BISAC</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <asp:Panel ID="pnlMeta" runat="server">
                    <tr>
                        <td align="left">
                            <b>Meta Information</b>
                        </td>
                        <td align="left">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Schema Name :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtMetaSName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Schema Revision :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtMetaSRev" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">File Name :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtMetaFName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Tag Date :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtMetaTDate" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Tag Operator :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:DropDownList ID="cboMetaTOperator" runat="server">
                                <asp:ListItem Value="pakistan" Selected="True">Pakistan</asp:ListItem>
                                <asp:ListItem Value="canberra">Canberra</asp:ListItem>
                                <asp:ListItem Value="newcastle">Newcastle</asp:ListItem>
                                <asp:ListItem Value="other">Other</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Book Title :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtMetaBTitle" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Book Type :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:DropDownList ID="cboMetaBType" runat="server">
                                <asp:ListItem Value="OTHER" Selected="True">OTHER</asp:ListItem>
                                <asp:ListItem Value="PBPress Poetry">PBPress Poetry</asp:ListItem>
                                <asp:ListItem Value="PBPress Novel">PBPress Novel</asp:ListItem>
                                <asp:ListItem Value="PBPress Technical">PBPress Technical</asp:ListItem>
                                <asp:ListItem Value="PBPress Cook Book">PBPress Cook Book</asp:ListItem>
                                <asp:ListItem Value="PBPress Drama">PBPress Drama</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Pub. Status :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:DropDownList ID="cboMetaPStatus" runat="server">
                                <asp:ListItem Value="NOT FOR PUBLICATION" Selected="True">NOT FOR PUBLICATION</asp:ListItem>
                                <asp:ListItem Value="FREE">FREE</asp:ListItem>
                                <asp:ListItem Value="DISABILITY">DISABILITY</asp:ListItem>
                                <asp:ListItem Value="RESTRICTED">RESTRICTED</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Copyright Status :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:DropDownList ID="cboMetaCStatus" runat="server">
                                <asp:ListItem Value="IN COPYRIGHT" Selected="True">IN COPYRIGHT</asp:ListItem>
                                <asp:ListItem Value="OUT OF COPYRIGHT">OUT OF COPYRIGHT</asp:ListItem>
                                <asp:ListItem Value="OTHER">OTHER</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="left">
                            <asp:Button ID="btnSaveMeta" runat="server" Text="Save Meta Information" OnClick="btnSaveMeta_Click" />
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel ID="pnlTitle" runat="server" Visible="false">
                    <tr>
                        <td align="left">
                            <b>Book Title</b>
                        </td>
                        <td align="left">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Main Title :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtBMTitle" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Sub Title :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtBSTitle" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Running Header :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtBRHeader" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left" style="border: 1px solid gray;">
                            <asp:PlaceHolder ID="authorsPlace" runat="server"></asp:PlaceHolder>
                            <asp:Button ID="btnAuthor" runat="server" Text="Add more Author" OnClick="btnAuthor_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <b>Translator</b>
                        </td>
                        <td align="left">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Full Name :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtTFulName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Prenominal :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtTPrenominal" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">First Name:
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtTFName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Last Name :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtTLName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="left">
                            <asp:Button ID="btnSaveTitle" runat="server" Text="Save Title" OnClick="btnSaveTitle_Click" />
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel ID="pnlBookrepInfo" runat="server" Visible="false">
                    <tr>
                        <td align="left">
                            <b>Bookrep Info</b>
                        </td>
                        <td align="left">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Author ID :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtInfoAID" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" style="border: 1px solid gray;">Book Summary :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtBookSummary" runat="server" TextMode="MultiLine" Width="300px"
                                Height="60px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top" style="border: 1px solid gray;">Author Information :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtAuthorInfo" runat="server" TextMode="MultiLine" Width="300px"
                                Height="60px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="left">
                            <asp:Button ID="btnBookrepInfoSave" runat="server" Text="Save Bookrep Info" OnClick="btnBookrepInfoSave_Click" />
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel ID="pnlBisac" runat="server" Visible="false">
                    <tr>
                        <td align="left">
                            <b>BISAC Code</b>
                        </td>
                        <td align="left">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Main Category :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:DropDownList ID="cboBisacMainCat" runat="server" OnSelectedIndexChanged="cboBisacMainCat_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Sub Category :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:DropDownList ID="cboBisacSubCat" runat="server" OnSelectedIndexChanged="cboBisacSubCat_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">Subject Code :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:TextBox ID="txtBisacSubCode" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="border: 1px solid gray;">BISAC Code :
                        </td>
                        <td align="left" style="border: 1px solid gray;">
                            <asp:ListBox ID="lstBISAC" runat="server" Width="275px"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="left">
                            <asp:Button ID="btnBISACAdd" runat="server" Text="Add BISAC" OnClick="btnBISACAdd_Click" />&nbsp;&nbsp;<asp:Button
                                ID="btnBISACRemove" runat="server" Text="Remove BISAC" OnClick="btnBISACRemove_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnSaveBISAC" runat="server" Text="Save BISAC Code" OnClick="btnSaveBISAC_Click" />&nbsp;&nbsp;<asp:Button
                        ID="btnSubmitMeta" runat="server" Text="Submit Meta" OnClick="btnSubmitMeta_Click" />
                        </td>
                    </tr>
                </asp:Panel>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

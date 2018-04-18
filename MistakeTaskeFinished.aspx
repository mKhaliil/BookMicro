<%@ Page Title="" Language="C#" MasterPageFile="Test.master" AutoEventWireup="true" Inherits="web_MistakeTaskeFinished" Codebehind="MistakeTaskeFinished.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="position: fixed; left: 0px; top: 0px; width: 100%; height: 100%; background-color: Black;"
        id="divDialogue" runat="server" visible="false">
        <div style="border: 3px solid red; background-color: White; width: 400px; position: absolute;
            border-radius: 1em; top: 35%; left: 40%; height: 75px;">
            <table style="width: 100%;">
                <tr>
                    <td align="right">
                        <asp:Label ID="lblResultShow" runat="server" Text="" ForeColor="White"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                         Task is Completed Successfully,
                        <asp:Button ID="btnGoToHome" runat="server" Text="Start New Task" OnClick="btnGoToHome_Click" />?
                    </td>
                </tr>
                <tr>
                    <td align="center">
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

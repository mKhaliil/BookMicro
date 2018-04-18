<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucShowMessage.ascx.cs" Inherits="Outsourcing_System.UserControls.CommonControl.ucShowMessage" %>
<%--<link href="../../FinalStyles/MessageStyles.css" rel="stylesheet" />--%>

<div class="info" id="divInfo" runat="server" style="display: none;">
    <asp:Label ID="lblInfoMessage" runat="server"></asp:Label>
</div>
<div class="success" id="divSuccess" runat="server" style="display: none">
    <asp:Label ID="lblSuccessMessage" runat="server"></asp:Label>
</div>
<div class="error" id="divError" runat="server" style="display: none">
    <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
</div>


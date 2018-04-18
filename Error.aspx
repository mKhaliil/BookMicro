<%@ Page Title="" Language="C#" MasterPageFile="~/UserMaster.Master" AutoEventWireup="true"
    CodeBehind="Error.aspx.cs" Inherits="Outsourcing_System.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainHeadContents" runat="server">
    <style type="text/css">
        .ButtonFormitting
        {
            border-radius: 14px;
            cursor: pointer;
            text-align: center;
            padding: 5px 5px;
            width: 114px;
            color: black;
            background-color: #ADDFFF;
            font: 24px/19px "AvenirLTStd-Heavy";
            height: 57px;
        }
    </style>
    
    <%--background-color: #FFDDBB--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainBodyContents" runat="server">
    <div style="margin-right:auto;margin-left:auto;margin-top:4%;margin-bottom:0%; padding-top: 7px; padding-bottom: 7px; width: 787px; height:275px; ">
        <div style="margin-left: auto; margin-right: auto; width: 470px; height: 277px;">
            <asp:Image ID="imgSorry" ImageUrl="img/sorry.png" runat="server" />
            <p style="font-size: 22px;">
                <b>Sorry!</b> Some technical problem has occured.</p>
            <p style="font-size: 22px;">
                We’ll get it sorted as soon as possible!
            </p>
            <p style="font-size: 22px;">
                Please contact support on skype (bookmicro.support) or email on bookmicro123@gmail.com.</p>
<%--            <asp:Button ID="btnBack" CssClass="ButtonFormitting" runat="server" Text="Back" 
                onclick="btnBack_Click" />--%>
        </div>
    </div>
</asp:Content>

<%--
  <div style="margin: 5% auto; padding-top: 7px; padding-bottom: 7px; width: 695px; height:226px; ">
        <div style="margin-left: auto; margin-right: auto; width: 636px; height: 197px;">
            <table width="100%">
                <tr>
                    
                    <td><asp:Image ID="Image1" ImageUrl="img/sorry.png" runat="server" /></td>
                    <td><p style="font-size: 22px; width: 420px;"><b>Sorry!</b> Some technical problem has occured.</p></td>
                </tr>
                 <tr>
                    
                  <td></td><td> <p style="font-size: 22px;">
                We’ll get it sorted out as soon as possible.
            </p></td>
                </tr>
                <tr>
                    <td></td>
                    <td> <p style="font-size: 22px;">
                Please contact support on skype (bookmicro.support) or mail on bookmicro123@gmail.com.</p></td>
                </tr>

            </table>
                
                
           
           
<%--            <asp:Button ID="btnBack" CssClass="ButtonFormitting" runat="server" Text="Back" 
                onclick="btnBack_Click" />--%>
 <%--       </div>
    </div>--%>
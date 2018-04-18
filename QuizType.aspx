<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master"
    AutoEventWireup="true" CodeBehind="QuizType.aspx.cs" Inherits="Outsourcing_System.QuizType" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
    <div id="tests" style="margin: auto; height: 260px; margin-left: auto; margin-right: auto;
        width: 500px;">
        <asp:MultiView ID="mvQuiz" runat="server">
            <asp:View ID="vDifficultyLevel" runat="server">
                <div class="roundBox_Quiz">
                    <h4>
                        Select Difficulty Level:
                    </h4>
                   
                    <div>
                        <asp:Button ID="btnBeginner" runat="server" Text="Beginner" Style="font-size: large;
                            width: 120px; background-color: #5BC85B; color: white; height: 40px;" 
                            onclick="btnBeginner_Click" />
                        <asp:Button ID="btnIntermediate" runat="server" Text="Intermediate" Style="font-size: large;
                            width: 120px; background-color: #5BC85B; color: white; height: 40px;" 
                            onclick="btnIntermediate_Click" />
                        <asp:Button ID="btnExpert" runat="server" Text="Expert" Style="font-size: large;
                            width: 120px; background-color: #5BC85B; color: white; height: 40px;" 
                            onclick="btnExpert_Click" />
                    </div>
                    <%--<hr />--%>
                </div>
            </asp:View>
            <asp:View ID="vQuizTypes" runat="server">
                <div class="roundBox_Quiz">
                    <h4>
                        Select Quiz Type:
                    </h4>
                     <div style="margin-top: -13.5%; margin-left: 87.5%; margin-bottom: 7%;">
                        <asp:Button ID="btnGoBack" runat="server" Text="Back" Style="font-size: large; width: 60px;
                            background-color: #5BC85B; color: white; height: 30px;" OnClick="btnGoBack_Click" />
                    </div>
                    <div>
                        <asp:Button ID="btnSplitting" runat="server" Text="Splitting" Style="font-size: large;
                            width: 120px; background-color: #5BC85B; color: white; height: 40px;" OnClick="btnSplitting_Click" />
                        <asp:Button ID="btnMerging" runat="server" Text="Merging" Style="font-size: large;
                            width: 120px; background-color: #5BC85B; color: white; height: 40px;" OnClick="btnMerging_Click" />
                        <asp:Button ID="btnSpace" runat="server" Text="Word Spacing" Style="font-size: large;
                            width: 130px; background-color: #5BC85B; color: white; height: 40px;" OnClick="btnSpace_Click" />
                        <asp:Button ID="btnGeneralQuiz" runat="server" Text="General" Style="font-size: large;
                            width: 120px; background-color: #5BC85B; color: white; height: 40px;" OnClick="btnGeneralQuiz_Click" />
                    </div>
                    <%--<hr />--%>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>

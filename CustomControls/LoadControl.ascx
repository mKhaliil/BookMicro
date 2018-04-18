<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoadControl.ascx.cs"
    Inherits="Outsourcing_System.CustomControls.LoadControl" %>
<link rel="stylesheet" href="../scripts/lightbox.css" type="text/css" media="screen" />

<script type="text/javascript" src="../scripts/prototype.js"></script>

<script type="text/javascript" src="../scripts/scriptaculous.js?load=effects,builder"></script>

<script type="text/javascript" src="../scripts/lightbox.js"></script>

<script type="text/javascript" language="javascript">

    window.onload = function load() {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }

    function showLoading() {
        alert('Client clicked');
        var firstLight = document.getElementById('clickMe');
        firstLight.onclick.apply(firstLight);
    }
    function EndRequestHandler() {
        //alert('i am called');
        document.getElementById('overlay').style.display = 'none';  //style.visibility = 'hidden';
        document.getElementById('lightbox').style.display = 'none';
    }
    function aClick() {
        //alert('called click');
    }

    function fakeClick(event, anchorObj) {
    //alert('i am called');
        if (anchorObj.click) {
            anchorObj.click()
        } else if (document.createEvent) {
            if (event.target !== anchorObj) {
                var evt = document.createEvent("MouseEvents");
                evt.initMouseEvent("click", true, true, window,
          0, 0, 0, 0, 0, false, false, false, false, 0, null);
                var allowDefault = anchorObj.dispatchEvent(evt);
                // you can check allowDefault for false to see if
                // any handler called evt.preventDefault().
                // Firefox will *not* redirect to anchorObj.href
                // for you. However every other browser will.
            }
        }
    }
   
</script>

<a id="clickMe" href="../img/loading.gif" rel="lightbox" title=""></a>
<%--<asp:LinkButton ID="lnkBtn" runat="server" Text="chk this" OnClick="lnkBtn_Click"
    OnClientClick="fakeClick(event, document.getElementById('clickMe'))" />--%>
<asp:Button ID="btnSubmit" runat="server" OnClick="lnkBtn_Click"
    OnClientClick="fakeClick(event, document.getElementById('clickMe'))" />
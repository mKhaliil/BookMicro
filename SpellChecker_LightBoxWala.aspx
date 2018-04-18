<%@ Page Title="" Language="C#" MasterPageFile="~/EditorMaster.Master" AutoEventWireup="true"
    CodeBehind="SpellChecker_LightBoxWala.aspx.cs" Inherits="Outsourcing_System.SpellChecker" %>

<%@ Register Assembly="ASPNetSpell" Namespace="ASPNetSpell" TagPrefix="ASPNetSpell" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder0" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    <link href="scripts/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery.colorbox.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //Examples of how to assign the Colorbox event to elements
            $(".group1").colorbox({ rel: 'group1' });
            $(".group2").colorbox({ rel: 'group2', transition: "fade" });
            $(".group3").colorbox({ rel: 'group3', transition: "none", width: "75%", height: "75%" });
            $(".group4").colorbox({ rel: 'group4', slideshow: true });
            $(".ajax").colorbox();
            $(".youtube").colorbox({ iframe: true, innerWidth: 640, innerHeight: 390 });
            $(".vimeo").colorbox({ iframe: true, innerWidth: 500, innerHeight: 409 });
            $(".iframe").colorbox({ iframe: true, width: "80%", height: "80%" });
            $(".inline").colorbox({ inline: true, width: "50%" });
            $(".callbacks").colorbox({
                onOpen: function () { alert('onOpen: colorbox is about to open'); },
                onLoad: function () { alert('onLoad: colorbox has started to load the targeted content'); },
                onComplete: function () { alert('onComplete: colorbox has displayed the loaded content'); },
                onCleanup: function () { alert('onCleanup: colorbox has begun the close process'); },
                onClosed: function () { alert('onClosed: colorbox has completely closed'); }
            });

            $('.non-retina').colorbox({ rel: 'group5', transition: 'none' })
            $('.retina').colorbox({ rel: 'group5', transition: 'none', retinaImage: true, retinaUrl: true });

            //Example of preserving a JavaScript event for inline calls.
            $("#click").click(function () {
                $('#click').css({ "background-color": "#f00", "color": "#fff", "cursor": "inherit" }).text("Open this window again and this message will still be here.");
                return false;
            });
        });
		</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LinkPortion" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h5 style="text-align: left; padding-left:40px;">
        You have Done All the Pre operations. So now in final step you have to Check spells
        of entire book. To Proceed please <a class='iframe' id="anchorTag" href="">Click Here</a></h5>
    <div style="z-index: -5000; position: absolute;">
        <ASPNetSpell:SpellTextBox ID="SpellTextBox1" Width="20" Height="20" TextMode="MultiLine"
            runat="server">
        </ASPNetSpell:SpellTextBox><ASPNetSpell:SpellButton ID="SpellButton1" runat="server" />
    </div>
    <%--<input type="button" value="Test button" onclick="demoFunction();" />--%>
    <script type="text/javascript">
        function demoFunction(a) {
            var qs = (function (a) {
                if (a == "") return {};
                var b = {};
                for (var i = 0; i < a.length; ++i) {
                    var p = a[i].split('=');
                    if (p.length != 2) continue;
                    b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
                }
                return b;
            })(window.location.search.substr(1).split('&'));
            var result = 'http://localhost:16962/ASPNetSpellInclude/dialog.html?instance=1&bid=' + qs["bid"] + '&TotalPages=' + qs["TotalPages"];
            return result;
        }
        $(document).ready(function () { init() })


        function init() {
            var downloadHref = demoFunction();
            $('#anchorTag').attr("href", downloadHref);
            //Josh's code above

        }
  </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

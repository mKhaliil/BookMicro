window.onload = function load() {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
}
$(document).ready(function () {
    $("a#show-panel").click(function () {
        $("#lightbox, #lightbox-panel").fadeIn(400);
    })
    $("a#close-panel").click(function () {
        $("#lightbox, #lightbox-panel").fadeOut(400);
    })
    $("a#close-panel1").click(function () {
        $("#lightbox, #lightbox-panel").fadeOut(400);
    })
    $("a#close-panel2").click(function () {
        $("#lightbox, #lightbox-panel").fadeOut(400);
    })
    $("a#close-panel3").click(function () {
        $("#lightbox, #lightbox-panel").fadeOut(400);
    })
})
/*function HideLightDialogAndShowLoading() {
$("#lightbox, #lightbox-panel").fadeOut(400);
$("#lightbox, #lightbox-panel1").fadeIn(400);
}*/
function showLoading() {
    alert('Client clicked');
    var firstLight = document.getElementById('clickMe');
    firstLight.onclick.apply(firstLight);
}
function TableConfirmationPostBack() {
    document.getElementById('isTreePostBack').value = 'TablePostBack';
}
function ImageBoxPostBack() {
    document.getElementById('isTreePostBack').value = 'ImagePostBack';
}
function GeneralBoxPostBack() {
    document.getElementById('isTreePostBack').value = 'General';
}
function treeViewPostBack() {
    document.getElementById('isTreePostBack').value = 'true';
}
function showBoxDiv() {
    document.getElementById('isTreePostBack').value = 'Box';
}
function showConvertPara() {
    document.getElementById('isTreePostBack').value = 'ParaConvert';
}
function showLoadingPanel() {
    document.getElementById('isTreePostBack').value = 'BlackDiv';
}
function showMessageDiv() {
    document.getElementById('isTreePostBack').value = 'Message';
}
function showRootNodeActions() {
    document.getElementById('isTreePostBack').value = 'RootNode';
}
function ShowLoading() {
    $("#lightbox").fadeIn(400);
}
function ShowLoadingGif() {
    $("#lightbox, #lightbox-panel1").fadeIn(400);
    //document.getElementById('simpleDialog').style.display = 'block';
    //document.getElementById('pnlTableConf').style.display = 'none';
}
function HideLoadingGif() {
    $("#lightbox, #lightbox-panel1,#lightbox-panel").fadeOut(400);
}
function ShowTableConfirmation() {
    $("#lightbox, #lightbox-panel").fadeIn(400);
    document.getElementById('SetRegion').style.display = 'block';
    document.getElementById('simpleDialog').style.display = 'none';
    document.getElementById('imageBox').style.display = 'none';
    document.getElementById('divBox').style.display = 'none';
    document.getElementById('divParaConvert').style.display = 'none';
    document.getElementById('divMessageBox').style.display = 'none';
    document.getElementById('divRootNodeActions').style.display = 'none';
}
function ShowImageBox() {
    $("#lightbox, #lightbox-panel").fadeIn(400);
    document.getElementById('imageBox').style.display = 'block';
    document.getElementById('SetRegion').style.display = 'none';
    document.getElementById('simpleDialog').style.display = 'none';    
    document.getElementById('divMessageBox').style.display = 'none'; 
    document.getElementById('divBox').style.display = 'none';
    document.getElementById('divParaConvert').style.display = 'none';
    document.getElementById('divRootNodeActions').style.display = 'none';
}
function ShowParaConvert() {
    $("#lightbox, #lightbox-panel").fadeIn(400);
    document.getElementById('divParaConvert').style.display = 'block';
    document.getElementById('SetRegion').style.display = 'none';
    document.getElementById('simpleDialog').style.display = 'none';
    document.getElementById('divMessageBox').style.display = 'none'; 
    document.getElementById('imageBox').style.display = 'none';
    document.getElementById('divBox').style.display = 'none';
    document.getElementById('divMessageBox').style.display = 'none';
    document.getElementById('divRootNodeActions').style.display = 'none';
}
function ShowMessageDiv() {
    $("#lightbox, #lightbox-panel").fadeIn(400);
    document.getElementById('imgLoading').style.display = 'none';    
    document.getElementById('divMessageBox').style.display = 'block'; 
    document.getElementById('SetRegion').style.display = 'none';    
    document.getElementById('imageBox').style.display = 'none';
    document.getElementById('divBox').style.display = 'none';
    document.getElementById('divParaConvert').style.display = 'none';
    document.getElementById('simpleDialog').style.display = 'none';
    document.getElementById('divRootNodeActions').style.display = 'none';

}
function ShowRootNodeActions() {
    $("#lightbox, #lightbox-panel").fadeIn(400);
    document.getElementById('divRootNodeActions').style.display = 'block';
    document.getElementById('SetRegion').style.display = 'none';
    document.getElementById('divMessageBox').style.display = 'none';
    document.getElementById('imageBox').style.display = 'none';
    document.getElementById('divBox').style.display = 'none';
    document.getElementById('divParaConvert').style.display = 'none';
    document.getElementById('simpleDialog').style.display = 'none';
}
function HideTableConfirmation() {

    $("#lightbox, #lightbox-panel").fadeOut(400);
}
function HideImagePanel() {

    $("#lightbox, #lightbox-panel").fadeOut(400);
}
function showLightDialog() {
    $("#lightbox, #lightbox-panel").fadeIn(400);
    document.getElementById('SetRegion').style.display = 'none';
    document.getElementById('simpleDialog').style.display = 'block';
    document.getElementById('imageBox').style.display = 'none';
    document.getElementById('divBox').style.display = 'none';
    document.getElementById('divParaConvert').style.display = 'none';
    document.getElementById('divMessageBox').style.display = 'none';
    document.getElementById('divRootNodeActions').style.display = 'none';
}
function functionShowBoxDiv() {
    $("#lightbox, #lightbox-panel").fadeIn(400);
    document.getElementById('SetRegion').style.display = 'none';
    document.getElementById('simpleDialog').style.display = 'none';
    document.getElementById('imageBox').style.display = 'none';
    document.getElementById('divBox').style.display = 'block';
    document.getElementById('divParaConvert').style.display = 'none';
    document.getElementById('divMessageBox').style.display = 'none';
    document.getElementById('divRootNodeActions').style.display = 'none';
}
function hideFreeTextBox() {
    $("#lightbox, #lightbox-panel").fadeOut(400);
}
function EndRequestHandler() {
    //alert('end request called');
    try {
        if (document.getElementById('isTreePostBack').value == 'true') {
            showLightDialog();
            document.getElementById('isTreePostBack').value = 'false';
        }
        else if (document.getElementById('isTreePostBack').value == 'TablePostBack') {
            ShowTableConfirmation();
            document.getElementById('isTreePostBack').value = 'false';
        }
        else if (document.getElementById('isTreePostBack').value == 'ImagePostBack') {
            ShowImageBox();
            document.getElementById('isTreePostBack').value = 'false'; Box
        }
        else if (document.getElementById('isTreePostBack').value == 'Box') {
            functionShowBoxDiv();
            document.getElementById('isTreePostBack').value = 'false';
        }
        else if (document.getElementById('isTreePostBack').value == 'ParaConvert') {
            ShowParaConvert();
            document.getElementById('isTreePostBack').value = 'false';
        }
        else if (document.getElementById('isTreePostBack').value == 'BlackDiv') {
            ShowLoading();
            document.getElementById('isTreePostBack').value = 'false';
        }
        else if (document.getElementById('isTreePostBack').value == 'RootNode') {
            ShowRootNodeActions();
            document.getElementById('isTreePostBack').value = 'false';
        }
        else if (document.getElementById('isTreePostBack').value == 'Message') {
            ShowMessageDiv();
            document.getElementById('isTreePostBack').value = 'false';
        }
        else {
            HideLoadingGif();
        }
    }
    catch (e) {
        //alert('EndRequest: '+e);
    }
    //document.getElementById('pdivFront').style.display = 'none';
}
function aClick() {
    alert('called click');
}

function fakeClick(event, anchorObj) {
    try {
        document.getElementById('pdivfront').style.display = 'block';
        document.getElementById('pdivback').style.display = 'block';
    }
    catch (e) {
        alert(e);
    }
    //alert('removed');
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

    //alert('removed');
    //document.getElementById('pdivFront').style.display = 'inline';
}
Telerik.Web.UI.Editor.CommandList["ApplySizeColor"] = function (commandName, editor, args) {
    if (editor.getSelectionHtml() != "") {
        editor.fire("FontSize", { value: "4" }); //fire the FontSize command
        editor.fire("ForeColor", { value: "red" }); //fire the ForeColor command
    }
    else {
        alert("Please, select some text!");
        args.set_cancel(true);
    }
};

Telerik.Web.UI.Editor.CommandList["InsertCustomDate"] = function (commandName, editor, args) {
    editor.pasteHtml('<span style="width:200px;border: 1px dashed #bb0000;background-color: #fafafa;color: blue;"> ' + new Date() + ' </span>');
};

Telerik.Web.UI.Editor.CommandList["Superscript"] = function (commandName, editor, args) {
    if (editor.getSelectionHtml() != "") {
        editor.pasteHtml('<sup>' + editor.getSelectionHtml() + ' </sup>');
    }
    else {
        alert("Please, select some text!");
        args.set_cancel(true);
    }
};
Telerik.Web.UI.Editor.CommandList["Subscript"] = function (commandName, editor, args) {
    if (editor.getSelectionHtml() != "") {
        editor.pasteHtml('<sub>' + editor.getSelectionHtml() + ' </sub>');
    }
    else {
        alert("Please, select some text!");
        args.set_cancel(true);
    }
};
Telerik.Web.UI.Editor.CommandList["BoldItalic"] = function (commandName, editor, args) {
    if (editor.getSelectionHtml() != "") {
        editor.fire("Bold"); //fire the FontSize command
        editor.fire("Italic"); //fire the ForeColor command

    }
    else {
        alert("Please, select some text!");
        args.set_cancel(true);
    }
};
Telerik.Web.UI.Editor.CommandList["AllLower"] = function (commandName, editor, args) {
    var val = editor.getSelectionHtml();
    if (val != "") {
        val = val.toLowerCase();
        editor.pasteHtml(val);
    }
};
Telerik.Web.UI.Editor.CommandList["TemplateManager"] = function (commandName, editor, args) {

    alert(document.getElementById('ctl00_ContentPlaceHolder1_txtFootNote').value);
    if (editor.getSelectionHtml() != "" && document.getElementById('ctl00_ContentPlaceHolder1_txtFootNote').value != "") {

        editor.pasteHtml('<footnote id="1">' + document.getElementById('ctl00_ContentPlaceHolder1_txtFootNote').value + '</footnote>');
    }
    else {
        alert("Please, select some text!");
        args.set_cancel(true);
    }
};




Telerik.Web.UI.Editor.CommandList["InsertCustomDate"] = function (commandName, editor, args) {
    editor.pasteHtml('<span style="width:200px;border: 1px dashed #bb0000;background-color: #fafafa;color: blue;"> ' + new Date() + ' </span>');
};

Telerik.Web.UI.Editor.CommandList["AllCaps"] = function (commandName, editor, args) {
    var val = editor.getSelectionHtml();
    if (val != "") {
        val = val.toUpperCase();
        editor.pasteHtml(val);
    }
};

Telerik.Web.UI.Editor.CommandList["ResetContent"] = function (commandName, editor, args) {
    editor.set_html(""); //set empty content
};
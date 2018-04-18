window.onload = function load() {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    var prmVal = getQuerystring('prm', 'NA');
    if (prmVal != 'NA') {
        ShowLightBoxMessage(prmVal);
    }
    //alertMe();
    ShowLightBoxMessage('');

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

function aClick() {
    alert('called click');
}
function sClick() {
    alert('called from submitTask.js ');
}

String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/g, "");
}
String.prototype.ltrim = function () {
    return this.replace(/^\s+/, "");
}
String.prototype.rtrim = function () {
    return this.replace(/\s+$/, "");
}
function ShowLoadingGif() {
    $("#lightbox, #lightbox-panel1").fadeIn(400);
    //document.getElementById('simpleDialog').style.display = 'block';
    //document.getElementById('pnlTableConf').style.display = 'none';
}

function ShowLightBoxMessage(Message) {
    try {
        var MsgVal = document.getElementById('lblDiv').textContent.trim();
        //alert('Msg is: ' + MsgVal);
        if (MsgVal != '') {
            $("#lightbox, #lightbox-panel").fadeIn(400);
        }
    }
    catch (e) {
        alert('Error: ' + e);
    }
}

function HideLoadingGif() {
    $("#lightbox, #lightbox-panel1").fadeOut(400);
}

//Gets the queryString value returns default_ if it doesnt exist
function getQuerystring(key, default_) {
    if (default_ == null) default_ = "";
    key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
    var qs = regex.exec(window.location.href);
    if (qs == null)
        return default_;
    else
        return qs[1];
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


<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Process1.aspx.cs" Inherits="Outsourcing_System.Process1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="FinalScripts/TableDetectionPdfJs/jquery-1.9.1.js"></script>
    <link href="FinalStyles/minimal.css" rel="stylesheet" type="text/css" />
    <link href="FinalStyles/viewer.css" rel="stylesheet" />
    <script src="FinalScripts/TableDetectionPdfJs/pdf.js" type="text/javascript"></script>
    <script src="FinalScripts/TableDetectionPdfJs/ui_utils.js" type="text/javascript"></script>
    <script src="FinalScripts/TableDetectionPdfJs/text_layer_builder.js" type="text/javascript"></script>
    <script src="FinalScripts/TableDetectionPdfJs/minimal.js" type="text/javascript"></script>

    <script src="scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="FinalStyles/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" type="text/css" />
    <script src="FinalScripts/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>

    <script src="FinalScripts/ifvisible.js" type="text/javascript"></script>
    <script src="FinalScripts/TImeMe.js" type="text/javascript"></script>

    <style type="text/css">
        .success {
            color: #4F8A10;
            background: #DFF2BF url('img/green-ok.gif') no-repeat 10px center;
        }

        .error {
            color: #D8000C;
            background: #FFBABA url('img/red-error.gif') no-repeat 10px center;
        }

        .selected {
            background-color: #f0f0f0;
        }

        .ui-selecting, .ui-selected {
            background: lightBlue;
        }

        /*table, th, td {
    border: 1px solid black;
    border-collapse: collapse;
}*/
        .auto-style1 {
            width: 8%;
        }

        .auto-style2 {
            height: 90px;
            width: 4%;
        }
    </style>

    <script type="text/javascript">

        ///////////////////////////////////////Table PopUpCode//////////////////////////////////////////

        $(function () {

            //$("#divSize").toggle();

            $('#mytable>tbody>tr').click(function () {
                $('.selected').removeClass('selected');
                $(this).addClass('selected');
            });
        });

        // modify table column width
        function validateColumnTotalWidth() {

            var totalColumns = $('#tblAdjustColWidth tr:eq(1)>td').length;

            var myTotal = 0;
            var isColWidthExceeds = 'false';

            for (var i = 0; i < totalColumns; i++) {

                var width = $('#tblAdjustColWidth tr:eq(1)>td:eq(' + (i + 1) + ')').html();

                myTotal += parseInt(width);

                if (myTotal > 100)
                {
                    isColWidthExceeds = 'true';
                    alert('Error! Sum of columns width should be less then 100');
                    return isColWidthExceeds;
                }
            }

            return isColWidthExceeds;
        }

        // modify table column width
        function updateColunmWidth() {

            var result = validateColumnTotalWidth();

            if (result == 'false') {

                var totalColumns = $('#tblAdjustColWidth tr:eq(1)>td').length;
                var tableRows = $('#mytable tr').length;

                for (var i = 0; i < totalColumns; i++) {

                    var width = $('#tblAdjustColWidth tr:eq(1)>td:eq(' + (i + 1) + ')').html();

                    for (var j = 0; j < tableRows; j++) {

                        $('#mytable tr:eq(' + j + ')>td:eq(' + i + ')').css('width', width + '%');
                    }
                }
            }
        }

        // append row to the HTML table
        function mergeRows() {

            var selectedRows = $('#mytable>tbody>tr.ui-selected');
            document.getElementById('<%= hfMergedRows.ClientID %>').value = selectedRows;

            var width = '99%';
            var tbl = document.getElementById('mytable');
            var selectedIndex = $('#mytable>tbody>tr:first.ui-selected').index();

            //alert('new row index ' + selectedIndex);

            var row = tbl.insertRow(selectedIndex);

            row.setAttribute('style', 'white-space: pre');

            var maxColumns = 0;
            $('#mytable>tbody>tr.ui-selected').each(function () { maxColumns = Math.max(maxColumns, $('td', this).length); });

            var trs = selectedRows;
            var tds = null;
            var html = [];

            //var startIndex = $('#mytable>tbody>tr:first.ui-selected').index();
            //var endIndex = $('#mytable>tbody>tr:last.ui-selected').index();

            //alert('before '+startIndex + ', ' + endIndex);

            for (var i = 0; i < maxColumns; i++) {
                for (var j = 0; j < selectedRows.length; j++) {
                    tds = trs[j].getElementsByTagName("td");
                    //alert(tds[i].innerText);
                    html.push(tds[i].innerText);
                }
                //alert(html.join(""));
                //createMergedCell(row.insertCell(i), html.join("\n"), width);
                //$('.divtdContent').text().split()[0].replace("↵", " ");

                var rowText = html.join("\n");
                var finaltd = rowText.split()[0].replace(/\n/ig, '\n');

                createMergedCell(row.insertCell(i), finaltd, width);
                html = [];
            }

            //var newselectedIndex = $('#mytable>tbody>tr:first.ui-selected').index();

            //alert('newselectedIndex after adding new row  = ' + newselectedIndex);

            //var startIndex1 = $('#mytable>tbody>tr:first.ui-selected').index();
            //var endIndex1 = $('#mytable>tbody>tr:last.ui-selected').index();

            //alert('after ' +startIndex1 + ', ' + endIndex1);

            deleteSelectedRows();
        }

        function deleteSelectedRows() {
            var startIndex = $('#mytable>tbody>tr:first.ui-selected').index();
            var seletectedRowCount = $('#mytable>tbody>tr.ui-selected').index();

            //alert('before deleting ' + startIndex + ', ' + endIndex);

            for (var i = startIndex; i <= seletectedRowCount; i++) {
                document.getElementById("mytable").deleteRow($('#mytable>tbody>tr.ui-selected').index());
            }
        }

        function createRow(selectedIndex, columnCount, text) {

            var tt = $('.divtdContent').text().split();

            alert(text + ',  ' + tt);

            for (var i = 0; i < columnCount; i++) {

                var col = document.createElement('td');
                row.appendChild(col);
                col.innerHTML = text;
                $('#mytable tr').eq(selectedIndex).before(row);
            }
        }

        // create DIV element and append to the table cell
        function createMergedCell(cell, text, width) {
            var div = document.createElement('div'), // create DIV element
                txt = document.createTextNode(text); // create text node
            div.appendChild(txt);                    // append text node to the DIV
            //div.setAttribute('height', '30px;');        // set DIV class attribute

            //div.setAttribute("style", "height:100px");

            div.setAttribute('class', 'divtdContent');    // set DIV class attribute for IE (?!)

            div.setAttribute('style', 'border: 1px solid black; height:auto; width:' + width + "'");
            cell.appendChild(div);                   // append DIV to the table cell
        }


        function getColumnWidth(height) {

            var maxColumns = 0;
            $('#mytable tr').each(function () { maxColumns = Math.max(maxColumns, $('td', this).length); });

            var oneColWidth = 100 / (maxColumns + 1);

            //updateNewColWidth(oneColWidth, maxColumns + 1, height);

            return oneColWidth;
        }

       


        // append row to the HTML table
        function appendRows(position) {

            var width = '99%';
            var tbl = document.getElementById('mytable');

            var selectedIndex = $('#mytable>tbody>tr.selected').index();
            var insertIndex, i;

            if (position == 'previous') {

                if (selectedIndex == 0)
                    insertIndex = 0;
                else
                    insertIndex = selectedIndex;
            }
            else
                insertIndex = selectedIndex + 1;

            var columnSize = tbl.rows[0].cells.length;

            var row = tbl.insertRow(insertIndex);
            $("#lblRowCount").html(tbl.rows.length);

            // insert table cells to the new row
            for (i = 0; i < columnSize; i++) {
                createCell(row.insertCell(i), '', width);
            }
        }

       

        //// create DIV element and append to the table cell old 
        //function createCell(cell, text, width) {
        //    var div = document.createElement('div'), // create DIV element
        //        txt = document.createTextNode(text); // create text node
        //    div.appendChild(txt);                    // append text node to the DIV
        //    //div.setAttribute('class', style);        // set DIV class attribute
        //    //div.setAttribute('className', style);    // set DIV class attribute for IE (?!)

        //    var height = 'auto';

        //    if (text == '')
        //        height = '20px';

        //    div.setAttribute('style', 'border: 1px solid black; height:' + height + '; width:' + width + "'");
        //    cell.appendChild(div);                   // append DIV to the table cell
        //}

        // append column to the HTML table
        function appendColumns(position) {
            var tbl = document.getElementById('mytable');
            //var width = '99%';

            var i;
            var max = 0;
            $('#mytable tr').each(function () { max = Math.max(max, $('td', this).length); });
            $("#lblColumnCount").html(max);

            var height = '20px';

            var width = getColumnWidth(height);
            updateBodyColWidth(width);

            updateAdjustColTable();

            // open loop for each row and append cell
            for (i = 0; i < tbl.rows.length; i++) {
                //createCell(tbl.rows[i].insertCell(tbl.rows[i].cells.length), '', width);

                if (position == 'previous')
                    createCell(tbl.rows[i].insertCell(0), '', width);
                else
                    createCell(tbl.rows[i].insertCell(tbl.rows[i].cells.length), '', width);
            }
        }

        // create DIV element and append to the table cell
        function createCell(cell, text, width) {

            var height = 'auto';
            if (text == '')
                height = '20px';

            //width = getColumnWidth(height);

            cell.setAttribute('style', 'border: 1px solid black; height:' + height + '; width:' + width + "%'");
        }

        // modify table column width
        function updateBodyColWidth(colWidth) {

            var tableRows = $('#mytable tr').length;

                for (var i = 0; i < tableRows; i++) {

                    var cols = $('#mytable tr:eq(' + i + ')>td').length;

                    for (var j = 0; j < cols; j++) {

                        $('#mytable tr:eq(' + i + ')>td:eq(' + j + ')').css('width', colWidth + '%');
                    }
                }
        }

        // update adjust column table width
        function updateAdjustColTable() {

            var totalColumns = $('#tblAdjustColWidth tr:eq(1)>td').length + 1;
            var widthPer = Math.round(86 / totalColumns);
            var tableColWidth = Math.round(100 / (totalColumns - 1));

            $("#tblAdjustColWidth tr:eq(0)").append("<td style='border: 1px solid black; width:" + widthPer + "%' >Col " + (totalColumns - 1) + '</td>');
            $("#tblAdjustColWidth tr:eq(1)").append("<td style='border: 1px solid black; width:" + widthPer + "%' contenteditable='true'>" + widthPer + " </td>");
            
            var tableRows = $('#tblAdjustColWidth tr').length;
            for (var i = 0; i < tableRows; i++) {

                for (var j = 0; j < totalColumns; j++) {
                    $('#tblAdjustColWidth tr:eq(' + i + ')>td:eq(' + (j + 1) + ')').css('width', widthPer + '%');
                    
                    if (i == 1)
                    {
                        $('#tblAdjustColWidth tr:eq(' + i + ')>td:eq(' + (j + 1) + ')').text(tableColWidth);
                        $('#tblAdjustColWidth tr:eq(' + i + ')>td:eq(' + (j + 1) + ')').css('background-color', '#f0f0f0');
                    }
                }
            }
        }

        // update adjust column table width
        function deleteAdjWidthCol() {

            var tblAdjustCol = document.getElementById('tblAdjustColWidth');

            for (i = 0; i < tblAdjustCol.rows.length; i++) {

                tblAdjustCol.rows[i].deleteCell(tblAdjustCol.rows[i].cells.length - 1);
            }

            var totalColumns = $('#tblAdjustColWidth tr:eq(1)>td').length;
            var widthPer = Math.round(86 / totalColumns);
            var tableColWidth = Math.round(100 / (totalColumns - 1));

            //$("#tblAdjustColWidth tr:eq(0)").append("<td style='border: 1px solid black; width:" + widthPer + "%' >Col " + (totalColumns - 1) + '</td>');
            //$("#tblAdjustColWidth tr:eq(1)").append("<td style='border: 1px solid black; width:" + widthPer + "%' contenteditable='true'>" + widthPer + " </td>");

            var tableRows = $('#tblAdjustColWidth tr').length;
            for (var i = 0; i < tableRows; i++) {

                for (var j = 0; j < totalColumns; j++) {
                    $('#tblAdjustColWidth tr:eq(' + i + ')>td:eq(' + (j + 1) + ')').css('width', widthPer + '%');

                    if (i == 1)
                        $('#tblAdjustColWidth tr:eq(' + i + ')>td:eq(' + (j + 1) + ')').text(tableColWidth);
                }
            }
        }

        // delete table rows with index greater then 0
        function deleteRows(position) {

            var tbl = document.getElementById('mytable');

            var selectedIndex = $('#mytable>tbody>tr.selected').index();
            var insertIndex;

            if (position == 'previous')
                insertIndex = selectedIndex - 1;
            else
                insertIndex = selectedIndex + 1;

            tbl.deleteRow(insertIndex);
            $("#lblRowCount").html(tbl.rows.length);
        }

        function deleteColumns(position) {

            var tbl = document.getElementById('mytable');

            for (i = 0; i < tbl.rows.length; i++) {

                if (position == 'previous')
                    tbl.rows[i].deleteCell(tbl.rows[i].cells[i]);
                else
                    tbl.rows[i].deleteCell(tbl.rows[i].cells.length - 1);
            }

            var max = 0;
            $('#mytable tr').each(function () { max = Math.max(max, $('td', this).length); });
            $("#lblColumnCount").html(max);

            deleteAdjWidthCol();
        }

        $(function () {

            function bindMultipleSelect(element) {
                var self = this;
                var isCtrlDown = false;
                element.on('click', 'tr', function () {
                    var tr = $(this);
                    if (!isCtrlDown)
                        return;
                    tr.toggleClass('ui-selected');
                });
                $(document).on('keydown', function (e) {
                    isCtrlDown = (e.which === 17);
                });
                $(document).on('keyup', function (e) {
                    isCtrlDown = !(e.which === 17);
                    { isCtrlDown = false; }
                });
                self.getSelectedRows = function () {
                    var arr = [];
                    element.find('.ui-selected').each(function () {
                        arr.push($(this).find('td').eq(0).text());
                    });
                    return arr;
                };
                return self;
            }
            window.myElement = bindMultipleSelect($('#mytable'));
        });

        ////////////////////////////////////////////end//////////////////////////////////////////////////

        $(function () {

            $("#divNavigationPanel").hover(
                function () {
                    $(this).css("height", "7%");
                    $(this).css({ 'z-index': '999' });

                }, function () {
                    $(this).css("height", "0.01%");
                    $(this).css({ 'z-index': '999' });
                });
        });



        function getSelectedHtml() {
            var range;
            if (document.selection && document.selection.createRange) {
                range = document.selection.createRange();
                return range.htmlText;
            }
            else if (window.getSelection) {
                var selection = window.getSelection();
                if (selection.rangeCount > 0) {
                    range = selection.getRangeAt(0);
                    var clonedSelection = range.cloneContents();
                    var div = document.createElement('div');
                    div.appendChild(clonedSelection);
                    return div.innerHTML;
                }
                else {
                    return '';
                }
            }
            else {
                return '';
            }
        }

        //function getSelectionText() {
        //    var text = "";

        //    //var text = $('.textLayer').text();

        //    if (window.getSelection) {
        //        text = window.getSelection().toString();
        //    } else if (document.selection && document.selection.type != "Control") {
        //        text = document.selection.createRange().text;
        //    }
        //    return text;
        //}

        //function SetDimensions() {
        //    alert('11222');
        //}
        //window.onbeforeunload = SetDimensions;

        function ShowMessege(updateStatus) {

            if (updateStatus == true) {
                ShowSuccessMessege();
            }
            else if (updateStatus == false) {
                ShowErrorMessege();
            }
        }

        function ShowSuccessMessege() {

            $('#divSuccessMessage').html('');
            $('#divSuccessMessage').append('Table is saved successfully.');
            $("#divSuccessMessage").display = 'block';
            $("#divSuccessMessage").slideDown("slow");
            setTimeout(function () { $("#divSuccessMessage").slideUp("slow"); }, 5000);
        }

        function ShowErrorMessege() {

            $('#divErrorMessage').html('');
            $('#divErrorMessage').append('Sorry! Some error has occured while updating xml.');
            $("#divErrorMessage").display = 'block';
            $("#divErrorMessage").slideDown("slow");
            setTimeout(function () { $("#divErrorMessage").slideUp("slow"); }, 10000);
        }

        $(function () {

            document.onkeydown = function (e) {

                // ctrl + e (e = 69)
                
                if (e.ctrlKey === true) {
                    if (e.keyCode === 69) {
                        //                alert('dddddddddddddd');

                        //alert(getSelectedHtml());
                        e.preventDefault();
                        LogMistakeCallBack();
                    }
                }

                // ctrl + Right arrow (Right arrow 39)
                if (e.ctrlKey === true) {
                    if (e.keyCode === 39) {
                        //                alert('NavigateNextPageCallBack');
                        e.preventDefault();
                        NavigateNextPageCallBack();
                    }
                }

                // ctrl + Left arrow (Left arrow 	37)
                if (e.ctrlKey === true) {
                    if (e.keyCode === 37) {
                        //                alert('NavigatePrevPageCallBack');
                        e.preventDefault();
                        NavigatePrevPageCallBack();
                    }
                }

                // ctrl + m (e = 77)
                if (e.ctrlKey === true) {
                    if (e.keyCode === 77) {
                        //                alert('NavigatePrevPageCallBack');
                        e.preventDefault();
                        mergeRows();
                    }
                }

                // ctrl + q (e = 81)
                if (e.ctrlKey === true) {
                    if (e.keyCode === 81) {
                        //                alert('NavigatePrevPageCallBack');
                        e.preventDefault();
                        mergeRows();
                    }
                }
            };
        });

        function LogMistakeCallBack() {
            $('#<%= btnMarkTableLines.ClientID %>').click();
        }
        function NavigateNextPageCallBack() {
            $('#<%= btnNextPage.ClientID %>').click();
        }
        function NavigatePrevPageCallBack() {
            $('#<%= btnPrevPage.ClientID %>').click();
        }

        function MarkTableAsImageInXml() {

            var cbxIgnoreAlgo = document.getElementById('<%= hfIsIgnoreAlgo.ClientID %>');
            var pageCompleteText = $('.textLayer')[0].innerHTML;

            var tableLines = getSelectedHtml();

            if (tableLines != null || tableLines != '') {

                var obj = {};

                if (cbxIgnoreAlgo.value == null || cbxIgnoreAlgo.value == '')
                    cbxIgnoreAlgo.value = 'false';

                obj.text = tableLines + "/~/" + cbxIgnoreAlgo.value + '/~/' + pageCompleteText;;

                methodURL = "Process1.aspx/MarkTable";
                var returnVal = false;

                $.ajax({
                    type: "POST",
                    data: JSON.stringify(obj),
                    contentType: "application/json; charset=utf-8",
                    url: methodURL,
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        $('#divMarkWholeTable').css("dislay", "none");
                        $('#divEditText').css("dislay", "none");
                        $('#divDialogue').css("display", "none");
                    },
                    error: function (jqXHR, textStatus, errorThrown) {

                    },
                    success: function (data) {

                        document.getElementById('<%= hfIsIgnoreAlgo.ClientID %>').value = 'false';

                        ShowMessege(data.d);
                    }
                });
                }
            }

            function MarkTableHeaderCapInXml(RowType) {

                //var parentElement = getSelectionParentsOuterHTML();
                //var selectedTextLine = getInnerHTML(parentElement);
                //alert(selectedTextLine);

                var selectedTextLine = getSelectedHtml();

                //alert(selectedTextLine);

                if (selectedTextLine != null || selectedTextLine != '') {

                    var obj = {};

                    obj.text = selectedTextLine + "/~/" + RowType;

                    methodURL = "Process1.aspx/MarkTableHeaderCaption";
                    var returnVal = false;

                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(obj),
                        contentType: "application/json; charset=utf-8",
                        url: methodURL,
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            $('#divMarkWholeTable').css("dislay", "none");
                            $('#divMarkTableHeadCap').css("dislay", "none");
                            $('#divEditText').css("dislay", "none");
                            $('#divDialogue').css("display", "none");
                        },
                        error: function (jqXHR, textStatus, errorThrown) {

                            alert('error');
                        },
                        success: function (data) {

                            if (RowType == 'tableHeader')
                                document.getElementById('<%= hfSetManualTableHeader.ClientID %>').value = 'false';
                            else if (RowType == 'tableCaption')
                                document.getElementById('<%= hfSetManualTableCaption.ClientID %>').value = 'false';

                            ShowMessege(data.d);
                        }
                    });
            }
        }

        function MarkTableInXml() {

            var tableLines = getSelectedHtml();

            if (tableLines != null || tableLines != '') {
                document.getElementById('<%= hfMarkedTableText.ClientID %>').value = tableLines;
            }
        }

        function FindPosition(oElement) {

            if (typeof (oElement.offsetParent) != "undefined") {
                for (var posX = 0, posY = 0; oElement; oElement = oElement.offsetParent) {
                    posX += oElement.offsetLeft;
                    posY += oElement.offsetTop;
                }
                return [posX, posY];
            }
            else {
                return [oElement.x, oElement.y];
            }
        }

        function getInnerHTML(ParentNodeHTML) {
            var div = document.createElement('div');
            div.innerHTML = ParentNodeHTML;
            var HTMLWithBreaks = div.textContent;
            try {
                var cleaned = HTMLWithBreaks.replace(/<br[ ]?[/]>/g, "\n");
            }
            catch (e) {
                document.write(e);
                alert(e);
            }
            return cleaned;
        }

        function getSelectionParentsOuterHTML() {
            var parentEl = null;
            if (window.getSelection) {
                var sel = window.getSelection();
                var allParentHTML = "";
                if (sel.rangeCount) {
                    /*if (sel.getRangeAt(0).endContainer == sel.getRangeAt(0).startContainer) //Only one div is selected
                    {
                        parentEl = sel.getRangeAt(0).commonAncestorContainer;
                        if (parentEl.nodeType != 1) {
                            parentEl = parentEl.parentNode;
                            //allParentHTML = parentEl;
                            allParentHTML = parentEl.outerHTML;
                        }
                    }
                    else */
                    {
                        var selElements = new Array();
                        var selElementsTop = new Array();

                        var cuurrEl = null;
                        var cuurrEl2 = null;
                        var tempEl = null;
                        var startTop, endTop;

                        if (sel.getRangeAt(0).startContainer.nextSibling == null) {
                            currEl = sel.getRangeAt(0).startContainer.parentNode;
                        }
                        else {
                            currEl = sel.getRangeAt(0).startContainer;
                        }

                        startTop = currEl.style["top"];

                        if (sel.getRangeAt(0).endContainer.nextSibling == null) {
                            currEl2 = sel.getRangeAt(0).endContainer.parentNode;
                        }
                        else {
                            currEl2 = sel.getRangeAt(0).endContainer;
                        }
                        endTop = currEl2.style["top"];
                        if (endTop == "")
                            currEl2 = sel.getRangeAt(0).endContainer;

                        tempEl = currEl;


                        if (endTop == startTop) {
                            allParentHTML += getCompleteLine(currEl);
                        } else {

                            var endTop1 = endTop.replace('px', '');//trimmed
                            var startTop1 = startTop.replace('px', '');//trimmed

                            while (parseFloat(startTop1) <= parseFloat(endTop1)) //Getting all the distinct elements in array
                            {
                                startTop = tempEl.style["top"];
                                startTop1 = startTop.replace('px', '');

                                var chckElementTop = true;
                                for (var x = 0; x <= selElementsTop.length - 1; x++) {
                                    var checkElement = selElementsTop[x];
                                    if (checkElement == startTop) {
                                        chckElementTop = false;
                                        break;
                                    }
                                }

                                if (chckElementTop) {
                                    selElements.push(tempEl);
                                    selElementsTop.push(tempEl.style["top"]);
                                }
                                tempEl = tempEl.nextSibling;
                                startTop = tempEl.style["top"];
                                startTop1 = startTop.replace('px', '');
                            }

                            var tempCount = true;
                            for (var x = 0; x <= selElementsTop.length - 1; x++) {
                                var lineElement = selElements[x];
                                if (tempCount) {
                                    allParentHTML += getCompleteLine(lineElement);
                                    tempCount = false;
                                } else {
                                    allParentHTML += "&lt;br/&gt;" + getCompleteLine(lineElement);
                                }
                            }
                        }
                    }

                }
            } else if (document.selection && document.selection.type != "Control") {
                parentEl = document.selection.createRange().parentElement();
                allParentHTML = parentEl;
            }
            //        if(allParentHTML.indexOf("&lt;br/&gt;")!=-1)
            //                           {
            //                           allParentHTML+=" khalil";
            //   }     
            return allParentHTML;
        }

        function getCompleteLine(currEl) {
            var tempTop = currEl.style["top"];
            //alert($('[top='+tempTop+']'));

            var prevTop = "", newTop = "";
            //var cuurrEl  = null;
            var origCurrEl = currEl;
            //currEl = sel.getRangeAt(0).startContainer.parentNode;
            //alert(currEl.outerHTML);
            allParentHTML = '<div id="ParentHTML">';
            //prevTop = currEl.style["top"]; //current selected div's top

            prevTop = currEl.style["top"];
            if (currEl.previousSibling != null) {
                currEl = currEl.previousSibling;
                newTop = currEl.style["top"];
            }

            while (Math.round(newTop.replace('px', '')) == Math.round(prevTop.replace('px', ''))) //THis condition is used for picking complete line before selection
            {
                allParentHTML = currEl.outerHTML + allParentHTML;
                prevTop = currEl.style["top"];
                if (currEl.previousSibling != null) {
                    currEl = currEl.previousSibling;
                    newTop = currEl.style["top"];
                }
                else {
                    newTop = currEl.style["top"];
                    break;
                }
            }
            currEl = origCurrEl;//sel.getRangeAt(0).startContainer.parentNode;
            do {
                allParentHTML += currEl.outerHTML;
                //alert(currEl.outerHTML);
                prevTop = currEl.style["top"];
                if (currEl.nextSibling != null) {
                    currEl = currEl.nextSibling;
                    newTop = currEl.style["top"];
                } else {
                    newTop = currEl.style["top"];
                    break;
                }
                //if (newTop != prevTop) {
                //  allParentHTML += "&lt;br/&gt;";
                //}
            } while (Math.round(newTop.replace('px', '')) == Math.round(prevTop.replace('px', ''))); //THis condition is used for picking complete line after selection
            //while (currEl != sel.getRangeAt(0).endContainer.parentNode);  //THis condition is used for picking the selected text
            //allParentHTML += currEl.outerHTML;
            allParentHTML += '</div>';
            return allParentHTML;
        }

        function highlightSelection(text) {

        }

        $(function () {

            var bodyRowstable = document.getElementById("mytable");
            bodyRowstable.onmouseup = GetTableCoordinates;

            function GetTableCoordinates(e) {

                if (window.getSelection() != "") {

                    //document.getElementById("selectedRow").addEventListener("click", hightSelection, false);
                    //hightSelection();

                    //alert('Selected rows are '+ window.getSelection());

                    ////var lines = window.getSelection().toString().replace('↵', '').split(/\n/);
                    ////alert(lines[0] +' ,' + lines[1]);

                    ////for (var i = 0; i < lines.length; i++) {

                    ////    highlightSelection(lines[i].replace(/\s/g, '&nbsp;'));
                    ////}


                    //highlightSelection(window.getSelection().toString().replace('↵',''));


                    //ShowTableEditDialog();

                    //var posX = 0;
                    //var posY = 0;
                    //var divPos;
                    //divPos = FindPosition(bodyRowstable);
                    //if (!e) var e = window.event;
                    //if (e.pageX || e.pageY) {
                    //    posX = e.pageX;
                    //    posY = e.pageY;
                    //}
                    //else if (e.clientX || e.clientY) {
                    //    posX = e.clientX + document.body.scrollLeft
                    //        + document.documentElement.scrollLeft;
                    //    posY = e.clientY + document.body.scrollTop
                    //        + document.documentElement.scrollTop;
                    //}
                    //posX = posX - divPos[0];
                    //posY = posY - divPos[1];


                    //alert('posX = ' + (posX - 650) + ' posY = ' + (posY - 40));

                    //$('#divTableBodyDialogue').css("display", "block");
                    //$("#divTableBodyDialogue").css("left", posX - 145 + "px");
                    //$("#divTableBodyDialogue").css("top", posY - 0 + "px");
                    //$('#divTextEdit').css("display", "block");
                }
                else {

                    $('#divTableBodyDialogue').css("display", "none");
                    $('#divTextEdit').css("display", "none");
                }
            }

            var mainDiv = document.getElementById("divPdfPanel");
            mainDiv.onmouseup = GetCoordinates;

            function GetCoordinates(e) {

                getPage_Comments = 0;

                //var parentElement = getSelectionParentsOuterHTML();
                //var selectedTextLine = getInnerHTML(parentElement);

                <%--var selectedTextLine = getSelectedHtml();

                 document.getElementById('<%= hfTableSelectedLine.ClientID %>').value = '';
                 document.getElementById('<%= hfTableSelectedLine.ClientID %>').value = selectedTextLine;--%>

                //OriginalText.value = selectedTextLine;

                //hfPageAllText.value = e.target.innerHTML;

                if (window.getSelection() != "") {
                    var posX = 0;
                    var posY = 0;
                    var divPos;
                    divPos = FindPosition(mainDiv);
                    if (!e) var e = window.event;
                    if (e.pageX || e.pageY) {
                        posX = e.pageX;
                        posY = e.pageY;
                    }
                    else if (e.clientX || e.clientY) {
                        posX = e.clientX + document.body.scrollLeft
                            + document.documentElement.scrollLeft;
                        posY = e.clientY + document.body.scrollTop
                            + document.documentElement.scrollTop;
                    }
                    posX = posX - divPos[0];
                    posY = posY - divPos[1];

                    document.getElementById('<%= hfPopupPosX.ClientID %>').value = posX;
                document.getElementById('<%= hfPopupPosY.ClientID %>').value = posY;

                //            alert('posX = ' + (posX - 650) + ' posY = ' + (posY - 40));

                if ((document.getElementById('<%= hfSetManualTableHeader.ClientID %>').value == 'true') ||
                (document.getElementById('<%= hfSetManualTableCaption.ClientID %>').value == 'true')) {

                    $('#divDialogue').css("display", "block");
                    $("#divDialogue").css("left", posX - 145 + "px");
                    $("#divDialogue").css("top", posY - 0 + "px");
                    $('#divMarkWholeTable').css("display", "none");
                    $('#divMarkTableHeadCap').css("display", "block");

                    if (document.getElementById('<%= hfSetManualTableHeader.ClientID %>').value == 'false' ||
                        document.getElementById('<%= hfSetManualTableHeader.ClientID %>').value == '')
                        $('#btnMarkTableHeader').css("display", "none");

                    if (document.getElementById('<%= hfSetManualTableCaption.ClientID %>').value == 'false' ||
                        document.getElementById('<%= hfSetManualTableCaption.ClientID %>').value == '')
                            $('#btnMarkTableCaption').css("display", "none");

                } else {

                    $('#divDialogue').css("display", "block");
                    $("#divDialogue").css("left", posX - 145 + "px");
                    $("#divDialogue").css("top", posY - 0 + "px");
                    $('#divMarkWholeTable').css("display", "block");
                    $('#divMarkTableHeadCap').css("display", "none");
                }

                //$('#divDialogue').css("display", "block");
                //$("#divDialogue").css("left", posX - 145 + "px");
                //$("#divDialogue").css("top", posY - 0 + "px");
                //$('#divMarkWholeTable').css("display", "block");
            }
            else {
                $('#divMarkWholeTable').css("dislay", "none");
                $('#divDialogue').css("display", "none");
                $('#divMarkTableHeadCap').css("dislay", "none");

                $('#divTableBodyDialogue').css("display", "none");
                $('#divTextEdit').css("display", "none");

            } //end main else
        }
        });


    function CloseMarkTableDialog() {
        $("#btnCloseTableDialog").dialog('close');
    }

    function CloseSaveImageDialog() {
        $("#btnCloseImageDialog").dialog('close');
    }

    var winW = $(window).width() - 800;
    var winH = $(window).height() - 80;

    function ShowMarkedTableDialog() {

        $('#divMarkTable').css("display", "block");
        $('#divTable').css("display", "block");

        var tableBodyRows = document.getElementById('<%= hfTableBodyRows.ClientID %>').value;
            $('#mytable').append(tableBodyRows);

            var tableColumnWidth = document.getElementById('<%= hfColumnWidths.ClientID %>').value;
            $('#tblAdjustColWidth').append(tableColumnWidth);

            var tbl = document.getElementById('mytable');
            $("#lblRowCount").html(tbl.rows.length);
            //$("#lblColumnCount").html(tbl.rows[0].cells.length);

            var max = 0;
            $('#mytable tr').each(function () { max = Math.max(max, $('td', this).length); });
            $("#lblColumnCount").html(max);

            $("#divMarkTable").dialog({
                appendTo: "#dialogAfterMarkTable",
                resizable: false,
                height: winH,
                width: winW,
                position:
                {
                    my: "right",
                    at: "right top",
                    of: window
                },
                //height: 440,
                //width: 700,

                //height: 'auto',
                //width: 'auto',
                //maxHeight: 450,
                //maxWidth: 720,
                modal: false,
                beforeClose: function (event, ui) {
                    //$("#btnGoTo").hide();
                    //$("#btnFinish_Task").hide();
                }
            });
        }

        function ShowSaveImageDialog() {

            $('#divUploadImage').css("display", "block");
            $('#divImage').css("display", "block");

            $("#divUploadImage").dialog({
                appendTo: "#dialogAfterSaveImage",
                resizable: false,
                height: 220,
                width: 340,
                modal: true,
                beforeClose: function (event, ui) {
                    //$("#btnGoTo").hide();
                    //$("#btnFinish_Task").hide();
                }
            });
        }

        //var winWidth = $(window).width() - 1000;
        //var winHeight = $(window).height() - 400;

        //function ShowTableEditDialog() {

        //    $('#divTableBodyDialogue').css("display", "block");
        //    $('#divTextEdit').css("display", "block");

        //    $("#divTableBodyDialogue").dialog({
        //        appendTo: "#dialogAfterEditTable",
        //        resizable: false,
        //        height: winHeight,
        //        width: winWidth,
        //        modal: false,
        //        beforeClose: function (event, ui) {
        //            //$("#btnGoTo").hide();
        //            //$("#btnFinish_Task").hide();
        //        }
        //    });

        //    // remove the title bar
        //    //$(".ui-dialog-titlebar").hide();
        //}

        function DisplaySourcePath() {

            var file = document.getElementById("<%=fuSourceFile.ClientID%>");

            var path = file.value;
            if (path != '' && path != null) {
                var q = path.substring(path.lastIndexOf('\\') + 1);
                var name = q.substring(0, q.lastIndexOf('.'));
                $('#tbxImagePath').val(name);
            }
        };

        function SetManualTableHeader() {

            document.getElementById('<%= hfSetManualTableHeader.ClientID %>').value = 'true';
        };

        function SetManualTableCaption() {

            document.getElementById('<%= hfSetManualTableCaption.ClientID %>').value = 'true';
        };


        //function var1() {
        //    document.getElementById('text').innerHTML = 'hi';
        //}

        //window.onload = UpdateTableBody;

        function UpdateTableBody() {

            var tableRows = $('#mytable').html();
            document.getElementById('<%= hfTableBodyRows.ClientID %>').value = tableRows;
        };

        function showHideHeaderSelection() {

            var rbtnlTableHeader = document.getElementById("<%=rbtnlTableHeader.ClientID%>");
            var radio = rbtnlTableHeader.getElementsByTagName("input");

            if (radio[0].checked || radio[1].checked || radio[2].checked) {
                $('#btnManualHeader').css("display", "none");
                document.getElementById('<%= hfSetManualTableHeader.ClientID %>').value = 'false';
            } else {
                $("#btnManualHeader").attr("disabled", false);
                $('#btnManualHeader').css("display", "inline-block");

                $('#btnManualHeader').css("position", "relative");
                $('#btnManualHeader').css("top", "-5px");
            }
        }

        function showHideCaptionSelection() {

            var rbtnlTableCaption = document.getElementById("<%=rbtnlTableCaption.ClientID%>");
            var radio = rbtnlTableCaption.getElementsByTagName("input");

            if (radio[0].checked || radio[1].checked || radio[2].checked) {
                $('#btnManualCaption').css("display", "none");
                document.getElementById('<%= hfSetManualTableCaption.ClientID %>').value = 'false';
            } else {
                $("#btnManualCaption").attr("disabled", false);
                $('#btnManualCaption').css("display", "inline-block");
                $('#btnManualCaption').css("position", "relative");
                $('#btnManualCaption').css("top", "-5px");
            }
        }

        $(function () {
            $("#btnManualHeader").click(function () {
                $("#btnManualHeader").attr("disabled", true);
            });
            $("#btnManualCaption").click(function () {
                $("#btnManualCaption").attr("disabled", true);
            });
        });

        $(document).ready(function () {

            var textLayer = $('#pdfContainer').find('.textLayer');

            textLayer.bind('load', function () {
                //$('#loader1').hide();
                //$('.textLayer').hide();
            });
        });

        <%--function DisplaySourcePath() {

            var file = document.getElementById("<%=fuSourceFile.ClientID%>");

                var path = file.value;
                if (path != '' && path != null) {
                    var q = path.substring(path.lastIndexOf('\\') + 1);
                    var name = q.substring(0, q.lastIndexOf('.'));
                    $('#tbxSourceFilePath').val(name);
                }
            };--%>

    </script>

</head>
<body>
    <form id="form1" runat="server" style="background-color: white">
        <asp:HiddenField ID="hfFileLoadPath" runat="server" Value="WebHandlers/ShowTableDetection.ashx" />
        <asp:HiddenField ID="hfTableSelectedLine" runat="server" />
        <asp:HiddenField ID="hfIsIgnoreAlgo" runat="server" />
        <asp:HiddenField ID="hfPopupPosX" runat="server" />
        <asp:HiddenField ID="hfPopupPosY" runat="server" />
        <asp:HiddenField ID="hfMarkedTableText" runat="server" />
        <asp:HiddenField ID="hfSetManualTableHeader" runat="server" />
        <asp:HiddenField ID="hfSetManualTableCaption" runat="server" />
        <asp:HiddenField ID="hfTableBodyRows" runat="server" />
        <asp:HiddenField ID="hfMergedRows" runat="server" />
        <asp:HiddenField ID="hfColumnWidths" runat="server" />

        <div runat="server" id="divPdfPanel">

            <div runat="server" id="divNavigationPanel" style="position: fixed; top: 0px; left: 43%; width: 15%; border-bottom-right-radius: 20px; 
                                                               padding: 6px; border-bottom-left-radius: 20px; color: White; 
                                                               background-color: lightsteelblue; height: 1px; overflow: hidden; padding-top: 6px;">
                <%--<div style="width: 70%; padding-top: 6px; padding-left: 15px; margin-left: auto; margin-right: auto;">
                    <p style="margin-bottom: 2%">ShortcutKeys:</p>
                    <p style="margin-bottom: 1%; margin-top: 0%;">Press ctrl + Right arrow for next page</p>
                    <p style="margin-bottom: 0%; margin-top: 1%;">Press ctrl + Left arrow for previous page</p>
                    <p style="margin-bottom: 0%; margin-top: 1%;">Press ctr + e for marking table</p>
                </div>--%>
                
                <div style="width: 70%; padding-top: 6px; margin-left: auto; margin-right: auto;">
                    
                    <div style="text-align: center;">
                    <asp:Button ID="btnPrevPage" Width="60px" runat="server" Text="<" OnClick="btnPrevPage_Click" />
                        
                    <asp:Button ID="btnNextPage" Width="60px" runat="server" Text=">" OnClick="btnNextPage_Click" />
                        </div>
                    <div style="text-align: center; color:black;">
                    <asp:Label runat="server" ID="lblPageNum" />&nbsp;/
                            <asp:Label ID="lblTotalPages" runat="server"></asp:Label>   
                    </div>
                </div>
            </div>

            <table cellspacing="4" style="width: 95%; margin-left: auto; margin-right: auto;">
                <tr>
                    <td align="center" colspan="4">
                        <div id="divSuccessMessage" class="success" style="display: none; margin-left: auto; margin-right: auto; 
                                                                                width: 40%; height: 40px; padding-top: 2%; border-radius:10px;">
                        </div>
                        <div id="divErrorMessage" class="error" style="display: none; margin-left: auto; margin-right: auto; width: 40%; 
height: 40px; padding-top: 2%; border-radius:10px;">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="top" class="auto-style1" valign="top">
                        <%--<asp:Button ID="btnPrevPage" Width="60px" runat="server" Text="<" OnClick="btnPrevPage_Click" />--%>
                        <div style="width:100%;">
                            <%-- <asp:LinkButton ID="lbtnHome" OnClick="lbtnHome_Click" runat="server" Text="Home" Style="display: inline-block; font-size: 18px; font-weight: normal; color: darkcyan"></asp:LinkButton>
                        <asp:LinkButton ID="lbtnLogOut" OnClick="lbtnLogOut_Click" runat="server" Style="display: inline-block; position: relative; margin-left: 0px; font-size: 18px; font-weight: normal; color: darkcyan">Log Out</asp:LinkButton>--%>

                        </div>
                    </td>

                    <td id="tdPrdPdf" align="left" style="width: 468px; height: 716px; min-height: 611px; vertical-align: top;">
                        <div id="pdfContainer">
                        </div>
                    </td>
                    <td align="left" style="width: 2%;">

                        <%--<asp:Button ID="btnNextPage" Width="60px" runat="server" Text=">" OnClick="btnNextPage_Click" />--%>

                    </td>
                    <td style="vertical-align: top; width: 26%;">

                        <table cellspacing="5" style="width: 100%; position: relative; margin-top: 1%; top: 0px; left: 0px; background-color: aliceblue; padding-top: 5%; padding-bottom: 5%; padding-right: 5%; padding-left: 7%; border-radius: 2%; border: 1px solid black;">
                           <tr>
                                <td align="left" style="width: 30%; vertical-align: central" colspan="2">
                                      <div style="width: 100%; padding-top: 6px; padding-left: 3%; padding-bottom:3%; font-size:95%;">
                    <p style="margin-bottom: 2%; font-weight:bold;">ShortcutKeys:</p>
                    <p style="margin-bottom: 1%; margin-top: 0%;">Press ctrl + Right arrow for next page</p>
                    <p style="margin-bottom: 0%; margin-top: 1%;">Press ctrl + Left arrow for previous page</p>
                    <p style="margin-bottom: 0%; margin-top: 1%;">Press ctr + e for marking table</p>
                    <p style="margin-bottom: 0%; margin-top: 1%;">Press ctr + q for merging table rows</p>
                </div>
                                </td>
                               
                            </tr>
                            <tr>
                                <td colspan="2" style="border: thin solid black;"></td>
                            </tr>
                             <tr>
                                <td align="center" style="width: 30%; vertical-align: central">
                                    <div style="width: 80%; height: 20px; background-color: lawngreen; margin-bottom: 5%;">
                                    </div>
                                    <div style="width: 80%; height: 20px; background-color: yellow; margin-bottom: 5%;">
                                    </div>
                                    <div style="width: 80%; height: 20px; background-color: deepskyblue; margin-bottom: 5%;">
                                    </div>
                                </td>
                                <td style="width: 70%; vertical-align: central; font-size:95%;">Table&nbsp;
                                    Body</td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border: thin solid black;"></td>
                            </tr>

                            <tr>
                                <td align="center" style="vertical-align: central">
                                    <div style="width: 80%; height: 20px; background-color: darkgrey;">
                                    </div>
                                </td>
                                <td style="font-size:95%;">Table Header
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="vertical-align: central">
                                    <div style="width: 80%; height: 20px; background-color: #d6d6d6;" >
                                    </div>
                                </td>
                                <td style="font-size:95%;">Table Header Row
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="vertical-align: central">
                                    <div style="width: 80%; height: 20px; background-color: pink;">
                                    </div>
                                </td>
                                <td style="font-size:95%;">Table Caption
                                </td>
                            </tr>
                        </table>
                        
                        <div style="position: absolute;bottom: 0; width:100%; margin-left:auto; margin-right:auto;">
                         <asp:Button ID="btnPrevTable" Width="120px" runat="server" Text="Previous Table" OnClick="btnPrevTable_Click" Visible="False"
                             Style="margin-left: -2%; border-radius:6px;height:30px;"/>
                        <asp:Button ID="btnNextTable" Width="100px" runat="server" Text="Next Table" OnClick="btnNextTable_Click" Visible="False"
                            Style="display: inline-block; margin-left: 0.1%; border-radius:6px;height:30px;"/>
                            <asp:Label ID="lblCurrentTable" runat="server" Style="margin-left: 2%;"></asp:Label>
                            <%--&nbsp;/--%>
                            <asp:Label ID="lblTotalTables" runat="server"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <div style="width:100%;">
                             <asp:LinkButton ID="lbtnHome" OnClick="lbtnHome_Click" runat="server" Text="Home" Style="display: inline-block; font-size: 18px; font-weight: normal; color: darkcyan"></asp:LinkButton>
                        <asp:LinkButton ID="lbtnLogOut" OnClick="lbtnLogOut_Click" runat="server" Style="display: inline-block; position: relative; margin-left: 0px; font-size: 18px; font-weight: normal; color: darkcyan">Log Out</asp:LinkButton>

                        </div>
                    </td>
                    <td style="width: 30%;">
                        <div style="margin-left:22%;">
                            <asp:CheckBox ID="cbxIgnoreAlgo" runat="server" Checked="False" Text="Ignore Algo Mapped" OnCheckedChanged="cbxIgnoreAlgo_CheckedChanged" AutoPostBack="True" />
                            <asp:Button ID="btnFinishTask" runat="server" Text="Finish Task" Visible="False" Style="display: inline-block; margin-left: 6%; width:20%;" OnClick="btnFinishTask_Click" />
                        </div>
                    </td>
                    <td></td>
                    <td>
                       <%-- <asp:Button ID="btnPrevTable" Width="120px" runat="server" Text="Previous Table" OnClick="btnNextPage_Click" />
                        <asp:Button ID="btnNextTable" Width="100px" runat="server" Text="Next Table" OnClick="btnNextPage_Click" Style="display: inline-block; margin-left: 6%;"/>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        
                         <%--<div style="width:100%;">
                             <asp:LinkButton ID="lbtnHome" OnClick="lbtnHome_Click" runat="server" Text="Home" Style="display: inline-block; font-size: 18px; font-weight: normal; color: darkcyan"></asp:LinkButton>
                        <asp:LinkButton ID="lbtnLogOut" OnClick="lbtnLogOut_Click" runat="server" Style="display: inline-block; position: relative; margin-left: 0px; font-size: 18px; font-weight: normal; color: darkcyan">Log Out</asp:LinkButton>

                        </div>--%>

                    </td>
                    <td style="width: 46%;" align="center"></td>
                    <td></td>
                </tr>
            </table>

        </div>

        <div id="divDialogue" style="display: none; border: 4px solid #D6D6D6; position: absolute; left: 150px; top: 250px; border-top-right-radius: 20px; border-bottom-right-radius: 20px; padding: 6px; border-bottom-left-radius: 20px; background-color: #F0F0F0;">
            <div id='divMarkWholeTable'>
                <asp:Button ID="btnMarkTableLines" runat="server" Text="Mark Table" OnClientClick="MarkTableInXml();return true;"
                    OnClick="btnMarkTableLines_Click" Style="margin-left: 10px;" />
                <asp:Button ID="btnMarkImage" runat="server" Text="Mark Image" OnClientClick="MarkTableAsImageInXml();return true;" OnClick="btnMarkImage_Click" />
            </div>
            <div id='divMarkTableHeadCap' style="display: none;">
                <%--<asp:Button ID="btnCancelSelection" runat="server" Text="Cancel" Style="margin-left: 10px;" />--%>
                <asp:Button ID="btnMarkTableHeader" runat="server" Text="Mark Table Header" OnClientClick="MarkTableHeaderCapInXml('tableHeader');return false;"
                    OnClick="btnMarkTableHeader_Click" Style="margin-left: 10px;" ClientIDMode="Static"/>
                <asp:Button ID="btnMarkTableCaption" runat="server" Text="Mark Table Caption" OnClientClick="MarkTableHeaderCapInXml('tableCaption');return false;"
                    OnClick="btnMarkTableCaption_Click" ClientIDMode="Static"/>
            </div>
        </div>

        <div id="divMarkTable" title="Selected Table" style="margin-left: auto; margin-right: auto; display: none; padding: 5px; z-index: 10">
            <div id="divTable" style="padding-left: 5%; padding-right: 5%; padding-top: 1%; padding-bottom: 8%; display: none; font-size: 72%;">
                <div>
                    <fieldset id="fsBorder" runat="server">
                    <table style="width:100%; margin-bottom:1%;">
                        <tr>
                            <td align="center">
                                <label>Border :</label>
                                <asp:RadioButtonList ID="rbtnlTableBorder" runat="server" RepeatDirection="Horizontal"
                        Style="display: inline-block; position: relative; top: 7px; left: 4%;">
                        <asp:ListItem>On</asp:ListItem>
                        <asp:ListItem>Off</asp:ListItem>
                    </asp:RadioButtonList>
                            </td>
                            <td align="center">
                                <label>Head-Row :</label>
                                <asp:RadioButtonList ID="rbtnlHeaderRow" runat="server" RepeatDirection="Horizontal" 
                                    Style="display: inline-block; position: relative; top: 7px;">
                        <asp:ListItem>On</asp:ListItem>
                        <asp:ListItem>Off</asp:ListItem>
                    </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                       <%--  </fieldset>
                    <fieldset id="fsSize" runat="server">
                        <legend>Size</legend>--%>
                        <div id="divSize" style="margin-left: auto; margin-right: auto; width: 100%; padding-left: 3%; padding-top: 1%; padding-bottom: 2%; font-size: 90%;">
                            <div style="width: 100%; display: inline;">
                                <label id="lblRow">Rows : </label>
                                <label id="lblRowCount"></label>
                                <input type="button" id="ibtnAddRowBefore" style="margin-left: 0.4%; display: inline-block; width: 16%; background-color: #ADDFFF;" value="Add Before" onclick="appendRows('previous');" />
                                <input type="button" id="ibtnAddRowAfter" style="margin-left: 0.4%; display: inline-block; width: 16%; background-color: #ADDFFF;" value="Add After" onclick="appendRows('next');" />
                                <input type="button" id="ibtnDeleteRowBefore" style="margin-left: 0.4%; display: inline-block; width: 20%; background-color: #ADDFFF;" value="Delete Before" onclick="deleteRows('previous');" />
                                <input type="button" id="ibtnDeleteRowAfter" style="margin-left: 0.4%; display: inline-block; width: 20%; background-color: #ADDFFF;" value="Delete After" onclick="    deleteRows('next');" />
                                <input type="button" id="ibtnMergeRow" style="margin-left: 0.4%; display: none; width: 25%; background-color: #ADDFFF;" value="Merge Rows" onclick="    mergeRows();" />
                            </div>
                            <div style="width: 100%; margin-top: 3%;">

                                <label id="lblColumn">Columns :</label>
                                <label id="lblColumnCount"></label>
                                <input type="button" id="ibtnAddColumnBefore" style="margin-left: 0.4%; display: inline-block; width: 16%; background-color: #ADDFFF;" value="Add Before" onclick="appendColumns('previous');" />
                                <input type="button" id="ibtnAddColumnAfter" style="margin-left: 0.4%; display: inline-block; width: 16%; background-color: #ADDFFF;" value="Add After" onclick="appendColumns('next');" />
                                <input type="button" id="ibtnDeleteColumnBefore" style="margin-left: 0.4%; display: inline-block; width: 20%; background-color: #ADDFFF; border-radius: 3px;" value="Delete Before" onclick="deleteColumns('previous');" />
                                <input type="button" id="ibtnDeleteColumnAfter" style="margin-left: 0.4%; display: inline-block; width: 20%; background-color: #ADDFFF; border-radius: 3px;" value="Delete After" onclick="deleteColumns('next');" />
                            </div>
                        </div>
                        <div style="margin-left: auto; margin-right: auto; width: 100%; padding-left: 3%; padding-top: 1%; padding-bottom: 2%; font-size: 90%;">
                            <table id="tblAdjustColWidth" style="width: 94%;">
                            </table>
                            <input type="button" id="ibtnUpdateColWidth" onclick="updateColunmWidth();"
                                style="margin-left: 0.4%; margin-top:1%; display: inline-block; width: 22%; background-color: #ADDFFF;" value="Update Width" />
                        </div>
                    </fieldset>
                     <fieldset id="fsHeaderRow" runat="server" style="margin-top:1%">
                        <legend>
                            <%--<label style="float: left;">Header Row</label>--%>
                            <asp:RadioButtonList ID="rbtnlTableHeader" runat="server" RepeatDirection="Horizontal" onchange="return showHideHeaderSelection();"
                                Style="display: inline-block; position: relative; top: 0px;">
                                <%--<asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                                <asp:ListItem>Para</asp:ListItem>
                                <asp:ListItem>Table Body</asp:ListItem>--%>
                                
                                <asp:ListItem>Para</asp:ListItem>
                                <asp:ListItem>Table Body</asp:ListItem>
                                <asp:ListItem>Header Row</asp:ListItem>
                                <asp:ListItem>Not Header Row</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:Button ID="btnManualHeader" runat="server" OnClientClick="SetManualTableHeader(); return false;" Text="Select Manually"
                                Style="width: 130px; position: relative; display: none; bottom: 4px;" />
                        <%--  <asp:RadioButton runat="server" ID="rbtnHeaderAsPara" Text="Para" Style="position: relative; bottom: 4px;"/>
                          <asp:RadioButton runat="server" ID="rbtnHeaderAsTblBody" Text="Table Body" Style="position: relative; bottom: 4px;"/>--%>
                        </legend>
                        <div id="divHeaderRow" contenteditable="true" runat="server" style="font-size: 90%; margin-top: 2%; margin-bottom: 2%; background-color: #f0f0f0; overflow: auto;">
                        </div>
                        <div>
                        </div>
                    </fieldset>
                    <fieldset style="margin-top:1%">
                        <legend>Body
                        </legend>
                       
                        <div style="overflow: auto; font-size: 90%;">
                            <table id="mytable" contenteditable="true" style="width: 100%;" class="rowSelection">
                            </table>
                        </div>
                    </fieldset>
                    <fieldset style="margin-top:1%">
                        <legend>
                            <%--<label style="float: left;">Caption Row</label>--%>
                            <asp:RadioButtonList ID="rbtnlTableCaption" runat="server" RepeatDirection="Horizontal" onchange="return showHideCaptionSelection();"
                                Style="display: inline-block; position: relative; top: 0px;">
                               <%-- <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                                <asp:ListItem>Para</asp:ListItem>
                                <asp:ListItem>Table Body</asp:ListItem>--%>
                                
                                 <asp:ListItem>Para</asp:ListItem>
                                <asp:ListItem>Table Body</asp:ListItem>
                                <asp:ListItem>Caption Row</asp:ListItem>
                                <asp:ListItem>Not Caption Row</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:Button ID="btnManualCaption" runat="server" OnClientClick="SetManualTableCaption(); return false;" Text="Select Manually"
                                Style="width: 130px; position: relative; display: none; bottom: 4px;" />
                            <%--<asp:RadioButton runat="server" ID="rbnCapAsPara" Text="Para" Style="position: relative; bottom: 4px;"/>
                          <asp:RadioButton runat="server" ID="rbnCapAsTblBody" Text="Table Body" Style="position: relative; bottom: 4px;"/>--%>
                        </legend>
                        <div id="divCaptionRow" contenteditable="true" runat="server" 
                            style="font-size: 90%; margin-top: 2%; margin-bottom: 2%; background-color: #f0f0f0; overflow: auto;">
                        </div>
                        <div>
                        </div>
                    </fieldset>
                </div>
                <br />
                <div style="float: right;">
                    <asp:Button ID="btnSaveTable" runat="server" Text="Save Table" OnClientClick="UpdateTableBody();" OnClick="btnSaveTable_Click" Width="130px" />
                    <asp:Button ID="btnCloseTableDialog" runat="server" OnClientClick="CloseMarkTableDialog(); return false;" Text="Close" Width="100px" />
                </div>
            </div>
        </div>

        <div id="divUploadImage" title="Save Image" style="width: 60%; height: 90px; margin-left: auto; margin-right: auto; display: block; padding: 5px; display: none;">
            <div id="divImage" style="width: 90%; padding-left: 5%; padding-right: 5%; padding-top: 3%; padding-bottom: 3%; display: none;">
                <label>Upload Image :</label>
                <asp:TextBox ID="tbxImagePath" runat="server" Width="289px" Style="display: inline-block;"></asp:TextBox>
                <asp:FileUpload ID="fuSourceFile" AllowMultiple="true" runat="server" Width="120px" onchange="DisplaySourcePath();" Style="display: inline-block;" />
                <br />
                <br />
                <div style="width: 98%; margin-left: auto; margin-right: auto;">
                    <div style="float: right;">
                        <asp:Button ID="btnSaveImage" runat="server" Text="Save Image" OnClick="btnSaveImage_Click" Width="120px" />
                        <asp:Button ID="btnCloseImageDialog" OnClientClick="CloseSaveImageDialog(); return false;" runat="server" Text="Close" Width="84px" />
                    </div>
                </div>
            </div>
        </div>

        <div id="dialogAfterMarkTable">
        </div>
        <div id="dialogAfterSaveImage">
        </div>
    </form>
</body>
</html>

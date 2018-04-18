// Minimal PDF rendering and text-selection example using PDF.js by Vivin Suresh Paliath (http://vivin.net)
// This example uses a built version of PDF.js that contains all modules that it requires.
//
// The problem with understanding text selection was that the text selection code has heavily intertwined
// with viewer.html and viewer.js. I have extracted the parts I need out of viewer.js into a separate file
// which contains the bare minimum required to implement text selection. The key component is TextLayerBuilder,
// which is the object that handles the creation of text-selection divs. I have added this code as an external
// resource.
//
// This demo uses a PDF that only has one page. You can render other pages if you wish, but the focus here is
// just to show you how you can render a PDF with text selection. Hence the code only loads up one page.
//
// The CSS used here is also very important since it sets up the CSS for the text layer divs overlays that
// you actually end up selecting.
//
// NOTE: The original example was changed to remove jQuery usage, re-structure and add more comments.

window.onload = function () {
    if (typeof PDFJS === 'undefined') {
        alert('Built version of pdf.js is not found\nPlease run `node make generic`');
        return;
    }

    //var scale = 1.08; //Set this to whatever you want. This is basically the "zoom" factor for the PDF.
    var scale = 1.08; //Set this to whatever you want. This is basically the "zoom" factor for the PDF.
    PDFJS.workerSrc = 'FinalScripts/ParaSelectionPdfJs/pdf.worker.js'; /// <reference path="pdf.worker.js" />
    /// <reference path="pdf.worker.js" />

    //  PDFJS.workerSrc = '../../../build/pdf.worker.js';
    //  var PDFURL = document.getElementById('FileLoadPath').value;

    function loadPdf() {

        //alert("parasel");
        var pdfPath = document.getElementById('hfParaSelFileLoadPath').value;
        var pdf = PDFJS.getDocument(pdfPath);
        return pdf.then(renderPdf);
    }

    function renderPdf(pdf) {
        return pdf.getPage(1).then(renderPage);
    }

    function setDivDimensions(viewport) {

        var width = viewport.width;
        var height = viewport.height;

        //alert('11');

        //alert('width= ' + width);
        //alert('height= ' + height);

        document.getElementById('tdPrdPdf').style.width = width + 'px';
        document.getElementById('tdPrdPdf').style.height = height + 'px';
        document.getElementById('pdfContainer').style.width = width + 'px';
        document.getElementById('pdfContainer').style.height = height + 'px';


        ////////$('#loader1').hide();
    }

    function renderPage(page) {
        var viewport = page.getViewport(scale);
//        document.getElementById('hfPdfDimensions').value = viewport.width + ',' + viewport.height;
        setDivDimensions(viewport);
//        alert(viewport.width + ', ' + viewport.height);
        // Create and append the 'pdf-page' div to the pdf container.
        var pdfPage = document.createElement('div');
        pdfPage.className = 'pdfPage';
        var pdfContainer = document.getElementById('pdfContainer');
        pdfContainer.appendChild(pdfPage);

        // Set the canvas height and width to the height and width of the viewport.
        var canvas = document.createElement('canvas');
        var context = canvas.getContext('2d');

        //alert('width = '+viewport.width);
        //alert('height = ' + viewport.height);

        // The following few lines of code set up scaling on the context, if we are
        // on a HiDPI display.
        var outputScale = getOutputScale(context);
        canvas.width = (Math.floor(viewport.width) * outputScale.sx) | 0;
        canvas.height = (Math.floor(viewport.height) * outputScale.sy) | 0;
        context._scaleX = outputScale.sx;
        context._scaleY = outputScale.sy;
        if (outputScale.scaled) {
            context.scale(outputScale.sx, outputScale.sy);
        }

        // The page, canvas and text layer elements will have the same size.
        canvas.style.width = Math.floor(viewport.width) + 'px';
        canvas.style.height = Math.floor(viewport.height) + 'px';

        pdfPage.style.width = canvas.style.width;
        pdfPage.style.height = canvas.style.height;
        pdfPage.appendChild(canvas);

        var textLayerDiv = document.createElement('div');
        textLayerDiv.className = 'textLayer';
        textLayerDiv.style.width = canvas.style.width;
        textLayerDiv.style.height = canvas.style.height;
        pdfPage.appendChild(textLayerDiv);

        // Painting the canvas...
        var renderContext = {
            canvasContext: context,
            viewport: viewport
        };
        var renderTask = page.render(renderContext);

        // ... and at the same time, getting the text and creating the text layer.
        var textLayerPromise = page.getTextContent().then(function (textContent) {
            var textLayerBuilder = new TextLayerBuilder({
                textLayerDiv: textLayerDiv,
                viewport: viewport,
                pageIndex: 0
            });
            textLayerBuilder.setTextContent(textContent);
        });

        // We might be interested when rendering complete and text layer is built.
        return Promise.all([renderTask.promise, textLayerPromise]);
    }
    /// <reference path="../pdf/Pages from 12826.pdf" />
    /// <reference path="../Pages from 12826.pdf" />
    /// <reference path="" />/// <reference path="../1.pdf" />

    loadPdf();
};

//function GetPdf() {
//    $.ajax({
//        type: "POST",
//        url: "GetPdf.ashx",
//        contentType: "application/json; charset=utf-8",
//        success: function (respose) {

//        }
//    });
//}
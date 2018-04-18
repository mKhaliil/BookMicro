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


//function GetXMLFromPDFJs(pdfPath) {

//    alert('cccc');
//    PDFJS.workerSrc = "/PDFJS_SVG/pdf.worker.js";

//    //var pdfPath = document.getElementById('hfSvgContentPath').value;

//    if (pdfPath != '' || pdfPath != null) {

//        PDFJS.getDocument(pdfPath)
//            .then(function(pdf) {

//                // Get div#container and cache it for later use
//                var container = document.getElementById("the-svg");

//                // Loop from 1 to total_number_of_pages in PDF document
//                for (var i = 1; i <= pdf.numPages; i++) {

//                    // Get desired page
//                    pdf.getPage(i).then(function(page) {

//                        var scale = 1.5;
//                        var viewport = page.getViewport(scale);

//                        // Set dimensions
//                        //container.style.width = viewport.width + 'px';
//                        //container.style.height = viewport.height + 'px';

//                        var div = document.createElement("div");

//                        // Set id attribute with page-#{pdf_page_number} format
//                        div.setAttribute("id", "page-" + (page.pageIndex + 1));

//                        // This will keep positions of child elements as per our needs
//                        div.setAttribute("style", "position: relative");

//                        // SVG rendering by PDF.js
//                        page.getOperatorList()
//                            .then(function(opList) {
//                                var svgGfx = new PDFJS.SVGGraphics(page.commonObjs, page.objs);
//                                return svgGfx.getSVG(opList, viewport);
//                            })
//                            .then(function(svg) {

//                                div.appendChild(svg);
//                                container.appendChild(div);

//                                document.getElementById('hfSvgContent').value = $('#the-svg')[0].outerHTML;

//                                //if (i == pdf.numPages) {

//                                //    alert(i);
//                                //    alert($('#the-svg')[0].outerHTML);
//                                //    document.getElementById('hfSvgContent').value = $('#the-svg')[0].outerHTML;
//                                //}
//                            });
//                    });
//                }
//            });
//    }

//    alert('ddddd');
//};


window.onload = function () {
    if (typeof PDFJS === 'undefined') {
        alert('Built version of pdf.js is not found\nPlease run `node make generic`');
        return;
    }

    PDFJS.workerSrc = "/PDFJS_SVG/pdf.worker.js";

    var pdfPath = document.getElementById('hfSvgContentPath').value;

    if (pdfPath != '' || pdfPath != null) {

        PDFJS.getDocument(pdfPath)
            .then(function (pdf) {

                // Get div#container and cache it for later use
                var container = document.getElementById("the-svg");

                // Loop from 1 to total_number_of_pages in PDF document
                for (var i = 1; i <= pdf.numPages; i++) {

                    // Get desired page
                    pdf.getPage(i).then(function (page) {

                        var scale = 1.5;
                        var viewport = page.getViewport(scale);

                        // Set dimensions
                        //container.style.width = viewport.width + 'px';
                        //container.style.height = viewport.height + 'px';

                        var div = document.createElement("div");

                        // Set id attribute with page-#{pdf_page_number} format
                        div.setAttribute("page", (page.pageIndex + 1));

                        // This will keep positions of child elements as per our needs
                        div.setAttribute("style", "position: relative");

                        // SVG rendering by PDF.js
                        page.getOperatorList()
                            .then(function (opList) {
                                var svgGfx = new PDFJS.SVGGraphics(page.commonObjs, page.objs);
                                return svgGfx.getSVG(opList, viewport);
                            })
                            .then(function (svg) {

                                div.appendChild(svg);
                                container.appendChild(div);

                                document.getElementById('hfSvgContent').value = $('#the-svg')[0].outerHTML;

                                //if (i == pdf.numPages) {

                                //    alert(i);
                                //    alert($('#the-svg')[0].outerHTML);
                                //    document.getElementById('hfSvgContent').value = $('#the-svg')[0].outerHTML;
                                //}
                            });
                    });
                }
            });
    }

    //alert('all loaded');
    //SaveSvgXml();
};
//      Pop Easy | jQuery Modal Plugin
//      Version 1.0
//      Created 2013 by Thomas Grauer
///////////////////////////////////////////////////////////////////////////////////////
//      Permission is hereby granted, free of charge, to any person obtaining
//      a copy of this software and associated documentation files (the
//      "Software"), to deal in the Software without restriction, including
//      without limitation the rights to use, copy, modify, merge, publish,
//      distribute, sublicense, and/or sell copies of the Software, and to
//      permit persons to whom the Software is furnished to do so, subject to
//      the following conditions:
//
//      The above copyright notice and this permission notice shall be
//      included in all copies or substantial portions of the Software.
//
//      THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//      EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//      MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//      NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//      LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
//      OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//      WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////

(function($){

    $.fn.modal= function(options){
       
        options = $.extend({
/*
            trigger: '.modalLink',
            olay: 'div.overlay',
            modals: 'div.modal',
            animationEffect: 'fadeIn',
            animationSpeed: 400,
            moveModalSpeed: 'slow',
            background: '000',
            opacity: 0.8,
            openOnLoad: false,
            docClose: true,
            closeByEscape: true,
            moveOnScroll: true,
            resizeWindow: true,
            video:'',
            videoClass:'video',
            close:'.closeBtn'
*/
        trigger: '.modalLink',          // id or class of link or button to trigger modal
        olay:'div.overlay',             // id or class of overlay
        modals:'div.modal',             // id or class of modal
        animationEffect: 'slideDown',   // overlay effect | slideDown or fadeIn | default=fadeIn
        animationSpeed: 400,            // speed of overlay in milliseconds | default=400
        moveModalSpeed: 'slow',         // speed of modal movement when window is resized | slow or fast | default=false
        background: 'a2d3cd',           // hexidecimal color code - DONT USE #
        opacity: 0.7,                   // opacity of modal |  0 - 1 | default = 0.8
        openOnLoad: false,              // open modal on page load | true or false | default=false
        docClose: true,                 // click document to close | true or false | default=true    
        closeByEscape: true,            // close modal by escape key | true or false | default=true
        moveOnScroll: true,             // move modal when window is scrolled | true or false | default=false
        resizeWindow: true,             // move modal when window is resized | true or false | default=false
        video: 'http://player.vimeo.com/video/2355334?color=eb5a3d',    // enter the url of the video
        videoClass:'video',             // class of video element(s)
        close:'.closeBtn'               // id or class of close button			
            
        },options);
       
        var olay = $(options.olay);
        var modals = $(options.modals);
        var currentModal;
        var isopen=false;
       
        if (options.animationEffect==='fadein'){options.animationEffect = 'fadeIn';}
        if (options.animationEffect==='slidedown'){options.animationEffect = 'slideDown';}
        
        olay.css({opacity : 0});
                
        if(options.openOnLoad) {
            openModal();
        }else{
            olay.hide();
            modals.hide();
        }
        
        $(options.trigger).bind('click', function(e){
            e.preventDefault();
            
            if ($('.modalLink').length >1) {
                getModal = $(this).attr('href');
                currentModal = $(getModal);    
            }else{
                currentModal = $('.modal');
            }
            openModal();

            //aamir model

            //alert('cbx state = ' + $('#ctl00_mainBodyContents_cbxRememberMe').checked);
            //if ($('#ctl00_mainBodyContents_cbxRememberMe').checked) {
            //}
            //else {
            //    $('#ctl00_mainBodyContents_tbxPassword').val('');
            //}

           

        });
        
        function openModal(){
            $('.' + options.videoClass).attr('src',options.video);
            modals.hide();
            currentModal.css({
                top:$(window).height() /2 - currentModal.outerHeight() /2 + $(window).scrollTop(),
                left:$(window).width() /2 - currentModal.outerWidth() /2 + $(window).scrollLeft()
            });
                
            if(isopen===false){
                olay.css({opacity : options.opacity, backgroundColor: '#'+options.background});
                olay[options.animationEffect](options.animationSpeed);
                currentModal.delay(options.animationSpeed)[options.animationEffect](options.animationSpeed); 
            }else{
                currentModal.show();
            }
            
            isopen=true;
        }
        
        function moveModal(){
            modals
            .stop(true)
            .animate({
            top:$(window).height() /2 - modals.outerHeight() /2 + $(window).scrollTop(),
            left:$(window).width() /2 - modals.outerWidth() /2 + $(window).scrollLeft()
            },options.moveModalSpeed);
        }
        
        function closeModal(){
            $('.' + options.videoClass).attr('src',''); 
            isopen=false;
            modals.fadeOut(100, function(){
                if (options.animationEffect === 'slideDown') {
                    olay.slideUp();
                }else if (options.animationEffect === 'fadeIn') {
                    olay.fadeOut();
                }
            });
            return false;
        }
        
        if(options.docClose){
            olay.bind('click', closeModal);
        }
        
        $(options.close).bind('click', closeModal);
        
        if (options.closeByEscape) {
            $(window).bind('keyup', function(e){
                if(e.which === 27){
                    closeModal();
                }
            });
        }
        
        if (options.resizeWindow) {
            $(window).bind('resize', moveModal);
        }else{
            return false;
        }
        
        if (options.moveOnScroll) {
            $(window).bind('scroll', moveModal);
        }else{
            return false;
        }
    };
})(jQuery);

/**
 * Copyright (c) 2014 Mark Li
 * http://git.markli.net/
 * Code licensed under  the terms of the MIT License:
 * http://git.markli.net/license.txt
 * 
 * @version 1.0
 * @description Magnifier Effect, based on jQuery
 */
(function (window, document, $) {
	"use strict";
	$.fn.mkMagnifier = function(cfg) {
		var $config={width:300,height:460,ratio:1.65,magnifier_radius:90};
		$.extend($config,cfg);
		var $pos_x_ratio = $config.ratio-2*$config.magnifier_radius/$config.width,
			$pos_y_ratio = $config.ratio-2*$config.magnifier_radius/$config.height;
		var hasTouch = 'ontouchstart' in window;
		this.each(function () {
			var elem = $(this);
			var magnifier	= $('<div class="magnifier" style="background-image:url(\''+elem.attr("href")+'\')"></div>');
			elem.append(magnifier);
			if(!hasTouch){ /* mouse event */
				if($.browser.webkit){
					magnifier.addClass('cursor_compatible');
				}
				elem.mousemove(function(e){
					var left = e.pageX - elem.offset().left,
						top = e.pageY - elem.offset().top;					
					if(left<0 || top<0 || left > $config.width || top > $config.height){
						/*	If the cursor out of the range, hide the magnifier */
						magnifier.stop(true,true).fadeOut('fast');
						return false;
					}
					var pos_x = Math.round($pos_x_ratio*left),
						pos_y = Math.round($pos_y_ratio*top);
					magnifier.css({
						left				: left - $config.magnifier_radius,
						top					: top - $config.magnifier_radius,
						backgroundPosition	: '-'+pos_x+'px -'+pos_y+'px'
					});
				}).mouseenter(function(){
					magnifier.stop(true,true).fadeIn('fast');
				});	
			}
			else{ /* Touchscreen event */
				elem[0].addEventListener('touchstart', function(e) {
					e.preventDefault();
					var left = e.touches[0].pageX - elem.offset().left,
						top = e.touches[0].pageY - elem.offset().top;
					var pos_x = Math.round($pos_x_ratio*left),
						pos_y = Math.round($pos_y_ratio*top);
					magnifier.css({
						left				: left - $config.magnifier_radius,
						top					: top - $config.magnifier_radius,
						backgroundPosition	: '-'+pos_x+'px -'+pos_y+'px'
					});	
					magnifier.stop(true,true).fadeIn('fast');
				});

				elem[0].addEventListener('touchmove', function(e) {
					e.preventDefault();
					var left = e.touches[0].pageX - elem.offset().left,
						top = e.touches[0].pageY - elem.offset().top;
					if(left<0 || top<0 || left > $config.width || top > $config.height){
					/*	If the finger out of the range, hide the magnifier */
						magnifier.stop(true,true).fadeOut('fast');
						return false;
					}
					var pos_x = Math.round($pos_x_ratio*left),
						pos_y = Math.round($pos_y_ratio*top);
					magnifier.css({
						left				: left - $config.magnifier_radius,
						top					: top - $config.magnifier_radius,
						backgroundPosition	: '-'+pos_x+'px -'+pos_y+'px'
					});
				});
				elem[0].addEventListener('click',function(e){
					e.preventDefault();
				});
			}
		});
	};
}(window, document, jQuery));
/****************************************/
/* jQuery Custom Select Plugin			*/
/* 										*/
/* Author: 	David Dexter				*/
/* Version: 0.01						*/
/* Date: 	12/28/2006					*/
/* Email: 	blemming [at] gmail.com	 	*/
/* 										*/
/****************************************/

jQuery.fn.customSelect1 = function(){
	CS = jQuery('#'+jQuery(this).id());
	init1();
}

CS = 			new Object();
CS1_bodyClick 	= Array();
CS1_option 		= '';

function init1(obj){
	var sel = 	'';
	// CREATE THE HTML FOR THE NEW "SELECT BOX" 
		var tmp = 	'<div class="CS1_select" id="CS1_'+CS.id()+'">'+
					'<a id="curr_'+CS.id()+'" class="CS1_current1" onClick="CS1_toggle(this.parentNode);event.cancelBubble=true;">'+
					'&nbsp;</a>'+
					'<div class="CS1_hidden">';
	// LOOP THROUGH ALL OF THE NESTED DIV TAGS TO 
	// CREATE THE FAUX OPTION TAGS
		var i = 0;
		jQuery('#'+CS.id()+' div').each(function(){
													// STORE THE FIRST VALUE TO PUT IN THE 
													// HIDDEN TEXT FIELD FOR THE CUSTOM SELECT BOX
														if(i==0){
															val = jQuery(this).attr('val');
															i++;
														}

													// SET AN ATTRIBUT OF "SEL" = TRUE IN ORDER 
													// TO DEFAULT TO A GIVEN OPTION
														if(jQuery(this).attr('sel') == 'true'){
															sel 	= 	jQuery(this).html();
															sel_val = 	jQuery(this).attr('val');
														}
													// CREATE OPTIONS
														tmp += '<a vl="'+jQuery(this).attr('val')+'" href="#" class="choice_'+CS.id()+'" onclick="CS1_toggle(document.getElementById(\'CS1_'+CS.id()+'\'));event.cancelBubble=true;">'+jQuery(this).html()+'</a>';
												});
	
		tmp +=		'<input type="hidden" value="'+val+'" name="'+CS.id()+'" id="val_'+CS.id()+'"><img hspace="2" vspace="2" src="images/icon0.png" onClick="CS1_toggle(document.getElementById(\'CS1_'+CS.id()+'\'));event.cancelBubble=true;" /></div></div>';
	// REPLACE THE EXISTING DIVS WITH THE NEW 
	// CUSTOM SELECT BOX
		CS.html(tmp);

	// IF THERE IS A DEFAULT OPTION SELECT IT NOW
		if(sel != ''){
			jQuery('#curr_'+CS.id()).html(sel);
			jQuery('#val_'+CS.id()).val(sel_val);
		}
	
	// CREATE THE ON CLICK EVENTS FOR THE CUSTOM OPTION TAGS
		CS1_select(CS.id());
	
	// CREATE THE EVENT THAT CLOSES ANY OPEN BOXES WHEN THE BODY IS CLICKED
		CS1_bodyOnclick(CS1_close); 
}

// TOGGLE THE SELECT BOX OPEN OR CLOSED
	function CS1_toggle(obj){
		if(CS1_option != ''){
			jQuery('#'+CS1_option).next().addClass('CS1_hidden');
			CS1_option = '';
		}
		var h = jQuery('#'+obj.id).find('div');
		choices = h.attr("class");
		if (choices == 'CS1_hidden'){
			CS1_option = obj.id;
			CS1_close(); 
			h.addClass('CS1_options');
		}else{
			h.removeClass('CS1_options');
			CS1_option = '';
		}
	}

// CREATE THE ON CLICK FOR THE OPTIONS 
	function CS1_select(obj){
		jQuery('.choice_'+obj).click(function(){
								jQuery('#val_'+obj).val(jQuery(this).attr("vl"));
								jQuery('#curr_'+obj).html(jQuery(this).html());
							});
	}

// CLOSE ALL OPEN CUSTOM SELECT BOXES
	function CS1_close(){
		jQuery('div').removeClass('CS1_options');
	}
	function CS1_bodyOnclick(fnc){
		document.onclick = CS1_bodyOnClick;
		CS1_bodyClick[CS1_bodyClick.length] = fnc;
	}
	function CS1_bodyOnClick(){
		for (var i=0;i<CS1_bodyClick.length;i++){
			CS1_bodyClick[i]();
		}
	}

/*************** Begin Global Variables **************/
//alert('Loading Global Variables');
/* Used to aid in handeling enter key */
var enterKeyPressed = false;
var oButtonArray = new Array();
var clientMSG = '';
try{
	var helpPopup = window.createPopup();
}catch(e){
	//alert('Error with window.createPopup()' + e.message);
}

/******** Used for swapping div's or tables for the tab strip control *********/
var panels = new Array();

try{
	panels = document.getElementsByTagName("div");
}catch(e){
	//alert(e);
}

/******** Used for swapping div's or tables for the tab strip control *********/

window.onload=function(){ // needed for the tab strips
	//window.history.forward(0);
	try{
		try{
		    var selectedTab = getCookie('selectedTab');
		    if(selectedTab==null){
    			showFirstPanel();
		    }else{
		        showPanel(selectedTab);
		    }
		}catch(e){
			//do nothing
		}
		try{
			if (!NiftyCheck()){
			    // do nothing
				//return;
			}else{
			    try{ // for default UI
			        RoundedTop("div#roundedCorner","",SubheadColor);
			    }catch(e){
			        /*do nothing*/
			    }
			    try{ // for tab strips
    				RoundedTop("ul#tabStrip li","",SubheadColor);
			    }catch(e){
			        /*do nothing*/
			    }
			}
		}catch(e){
			//do nothing
			//alert('Error window.onload-RoundedTop ' + e + ' ' + e.message);
		}
		doWindowOnLoad(); // not every page has one so do this last
	}catch(e){
		//alert('Error window.onload ' + e + ' ' + e.message);
	}
}

/*************** End Global Variables ****************/
function calendarPicker(strDatePickerForm,strField){
	window.open(strDatePickerForm + '?field=' + strField,'calendarPopup','width=304,height=224,resizable=yes');
}

function SwitchMenu(obj){
    if(document.getElementById){
		try{
			var el = document.getElementById(obj);
			var ar = document.getElementById("HierarchalMenu").getElementsByTagName("span");
    		if(el.style.display != "block"){
    			for (var i=0; i<ar.length; i++){
    				if (ar[i].className=="MenuOptions")
    				ar[i].style.backgroundColor = SubheadColor;
    				ar[i].style.display = "none";
    			}
    			el.style.display = "block";
    		}else{
    			el.style.display = "none";
    		}
		}catch(e){
			//alert("Error from 'SwitchMenu' " + e.message + "\n" + "obj: " + obj);
		}
    }
}

function setFocus(elementID) {
/*
  Created to set focus on an element when the element ID is passed
*/
	try{
		document.getElementById(elementID).focus();
	}catch(e){
		// do nothing just don't want to show any error messages
		//alert('Error in setFocus() \n' + e.message);
	}
}

function buildNewWindow(content,title){
	top.consoleRef = window.open('','myconsole',
							'width=350,height=250'
								+',menubar=0'
								+',toolbar=0'
								+',status=0'
								+',scrollbars=1'
								+',resizable=1'
								)

	top.consoleRef.document.writeln("<html>");
	top.consoleRef.document.writeln("	<head>");
	if(title == null){
		title="Error"
	}
	top.consoleRef.document.writeln("		<title>" + title + "</title>");
	top.consoleRef.document.writeln("	</head>");
	top.consoleRef.document.writeln("	<body onload=\"self.focus();\">");
	top.consoleRef.document.writeln("		<form name=\"" + title + "Page\">");
	top.consoleRef.document.writeln(content);
	top.consoleRef.document.writeln("		</form>");
	top.consoleRef.document.writeln("	</body>");
	top.consoleRef.document.writeln("</html>");
	top.consoleRef.document.close()
}

/*
	Function:  handleEnterKey()
	Author: Michael Regan
	Created On: 08/11/2004
	Purpose: Initiates a button click when the enter key is pressed
	Flow:
		
*/
function handleEnterKey(buttonToClick){
	//alert(buttonToClick);
	if (event.keyCode == 13 && enterKeyPressed == false){
		enterKeyPressed=true;
		var submitButton = document.getElementById(buttonToClick);
		try{
			//alert("testing");
			submitButton.focus();
			submitButton.click();
		}
		catch(e){
			/*
			 *	At this point if your getting an error then the button could not be found
			 *	We are going to ignor the error at this point.
			 *	One instance when this error is acceptable is when
			 *	the button has already been disabled...you can not set focus to a button in that state.
			 *	
			 *	In this case the button has alread been press with the enter key and all is ok.
			 *	
			 *	Use the next commented line for trouble shooting.
			*/
			//alert('Could not find button ' + buttonToClick + ' '  + e);
		}
	}
}
/*
	Function:  disableButtions()
	Author: Michael Regan
	Created On: 08/10/2004
	Purpose: Disables buttons on the page
		This was done to prevent the client from pressing
		any "submit" button more than once.
	Flow:
		Loop through the array setup by the page and
		disable all of those buttons.
*/
function disableButtons(){
//	alert('disableButtons');
	document.body.style.cursor = 'wait'; // Takes care of the body
	var i = 1;		//counter
	var numElements;	//number of HTML elements on the form
	try{
		numElements = document.forms[0].elements.length;
	}catch(e){
//			alert(e + '\n' + formName + '\n')
		return;
	}
	for (i=0; i<numElements; i++){ // Go through all of the elements on the page and set the cursor to wait
		document.forms[0].elements[i].style.cursor = 'wait';
	}
	try{ // go through the button array setup in the pages javascript file and set all buttons to disabled = true
		for(var x=0;x<oButtonArray.length;x++){
			try{
				oButtonArray[x].disabled = true;
//				alert(oButtonArray[x].name + ' disabled property = ' + oButtonArray[x].disabled);
			}
			catch(e){
				/* 
					In most cases this will only happen if there are no buttons or
					the button array was setup improperly in the from doBodyOnLoad.
				alert(e);
				*/
			}
		}
	}catch(e){
		// do nothing
	}
}
/*
	Function:  enableButtions()
	Author: Michael Regan
	Created On: 08/10/2004
	Purpose: Enables buttons on the page
		This was done when there is an error submitting the page.
	Flow:
		Loop through the array setup by the page and
		enable all of those buttons.
*/
function enableButtions(){
	document.body.style.cursor = '';
	var i = 1;		//counter
	var numElements;	//number of HTML elements on the form
	window.status='';
	try{
		numElements = document.forms[0].elements.length;
	}catch(e){
//			alert(e + '\n' + formName + '\n')
		return;
	}
	for (i=0; i<numElements; i++){ // Go through all of the elements on the page and test them
		document.forms[0].elements[i].style.cursor = '';
	}
	for(var x=0;x<oButtonArray.length;x++){
		try{
			oButtonArray[x].disabled = false;
		}
		catch(e){
			/*	In most cases this will only happen if there are no buttons or
			 *	the button array was setup improperly in the from doBodyOnLoad.
			*/
			//alert(e);
		}
	}
}
function showMSG(content,style,title){
	switch (style){
		case 1: // Normal pop up box
			regExp = /<br>/gi;
			content = content.replace(regExp,"\n");
			alert(content);
			break;
		case 2: // Popup window
			if(title != null){
				title='';
			}
			buildNewWindow(content,title);
			break;
		case 3: // Floating window
			/*	Must include floatingWindow.js
			 *  Please see floatingWindow.js for more requirements
			 */
			if(title != null){
				title='Err';
			}
			regExp = /<br>/gi;
			content = content.replace(regExp,"\n");
			loadwindow(title,content,'200','300');
			break;
		case 4: // Modal window
			/*	Must include modalWindow.js
			 *  Please see modalWindow.js for more requirements
			 */
			if(title != null){
				title='';
			}
			ModalDialogShow(title,content,'','');
			break;
		default:
			regExp = /<br>/gi;
			content = content.replace(regExp,"\n");
			alert(content);
			break;
	}
	clientMSG = '';
}

function getElementByID(formName,elementID){
	return document.forms[formName].elements[elementID];
}
/************************* Begin Input Mask ***********************/
function dFilterStrip (dFilterTemp, dFilterMask){
    dFilterMask = replace(dFilterMask,'#','');
    for (dFilterStep = 0; dFilterStep < dFilterMask.length++; dFilterStep++){
		dFilterTemp = replace(dFilterTemp,dFilterMask.substring(dFilterStep,dFilterStep+1),'');
	}
	return dFilterTemp;
}

function dFilterMax (dFilterMask){
	dFilterTemp = dFilterMask;
    for (dFilterStep = 0; dFilterStep < (dFilterMask.length+1); dFilterStep++){
		if (dFilterMask.charAt(dFilterStep)!='#'){
			dFilterTemp = replace(dFilterTemp,dFilterMask.charAt(dFilterStep),'');
		}
	}
	return dFilterTemp.length;
}

function dFilter (key, textbox, dFilterMask){
	dFilterNum = dFilterStrip(textbox.value, dFilterMask);
	if((key > 95) && (key < 106)) key=key-48 // used for key pad input
	if (key==9){
		return true;
	}else if((key==46) && (selectedText == textbox.value)){
			textbox.value = "";
			selectedText = "";
			return;
	}else if ((key==8 && dFilterNum.length!=0) || (key==46 && dFilterNum.length!=0)){ // Delete or backspace
		dFilterNum = dFilterNum.substring(0,dFilterNum.length-1);
	}else if ( ((key > 47&& key <58)||(key > 95 && key < 106)) && dFilterNum.length<dFilterMax(dFilterMask) ){
		
		dFilterNum=dFilterNum+String.fromCharCode(key);
	}
	
	var dFilterFinal='';
    for (dFilterStep = 0; dFilterStep < dFilterMask.length; dFilterStep++){
        if (dFilterMask.charAt(dFilterStep)=='#'){
			if (dFilterNum.length!=0){
				dFilterFinal = dFilterFinal + dFilterNum.charAt(0);
				dFilterNum = dFilterNum.substring(1,dFilterNum.length);
			}else{
				dFilterFinal = dFilterFinal + "";
			}
		}else if (dFilterMask.charAt(dFilterStep)!='#'){
			dFilterFinal = dFilterFinal + dFilterMask.charAt(dFilterStep); 			
		}
		    //dFilterTemp = replace(dFilterTemp,dFilterMask.substring(dFilterStep,dFilterStep+1),'');
	}
	textbox.value = dFilterFinal;
    return false;
}

function replace(fullString,text,by) {
// Replaces text with by in string
    var strLength = fullString.length, txtLength = text.length;
    if ((strLength == 0) || (txtLength == 0)) return fullString;

    var i = fullString.indexOf(text);
    if ((!i) && (text != fullString.substring(0,txtLength))) return fullString;
    if (i == -1) return fullString;

    var newstr = fullString.substring(0,i) + by;

    if (i+txtLength < strLength)
        newstr += replace(fullString.substring(i+txtLength,strLength),text,by);

    return newstr;
}
/************************* End Input Mask *************************/
/************************* Tab Strip ******************************/
function showFirstPanel(){
	var foundFirst = -1;
	try{
		if (panels.length >= 1){
			for (i=1; i < panels.length; i++){
				if (panels[i].className == 'tabPanel'){
					foundFirst = parseFloat(foundFirst) + 1
					if(foundFirst > 0){
						panels[i].style.display = 'none';
					}else{
					    var anchor = document.getElementById(panels[i].id  + 'Anchor');
					    anchor.style.backgroundColor = HeadColor;
					}
				}
			}
		}
	}catch(e){
		//alert(e + ' ' + e.message);
	}
}

function showPanel(id) {
    // Show the Panel (and hide the rest)
	try{
	    setCookie('selectedTab',id);
		for (i=0; i < panels.length; i++) { 
			try{
				//alert(divs[i].className);
				if (panels[i].className == 'tabPanel') {
					var strPanelID = panels[i].id;
				    var anchor = document.getElementById(panels[i].id  + 'Anchor');
					if (strPanelID.indexOf(id) >= 0){
						panels[i].style.display = 'block';
						try{
					        anchor.style.backgroundColor = HeadColor;
					    }catch(e){
					        // do nothing
					    }
					}else{
						panels[i].style.display = 'none';
						try{
    					    anchor.style.backgroundColor = SubheadColor;
					    }catch(e){
					        // do nothing
					    }
					}
				}
			}catch(e){
				alert(e);
			}
		}
	}catch(e){
		alert(e);
	}
}
/************************* End Tab Strip ***************************/

/************************* ShowMSG ***********************************/
function showMSG(content,style,title){
	switch (style){
		case 1: // Normal pop up box
			regExp = /<br>/gi;
			content = content.replace(regExp,"\n");
			alert(content);
			break;
		case 2: // Popup window
			if(title == null){
				title='Error Message';
			}
			buildNewWindow(content,title);
			break;
		case 3: // Floating window
			/*	Must include floatingWindow.js
			 *  Please see floatingWindow.js for more requirements
			 */
			if(title == null){
				title='Error Message';
			}
			regExp = /<br>/gi;
			content = content.replace(regExp,"\n");
			loadwindow(title,content,'200','300');
			break;
		case 4: // Modal window
			/*	Must include modalWindow.js
			 *  Please see modalWindow.js for more requirements
			 */
			if(title == null){
				title='Error Message';
			}
			ModalDialogShow(title,content,'','');
			break;
		default:
			regExp = /<br>/gi;
			content = content.replace(regExp,"\n");
			alert(content);
			break;
	}
	clientMSG = '';
}
/************************* End ShowMSG ********************************/

/************************* Moving selectetions from one box to the next ***/
function switchList(objFromBox,objToBox){
/*
	SwitchList was created to move data from one list box to another
	keeping the sort order based on the text of the options.
	Parameters:
		objFromBox - The from listbox as an object
		objToBox - The to listbox as an object
*/

	var arrFbox = new Array();
	var arrTbox = new Array();
	var arrLookup = new Array();
	var i;
	for (i = 0; i < objToBox.options.length; i++){
		arrLookup[objToBox.options[i].text] = objToBox.options[i].value;
		arrTbox[i] = objToBox.options[i].text;
	}
	var fLength = 0;
	var tLength = arrTbox.length;
	for(i = 0; i < objFromBox.options.length; i++){
		arrLookup[objFromBox.options[i].text] = objFromBox.options[i].value;
		if (objFromBox.options[i].selected && objFromBox.options[i].value != ""){
			arrTbox[tLength] = objFromBox.options[i].text;
			tLength++;
		}else{
			arrFbox[fLength] = objFromBox.options[i].text;
			fLength++;
		}
	}
	arrFbox.sort();
	arrTbox.sort();
	objFromBox.length = 0;
	objToBox.length = 0;
	var c;
	for(c = 0; c < arrFbox.length; c++){
		var no = new Option();
		no.value = arrLookup[arrFbox[c]];
		no.text = arrFbox[c];
		objFromBox[c] = no;
	}
	for(c = 0; c < arrTbox.length; c++){
		var no = new Option();
		no.value = arrLookup[arrTbox[c]];
		no.text = arrTbox[c];
		objToBox[c] = no;
	}

	var listID = objFromBox.id.substr(0,objFromBox.id.length - 7);
	var objSrcList = document.getElementById(listID + 'SrcList');
	var objDstList = document.getElementById(listID + 'DstList');
	var selectedState = '';
	var selectedStateField = document.getElementById(listID + 'SelectedState');
    var allStateField = document.getElementById(listID + 'AllState');
	var allState = '';

	for (i=0; i < objSrcList.length; i++){
		allState += objSrcList.options[i].text + ',';
	}
	allStateField.value = allState.substr(0,allState.length -1);

	for (i=0; i < objDstList.length; i++){
		selectedState += objDstList.options[i].text + ',';
	}
	selectedStateField.value = selectedState.substr(0,selectedState.length -1);


}

function selectAllInListBox(objListBox){
/*
	selectAllInListBox was created to select all of the options in a listbox.
	Need for when you want to move all of the data from one list box to the other.
	Parameters:
		objListBox - The list box as an object to be selected
*/
	var lengthOfListBox = objListBox.length;
	var iCounter = 0;
	for(iCounter=0;iCounter<lengthOfListBox;iCounter++){
		try{
			objListBox.options[iCounter].selected = true;
 		}catch(e){
			//alert(e); // just here for testing wouldn't suggest you have it in your code
			return;
		}
	}
}

function switchAll(objFromBox,objToBox){
/*
	switchAll was created to move all of the options from one list box to another.
	Parameters:
		objFromBox - The from list box as an object
		objToBox - The to list box as an object
*/
	selectAllInListBox(objFromBox);
	switchList(objFromBox,objToBox);
}
/************************* End Moving selectetions from one box to the next */

function getCookieVal (offset) {
  var endstr = document.cookie.indexOf (";", offset);
  if (endstr == -1)
    endstr = document.cookie.length;
  return unescape(document.cookie.substring(offset, endstr));
}

function getCookie (name) {
  var arg = name + "=";
  var alen = arg.length;
  var clen = document.cookie.length;
  var i = 0;
  while (i < clen) {
    var j = i + alen;
    if (document.cookie.substring(i, j) == arg)
      return getCookieVal (j);
    i = document.cookie.indexOf(" ", i) + 1;
    if (i == 0) break; 
  }
  return null;
}

function setCookie (name, value) {
  var argv = setCookie.arguments;
  var argc = setCookie.arguments.length;
  var expires = (argc > 2) ? argv[2] : null;
  var path = (argc > 3) ? argv[3] : null;
  var domain = (argc > 4) ? argv[4] : null;
  var secure = (argc > 4) ? argv[5] : false;
  document.cookie = name + "=" + escape (value) + ((expires == null) ? "" : ("; expires=" + expires.toGMTString())) + ((path == null) ? "" : ("; path=" + path)) + ((domain == null) ? "" : ("; domain=" + domain));
}

function deleteCookie(name){
exp=new Date();
exp.setTime (exp.getTime() - 1);
var cval = getCookie ("name");
document.cookie = name + "=" + cval +"; expires=" + exp.toGMTString();
}

/************************* TESTING NEW SCRIPTS ***********************/

/************************* END NEW SCRIPTS ***************************/
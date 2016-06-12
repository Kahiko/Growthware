var ModalDialogWindow;
var ModalDialogInterval;
var ModalDialog = new Object;

ModalDialog.value = '';
ModalDialog.eventhandler = '';
 

function ModalDialogMaintainFocus(){
	try{
		if (ModalDialogWindow.closed){
			window.clearInterval(ModalDialogInterval);
			eval(ModalDialog.eventhandler);       
			return;
		}
		ModalDialogWindow.focus(); 
	}catch(everything){
		// Do nothing
	}
}
        
function ModalDialogRemoveWatch(){
	ModalDialog.value = '';
	ModalDialog.eventhandler = '';
}
        
function ModalDialogShow(Title,BodyText,Buttons,EventHandler){
	ModalDialogRemoveWatch();
	ModalDialog.eventhandler = EventHandler;

	var args='width=350,height=125,left=325,top=300,toolbar=0,';
		args+='location=0,status=0,menubar=0,scrollbars=1,resizable=0';  

	ModalDialogWindow=window.open("","",args); 
	ModalDialogWindow.document.open(); 
	ModalDialogWindow.document.write('<html>');
	ModalDialogWindow.document.write('<head>'); 
	ModalDialogWindow.document.write('<title>' + Title + '</title>');
	ModalDialogWindow.document.write('<script' + ' language=JavaScript>');
	ModalDialogWindow.document.write('function CloseForm(Response) ');
	ModalDialogWindow.document.write('{ ');
	ModalDialogWindow.document.write(' window.opener.ModalDialog.value = Response; ');
	ModalDialogWindow.document.write(' window.close(); ');
	ModalDialogWindow.document.write('} ');
	ModalDialogWindow.document.write('</script' + '>');        
	ModalDialogWindow.document.write('</head>');   
	ModalDialogWindow.document.write('<body onblur="window.focus();">');
	ModalDialogWindow.document.write('<table border=0 width="95%" align=center cellspacing=0 cellpadding=2>');
	ModalDialogWindow.document.write('<tr><td align=left>' + BodyText + '</td></tr>');
	ModalDialogWindow.document.write('<tr><td align=left><br></td></tr>');
	ModalDialogWindow.document.write('<tr><td align=center>' + Buttons + '</td></tr>');
	ModalDialogWindow.document.write('</body>');
	ModalDialogWindow.document.write('</html>'); 
	ModalDialogWindow.document.close(); 
	ModalDialogWindow.focus(); 
	ModalDialogInterval = window.setInterval("ModalDialogMaintainFocus()",5);
}

function YesNoCancel(BodyText,EventHandler){
	var Buttons=''; 
	Buttons = '<a href=javascript:CloseForm("Yes");>Yes</a>&nbsp;&nbsp;';
	Buttons += '<a href=javascript:CloseForm("No");>No</a>&nbsp;&nbsp;';
	Buttons += '<a href=javascript:CloseForm("Cancel");>Cancel</a>&nbsp;&nbsp;';
	ModalDialogShow("Dialog",BodyText,Buttons,EventHandler);
}

function YesNoMaybe(BodyText,EventHandler){
    var Buttons=''; 
    Buttons = '<a href=javascript:CloseForm("Yes");>Yes</a>&nbsp;&nbsp;';
    Buttons += '<a href=javascript:CloseForm("No");>No</a>&nbsp;&nbsp;';
    Buttons += '<a href=javascript:CloseForm("Maybe");>Maybe</a>&nbsp;&nbsp;';
    ModalDialogShow("Dialog",BodyText,Buttons,EventHandler);
}
 
function YesNoCancelReturnMethod(){
	document.getElementById('modalreturn1').value =  ModalDialog.value;
	ModalDialogRemoveWatch();
	alert('YesNoCancelReturnMethod has just executed.');
}

function YesNoMaybeReturnMethod(){
	document.getElementById('modalreturn2').value = ModalDialog.value;
	ModalDialogRemoveWatch();
	alert('YesNoMaybeReturnMethod has just executed.');
}
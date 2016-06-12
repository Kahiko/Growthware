	/*******************  Begin DHTML Window ***************/
var dragapproved=false;
var minrestore=0;
var initialwidth,initialheight;
var ie5=document.all&&document.getElementById;
var ns6=document.getElementById&&!document.all;

buildFloatingWindow(); // create the necessary elements

function iecompattest(){
	return (!window.opera && document.compatMode && document.compatMode!="BackCompat")? document.documentElement : document.body;
}

function drag_drop(e){
	if (ie5&&dragapproved&&event.button==1){
		document.getElementById("dwindow").style.left=tempx+event.clientX-offsetx+"px";
		document.getElementById("dwindow").style.top=tempy+event.clientY-offsety+"px";
	}else if (ns6&&dragapproved){
		document.getElementById("dwindow").style.left=tempx+e.clientX-offsetx+"px";
		document.getElementById("dwindow").style.top=tempy+e.clientY-offsety+"px";
	}
}

function initializedrag(e){
	offsetx=ie5? event.clientX : e.clientX;
	offsety=ie5? event.clientY : e.clientY;
	tempx=parseInt(document.getElementById("dwindow").style.left);
	tempy=parseInt(document.getElementById("dwindow").style.top);
	dragapproved=true;
	document.getElementById("dwindow").onmousemove=drag_drop;
}
function maximize(){
	if (minrestore==0){
		minrestore=1; //maximize window
		document.getElementById("maxname").setAttribute("src","images/restore.gif");
		document.getElementById("contentTextArea").style.width=ns6? window.innerWidth-30+"px" : parseFloat(iecompattest().clientWidth - 40)+"px";
		document.getElementById("contentTextArea").style.height=ns6? window.innerHeight-30+"px" : parseFloat(iecompattest().clientHeight - 40)+"px";
		document.getElementById("dwindowcontent").style.width=ns6? window.innerWidth-20+"px" : parseFloat(iecompattest().clientWidth - 30)+"px";
		document.getElementById("dwindowcontent").style.height=ns6? window.innerHeight-20+"px" : parseFloat(iecompattest().clientHeight - 30)+"px";
		document.getElementById("dwindow").style.width=ns6? window.innerWidth-10+"px" : parseFloat(iecompattest().clientWidth - 20)+"px";
		document.getElementById("dwindow").style.height=ns6? window.innerHeight-10+"px" : parseFloat(iecompattest().clientHeight - 20)+"px";
	}else{
		minrestore=0; //restore window
		try{
			document.getElementById("contentTextArea").style.width= (parseFloat(initialwidth) - 10);
			document.getElementById("contentTextArea").style.height= (parseFloat(initialheight) - 30);
			document.getElementById("maxname").setAttribute("src","images/max.gif");
			document.getElementById("dwindowcontent").style.width=initialwidth;
			document.getElementById("dwindowcontent").style.height=(parseFloat(initialheight) - 35);
			document.getElementById("dwindow").style.width=initialwidth;
			document.getElementById("dwindow").style.height=initialheight;
		}catch(e){
			//alert('maximize() Error:\n' + e);
		}
	}
	document.getElementById("dwindow").style.left=ns6? window.pageXOffset+"px" : iecompattest().scrollLeft+"px";
	document.getElementById("dwindow").style.top=ns6? window.pageYOffset+"px" : iecompattest().scrollTop+"px";
}

function closeit(){
	minrestore=1; //maximize window
	maximize();
	document.getElementById("dwindow").style.display="none";
}

function stopdrag(){
	dragapproved=false;
	document.getElementById("dwindow").onmousemove=null;
}
	/*******************  End DHTML Window ***************/

function buildFloatingWindow(){
	document.write("<div id='dwindow' style='BORDER-RIGHT: #000000 1px solid; BORDER-TOP: #000000 1px solid; DISPLAY: none; LEFT: 0px; BORDER-LEFT: #000000 1px solid; BORDER-BOTTOM: #000000 1px solid; POSITION: absolute; TOP: 0px; BACKGROUND-COLOR: #ebebeb'>");
	document.write("	<table border='0' cellpadding='0' cellspacing='0' ID='Table12'>");
	document.write("		<tr>");
	document.write("			<td>");
	document.write("				<table  onMousedown='initializedrag(event)' onMouseup='stopdrag()' onSelectStart='return false' border='0' cellpadding='0' cellspacing='0' style='BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; BORDER-LEFT: black 1px solid; BORDER-BOTTOM: black 1px solid' ID='Table13'>");
	document.write("					<tr style='CURSOR: hand; COLOR: #ffffff; BACKGROUND-COLOR: royalblue'>");
	document.write("						<td width='100%'>&nbsp;<label id='Title'></label> </td>");
	document.write("						<td>");
	document.write("							<img border='0px' src='" + imagePath + "max.gif' id='maxname' onClick='maximize()' width='16' height='14'>");
	document.write("						</td>");
	document.write("						<td>");
	document.write("							<img border='0px' src='" + imagePath + "close.gif' onClick='closeit()' width='16' height='14'>");
	document.write("						</td>");
	document.write("					</tr>");
	document.write("				</table>");
	document.write("			</td>");
	document.write("		</tr>");
	document.write("		<tr>");
	document.write("			<td>");
	document.write("				<table id='dwindowcontent' height='175' bgcolor=LightGrey border='0' cellpadding='5' cellspacing='0' ID='Table5'>");
	document.write("					<tr>");
	document.write("						<td valign='top'>");
	document.write("							<textarea style='height: 150px; overflow-y: scroll;' id='contentTextArea' class='generalcontent NAME='contentTextArea' NAME='contentTextArea' NAME='contentTextArea' NAME='contentTextArea'></textarea>");
	document.write("						</td>");
	document.write("					</tr>");
	document.write("				</table>");
	document.write("			</td>");
	document.write("		</tr>");
	document.write("	</table>");
	document.write("</div>");
}

function loadwindow(title,content,width,height){
	document.getElementById("Title").innerHTML = title;
	document.getElementById("contentTextArea").style.width= (parseFloat(width) - 10)+"px";
	document.getElementById("contentTextArea").style.height= (parseFloat(height) - 30)+"px";
	document.getElementById("dwindowcontent").style.width= (parseFloat(width) - 15)+"px";
	document.getElementById("dwindowcontent").style.height= (parseFloat(height) - 35)+"px";
	document.getElementById("dwindow").style.display='';
	document.getElementById("dwindow").style.width=initialwidth=width+"px";
	document.getElementById("dwindow").style.height=initialheight=height+"px";
	document.getElementById("dwindow").style.left="50px";
	document.getElementById("dwindow").style.top=ns6? window.pageYOffset*1+30+"px" : iecompattest().scrollTop*1+30+"px";
	document.getElementById("contentTextArea").value = content;
}
	/*******************  End DHTML Window *****************/
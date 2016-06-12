<%@ Control AutoEventWireup="false" Inherits="ApplicationBase.Logon" Language="vb" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script>
function doWindowOnLoad(){
	oButtonArray[oButtonArray.length] = document.getElementById('ctl00_RightHandModulesLoader_ctl00_btnLogon'); // Add button to the disable array
	oButtonArray[oButtonArray.length] = document.getElementById('ctl00_RightHandModulesLoader_ctl00_btnRequestPasswordReset'); // Add button to the disable array
	enableButtions();
	var pageLocation = '';
	try{
		var whoGetsFocus = window.document.forms[0].ctl00_LeftHandModulesLoader1_ctl00_txtAccount.value;
		pageLocation = "ctl00_LeftHandModulesLoader1_ctl00_";
	}catch(e){
		var whoGetsFocus = window.document.forms[0].ctl00_RightHandModulesLoader_ctl00_txtAccount.value;
		pageLocation = "ctl00_RightHandModulesLoader_ctl00_";
	}
    try{
		if(whoGetsFocus.length == 0){
				setFocus(pageLocation + 'txtAccount');
		}else{
			setFocus(pageLocation + 'password');
		}
    }catch(e){
		//alert(e + ' ' + e.message);     
    }
}

function doSubmit(){
	disableButtons();
	window.status = "Logging on"
	if (!ValidatorOnSubmit()){
		window.status = "";
		enableButtions();
		return false;
	}
	__doPostBack(pageLocation + 'btnLogon','');
}
</script>
<table id="Table1" border="0" cellpadding="0" cellspacing="0" onblur="handleEnterKey('ctl00_LeftHandModulesLoader1_ctl00_btnLogon');">
	<tr>
		<td align="left">
			<font face="Arial" color="silver" size="-1">Account:</font>
			<br>
			<asp:textbox id="txtAccount" BorderStyle="Solid" Font-Size="0.8em" BorderWidth="1px" height="15px" Width="128px" Runat="server"  BorderColor="silver"/>
			<br>
			<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="(required)" ControlToValidate="txtAccount"></asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td align="left">
			<font face="Arial" color="silver" size="-1">Password:</font>
			<br>
			<input id="Password" type="password" style="BORDER-RIGHT: silver 1px solid; PADDING-RIGHT: 0px; BORDER-TOP: silver 1px solid; PADDING-LEFT: 0px; FONT-SIZE: 0.8em; PADDING-BOTTOM: 0px; MARGIN: 0px; BORDER-LEFT: silver 1px solid; WIDTH: 128px; COLOR: gray; PADDING-TOP: 0px; BORDER-BOTTOM: silver 1px solid; HEIGHT: 15px" runat="server" NAME="Password">
			<br>
			<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="(required)" ControlToValidate="Password"></asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td colspan="2" align="left">
			<input type="submit" value="     Logon     " onServerClick="btnLogon_Click" runat="server" ID="btnLogon" NAME="Submit1">
		</td>
	</tr>
	<tr>
		<td colspan="2" align="left">
			<span id="ErrorMsg" style="FONT:0.70em verdana, arial; COLOR:black" Visible="false" runat="server">
				<b>Invalid Account Name or Password!</b>
			</span>
		</td>
	</tr>
	<tr>
		<td colspan="2" align="left">
			<asp:Button ID="btnRequestPasswordReset" Font-Size="1em" Visible="False" Text="Reset Password?" Runat="server" />
		</td>
	</tr>
</table>
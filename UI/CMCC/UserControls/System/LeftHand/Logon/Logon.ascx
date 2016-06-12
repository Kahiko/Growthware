<%@ Control Language="vb" AutoEventWireup="false" Inherits="BaseApplication.Logon" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script>
function doWindowOnLoad(){
	try{
		//var whoGetsFocus = window.document.forms[0].LeftHandModulesLoader__ctl0_txtAccount.value;
		var whoGetsFocus = window.document.forms[0]._ctl0_LeftHandModulesLoader__ctl0_txtAccount.value;
		//var menuLocation = "LeftHand";
		var menuLocation = "_ctl0_LeftHand";
	}catch(e){
		var whoGetsFocus = window.document.forms[0]._ctl0_RightHandModulesLoader__ctl0_txtAccount.value;
		//var menuLocation = "RightHand";
		var menuLocation = "_ctl0_RightHand";
	}
    try{
		if(whoGetsFocus.length == 0){
				setFocus(menuLocation + 'ModulesLoader__ctl0_txtAccount');
		}else{
			setFocus(menuLocation + 'ModulesLoader__ctl0_password');
		}
    }catch(e){
		//alert(e + ' ' + e.message);     
    }
}
</script>
<table border="0" cellpadding="0" cellspacing="0" id=Table1>
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
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Logon.ascx.vb" Inherits="BaseApplication.Logon" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<table width="100%" id=Table1>
	<tr>
		<td align="right">
			<font face="Arial" size="-1">Account:</font>
		</td>
		<td>
			<asp:textbox id="txtAccount" size="14" Runat="server" /><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="(required)" ControlToValidate="txtAccount"></asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td align="right">
			<font face="Arial" size="-1">Password:</font>
		</td>
		<td>
			<input id="Password" type="password" size="16" runat="server" NAME="Password"> <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="(required)" ControlToValidate="Password"></asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td colspan="2" align="center">
			<input type="submit" value="     Logon     " onServerClick="btnLogon_Click" runat="server" ID="btnLogon" NAME="Submit1">
		</td>
	</tr>
	<tr>
		<td colspan="2" align="center">
			<span id="ErrorMsg" style="FONT:8pt verdana, arial; COLOR:black" Visible="false" runat="server">
				<b>Invalid Account Name or Password!</b> </span>
		</td>
	</tr>
	<tr>
		<td colspan="2" align="center">
			<asp:Button ID="btnRequestPasswordReset" Visible="False" Text="Request Password Reset" Runat="server" />
		</td>
	</tr>
</table>
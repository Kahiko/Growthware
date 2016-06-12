<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.Logon" Codebehind="Logon.ascx.vb" %>
<script>
function doWindowOnLoad(){
	oButtonArray[oButtonArray.length] = document.getElementById('ctl00_RightHandModulesLoader_ctl00_btnLogon'); // Add button to the disable array
	oButtonArray[oButtonArray.length] = document.getElementById('ctl00_RightHandModulesLoader_ctl00_btnRequestPasswordReset'); // Add button to the disable array
	enableButtions();
	try{
		var whoGetsFocus = window.document.forms[0].ctl00_LeftHandModulesLoader_ctl00_txtAccount.value;
		var pageLocation = "ctl00_LeftHand";
	}catch(e){
		var whoGetsFocus = window.document.forms[0].ctl00_RightHandModulesLoader_ctl00_txtAccount.value;
		var pageLocation = "ctl00_RightHand";
	}
    try{
		if(whoGetsFocus.length == 0){
				setFocus(pageLocation + 'ModulesLoader_ctl00_txtAccount');
		}else{
			setFocus(pageLocation + 'ModulesLoader_ctl00_password');
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
	try{
		var txtAccount = window.document.forms[0]._ctl0_LeftHandModulesLoader__ctl0_txtAccount.value;
		var pageLocation = "ctl00$LeftHandModulesLoader$ctl00$";
	}catch(e){
		var txtAccount = window.document.forms[0]._ctl0_RightHandModulesLoader__ctl0_txtAccount.value;
		var pageLocation = "ctl00$RightHandModulesLoader$ctl00$";
	}
	__doPostBack(pageLocation + 'btnLogon','');
}

</script>
<table id="Table1" onblur="handleEnterKey('_ctl0_RightHandModulesLoader__ctl0_btnLogon');" width="100%">
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
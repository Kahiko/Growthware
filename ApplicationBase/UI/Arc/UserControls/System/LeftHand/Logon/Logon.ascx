<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.Logon" Codebehind="~/Modules/System/LeftHand/Logon/Logon.ascx.vb" %>
<script type="text/javascript">
function doWindowOnLoad(){
	oButtonArray[oButtonArray.length] = document.getElementById('ctl00_RightHandModulesLoader_ctl00_btnLogon'); // Add button to the disable array
	oButtonArray[oButtonArray.length] = document.getElementById('ctl00_RightHandModulesLoader_ctl00_btnRequestPasswordReset'); // Add button to the disable array
	enableButtions();
	try{
		var whoGetsFocus = window.document.forms[0].ctl00_LeftHandModulesLoader_ctl00_txtAccount.value;
		pageLocation = "ctl00_LeftHandModulesLoader_ctl00_";
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

<table class="Form_Field" border="0" cellpadding="0" cellspacing="3" width="10px" id=Table1>
	<tr>
		<td>
			<font face="Arial" size="-1">Account:</font><br>
			<asp:textbox CssClass="Form_Field" id="txtAccount" size="14" Runat="server" /><br>
			<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="(required)" ControlToValidate="txtAccount"></asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td>
			<font face="Arial" size="-1">Password:</font><br>
			<input class="Form_Field" id="Password" type="password" size="16" runat="server" NAME="Password"><br>
			<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="(required)" ControlToValidate="Password"></asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td align="center">
			<input type="submit" value="     Logon     " onServerClick="btnLogon_Click" runat="server" ID="btnLogon" NAME="Submit1">
		</td>
	</tr>
	<tr>
		<td align="center">
			<span id="ErrorMsg" style="FONT:8pt verdana, arial; COLOR:black" Visible="false" runat="server">
				<b>Invalid Account Name or Password!</b> </span>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:Button ID="btnRequestPasswordReset" Visible="False" Text="Request Password Reset" Runat="server" />
		</td>
	</tr>
</table>
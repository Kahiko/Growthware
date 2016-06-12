<%@ Control AutoEventWireup="false" Inherits="ApplicationBase.Logon" Language="vb" %>
<script>
function doWindowOnLoad(){
	try{
		var whoGetsFocus = window.document.forms[0].ctl00_LeftHandModulesLoader_ctl00_txtAccount.value;
		var menuLocation = "_ctl0_LeftHand";
	}catch(e){
		try{
			var whoGetsFocus = window.document.forms[0].ctl00_RightHandModulesLoader_ctl00_txtAccount.value;
			var menuLocation = "_ctl0_RightHand";
		}catch(e){
			//alert(e + ' ' + e.message);
		}
	}
    try{
		if(whoGetsFocus.length == 0){
				//alert(menuLocation + 'ModulesLoader__ctl0_txtAccount');
				setFocus(menuLocation + 'ModulesLoader__ctl0_txtAccount');
		}else{
			setFocus(menuLocation + 'ModulesLoader__ctl0_password');
		}
    }catch(e){
		//alert(e + ' ' + e.message);
    }
}
</script>
<table align="left" border="0" cellpadding="0" cellspacing="0" width="200">
	<tbody>
		<tr>
			<td bgcolor="#ead8c9">&nbsp;</td>
			<td bgcolor="#ead8c9" nowrap >
				<p class="nav">&nbsp;</p>
			</td>
			<td><img src='<%=ResolveUrl( "~/UI/CHCF/Images/left-tab_end.gif" )%>' alt="left tab end" height="36" width="38"></td>
		</tr>
		<tr>
			<td valign="top"><img src='<%=ResolveUrl( "~/UI/CHCF/Images/left-tab_bottom.gif" )%>' alt="left tab bottom" height="58" width="39"></td>
			<td nowrap valign="top"><br>
<!-- Prepopulate userId from request or cookie-->
				<span class="h3">Login</span><br>
			        <p>
						User ID:<br>
						<asp:textbox id="txtAccount" Width="140" class="beige" height="18" Runat="server" /><asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ErrorMessage="(required)" ControlToValidate="txtAccount"></asp:RequiredFieldValidator>
			        </p>
			        <p>Password:<br>
						<input id="Password" type="password" size="20" class="beige" height="25" runat="server" NAME="Password"><asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" ErrorMessage="(required)" ControlToValidate="Password"></asp:RequiredFieldValidator>
					</p>
					<p>
						<!--
						<input name="chkrememeberpwd" id="chkrememeberpwd" value="checkbox" checked="checked" type="checkbox">Remember Me &nbsp;
						<input id="btnLogon" type="submit" value="OK" onserverclick="btnLogon_Click" runat="server">
						-->
						<asp:ImageButton ID="imgBtnLogon" AlternateText="Logon On" ImageUrl='../../../../Images/OK.gif' Runat="server"></asp:ImageButton>
					</p>
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
	</tbody>
</table>

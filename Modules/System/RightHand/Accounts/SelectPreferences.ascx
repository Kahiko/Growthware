<%@ Control Language="vb" AutoEventWireup="false" Codebehind="SelectPreferences.ascx.vb" Inherits="BaseApplication.SelectPreferences" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table border="0" cellpadding="0" cellspacing=0 width="100%">
	<tr align="center">
		<td align="right">
			<span class="Form_LabelText">Favorite Link:</span>
		</td>
		<td align="left">
			<asp:DropDownList ID="dropFavorite" Runat="server"></asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td align="right">
			<span class="Form_LabelText">Prefered records per page: </span>
		</td>
		<td align="left">
			<asp:TextBox id="txtPreferedRecordsPerPage" MaxLength="10" CssClass="Form_Field" Runat="Server" />
			<asp:RequiredFieldValidator ControlToValidate="txtPreferedRecordsPerPage" Display="Dynamic" Text="(required)" ErrorMessage="You must enter a prefered name" CssClass="Form_Message" Runat="Server" ID="Requiredfieldvalidator3"/>
			<asp:RangeValidator id=RangeValidator1 ControlToValidate="txtPreferedRecordsPerPage" Display="Dynamic" runat="server" ErrorMessage="Must be a number between 1 and 100" Type=Integer MaximumValue="100" MinimumValue="1"></asp:RangeValidator>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
		<td align="left">
			<asp:Button ID="btnSubmit" Text="Submit" Runat="server"></asp:Button>
		</td>
	</tr>
</table>
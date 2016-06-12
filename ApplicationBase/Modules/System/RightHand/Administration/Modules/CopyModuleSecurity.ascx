<%@ Control Language="vb" AutoEventWireup="false" Codebehind="CopyModuleSecurity.ascx.vb" Inherits="ApplicationBase.CopyModuleSecurity" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table border="0" cellpadding="5" cellspacing="0" width="100%">
	<tr>
		<td>
			Please use with great caution.<br>
			Doing this will remove any current module security information for the target business unit!!!
		</td>
	</tr>
	<tr>
		<td>
			<font style="COLOR: red">
				<asp:Literal ID="litClientMSG" Visible="False" Runat="server"></asp:Literal>
			</font>
		</td>
	</tr>
	<tr>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td align="center">
			<table border="0" cellpadding="5" cellspacing="0" width="1">
				<tr>
					<td align="center">Source</td>
					<td align="center">Target</td>
				</tr>
				<tr>
					<td align="right">
						<asp:DropDownList ID="SourceBU" Runat="server">
						</asp:DropDownList>
					</td>
					<td align="left">
						<asp:DropDownList ID="TargetBU" Runat="server">
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td colspan="2" align="center">
						<asp:Button ID="btnSubmit" Text=" Go " Runat="server"></asp:Button>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>

<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ChangeColors.ascx.vb" Inherits="BaseApplication.ChangeColors" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td style="PADDING-RIGHT:15px; PADDING-LEFT:15px; PADDING-BOTTOM:15px; PADDING-TOP:15px"
			align="center">
			<font face="Arial" size="-1">Select a color scheme, then click "Save" to accept the 
				setting.</font>
			<p>
				<table width="95%" border="0">
					<tr>
						<td width="5%" align="right">
							<input type="radio" name="colors" value="blue,#6699cc,#b6cbeb,#ffffff,#eeeeee" runat="server"
								ID="Radio1">
						</td>
						<td width="20%" align="center">
							<font face="Arial" size="-1"><b>Blue</b></font>
							<table cellspacing="1" cellpadding="1">
								<tr>
									<td align="center" valign="middle">
										<img id="blueImage" border="1" src="option_blue.gif" runat="server">
									</td>
								</tr>
							</table>
						</td>
						<td width="5%" align="right">
							<input type="radio" name="colors" value="green,#879966,#c5e095,#ffffff,#eeeeee" runat="server"
								ID="Radio2">
						</td>
						<td width="20%" align="center">
							<font face="Arial" size="-1"><b>Green</b></font>
							<table cellspacing="1" cellpadding="1">
								<tr>
									<td align="center" valign="middle">
										<img id="greenImage" border="1" src="option_green.gif" runat="server">
									</td>
								</tr>
							</table>
						</td>
						<td width="5%" align="right">
							<input type="radio" name="colors" value="yellow,#f8bc03,#f8e094,#ffffff,#f8e094" runat="server"
								ID="Radio3">
						</td>
						<td width="20%" align="center">
							<font face="Arial" size="-1"><b>Yellow</b></font>
							<table cellspacing="1" cellpadding="1">
								<tr>
									<td align="center" valign="middle">
										<img id="yellowImage" border="1" src="option_yellow.gif" runat="server">
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td width="5%" align="right">
							<input type="radio" name="colors" value="purple,#91619b,#be9cc5,#ffffff,#eeeeee" runat="server"
								ID="Radio4">
						</td>
						<td width="20%" align="center">
							<font face="Arial" size="-1"><b>Purple</b></font>
							<table cellspacing="1" cellpadding="1">
								<tr>
									<td align="center" valign="middle">
										<img id="purpleImage" border="1" src="option_purple.gif" runat="server">
									</td>
								</tr>
							</table>
						</td>
						<td width="5%" align="right">
							<input type="radio" name="colors" value="red,#a7342a,#df867f,#ffffff,#eeeeee" runat="server"
								ID="Radio5">
						</td>
						<td width="20%" align="center">
							<font face="Arial" size="-1"><b>Red</b></font>
							<table cellspacing="1" cellpadding="1">
								<tr>
									<td align="center" valign="middle">
										<img id="redImage" border="1" src="option_red.gif" runat="server">
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</p>
		</td>
	</tr>
	<tr id="trSubmit" runat="server" height="40">
		<td colspan="10" align="center">
			<asp:Button ID="btnSave" Text="Save" Runat="server"></asp:Button>
		</td>
	</tr>
</table>
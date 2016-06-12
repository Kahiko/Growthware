<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.AccessDenied" Codebehind="AccessDenied.ascx.vb" %>
	<table>
		<tr>
			<td>
				<asp:PlaceHolder ID="insufficientRights" Visible="False" Runat="server">
					The account you are using does not have access to <B>"<LABEL id="requesedAction" runat="server"></LABEL>".</B><BR>
					If you are receiving this in error please contact the help desk.
				</asp:PlaceHolder>
				<asp:PlaceHolder ID="mustLogon" Visible="False" Runat="server">
					You must be logged on to access <B>"<LABEL id="requesedAction1" runat="server"></LABEL>".</B><BR>
				</asp:PlaceHolder>
				
			</td>
		</tr>
	</table>
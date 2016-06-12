<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.LineMenu" Codebehind="LineMenu.ascx.vb" %>
<table id=tblUpperRightLinks style="PADDING-RIGHT: 10px; PADDING-LEFT: 10px; PADDING-BOTTOM: 0px; MARGIN: 0px 10px; PADDING-TOP: 0px" runat="server">
	<tr>
		<td noWrap>
			<CustomWebControls:NAVTRAIL id=Navtrail1 runat="server">
			</CustomWebControls:NAVTRAIL>
		</td>
	</tr>
</table>
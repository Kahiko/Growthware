<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.CopyRight" Codebehind="CopyRight.ascx.vb" %>
<%@OutputCache Duration="3600" VaryByParam="none" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td align="center" valign="top" class="Footer">
			UMASS © <%=Format(Date.Now, "yyyy")%>
		</td>
	</tr>
</table>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="CopyRight.ascx.vb" Inherits="BaseApplication.CopyRight" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@OutputCache Duration="3600" VaryByParam="none" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td align="center" valign="top" class="Footer">
			UMASS © <%=Format(Date.Now, "yyyy")%>
		</td>
	</tr>
</table>
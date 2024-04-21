<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GenericHome.aspx.vb" Inherits="GrowthWare.WebApplication.GenericHome" %>
<div>
	<table border="0" width="100%" cellpadding="0" cellspacing="0" style="FONT: 8pt verdana, arial">
		<tr>
			<td align="left" valign="top" style="PADDING-RIGHT:0px; PADDING-LEFT:0px; PADDING-BOTTOM:0px; PADDING-TOP:0px">
				<asp:Image ID="SideImage" ImageUrl='<%=ResolveUrl( "~/Content/GrowthWare/Images/Misc/sidebar_blue.gif" )%>' Runat="server"></asp:Image>
			</td>
			<td align="left" valign="top" style="PADDING-RIGHT:15px; PADDING-LEFT:15px; PADDING-BOTTOM:15px; PADDING-TOP:15px; width: 100%;">
				<b>Welcome to the <asp:Label id="lblAppName" runat="server">lblAppName</asp:Label></b>
				<p>
				Growthware is an idea dedicated to producing reusable and extendable core technologies used to produce a working web application (Web 2.0) in less than 10 minutes. At the core are the Framework, custom web controls, and the core web application.
				</p>
				<p>
				Click here to <a href="http://sourceforge.net/projects/bedrockgrowth/" target="_blank">Download</a> the latest version of GrowthWare!
				</p>

			</td>
		</tr>
	</table>
</div>

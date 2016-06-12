<%@ Control CodeBehind="Error.ascx.vb" Language="vb" AutoEventWireup="false" Inherits="BaseApplication._Error" %>
<h2>Sorry</h2>
<p>An error occurred when you requested the <asp:Literal ID="litRequestedPage" Runat="server"></asp:Literal> page</p>
<p>Please notify the help desk.</p>
<table border=1>
	<tr>
		<td>
			<b>Error Information</b><br>
			Date and time: <asp:Literal ID="litCurrentTime" Runat="server"></asp:Literal> <br>
			Action: <asp:Literal ID="litRequestedPageError" Runat="server"></asp:Literal> <br>
			Error Source: <asp:Literal ID="litErrorSource" Runat="server"></asp:Literal><br>
			Error Message: <asp:Literal ID="litErrorMessage" Runat="server"></asp:Literal><br>
		</td>
	</tr>
</table>
<p>
	<table>
		<tr>
			<td>
				<asp:Button ID="btnReturn" Text="Return" Runat="server"></asp:Button>
			</td>
			<td>
				<asp:Button ID="btnHome" Text="Home" Runat="server"></asp:Button>
			</td>
		</tr>
	</table>
</p>
<p>
	<b>Please Note:</b>&nbsp;If this page has been displayed after choosing "Return" use "Home" to go back to the application.
</p>
<p>We apologize for the inconvenience and will work to correct the problem.</p>

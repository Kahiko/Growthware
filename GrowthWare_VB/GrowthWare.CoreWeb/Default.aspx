<%@ Page Title="Home Page" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false"
	CodeBehind="Default.aspx.vb" Inherits="GrowthWare.CoreWeb._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">


	<script type="text/javascript" language="javascript">

	</script>

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
	<h2>
		Welcome to ASP.NET!
	</h2>
	<p>
		To learn more about ASP.NET visit <a href="http://www.asp.net" title="ASP.NET Website">www.asp.net</a>.
	</p>
	<p>
		You can also find <a href="http://go.microsoft.com/fwlink/?LinkID=152368&amp;clcid=0x409"
			title="MSDN ASP.NET Docs">documentation on ASP.NET at MSDN</a>.
	</p>
	<p>
		Start Time:&nbsp;<asp:Label ID="lblStartTime" runat="server" /><br />
		Stop Time:&nbsp;<asp:Label ID="lblStopTime" runat="server" /><br />
		Duration:&nbsp;<asp:Label ID="lblDuration" runat="server" /><br />
	</p>
	<p>
		<asp:Button ID="Button1" runat="server" Text="Button" />
	</p>
</asp:Content>

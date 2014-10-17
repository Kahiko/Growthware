<%@ Page Title="" Language="C#" MasterPageFile="~/Public/Skins/Default/Default.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="GrowthWare.WebApplication.index" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%: Scripts.Render("~/bundles/GrowthWare")%>
    <%: Styles.Render("~/Content/GrowthWare")%>
    <%: Styles.Render("~/Content/GrowthWare")%>

	<script type="text/javascript" language="javascript">

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="MainContentDiv">
	</div>
</asp:Content>
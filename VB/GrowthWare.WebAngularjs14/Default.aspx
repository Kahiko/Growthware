<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Content/Skins/Default/Default.Master" CodeBehind="Default.aspx.vb" Inherits="GrowthWare.WebAngularjs14._Default" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <%: Styles.Render("~/Content/Growthware/Styles/UI")%>
    <script type="text/javascript">
        $(document).ready(function () {
        });
    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div id="MainContentDiv">
        <div data-ng-view class="view-slide"></div>
	</div>
</asp:Content>

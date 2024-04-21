<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Content/Skins/Default/Default.Master" CodeBehind="Default.aspx.vb" Inherits="GrowthWare.WebAngularJS._Default" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div id="MainContentDiv">
		<gw-client-message></gw-client-message>
        <div data-ng-view class="view-slide"></div>
	</div>
</asp:Content>

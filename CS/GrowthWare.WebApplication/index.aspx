<%@ Page Title="" Language="C#" MasterPageFile="~/Public/Skins/Default/Default.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="GrowthWare.WebApplication.index" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%: Styles.Render("~/Content/GrowthWare")%>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            GW.Navigation.NavigationController.LoadFunctions(afterLoadFunctions);
            var currentAction = GW.Common.getParameterByName("Action")
            if (currentAction != '') {
                GW.Navigation.NavigationController.LoadPage(currentAction, "MainContentDiv");

            } else {
                GW.Navigation.NavigationController.LoadPage("GenericHome", "MainContentDiv");
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="MainContentDiv">
	</div>
</asp:Content>

<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Public/Skins/Default/Default.Master" CodeBehind="index.aspx.vb" Inherits="GrowthWare.WebApplication.index" %>
<%@ Import Namespace="System.Web.Optimization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%: Styles.Render("~/Content/GrowthWare")%>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            GW.Navigation.NavigationController.LoadFunctions(afterLoadFunctions);
            var currentHash = window.location.hash.substring(1);
            currentHash = currentHash.replace("?Action=", "");
            if (currentHash != '') {
                GW.Navigation.NavigationController.LoadPage(currentHash, "MainContentDiv");

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

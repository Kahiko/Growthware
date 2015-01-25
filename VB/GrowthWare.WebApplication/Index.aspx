<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Public/Skins/Default/Default.Master" CodeBehind="Index.aspx.vb" Inherits="GrowthWare.WebApplication.Index" %>
<%@ Import Namespace="System.Web.Optimization" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
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
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div id="MainContentDiv">
	</div>
</asp:Content>


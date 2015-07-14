<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Content/Skins/Default/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="GrowthWare.WebApplication._Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <%: Styles.Render("~/Content/Growthware/Styles/UI")%>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            GW.Navigation.NavigationController.LoadFunctions(afterLoadFunctions);
            var currentAction = GW.Common.getParameterByName("Action")
            if (currentAction != '') {
                GW.Navigation.NavigationController.LoadPage(currentAction, "MainContentDiv");

            } else {
                GW.Navigation.NavigationController.LoadPage("Generic_Home", "MainContentDiv");
            }
        });
    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div id="MainContentDiv">
	</div>
</asp:Content>
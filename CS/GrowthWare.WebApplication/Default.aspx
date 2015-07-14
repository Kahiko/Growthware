<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Content/Skins/Default/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GrowthWare.WebApplication._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="MainContentDiv">
	</div>
</asp:Content>

<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Public/Skins/Default/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="GrowthWare.WebApplication._Default" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
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
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div id="MainContentDiv">
	</div>
</asp:Content>

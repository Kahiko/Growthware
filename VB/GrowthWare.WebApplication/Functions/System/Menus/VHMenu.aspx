﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VHMenu.aspx.vb" Inherits="GrowthWare.WebApplication.VHMenu" %>
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $("#cssMenu").html('');
        var menuType = 3;  //MenuType.Hierarchical = 3
        var options = GW.Model.DefaultWebMethodOptions();
        options.async = true;
        options.url = GW.Common.getBaseURL() + "/gw/api/Accounts/GetMenuData?menuType=" + menuType;
        GW.Common.JQueryHelper.callWeb(options, getHorizontalHierarchicaSuccess);
    });

    function getHorizontalHierarchicaSuccess(xhr, status, error) {
        var source = GW.Navigation.buildData(JSON.parse(xhr));
        var ul = $("<ul></ul>");
        ul.appendTo("#cssMenuVHMenu");
        GW.Navigation.buildUL(ul, source);
    }
</script>
<div id='cssMenuVHMenu' runat="server" />

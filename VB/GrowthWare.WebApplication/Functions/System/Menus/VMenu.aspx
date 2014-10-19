<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VMenu.aspx.vb" Inherits="GrowthWare.WebApplication.VMenu" %>

<%@ Register Assembly="GrowthWare.WebSupport" Namespace="GrowthWare.WebSupport.CustomWebControls" TagPrefix="CustomWebControls" %>

<div id="VMenu">
	<CustomWebControls:NavigationTrail id="NavigationTrail" Orientation="Vertical" runat="server"/>
</div>

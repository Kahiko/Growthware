<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logoff.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Accounts.Logoff" %>
<script type="text/javascript" language="javascript">
	$(document).ready(function () {
		window.location.hash = '';
		jQuery.event.trigger('~reLoadUI');
	});	
</script>
<div id="logoffMessage" style="display: none"></div>

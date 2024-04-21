<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Logoff.aspx.vb" Inherits="GrowthWare.WebApplication.Logoff" %>
<script type="text/javascript" language="javascript">
	$(document).ready(function () {
		window.location.hash = '';
		jQuery.event.trigger('~reLoadUI');
	});	
</script>
<div id="logoffMessage" style="display: none"></div>

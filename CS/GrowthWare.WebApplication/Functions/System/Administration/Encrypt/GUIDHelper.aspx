<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GUIDHelper.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.Encrypt.GUIDHelper" %>
<!DOCTYPE html>

<form id="frmGUIDHelper" runat="server">
    <div>
		<asp:TextBox CssClass="rounded" ID="txtGUID" runat="server" Width="304px" />
		<asp:HyperLink ID="hyperCopyText" runat="server" NavigateUrl="">Select Text</asp:HyperLink><br />
		<br />
		<input type="button" id="btnGUID" value="Get GUID" />    
    </div>
</form>
<script language="javascript" type="text/javascript">
<!--
		$(document).ready(function () {
			$("#btnGUID").button();
			$('#btnGUID').click(function () {
				var $txtGUID = $('#txtGUID');
				btnGUIDClick($txtGUID);
			});

			$('#hyperCopyText').click(function () {
				GW.Common.highlightAll('txtGUID');
			});

			$('#txtGUID').focus(function () {
				this.select();
			});
		});

		function btnGUIDClick($txtGUID) {
			getGUID($txtGUID);
			//$txtGUID.val('hi');

		};

		function getGUID($txtGUID) {
			var options = GW.Model.DefaultWebMethodOptions();
			options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Encrypt/GUIDHelper.aspx/GetGUID?Action=" + GW.Navigation.currentAction;
			options.contentType = 'application/json; charset=utf-8';
			options.dataType = 'json';
			options.timeout = 3000;
			GW.Common.JQueryHelper.callWeb(options, getGUIDSuccess, getGUIDError);
		};

		function getGUIDSuccess(xhr) {
			$('#txtGUID').val(xhr.d);
		}

		function getGUIDError(xhr, status, error) {

		}
//-->	
</script>
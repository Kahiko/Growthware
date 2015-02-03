<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LineCount.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.LineCount" %>

<!DOCTYPE html>

<form id="frmLineCount" runat="server">
	<div>
		<table border="0" cellpadding="0" cellspacing="3" width="100%">
			<tr>
				<td align="right">
					<span id="Span3" class="Form_LabelText">Exclusion Pattern :</span>
				</td>
				<td align="left" colspan="3">
					<input type="text" id="txtExclusionPattern" class="rounded" value="ASSEMBLYINFO, .DESIGNER., JQUERY-, JQUERY.VALIDATE, MODERNIZR-, jquery.tmpl., jquery.unobtrusive" style="width: 500px;" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right">
					<span id="Span1" class="Form_LabelText">Files :</span>
				</td>
				<td align="left" colspan="3">
					<input type="text" id="txtFiles" class="rounded" value="*.cs, *.aspx, *.ascx, *.asax, *.config, *.js" style="width: 500px;" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right">
					<span id="Span2" class="Form_LabelText">Directory :</span>
				</td>
				<td align="left">
					<input type="text" id="txtDirectoryName" class="rounded" style="width: 500px;" runat="server" />
				</td>
				<td>
					<span id="litTotalLines" runat="server"></span>
				</td>
			</tr>
			<tr>
				<td colspan="3" align="center">
					<input type="button" id="btnCount" class="rounded" onclick="javascript: getCount();" value="Count" runat="server" />
				</td>
			</tr>
		</table>
		<div style="overflow: auto; height: 350px;">
			<span id="litLineCount" runat="server"></span>
		</div>
	</div>
</form>

<script type="text/javascript">
	function getCount() {
	    $litLineCount = $("#<%=litLineCount.ClientID %>");
	    $litLineCount.css({ display: 'none' });
		var countData = new Object();
		countData.TheDirectory = $("#<%=txtDirectoryName.ClientID %>").val();
		countData.ExcludePattern = $("#<%=txtExclusionPattern.ClientID %>").val();
		countData.IncludeFiles = $("#<%=txtFiles.ClientID %>").val();
		var options = GW.Model.DefaultWebMethodOptions();
		options.async = true;
		options.url = GW.Common.getBaseURL() + "/Functions/System/LineCount.aspx/GetLineCount?Action=Line_Count";
		options.data = JSON.stringify({ countInfo: countData });
		options.contentType = 'application/json; charset=utf-8';
		options.dataType = 'json';
		options.timeout = 3000;
		GW.Common.JQueryHelper.callWeb(options, getCountSucess, getCountError);
	}

	function getCountSucess(xhr) {
		$litLineCount = $("#<%=litLineCount.ClientID %>");
		$litLineCount.css({ display: 'inline' });
		$litLineCount.html(xhr.d);
	}

	function getCountError(xhr, status, error) {
		$litLineCount = $("#<%=litLineCount.ClientID %>");
		$litLineCount.html('error getting content\n' + error);
	}

</script>

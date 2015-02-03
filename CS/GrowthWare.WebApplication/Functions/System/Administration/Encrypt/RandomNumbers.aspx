<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RandomNumbers.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.Encrypt.RandomNumbers" %>

<!DOCTYPE html>

<form id="frmRandomNumbers" runat="server">
    <div>
		<table border="0" cellpadding="0" cellspacing="3" width="90%">
			<tr>
				<td>
					Max:<br>
					<asp:TextBox ID="txtMaxNumber" CssClass="rounded" runat="server">255</asp:TextBox>
				</td>
				<td>
					Min:<br>
					<asp:TextBox ID="txtMinNumber" CssClass="rounded" runat="server">0</asp:TextBox>
				</td>
				<td nowrap>
					How Many Numbers:<br>
					<asp:TextBox ID="txtAmountOfNumbers" CssClass="rounded" runat="server">8</asp:TextBox>
				</td>
			</tr>
			<tr>
				<td align="center" colspan="3">
					<asp:TextBox ID="txtResults" CssClass="rounded" runat="server" Width="403px" />
				</td>
			</tr>
			<tr>
				<td align="center" colspan="3">
					<input type="button" id="btnGetNumbers" onclick="javascript: getNumbers();" value="Get Numbers" />
				</td>
			</tr>
		</table>    
    </div>
</form>
<script type="text/javascript" language="javascript">

	$(document).ready(function () {
		$("#btnGetNumbers").button();
	});

	function getNumbers() {
		var theData = {
			'amountOfNumbers': $("#<%=txtAmountOfNumbers.ClientID %>").val(),
			'maxNumber': $("#<%=txtMaxNumber.ClientID %>").val(),
			'minNumber': $("#<%=txtMinNumber.ClientID %>").val()
		}

		GW.Common.debug(theData);
		var options = GW.Model.DefaultWebMethodOptions();
		options.async = false;
		options.data = theData;
		options.contentType = 'application/json; charset=utf-8';
		options.dataType = 'json';
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Encrypt/RandomNumbers.aspx/GetRandomNumbers"
		GW.Common.JQueryHelper.callWeb(options, getNumbersSucess, getNumbersError);
	}

	function getNumbersSucess(xhr) {
		$("#<%=txtResults.ClientID %>").val(xhr.d);
	}

	function getNumbersError(xhr, status, error) {
		//alert(status);
	}	
</script>

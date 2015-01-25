<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SelectPreferences.aspx.vb" Inherits="GrowthWare.WebApplication.SelectPreferences" %>

<!DOCTYPE html>

<script type="text/javascript" language="javascript">

	function save() {
		var $mClientMessage = $('#ClientMSG');
		$mClientMessage.css({ display: 'none' });
		var mClientChoices = new GW.Model.ClientChoices();
		var mRColors = $('input[name=colors]:checked').val()
		var mColors = $('input[name=colors]:checked').val().split(',');
		//Blue,#C7C7C7,Black,#b6cbeb,#6699cc,#b6cbeb,#ffffff,#eeeeee
		mClientChoices.ColorScheme = mColors[0];
		mClientChoices.HeadColor = mColors[1];
		mClientChoices.HeaderForeColor = mColors[2];
		mClientChoices.RowBackColor = mColors[3];
		mClientChoices.AlternatingRowBackColor = mColors[4];
		mClientChoices.SubheadColor = mColors[5];
		mClientChoices.BackColor = mColors[6];
		mClientChoices.LeftColor = mColors[7];
		mClientChoices.RecordsPerPage = parseInt($("#<%=txtPreferedRecordsPerPage.ClientID %>").val());
		mClientChoices.Action = $("#<%=dropFavorite.ClientID %> option:selected").val();
		mClientChoices.SecurityEntityID = 0;
		GW.Common.debug(mClientChoices);
		var options = GW.Model.DefaultWebMethodOptions();
		options.async = false;
		options.data = mClientChoices;
		options.contentType = 'application/json; charset=utf-8';
		options.dataType = 'json';
		options.url = GW.Common.getBaseURL() + "/gw/api/Accounts/SaveClientChoices";
		GW.Common.JQueryHelper.callWeb(options, saveSucess, saveError);
	}

	function saveSucess(xhr) {
		GW.Navigation.NavigationController.Refresh();
		var $mClientMessage = $('#ClientMSG');
		var mRetHTML = 'Your selections have been saved.\n';
		$mClientMessage.css({ display: '' });
		$mClientMessage.html(mRetHTML.toString());
	}

	function saveError(xhr, status, error) {
		var $mClientMessage = $('#ClientMSG');
		var mRetHTML = 'Error changePasswordError on\n' + error;
		$mClientMessage.css({ display: 'none' });
		$mClientMessage.html(mRetHTML.toString());
	}

	$(document).ready(function () {
		$("#btnSave").button();
	});
</script>
<form id="frmSelectPreferences" runat="server">
    <div>
		<label id="ClientMSG" runat="server" style="font-weight: bold; color: red" /><br />
		<table border="0" cellpadding="0" cellspacing="3">
			<thead>
			</thead>
			<tr visible="false">
				<th>
				</th>
				<th>
				</th>
			</tr>
			<tr align="center">
				<td colspan="2">
					Choose the color scheme
				</td>
			</tr>
			<tr align="center">
				<td colspan="2">
					<table border="0">
						<tr>
							<td align="right">
								<input type="radio" name="colors" value="Blue,#C7C7C7,Black,#b6cbeb,#6699cc,#b6cbeb,#ffffff,#eeeeee" runat="server" id="Radio1" />
							</td>
							<td align="center">
								<span class="formLabelText"><b>Blue</b></span>
							</td>
							<td align="right">
								<input type="radio" name="colors" value="Green,#808577,White,#879966,#c5e095,#879966,#ffffff,#eeeeee" runat="server" id="Radio2" />
							</td>
							<td align="center">
								<span class="formLabelText"><b>Green</b></span>
							</td>
							<td align="right">
								<input type="radio" name="colors" value="Yellow,#CF9C00,Black,#f8bc03,#f8e094,#f8bc03,#ffffff,#f8e094" runat="server" id="Radio3" />
							</td>
							<td align="center">
								<span class="formLabelText"><b>Yellow</b></span>
							</td>
						</tr>
						<tr>
							<td align="right">
								<input type="radio" name="colors" value="Purple,#C7C7C7,Black,#be9cc5,#91619b,#be9cc5,#ffffff,#eeeeee" runat="server" id="Radio4" />
							</td>
							<td align="center">
								<span class="formLabelText"><b>Purple</b></span>
							</td>
							<td align="right">
								<input type="radio" name="colors" value="Red,#BA706A,White,#DE8587,#A72A49,#df867f,#ffffff,#eeeeee" runat="server" id="Radio5" />
							</td>
							<td align="center">
								<span class="formLabelText"><b>Red</b></span>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td align="right" style="width: 50%;">
					<span class="formLabelText">Favorite Link:</span>
				</td>
				<td align="left">
					<asp:DropDownList ID="dropFavorite" CssClass="rounded" runat="server" />
				</td>
			</tr>
			<tr>
				<td align="right">
					<span class="formLabelText">Prefered records per page: </span>
				</td>
				<td align="left">
					<asp:TextBox ID="txtPreferedRecordsPerPage" Columns="1" MaxLength="3" CssClass="rounded" runat="Server" />
					<asp:RequiredFieldValidator ControlToValidate="txtPreferedRecordsPerPage" Display="Dynamic" Text="(required)" ErrorMessage="You must enter a prefered name" CssClass="failureNotification" runat="Server" ID="Requiredfieldvalidator3" />
					<asp:RangeValidator ID="RangeValidator1" ControlToValidate="txtPreferedRecordsPerPage" Display="Dynamic" runat="server" ErrorMessage="Must be a number between 1 and 100" Type="Integer" MaximumValue="100" MinimumValue="1" />
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;
				</td>
				<td align="left">
				</td>
			</tr>
			<tr id="trSubmit" runat="server" style="height: auto;">
				<td colspan="10" align="center">
					<input type="button" id="btnSave" onclick="javascript: save();" value="Save" />
				</td>
			</tr>
			<tfoot>
			</tfoot>
		</table>    
    </div>
</form>
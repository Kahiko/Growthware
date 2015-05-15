<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEditNVP.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.NVP.AddEditNVP" %>
<%@ Register Assembly="GrowthWare.WebSupport" Namespace="GrowthWare.WebSupport.CustomWebControls" TagPrefix="CustomWebControls" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<form id="frmAddEditNVP" runat="server">
	<div id="helpPopup" style="display: none;">
	</div>
	<input type="hidden" id="hdnCanSaveRoles" runat="server" />
	<input type="hidden" id="hdnCanSaveGroups" runat="server" />
	<div>
		<div id="tabs">
			<ul>
				<li><a href="#tabsGeneral">General</a></li>
				<li id="rolesTab" runat="server"><a href="#tabsRoles">Roles</a></li>
				<li id="groupsTab" runat="server"><a href="#tabsGroups">Groups</a></li>
			</ul>
			<div id="tabsGeneral">
				<p>
					<table border="0" cellpadding="3" cellspacing="0" style="width: 700px">
						<tr>
							<td>
								<asp:TextBox ID="txtNVP_SEQ_ID" Style="display: none;" runat="server" />
								<table border="0" cellpadding="2" width="100%">
									<tr>
										<td align="center" colspan="2">
											<asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
										</td>
									</tr>
									<tr>
										<td>
											<span class="Form_LabelText">Table Name: </span>
										</td>
										<td align="left">
											<div id="lblTableName" runat="server">
											</div>
										</td>
									</tr>
									<tr>
										<td>
											<span class="Form_LabelText">Schema Name: </span>
										</td>
										<td colspan="3">
											<asp:TextBox ID="txtSchemaName" CssClass="rounded" MaxLength="128" runat="server" />
											<asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="failureNotification" Display="Dynamic" runat="server" ErrorMessage=" (required)" ControlToValidate="txtSchemaName" />
											<asp:RegularExpressionValidator ID="Alphanumeric" CssClass="failureNotification" Display="Dynamic" runat="server" ErrorMessage=" Must be alphanumeric." ControlToValidate="txtSchemaName" ValidationExpression="^[a-zA-Z0-9_]*$"></asp:RegularExpressionValidator>
											<asp:Image ID="imgWarningSchemaName" ImageUrl="~/Public/Images/help.gif" AlternateText="Help Image" Visible="false" runat="server" />
											<asp:Literal ID="litSchemaName" runat="server"></asp:Literal>
										</td>
									</tr>
									<tr>
										<td valign="top">
											<span class="Form_LabelText">Static Name: </span>
										</td>
										<td valign="top">
											<table border="0" cellpadding="0" cellspacing="0">
												<tr>
													<td>
														<asp:Literal Visible="False" ID="litSTATIC_NAME" runat="server" />
														<asp:TextBox ID="txtSTATIC_NAME" CssClass="rounded" MaxLength="128" runat="server" />
														<asp:Image ID="imgWarningStaticName" ImageUrl="~/Public/Images/help.gif" AlternateText="Help Image" Visible="false" runat="server" />
														<asp:Literal Visible="False" ID="litStaticName" runat="server"></asp:Literal>
														<asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="failureNotification" Display="Dynamic" runat="server" ErrorMessage=" (required)" ControlToValidate="txtSTATIC_NAME" />
														<asp:RegularExpressionValidator ID="Alphanumeric2" CssClass="failureNotification" Display="Dynamic" runat="server" ErrorMessage=" Must be alphanumeric." ControlToValidate="txtSTATIC_NAME" ValidationExpression="^[a-zA-Z0-9_]*$"></asp:RegularExpressionValidator>
													</td>
												</tr>
											</table>
										</td>
										<td id="tdStatus" valign="top" runat="server">
											<span class="Form_LabelText">Status: </span>
										</td>
										<td valign="top">
											<asp:DropDownList ID="dropStatus" CssClass="rounded" runat="server">
												<asp:ListItem Value="1">Active</asp:ListItem>
												<asp:ListItem Value="3">Disabled</asp:ListItem>
											</asp:DropDownList>
										</td>
									</tr>
									<tr>
										<td valign="top">
											<span class="Form_LabelText">Display: </span>
										</td>
										<td valign="top">
											<asp:TextBox ID="txtDisplay" MaxLength="128" TextMode="MultiLine" onKeyPress="return GW.Common.Validation.textboxMultilineMaxNumber(this,128)" CssClass="rounded" runat="Server" Height="90px" Width="330px" />
										</td>
										<td valign="top">
											<span class="Form_LabelText">Description: </span>
										</td>
										<td valign="top">
											<asp:TextBox ID="txtDescription" MaxLength="256" TextMode="MultiLine" onKeyPress="return GW.Common.Validation.textboxMultilineMaxNumber(this,256)" CssClass="rounded" runat="Server" Height="90px" Width="331px" />
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</p>
			</div>
			<div id="tabsRoles" runat="server">
				<p>
					<CustomWebControls:ListPicker Size="200" Rows="6" ID="ctlRoles" AllItemsText="All Roles" SelectedItemsText="Selected Roles" runat="Server" />
				</p>
			</div>
			<div id="tabsGroups" class="formSection" runat="server">
				<p>
					<CustomWebControls:ListPicker Size="200" Rows="6" ID="ctlGroups" AllItemsText="All Groups" SelectedItemsText="Selected Groups" runat="Server" />
				</p>
			</div>
		</div>
	</div>
</form>
<script type="text/javascript" language="javascript">
	$(document).ready(function () {
		$("#btnSave").button();
		$("#tabs").tabs();
		$("#tabs").tabs("option", "selected", 0);
	});

	function updateData() {
		var profile = {};
		var roles = '';
		var groups = '';
		var canSaveRoles = false;
		var canSaveGroups = false;
		roles = $.map($('#ctlRoles_DstList option'), function (e) { return $(e).val(); });
		groups = $.map($('#ctlGroups_DstList option'), function (e) { return $(e).val(); });
		canSaveRoles = $("#<%=hdnCanSaveRoles.ClientID %>").val();
		canSaveGroups = $("#<%=hdnCanSaveGroups.ClientID %>").val();
		profile = {};
		profile.SchemaName = $("#<%=txtSchemaName.ClientID %>").val();
		profile.STATIC_NAME = $("#<%=txtSTATIC_NAME.ClientID %>").val();
		profile.NVP_SEQ_ID = parseInt($("#<%=txtNVP_SEQ_ID.ClientID %>").val());
		profile.Display = $("#<%=txtDisplay.ClientID %>").val();
		profile.Description = $("#<%=txtDescription.ClientID %>").val();
		profile.Status = parseInt($("#<%=dropStatus.ClientID %> option:selected").val());
		profile.Groups = groups;
		profile.Roles = roles;
		return profile;
	}

	function saveAddEditNVP($dialogWindow) {
		if (Page_ClientValidate()) {
			var theData = updateData();
			// profile is defined in AddEditAccounts.aspx
			GW.Common.debug(theData);
			var options = GW.Model.DefaultWebMethodOptions();
			options.async = false;
			options.data = theData;
			options.contentType = 'application/json; charset=utf-8';
			options.dataType = 'json';
			options.url = GW.Common.getBaseURL() + "/gw/api/NameValuePair/SaveNameValuePair";
			GW.Common.JQueryHelper.callWeb(options, saveAddEditNVPSucess, saveAddEditNVPError);
			if (!($dialogWindow === undefined)) {
				$dialogWindow.dialog("destroy")
				$dialogWindow.remove();
			}
		}
	}

	function saveAddEditNVPSucess(xhr) {
		GW.Search.GetSearchResults();
	}

	function saveAddEditNVPError(xhr, status, error) {

	}
</script>
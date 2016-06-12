<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupControl.ascx.cs" Inherits="GrowthWare.WebApplication.UserControls.GroupControl" %>
<%@ Register Assembly="GrowthWare.WebSupport" Namespace="GrowthWare.WebSupport.CustomWebControls" TagPrefix="CustomWebControls" %>
<table cellspacing="0" cellpadding="3" border="0">
	<tr>
		<td align="left" style="width: 480px;">
			<table class="pickListTable">
				<!-- View Groups -->
				<tr class="pickListTableHeader">
					<td>
						View Groups
					</td>
				</tr>
				<tr>
					<td class="pickListTableHelp">
						Determines who can view pages.&nbsp;<img alt="Help" onclick="GW.Common.showHelpMSG(document.getElementById('helpViewGroups').innerHTML,'Help View Groups')" src='<%=ResolveUrl("~/Content/GrowthWare/Images/help.gif")%>' title=" &nbsp;Groups that may View&nbsp;" />
					</td>
				</tr>
				<tr>
					<td class="pickListTableContents">
						<CustomWebControls:ListPicker CssClass="listPicker" Rows="6" Size="200" ID="ctlViewGroups" runat="server" SelectedItemsText="Selected Groups" AllItemsText="All Groups"></CustomWebControls:ListPicker>
					</td>
				</tr>
			</table>
			<!-- View Groups -->
			<div id="helpViewGroups" style="display: none">
				Determines who can view pages. For example, if the Everyone group is not selected, then anonymous&nbsp;accounts cannot view the page such as the logon page.
			</div>
		</td>
		<td align="left" style="width: 480px;">
			<table class="pickListTable">
				<!-- Add Groups -->
				<tr class="pickListTableHeader">
					<td>
						Add Groups
					</td>
				</tr>
				<tr>
					<td class="pickListTableHelp">
						Determines who can add information on&nbsp;pages.&nbsp;<img alt="Help" onclick="GW.Common.showHelpMSG(document.getElementById('helpAddGroups').innerHTML,'Help Add Groups')" src='<%=ResolveUrl("~/Content/GrowthWare/Images/help.gif")%>' title="&nbsp;Groups that may Add&nbsp;" />
					</td>
				</tr>
				<tr>
					<td class="pickListTableContents">
						<CustomWebControls:ListPicker CssClass="listPicker" Rows="6" Size="200" ID="ctlAddGroups" runat="server" SelectedItemsText="Selected Groups" AllItemsText="All Groups"></CustomWebControls:ListPicker>
					</td>
				</tr>
			</table>
			<!-- Add Groups -->
			<div id="helpAddGroups" style="display: none">
				Determines who can add information on a page. For example, if the Administrators group is selected, then members of the Administrators group can use add buttons.
			</div>
		</td>
	</tr>
	<tr>
		<td align="left" style="width: 480px">
			<table class="pickListTable">
				<!-- Edit Groups -->
				<tr class="pickListTableHeader">
					<td>
						Edit Groups
					</td>
				</tr>
				<tr>
					<td class="pickListTableHelp">
						Determines who can edit or save&nbsp;information on a page.&nbsp;<img alt="Help" onclick="GW.Common.showHelpMSG(document.getElementById('helpEditGroups').innerHTML,'Help Edit Groups')" src='<%=ResolveUrl("~/Content/GrowthWare/Images/help.gif")%>' title="&nbsp;Groups that may Edit&nbsp;" />
					</td>
				</tr>
				<tr>
					<td class="pickListTableContents">
						<CustomWebControls:ListPicker ID="ctlEditGroups" CssClass="listPicker" Rows="6" Size="200" runat="server" SelectedItemsText="Selected Groups" AllItemsText="All Groups"></CustomWebControls:ListPicker>
					</td>
				</tr>
			</table>
			<!-- Edit Groups -->
			<div id="helpEditGroups" style="display: none">
				Determines who can edit or update information on a page. For example, if the Administrators group is selected, then members of the Administrators group can use edit, update, or save buttons.
			</div>
		</td>
		<td align="left" style="width: 480px">
			<table class="pickListTable">
				<!-- Delete Groups -->
				<tr class="pickListTableHeader">
					<td>
						Delete Groups
					</td>
				</tr>
				<tr>
					<td class="pickListTableHelp">
						Determines who can delete information on a page.&nbsp;<img alt="Help" onclick="GW.Common.showHelpMSG(document.getElementById('helpDeleteGroups').innerHTML,'Help Delete Groups')" src='<%=ResolveUrl("~/Content/GrowthWare/Images/help.gif")%>' title="Groups that may Delete" />
					</td>
				</tr>
				<tr>
					<td class="pickListTableContents">
						<CustomWebControls:ListPicker ID="ctlDeleteGroups" CssClass="listPicker" Rows="6" Size="200" runat="server" SelectedItemsText="Selected Groups" AllItemsText="All Groups"></CustomWebControls:ListPicker>
					</td>
				</tr>
			</table>
			<!-- Delete Groups -->
			<div id="helpDeleteGroups" style="display: none">
				Determines who can delete information on a page. For example, if theAdministrators group is selected, then members of the Administrators group can use delete buttons.
			</div>
		</td>
	</tr>
</table>
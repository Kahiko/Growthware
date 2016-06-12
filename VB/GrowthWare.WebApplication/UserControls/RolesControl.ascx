<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RolesControl.ascx.vb" Inherits="GrowthWare.WebApplication.RolesControl" %>
<%@ Register Assembly="GrowthWare.WebSupport" Namespace="GrowthWare.WebSupport.CustomWebControls" TagPrefix="CustomWebControls" %>
<table cellspacing="0" cellpadding="3" border="0">
	<tr>
		<td align="left" style="width: 480px">
            <table class="pickListTable">
                <tr class="pickListTableHeader">
                    <td>
                        View Roles
                    </td>
                </tr>
                <tr>
                    <td class="pickListTableHelp">
                        Determines who can view pages.&nbsp;<img alt="Help" onclick="GW.Common.showHelpMSG(document.getElementById('helpViewRoles').innerHTML,'Help View Roles')" src='<%=ResolveUrl("~/Content/GrowthWare/Images/help.gif")%>' title=" &nbsp;Roles that may View&nbsp;" />
                    </td>
                </tr>
                <tr>
                    <td class="pickListTableContents">
                        <CustomWebControls:ListPicker CssClass="listPicker" Rows="6" Size="200" ID="ctlViewRoles" runat="server" SelectedItemsText="Selected Roles" AllItemsText="All Roles"></CustomWebControls:ListPicker>
                    </td>
                </tr>
            </table>
			<!-- View Roles -->
			<div id="helpViewRoles" style="display: none">
				Determines who can view pages. For example, if the Everyone role is not selected, then anonymous&nbsp;accounts cannot view the page such as the logon page.
			</div>
		</td>
		<td align="left" style="width: 480px">
			<table class="pickListTable">
				<!-- Add Roles -->
				<tr class="pickListTableHeader">
					<td>
						Add Roles
					</td>
				</tr>
				<tr>
					<td class="pickListTableHelp">
						Determines who can add information on pages.&nbsp;<img alt="Help" onclick="GW.Common.showHelpMSG(document.getElementById('helpAddRoles').innerHTML,'Help Add Roles')" src='<%=ResolveUrl("~/Content/GrowthWare/Images/help.gif")%>' title="&nbsp;Roles that may Add&nbsp;" />
					</td>
				</tr>
				<tr>
					<td class="pickListTableContents">
						<CustomWebControls:ListPicker CssClass="listPicker" ID="ctlAddRoles" Rows="6" Size="200" runat="server" SelectedItemsText="Selected Roles" AllItemsText="All Roles"></CustomWebControls:ListPicker>
					</td>
				</tr>
			</table>
			<!-- Add Roles -->
			<div id="helpAddRoles" style="display: none">
				Determines who can add information on a page. For example, if the Administrators role is selected, then members of the Administrators role can use add buttons.
			</div>
		</td>
	</tr>
	<tr>
		<td align="left" style="width: 480px">
			<table class="pickListTable">
				<!-- Edit Roles -->
				<tr class="pickListTableHeader">
					<td>
						Edit Roles
					</td>
				</tr>
				<tr class="pickListTableHelp">
					<td>
						Determines who can edit or save&nbsp;information on a page.&nbsp;<img alt="Help" onclick="GW.Common.showHelpMSG(document.getElementById('helpEditRoles').innerHTML,'Help Edit Roles')" src='<%=ResolveUrl("~/Content/GrowthWare/Images/help.gif")%>' title="&nbsp;Roles that may Edit&nbsp;" />
					</td>
				</tr>
				<tr>
					<td class="pickListTableContents">
						<CustomWebControls:ListPicker CssClass="listPicker" ID="ctlEditRoles" Rows="6" Size="200" runat="server" SelectedItemsText="Selected Roles" AllItemsText="All Roles"></CustomWebControls:ListPicker>
					</td>
				</tr>
			</table>
			<!-- Edit Roles -->
			<div id="helpEditRoles" style="display: none">
				Determines who can edit or update information on a page. For example, if the Administrators role is selected, then members of the Administrators role can use edit, update, or save buttons.
			</div>
		</td>
		<td align="left" style="width: 480px">
			<table class="pickListTable">
				<!-- Delete Roles -->
				<tr class="pickListTableHeader">
					<td>
						Delete Roles
					</td>
				</tr>
				<tr>
					<td class="pickListTableHelp">
						Determines who can delete information on a page.&nbsp;<img alt="Help" onclick="GW.Common.showHelpMSG(document.getElementById('helpDeleteRoles').innerHTML,'Help Delete Roles')" src='<%=ResolveUrl("~/Content/GrowthWare/Images/help.gif")%>' title="Roles that may Delete" />
					</td>
				</tr>
				<tr>
					<td class="pickListTableContents">
						<CustomWebControls:ListPicker ID="ctlDeleteRoles" CssClass="listPicker" Rows="6" Size="200" runat="server" SelectedItemsText="Selected Roles" AllItemsText="All Roles"></CustomWebControls:ListPicker>
					</td>
				</tr>
			</table>
			<!-- Delete Roles -->
			<div id="helpDeleteRoles" style="display: none">
				Determines who can delete information on a page. For example, if theAdministrators role is selected, then members of the Administrators role can use delete buttons.
			</div>
		</td>
	</tr>
</table>
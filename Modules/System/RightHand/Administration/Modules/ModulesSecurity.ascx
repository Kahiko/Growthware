<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ModulesSecurity.ascx.vb" Inherits="BaseApplication.ModulesSecurity" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<table cellpadding="2" width="100%">
	<tr>
		<td colspan="2" class="Form_SubTitle" align="center">
			<B>&nbsp;Security&nbsp;</B>
		</td>
	</tr>
	<tr>
		<td align="center">
			<!-- View Roles -->
			<p>
				<table class="formSection">
					<tr>
						<td class="formSectionHeader">
							<img border="0" src="Images/Help/help.gif" align="right" onmouseover="showHelp('helpViewRoles')"
								onmouseout="hideHelp()">&nbsp;Roles that may View&nbsp;
						</td>
					</tr>
					<tr>
						<td class="formSectionHelp">
							Determines who can view pages.
						</td>
					</tr>
					<tr>
						<td class="formSectionContents">
							<CustomWebControls:ListPICKER id="ctlViewRoles" AllItemsText="All Roles" SelectedItemsText="Selected Roles" runat="server" />
						</td>
					</tr>
				</table>
			</p>
			<div id="helpViewRoles" style="DISPLAY:none">
				Determines who can view pages. For example, if the Everyone role is not 
				selected, then anonymous&nbsp;accounts cannot view the page such as the logon 
				page.
			</div>
			<!-- Add Roles -->
			<p>
				<table class="formSection">
					<tr>
						<td class="formSectionHeader">
							<img border="0" src="Images/Help/help.gif" align="right" onmouseover="showHelp('helpAddRoles')"
								onmouseout="hideHelp()">&nbsp;Roles that may Add
						</td>
					</tr>
					<tr>
						<td class="formSectionHelp">
							Determines who can add information on&nbsp;pages.
						</td>
					</tr>
					<tr>
						<td class="formSectionContents">
							<CustomWebControls:ListPICKER id="ctlAddRoles" AllItemsText="All Roles" SelectedItemsText="Selected Roles" runat="server" />
						</td>
					</tr>
				</table>
			</p>
			<div id="helpAddRoles" style="DISPLAY:none">
				Determines who can add information on a page. For example, if the 
				Administrators role is selected, then members of the Administrators role can 
				use add buttons.
			</div>
			<!-- Edit Roles -->
			<p>
				<table class="formSection">
					<tr>
						<td class="formSectionHeader">
							<img border="0" src="Images/Help/help.gif" align="right" onmouseover="showHelp('helpEditRoles')"
								onmouseout="hideHelp()">&nbsp;Roles that may Edit
						</td>
					</tr>
					<tr>
						<td class="formSectionHelp"> Determines who can&nbsp;save information on a page.
						</td>
					</tr>
					<tr>
						<td class="formSectionContents">
							<CustomWebControls:ListPICKER id="ctlEditRoles" AllItemsText="All Roles" SelectedItemsText="Selected Roles" runat="server" />
						</td>
					</tr>
				</table>
			</p>
			<div id="helpEditRoles" style="DISPLAY:none">
				Determines who can edit or update information on a page. For example, if the 
				Administrators role is selected, then members of the Administrators role can 
				use edit, update, or save buttons.
			</div>
			<!-- Delete Roles -->
			<p>
				<table class="formSection">
					<tr>
						<td class="formSectionHeader">
							<img border="0" src="Images/Help/help.gif" align="right" onmouseover="showHelp('helpDeleteRoles')"
								onmouseout="hideHelp()">&nbsp;Roles&nbsp;that may Delete&nbsp;
						</td>
					</tr>
					<tr>
						<td class="formSectionHelp">
							Determines who can delete information on a page.
						</td>
					</tr>
					<tr>
						<td class="formSectionContents">
							<CustomWebControls:ListPICKER id="ctlDeleteRoles" AllItemsText="All Roles" SelectedItemsText="Selected Roles"
								runat="server" />
						</td>
					</tr>
				</table>
			</p>
			<div id="helpDeleteRoles" style="DISPLAY:none">
				Determines who can delete information on a page. For example, if 
				theAdministrators role is selected, then members of the Administrators role can 
				use delete buttons.
			</div>
		</td>
	</tr>
</table>

<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.RolesControl" Codebehind="RolesControl.ascx.vb" %>
<script type="text/javascript" src="Scripts/JS/Common/floatingWindow.js"></script>
<script type="text/javascript" src="Scripts/JS/Common/modalWindow.js"></script>
<table cellSpacing=0 cellPadding=2 width="100%" border=0>
	<tr>
		<td class=Form_SubTitle align=center colSpan=2>
			<B>&nbsp;Roles&nbsp;</B>
		</td>
	</tr>
	<tr>
		<td align=center>
			<table class=formSection width=0px> <!-- View Roles -->
				<tr>
					<td class=formSectionHeader>
						<img onclick="showMSG(document.getElementById('helpViewRoles').innerHTML,1,'Help View Roles')" src="Images/Help/help.gif" align=right border=0 >
							&nbsp;Roles that may View&nbsp; 
						</img>
					</td>
				</tr>
				<tr>
					<td class=formSectionHelp>
						Determines who can view pages.
					</td>
				</tr>
				<tr>
					<td class=formSectionContents>
						<CustomWebControls:LISTPICKER id=ctlViewRoles runat="server" SelectedItemsText="Selected Roles" AllItemsText="All Roles"></CustomWebControls:LISTPICKER>
					</td>
				</tr>
			</table><!-- View Roles -->
			<div id=helpViewRoles style="DISPLAY: none">
				Determines who can view pages. For example, if the Everyone role is not selected, then anonymous&nbsp;accounts cannot view the page such as the logon page.
			</div>
		</td>
	</tr>
	<tr>
		<td align=center>
			<table class=formSection> <!-- Add Roles -->
				<tr>
					<td class=formSectionHeader>
						<IMG onclick="showMSG(document.getElementById('helpAddRoles').innerHTML,2,'Help Add Roles')" src="Images/Help/help.gif" align=right border=0>
							&nbsp;Roles that may Add&nbsp;
						</IMG>
					</td>
				</tr>
				<tr>
					<td class=formSectionHelp>
						Determines who can add information on&nbsp;pages.
					</td>
				</tr>
				<tr>
					<td class=formSectionContents>
						<CustomWebControls:LISTPICKER id=ctlAddRoles runat="server" SelectedItemsText="Selected Roles" AllItemsText="All Roles"></CustomWebControls:LISTPICKER>
					</td>
				</tr>
			</table><!-- Add Roles -->
			<div id=helpAddRoles style="DISPLAY: none">
				Determines who can add information on a page. For example, if the Administrators role 
				is selected, then members of the Administrators role can use add buttons. 
			</div>
		</td>
	</tr>
	<tr>
		<td align=center>
			<table class=formSection> <!-- Edit Roles -->
				<tr>
					<td class=formSectionHeader>
						<img onclick="showMSG(document.getElementById('helpEditRoles').innerHTML,3,'Help Edit Roles')" src="Images/Help/help.gif" align=right border=0>
							&nbsp;Roles that may Edit
						</img>
					</td>
				</tr>
				<tr>
					<td class=formSectionHelp>
						Determines who can edit or save&nbsp;information on a page.
					</td>
				</tr>
				<tr>
					<td class=formSectionContents>
						<CustomWebControls:LISTPICKER id=ctlEditRoles runat="server" SelectedItemsText="Selected Roles" AllItemsText="All Roles"></CustomWebControls:LISTPICKER>
					</td>
				</tr>
			</table><!-- Edit Roles -->
			<div id=helpEditRoles style="DISPLAY: none">
				Determines who can edit or update information on a 
				page. For example, if the Administrators role is selected, then members of 
				the Administrators role can use edit, update, or save buttons.
			</div>
		</td>
	</tr>
	<tr>
		<td align=center>
			<table class=formSection> <!-- Delete Roles -->
				<tr>
					<td class=formSectionHeader>
						<img onclick="showMSG(document.getElementById('helpDeleteRoles').innerHTML,4,'Help Delete Roles')" src="Images/Help/help.gif" align=right border=0>
							Roles that may Delete
						</img>
					</td>
				</tr>
				<tr>
					<td class=formSectionHelp>
						Determines who can delete information on a page.
					</td>
				</tr>
				<tr>
					<td class=formSectionContents>
						<CustomWebControls:LISTPICKER id=ctlDeleteRoles runat="server" SelectedItemsText="Selected Roles" AllItemsText="All Roles"></CustomWebControls:LISTPICKER>
					</td>
				</tr>
			</table><!-- Delete Roles -->
			<div id=helpDeleteRoles style="DISPLAY: none">
				Determines who can delete information on a page. For 
				example, if theAdministrators role is selected, then members of the 
				Administrators role can use delete buttons.
			</div>
		</td>
	</tr>
</table>

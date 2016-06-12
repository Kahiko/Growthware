<%@ Register Assembly="ApplicationBaseCommon" Namespace="ApplicationBase.Common.CustomWebControls" TagPrefix="CustomWebControls" %>
<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.GroupsControl" Codebehind="GroupsControl.ascx.vb" %>
<table cellpadding="2" width="100%">
	<tr>
		<td colspan="2" class="Form_SubTitle" align="center">
			<B>&nbsp;Groups&nbsp;</B>
		</td>
	</tr>
	<tr>
		<td align="center">
			<!-- View Groups -->
			<p>
				<table class="formSection">
					<tr>
						<td class="formSectionHeader">
							<img border="0" src="Images/Help/help.gif" align="right" onclick="showMSG(document.getElementById('helpViewGroups').innerHTML,1,'Help View Groups')">
							&nbsp;Groups that may View&nbsp;
						</td>
					</tr>
					<tr>
						<td class="formSectionHelp">
							Determines who can view pages.
						</td>
					</tr>
					<tr>
						<td class="formSectionContents">
							<CustomWebControls:ListPICKER id="ctlViewGroups" AllItemsText="All Groups" SelectedItemsText="Selected Groups" runat="server" />
						</td>
					</tr>
				</table>
			</p>
			<div id="helpViewGroups" style="DISPLAY:none">
				Determines who can view pages. For example, if the Everyone Group is not 
				selected, then anonymous&nbsp;accounts cannot view the page such as the logon 
				page.
			</div>
			<!-- Add Groups -->
			<p>
				<table class="formSection">
					<tr>
						<td class="formSectionHeader">
							<img border="0" src="Images/Help/help.gif" align="right" onclick="showMSG(document.getElementById('helpAddGroups').innerHTML,2,'Help Add Groups')">&nbsp;Groups that may Add
						</td>
					</tr>
					<tr>
						<td class="formSectionHelp">
							Determines who can add information on&nbsp;pages.
						</td>
					</tr>
					<tr>
						<td class="formSectionContents">
							<CustomWebControls:ListPICKER id="ctlAddGroups" AllItemsText="All Groups" SelectedItemsText="Selected Groups" runat="server" />
						</td>
					</tr>
				</table>
			</p>
			<div id="helpAddGroups" style="DISPLAY:none">
				Determines who can add information on a page. For example, if the 
				Administrators Group is selected, then members of the Administrators Group can 
				use add buttons.
			</div>
			<!-- Edit Groups -->
			<p>
				<table class="formSection">
					<tr>
						<td class="formSectionHeader">
							<img border="0" src="Images/Help/help.gif" align="right" onclick="showMSG(document.getElementById('helpEditGroups').innerHTML,3,'Help Edit Groups')">&nbsp;Groups that may Edit
						</td>
					</tr>
					<tr>
						<td class="formSectionHelp"> Determines who can&nbsp;save information on a page.
						</td>
					</tr>
					<tr>
						<td class="formSectionContents">
							<CustomWebControls:ListPICKER id="ctlEditGroups" AllItemsText="All Groups" SelectedItemsText="Selected Groups" runat="server" />
						</td>
					</tr>
				</table>
			</p>
			<div id="helpEditGroups" style="DISPLAY:none">
				Determines who can edit or update information on a page. For example, if the 
				Administrators Group is selected, then members of the Administrators Group can 
				use edit, update, or save buttons.
			</div>
			<!-- Delete Groups -->
			<p>
				<table class="formSection">
					<tr>
						<td class="formSectionHeader">
							<img border="0" src="Images/Help/help.gif" align="right" onclick="showMSG(document.getElementById('helpDeleteGroups').innerHTML,4,'Help Delete Groups')">&nbsp;Groups&nbsp;that may Delete&nbsp;
						</td>
					</tr>
					<tr>
						<td class="formSectionHelp">
							Determines who can delete information on a page.
						</td>
					</tr>
					<tr>
						<td class="formSectionContents">
							<CustomWebControls:ListPICKER id="ctlDeleteGroups" AllItemsText="All Groups" SelectedItemsText="Selected Groups"
								runat="server" />
						</td>
					</tr>
				</table>
			</p>
			<div id="helpDeleteGroups" style="DISPLAY:none">
				Determines who can delete information on a page. For example, if 
				theAdministrators Group is selected, then members of the Administrators Group can 
				use delete buttons.
			</div>
		</td>
	</tr>
</table>

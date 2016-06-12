<%@ Control Language="VB" AutoEventWireup="false" Inherits="ApplicationBase.Home" Codebehind="Home.ascx.vb" %>
<table cellpadding="0" cellspacing="0" style="font: 8pt verdana, arial" width="100%">
	<tr>
		<td align="left" style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px" valign="top">
			<asp:Image ID="SideImage" runat="server" ImageUrl="sidebar_" />
		</td>
		<td align="left" style="padding-right: 15px; padding-left: 15px; padding-bottom: 15px; padding-top: 15px" valign="top">
			Welcome to the <b>
				<asp:Label ID="lblAppName" runat="server">lblAppName</asp:Label></b>.&nbsp;&nbsp;The application was developed to provide a data store independent/generic code base to supply the following functionality and features.
			<ul>
				<li>Account Management
					<ul>
						<li>Account creation </li>
						<li>Account edit (including the ability for each client to edit their personal information) </li>
						<li>Account authentication
							<ul>
								<li>By internal data store </li>
								<li>By LDAP/ADSI </li>
							</ul>
						</li>
						<li>Security Management </li>
					</ul>
				</li>
				<li>An E-Mail facility </li>
				<li>Reporting
					<ul>
						<li>Supporting Business Objects </li>
						<li>Supporting Crystal reports objects </li>
					</ul>
				</li>
				<li>Logging </li>
				<li>Business Unit Management
					<ul>
						<li>Manage properties of a Business Unit </li>
						<li>Add a Business Unit through the UI </li>
						<li>Unlimited number of Parent/Child Business Units </li>
					</ul>
				</li>
				<li>Role management
					<ul>
						<li>Adding a role </li>
						<li>Editing a role </li>
						<li>Deleting a role </li>
					</ul>
					Note: Role management is by Business Unit, roles are created and assigned to Page/Modules for 4 “Permissions” (View, Add, Edit, and Delete). Roles are also assigned to Accounts. When both the Client/Account and Module/Page for any given permission has the same role then that permission is granted. The role based security is accumulative meaning if the account is in a any role that has a permission then the permission is granted. </li>
				<li>Module/Page Management
					<ul>
						<li>Add Modules/Pages </li>
						<li>Edit Modules/Pages </li>
						<li>Delete Modules/Pages </li>
						<li>Security Management – Assigning roles by Business Units </li>
					</ul>
				</li>
				<li>Client Choice Management
					<ul>
						<li>As a client makes choices in the application the information is stored in the data store and retrieved when necessary. </li>
					</ul>
				</li>
				<li>Personalization
					<ul>
						<li>Choose from 5 color schemas </li>
						<li>Select your favoriate action </li>
						<li>Choose the number of records to show per page </li>
					</ul>
				</li>
				<li>Skinning
					<ul>
						<li>Skinning is the ability to put any look and feel to the application without changing the underpinnings of the application code. </li>
						<li>Skins can be applied on the fly </li>
						<li>Skins are setup on a per Business Unit basis, so when a Business Unit is selected the look an feel change to the business units skin. </li>
						<li>Skins make an excellent visual queue making it easier to identify the business unit your working with </li>
					</ul>
				</li>
				<li>Quick development
					<ul>
						<li>When using the base application as a staring point the setup time should be less than a single day, after the setup time all efforts can be concentrated on the business logic at hand saving precious development time. </li>
						<li>Applications starting with the Base Application start with a given level of <b>Q</b>uality <b>A</b>ssurance and behavior </li>
					</ul>
				</li>
			</ul>
		</td>
	</tr>
</table>
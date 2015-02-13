<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddEditWebConfig.aspx.vb" Inherits="GrowthWare.WebApplication.AddEditWebConfig" %>

<!DOCTYPE html>
<form id="frmAddEditWebConfig" runat="server">
	<div id="helpPopup"></div>
	<div>
		<div id="tabs">
			<ul>
				<li><a href="#tabsGeneral">General</a></li>
				<li><a href="#tabsEnvironment">Environment</a></li>
			</ul>
			<div id="tabsGeneral">
				<table border="0" cellpadding="3" cellspacing="0">
					<tr>
						<td colspan="6">
							<asp:Label CssClass="failureNotification" ID="lblError" Visible="false" runat="server"></asp:Label>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Append to file :</span>
						</td>
						<td valign="top" style="width: 16px;">
							<img onclick="GW.Common.showHelpMSG('Works with the applicaiton logger appender to append to the log file.','Help Append to file')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left" valign="top">
							<asp:DropDownList ID="dropAppend_To_File" CssClass="rounded" runat="server">
								<asp:ListItem Value="True" Text="True" />
								<asp:ListItem Value="False" Text="False" />
							</asp:DropDownList>
						</td>
						<td align="right" valign="top">
							<span class="formLabelText">Always show lefthand navigation :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Used to determine if the left hand menu should always be displyed.','Always show lefthand navigation')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left" valign="top">
							<asp:DropDownList ID="dropAlways_Left_Nav" CssClass="rounded" runat="server">
								<asp:ListItem Value="True" Text="True" />
								<asp:ListItem Value="False" Text="False" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Application name for display :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('This is what should be displayed as the application name.','Application name for display')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left" valign="top">
							<asp:TextBox ID="txtApp_Displayed_Name" runat="server" CssClass="rounded" />
						</td>
						<td align="right" valign="top">
							<span class="formLabelText">Security Entity Translation :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Most projects will not call what the system refers to as a Security Entity.  This is used in various pages in the system to in stead of Security Entity.','Security Entity Translation')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left" valign="top">
							<asp:TextBox ID="txtSecurity_Entity_Translation" runat="server" 
								CssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Base Page :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Used as the redirection file name.  If blank no page will be displayed in the URL','Base Page')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left" valign="top">
							<asp:TextBox ID="txtBase_Page" runat="server" CssClass="rounded" />
						</td>
						<td align="right" valign="top">
							<span class="formLabelText">Central Management :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Prevents caching and allows for central security management for multipule databases.','Central Management')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" /></td>
						<td align="left" valign="top">
							&nbsp;<asp:DropDownList ID="dropCentralManagement" CssClass="rounded" runat="server">
								<asp:ListItem Value="True" Text="True" />
								<asp:ListItem Value="False" Text="False" />
							</asp:DropDownList></td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Conversion Pattern :</span>
						</td>
						<td valign="top" style="width: 16px">
							<img onclick="GW.Common.showHelpMSG('Conversion pattern for the application logger.','Help Append to file','Conversion Pattern')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" /></td>
						<td align="left" valign="top">
							&nbsp;<asp:TextBox ID="txtConversion_Pattern" runat="server" 
								TextMode="MultiLine" CssClass="rounded" /></td>
						<td align="right" valign="top">
							<span class="formLabelText">Default Action :</span></td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Used as the navigation point for the anonymous account.','Default Action')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />&nbsp;</td>
						<td align="left" valign="top">
							&nbsp;<asp:TextBox ID="txtDefault_Action" runat="server" 
								CssClass="rounded" /></td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Default Authenticated Action :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Used as the navigation point for anyone logged onto the system and the action is unknown.','Default Authenticated Action')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" /></td>
						<td align="left" valign="top">
							&nbsp;<asp:TextBox ID="txtDefault_Authenticated_Action" 
								runat="server" CssClass="rounded" /></td>
						<td align="right" valign="top">
							<span class="formLabelText">Expected Up By :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Message to display on the under construction page.','Expected Up By')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" /></td>
						<td align="left" valign="top">
							&nbsp;<asp:TextBox ID="txtExpected_Up_By" CssClass="rounded" runat="server" /></td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Encryption Type :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('The type of encryption will be used within the applicaiton.','Encryption Type')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" /></td>
						<td align="left" valign="top">
							&nbsp;<asp:DropDownList ID="dropEncryption_Type" CssClass="rounded" runat="server">
								<asp:ListItem Value="1" Text="TripleDES" />
								<asp:ListItem Value="2" Text="DES" />
								<asp:ListItem Value="3" Text="None" />
							</asp:DropDownList></td>
						<td align="right" valign="top">
							<span class="formLabelText">Enforce Action :</span></td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Causes the application to ensure the Action= is always apart of the URL.','Enforce Action')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" /></td>
						<td align="left" valign="top">
							&nbsp;<asp:DropDownList ID="dropEnforce_Action" CssClass="rounded" runat="server">
								<asp:ListItem Value="True" Text="True" />
								<asp:ListItem Value="False" Text="False" />
							</asp:DropDownList></td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Environment :</span></td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Determins which environment settings to use.','Environment')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" /></td>
						<td align="left" valign="top">
							&nbsp;<asp:DropDownList ID="dropEnvironment" CssClass="rounded" runat="server">
							</asp:DropDownList></td>
						<td align="right" valign="top">
							<span class="formLabelText">Database Status : </span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Used to determine the database status.<br>When Online database access is permitted.<br>When Install the install database process occurs.<br>When OffLine no database access is attempted','Database Status')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" /></td>
						<td align="left" valign="top">
							&nbsp;<asp:DropDownList ID="dropDB_Status" CssClass="rounded" runat="server">
								<asp:ListItem Value="Install" Text="Install" />
								<asp:ListItem Value="OnLine" Text="OnLine" />
								<asp:ListItem Value="OffLine" Text="OffLine" />
							</asp:DropDownList></td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Enable Server Side View State :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Determines if the application will maintain view state.  <br>Doing this will decrease the amount of viewstate data sent to the client.<br> this is also slightly more secure because any data obtained will only make sense to this application.','Enable Server Side View State')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left" valign="top">
							<asp:DropDownList ID="dropServer_Side_View_State" CssClass="rounded" runat="server">
								<asp:ListItem Value="True" Text="True" />
								<asp:ListItem Value="False" Text="False" />
							</asp:DropDownList>
						</td>
						<td align="right" valign="top">
							<span class="formLabelText">Number of View State Pages :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Sets the number of pages view state data to keep.','Number of View State Pages')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left" valign="top">
							<asp:TextBox ID="txtServer_Side_View_State_Pages" runat="server" 
								CssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Under Construction :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('When set to true the system will only show the under construction page.','Under Construction')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left" valign="top">
							<asp:DropDownList ID="dropUnder_Construction" CssClass="rounded" runat="server">
								<asp:ListItem Value="True" Text="True" />
								<asp:ListItem Value="False" Text="False" />
							</asp:DropDownList>
						</td>
						<td valign="top">
								
						</td>
						<td valign="top">
								
						</td>
						<td valign="top">
								
						</td>
					</tr>
				</table>                
			</div>
			<div id="tabsEnvironment">
				<table border="0" cellpadding="3" cellspacing="0">
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Environment :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Selects which evnironment settings to save.<br>Use New to create a new set of environment specific settings.','Environment')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:DropDownList ID="dropEnvironments" CssClass="rounded" runat="server">
									
							</asp:DropDownList>&nbsp;
							<asp:CheckBox ID="chkDelete" Text=" Delete Environment" sytle="display: none;" Checked="false" runat="server" />
						</td>
					</tr>
					<tr id="trNewEnvironment">
						<td align="right" valign="top">
							<span class="formLabelText">New Environment :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Name of the new environment.<br>Use New to create a new set of environment specific settings.','New environment name')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td>
							<asp:TextBox ID="txtNewEnvironment" runat="server" CssClass="rounded" />
							<asp:TextBox ID="txtEnvironments" sytle="display: none;" runat="server" CssClass="rounded" />
						</td>					
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Virtual directory name :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('When running on a non IIS server this setting should be the virtual directory name<br /> else this should be blank.','Virtual Directory Name')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtApp_Name" runat="server" CssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Authentication Type :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Defines what authentication method to use.','Authentication Type')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:DropDownList ID="dropAuthentication_Type" CssClass="rounded" runat="server">
								<asp:ListItem Value="Internal" Text="Internal" />
								<asp:ListItem Value="LDAP" Text="LDAP" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Autocreate :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Used to ','Autocreate')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:DropDownList ID="dropAuto_Create" CssClass="rounded" runat="server">
								<asp:ListItem Value="True" Text="True" />
								<asp:ListItem Value="False" Text="False" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Autocreate client choices account :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Defind the account to use for client choices','Autocreate Client Choices Account')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtAuto_Create_ClientChoicesAccount" runat="server" Width="400px" ssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Autocreate Security Entity:</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Defines what security entity to use when auto creating an account.','Autocreate Security Entity')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtAuto_Create_SecurityEntity" runat="server" Width="400px" ssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Autocreate Roles:</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Defines what roles to use when auto creating an account.','Autocreate Roles')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtAuto_Create_Roles" runat="server" Width="400px" ssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Data Access Layer :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Which data access layer will the system be using.','Data Access Layer')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:DropDownList ID="dropDAL" CssClass="rounded" runat="server">
								<asp:ListItem Value="SQLServer" Text="SQL Server" />
								<asp:ListItem Value="Oracle" Text="Oracle" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Data access layer DLL Name :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('This is the name of the DLL for you data access layer without \'.DLL\'.','Data access layer DLL Name')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtAssembly_Name" runat="server" Width="400px" CssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Data access layer Namespace :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('The namespace the class object resides in.','Data access layer Namespace')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtName_Space" runat="server" Width="400px" ssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Connection String :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('This is the connection string.','Connection String')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtConnectionstring" TextMode="multiLine" MaxLength="512" 
								onKeyPress="return textboxMultilineMaxNumber(this,512)" runat="server" Rows="5" 
								Wrap="true" Width="400px" CssClass="rounded" />&nbsp;
							<asp:HyperLink ID="hyperConnectionString" NavigateUrl="http://www.connectionstrings.com/" Target="_blank" runat="server" >Build</asp:HyperLink>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Default Security Entity ID :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('This is the default Security Entity the system should use ... normaly this would be \'1\'.','Default Security Entity ID')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtDefault_Security_Entity_ID" runat="server" 
								CssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Enable Cache :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Enables cache, if false nothing will be cached.','Enable Cache')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:DropDownList ID="dropEnable_Cache" CssClass="rounded" runat="server">
								<asp:ListItem Value="True" Text="True" />
								<asp:ListItem Value="False" Text="False" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Enable Encryption :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Enables Encryption','Enable Encryption')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:DropDownList ID="dropEnable_Encryption" CssClass="rounded" runat="server">
								<asp:ListItem Value="True" Text="True" />
								<asp:ListItem Value="False" Text="False" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Enable Pooling :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('Enables Pooling','Enable Pooling')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:DropDownList ID="dropEnable_Pooling" CssClass="rounded" runat="server">
								<asp:ListItem Value="True" Text="True" />
								<asp:ListItem Value="False" Text="False" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Force HTTPS :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('When true the system will redirect to https.','Force HTTPS')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:DropDownList ID="dropForce_HTTPS" CssClass="rounded" runat="server">
								<asp:ListItem Value="True" Text="True" />
								<asp:ListItem Value="False" Text="False" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">LDAP Domain :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('The name of the LDAP domain.','LDAP Domain')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtLDAP_Domain" runat="server" CssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">LDAP Server :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('The name or IP of the LDAP server.','LDAP Server')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtLDAP_Server" runat="server" Width="400px" 
								CssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Log Path :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('The path for the logs.','Log Path')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtLog_Path" runat="server" Width="400px" CssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Log Retention :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('How many days to retain log files.  0 (zero) never deletes the logs.','Log Retention')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtLogRetention" runat="server" Width="400px" 
								CssClass="rounded" />
							<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLogRetention" ErrorMessage="(required)"></asp:RequiredFieldValidator>
							<asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtLogRetention" Type="Integer" MinimumValue="0" MaximumValue="120" ErrorMessage="Must be a number between 0 and 120"></asp:RangeValidator>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Registering Roles :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('The roles used for registering accounts.  Comma seporated.','Registering Roles')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtRegisteringRoles" runat="server" Width="400px" 
								CssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Registration Post Action :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('The action take after registration..','Registration Post Action')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtRegistration_Post_Action" runat="server" Width="400px" 
								CssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Starting Log Priority :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('The value to use when the application first starts.','Starting Log Priority')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:DropDownList ID="dropLog_Priority" CssClass="rounded" runat="server">
								<asp:ListItem Value="Debug" Text="Debug" />
								<asp:ListItem Value="Info" Text="Info" />
								<asp:ListItem Value="Warn" Text="Warn" />
								<asp:ListItem Value="Error" Text="Error" />
								<asp:ListItem Value="Fatal" Text="Fatal" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Skining type :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('The value to use when the application first starts.','Skining type')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:DropDownList ID="dropSkin_Type" CssClass="rounded" runat="server">
								<asp:ListItem Value="Internal" Text="Internal" />
								<asp:ListItem Value="MASTER_PAGES" Text="Master Pages" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">SMTP Server :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('The SMTP Server name.','SMTP Server')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtSMTP_Server" runat="server" Width="400px" 
								CssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">SMTP Authentication Account :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('The account used to authenticate to the SMTP Server.  Leave Blank when not needed','SMTP Authentication Account')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtSMTP_Account" runat="server" Width="400px" 
								CssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">SMTP Authentication Password :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('The password to use when authenticating to the SMTP Server.  Leave Blank when not needed','SMTP Authentication Password')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtSMTP_Password" runat="server" Width="400px" 
								CssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">SMTP From :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('The email addres to use when the application sends an email.','SMTP From')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtSMTP_From" runat="server" Width="400px" 
								CssClass="rounded" />
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Synchronize Password :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('When true the system will allways set the password when an account logs on.','Synchronize Password')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:DropDownList ID="dropSynchronize_Password" CssClass="rounded" runat="server">
								<asp:ListItem Value="True" Text="True" />
								<asp:ListItem Value="False" Text="False" />
							</asp:DropDownList>
						</td>
					</tr>
					<tr>
						<td align="right" valign="top">
							<span class="formLabelText">Failed Attempts :</span>
						</td>
						<td valign="top">
							<img onclick="GW.Common.showHelpMSG('The number of times an account can fail login before the account is disabled.<br>Set to 0 to turn off.','Failed Attempts')" style="border: 0px;" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help_and_support.png")%>' alt="Help" />
						</td>
						<td align="left">
							<asp:TextBox ID="txtFailedAttempts" runat="server" Width="400px" 
								CssClass="rounded" />
						</td>
					</tr>
				</table>                
			</div>    
		</div>
		<div align="right" style="width: 100%">
			<input type="button" id="btnSave" value="Save" />
		</div>
	</div>
</form>
<script type="text/javascript" language="javascript">
	$(document).ready(function () {
	    var $btnSave = $("#btnSave");
	    if (typeof jQuery.ui != 'undefined') {
	        $btnSave.button();
	        $("#tabs").tabs();
	        $("#tabs").tabs("option", "selected", 0);
	    }
	    $btnSave.click(function () {
	        saveProfile();
	    });

		$("#<%=dropEnvironments.ClientID %>").change(function (e) {
			getWebConfigSettings($(this).val())
		});
		$("#<%=dropDAL.ClientID %>").change(function (e) {
			setDefaulDAL($(this).val())
		});
		var $trNewEnvironment = $("#trNewEnvironment");
		$trNewEnvironment.val('');
		$trNewEnvironment.css({ display: 'none' });
		var $txtEnvironments = $("#txtEnvironments");
		$txtEnvironments.css({ display: 'none' });

	});

	function updateData() {
		var uiProfile = GW.Model.WebConfig();
		//uiProfile = $("#<%=dropEnvironments.ClientID %> option:selected").val();
		uiProfile.AlwaysLeftNav = $("#<%=dropAlways_Left_Nav.ClientID %> option:selected").val();
		uiProfile.AppDisplayedName = $("#<%=txtApp_Displayed_Name.ClientID %>").val();
		uiProfile.AppendToFile = $("#<%=dropAppend_To_File.ClientID %> option:selected").val();
		uiProfile.BasePage = $("#<%=txtBase_Page.ClientID %>").val();
		uiProfile.Central_Management = $("#<%=dropCentralManagement.ClientID %> option:selected").val();
		uiProfile.ConversionPattern = $("#<%=txtConversion_Pattern.ClientID %>").val();
		uiProfile.DBStatus = $("#<%=dropDB_Status.ClientID %> option:selected").val();
		uiProfile.DefaultAction = $("#<%=txtDefault_Action.ClientID %>").val();
		uiProfile.DefaultAuthenticatedAction = $("#<%=txtDefault_Authenticated_Action.ClientID %>").val();
		uiProfile.Delete = $("#<%=chkDelete.ClientID %>").is(":checked");
		uiProfile.EnvironmentDisplayed = $("#<%=dropEnvironment.ClientID %> option:selected").val();
		uiProfile.WorkingEnvironment = $("#<%=dropEnvironments.ClientID %> option:selected").val();
		uiProfile.Environments = $("#<%=txtEnvironments.ClientID %>").val();
		uiProfile.ExpectedUpBy = $("#<%=txtExpected_Up_By.ClientID %>").val();
		uiProfile.LDAP_Domain = $("#<%=txtLDAP_Domain.ClientID %>").val();
		uiProfile.LDAP_Server = $("#<%=txtLDAP_Server.ClientID %>").val();
		uiProfile.NewEnvironment = $("#<%=txtNewEnvironment.ClientID %>").val();
		uiProfile.SecurityEntityTranslation = $("#<%=txtSecurity_Entity_Translation.ClientID %>").val();
		uiProfile.ServerSideViewState = $("#<%=dropServer_Side_View_State.ClientID %> option:selected").val();
		uiProfile.ServerSideViewStatePages = $("#<%=txtServer_Side_View_State_Pages.ClientID %>").val();
		uiProfile.UnderConstruction = $("#<%=dropUnder_Construction.ClientID %> option:selected").val();

		uiProfile.App_Name = $("#<%=txtApp_Name.ClientID %>").val();
		uiProfile.Assembly_Name = $("#<%=txtAssembly_Name.ClientID %>").val();
		uiProfile.Authentication_Type = $("#<%=dropAuthentication_Type.ClientID %> option:selected").val();

		uiProfile.Auto_Create = $("#<%=dropAuto_Create.ClientID %> option:selected").val();
		uiProfile.Auto_Create_ClientChoicesAccount = $("#<%=txtAuto_Create_ClientChoicesAccount.ClientID %>").val();
		uiProfile.Auto_Create_SecurityEntity = $("#<%=txtAuto_Create_SecurityEntity.ClientID %>").val();
		uiProfile.Auto_Create_Roles = $("#<%=txtAuto_Create_Roles.ClientID %>").val();


		uiProfile.Connectionstring = $("#<%=txtConnectionstring.ClientID %>").val();
		uiProfile.DAL = $("#<%=dropDAL.ClientID %> option:selected").val();
		uiProfile.Default_Security_Entity_ID = $("#<%=txtDefault_Security_Entity_ID.ClientID %>").val();
		uiProfile.Enable_Cache = $("#<%=dropEnable_Cache.ClientID %> option:selected").val();
		uiProfile.Enable_Encryption = $("#<%=dropEnable_Encryption.ClientID %> option:selected").val();
		uiProfile.Enable_Pooling = $("#<%=dropEnable_Pooling.ClientID %> option:selected").val();
		uiProfile.Encryption_Type = $("#<%=dropEncryption_Type.ClientID %> option:selected").val();
		uiProfile.Failed_Attempts = $("#<%=txtFailedAttempts.ClientID %>").val();
		uiProfile.Force_HTTPS = $("#<%=dropForce_HTTPS.ClientID %> option:selected").val();
		uiProfile.Log_Path = $("#<%=txtLog_Path.ClientID %>").val();
		uiProfile.Log_Priority = $("#<%=dropLog_Priority.ClientID %> option:selected").val();
		uiProfile.Log_Retention = $("#<%=txtLogRetention.ClientID %>").val();
		uiProfile.Name_Space = $("#<%=txtName_Space.ClientID %>").val();
		uiProfile.Registering_Roles = $("#<%=txtRegisteringRoles.ClientID %>").val();
		uiProfile.Registration_Post_Action = $("#<%=txtRegistration_Post_Action.ClientID %>").val();
		uiProfile.Skin_Type = $("#<%=dropSkin_Type.ClientID %> option:selected").val();
		uiProfile.SMTP_Account = $("#<%=txtSMTP_Account.ClientID %>").val();
		uiProfile.SMTP_From = $("#<%=txtSMTP_From.ClientID %>").val();
		uiProfile.SMTP_Password = $("#<%=txtSMTP_Password.ClientID %>").val();
		uiProfile.SMTP_Server = $("#<%=txtSMTP_Server.ClientID %>").val();
		uiProfile.Synchronize_Password = $("#<%=dropSynchronize_Password.ClientID %> option:selected").val();

		var theData = { profile: uiProfile };
		return theData;
	}

	function saveProfile() {
		var mCurrentEnv = $("#<%=dropEnvironment.ClientID %> option:selected").val()
		var mSelectedEnv = $("#<%=dropEnvironments.ClientID %> option:selected").val()
		if (mCurrentEnv == mSelectedEnv) {
			if ($("#<%=chkDelete.ClientID %>").is(":checked")) {
				alert('Can not delete the currently selected environment!');
				return;
			}
		}
		var profile = updateData();
		GW.Common.debug(profile);
		var options = GW.Model.DefaultWebMethodOptions();
		options.async = false;
		options.data = profile;
		options.contentType = 'application/json; charset=utf-8';
		options.dataType = 'json';
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Configuration/AddEditWebConfig.aspx/InvokeSave"
		GW.Common.JQueryHelper.callWeb(options, saveSuccess, saveError);
	}

	function saveSuccess(xhr) {
		var $trNewEnvironment = $("#trNewEnvironment");
		$trNewEnvironment.val('');
		$trNewEnvironment.css({ display: '' });
		setTimeout(function () { getWebConfigSettings($("#<%=dropEnvironment.ClientID %> option:selected").val()); }, 1000);
			
	}

	function saveError(xhr, status, error) {
		alert('status: ' + status + ' error: ' + error);
	}

	function getWebConfigSettings(environtment) {
		var $trNewEnvironment = $("#trNewEnvironment");
		if (environtment == "New") {
			$trNewEnvironment.val('');
			$trNewEnvironment.css({ display: '' });
			return;
		} else {
			$trNewEnvironment.css({ display: 'none' });
		}
		var data = '{ "encryptionType" : "' + $("#<%=dropEncryption_Type.ClientID %> option:selected").val();
		data += '", "environment" : "' + environtment;
		data += '" }';
		GW.Common.debug(data);
		var options = GW.Model.DefaultWebMethodOptions();
		options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Configuration/AddEditWebConfig.aspx/GetWebConfigSettings";
		options.data = data;
		options.async = false;
		options.timeout = 16000;
		options.contentType = 'application/json; charset=utf-8';
		options.dataType = 'json';
		GW.Common.JQueryHelper.callWeb(options, getWebConfigSettingsSuccess, getWebConfigSettingsError);
	}

	function getWebConfigSettingsSuccess(xhr) {
		var defaultwebConfig = GW.Model.WebConfig();
		var webConfig = $.extend({}, defaultwebConfig, xhr.d);
		GW.Common.debug(webConfig);
		$chkDelete = $("#<%=chkDelete.ClientID %>")
		if ($chkDelete.is(":checked")) {
			$chkDelete.prop('checked', false);
		}

		$("#<%=txtApp_Name.ClientID %>").val(webConfig.App_Name);

		$("#<%=dropEnvironment.ClientID %> option[value='" + webConfig.Environment + "']").attr("selected", "selected");
		$("#<%=txtEnvironments.ClientID %>").val(webConfig.Environments);


		$("#<%=dropAuthentication_Type.ClientID %> option[value='" + webConfig.Authentication_Type + "']").attr("selected", "selected");
		$("#<%=dropDAL.ClientID %> option[value='" + webConfig.DAL + "']").attr("selected", "selected");
		$("#<%=dropEnable_Cache.ClientID %> option[value='" + webConfig.Enable_Cache + "']").attr("selected", "selected");
		$("#<%=dropEnable_Encryption.ClientID %> option[value='" + webConfig.Enable_Encryption + "']").attr("selected", "selected");
		$("#<%=dropEnable_Pooling.ClientID %> option[value='" + webConfig.Enable_Pooling + "']").attr("selected", "selected");

		$("#<%=dropAuto_Create.ClientID %> option[value='" + webConfig.Auto_Create + "']").attr("selected", "selected");
		$("#<%=txtAuto_Create_ClientChoicesAccount.ClientID %>").val(webConfig.Auto_Create_ClientChoicesAccount);
		$("#<%=txtAuto_Create_SecurityEntity.ClientID %>").val(webConfig.Auto_Create_SecurityEntity);
		$("#<%=txtAuto_Create_Roles.ClientID %>").val(webConfig.Auto_Create_Roles);


		$("#<%=dropForce_HTTPS.ClientID %> option[value='" + webConfig.Force_HTTPS + "']").attr("selected", "selected");
		$("#<%=dropSynchronize_Password.ClientID %> option[value='" + webConfig.Synchronize_Password + "']").attr("selected", "selected");
		$("#<%=txtAssembly_Name.ClientID %>").val(webConfig.Assembly_Name);
		$("#<%=txtName_Space.ClientID %>").val(webConfig.Name_Space);
		$("#<%=txtConnectionstring.ClientID %>").val(webConfig.Connectionstring);
		$("#<%=txtDefault_Security_Entity_ID.ClientID %>").val(webConfig.Default_Security_Entity_ID);
		$("#<%=txtLDAP_Domain.ClientID %>").val(webConfig.LDAP_Domain);
		$("#<%=txtLDAP_Server.ClientID %>").val(webConfig.LDAP_Server);
			
		$("#<%=txtLog_Path.ClientID %>").val(webConfig.Log_Path);
		$("#<%=txtLogRetention.ClientID %>").val(webConfig.Log_Retention);
		$("#<%=txtRegisteringRoles.ClientID %>").val(webConfig.Registering_Roles);
		$("#<%=txtRegistration_Post_Action.ClientID %>").val(webConfig.Registration_Post_Action);
		$("#<%=txtSMTP_Server.ClientID %>").val(webConfig.SMTP_Server);
		$("#<%=txtSMTP_Account.ClientID %>").val(webConfig.SMTP_Account);
		$("#<%=txtSMTP_Password.ClientID %>").val(webConfig.SMTP_Password);
		$("#<%=txtSMTP_From.ClientID %>").val(webConfig.SMTP_From);
		$("#<%=txtFailedAttempts.ClientID %>").val(webConfig.Failed_Attempts);

	}

	function getWebConfigSettingsError(xhr, status, error) {
		GW.Common.debug('status: ' + status + ' error: ' + error);
		alert('status: ' + status + ' error: ' + error);
	}

	function setDefaulDAL(dal) {
		var nameSpace = "GrowthWare.Framework.DataAccessLayer." + dal + ".V2008";
		$("#<%=txtAssembly_Name.ClientID %>").val("GrowthWareFramework");
		$("#<%=txtName_Space.ClientID %>").val(nameSpace);
		$("#<%=txtConnectionstring.ClientID %>").val('');
	}
</script>
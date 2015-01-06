<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AddEditAccount.ascx.vb" Inherits="GrowthWare.WebApplication.AddEditAccount1" %>
<%@ Register Assembly="GrowthWare.WebSupport" Namespace="GrowthWare.WebSupport.CustomWebControls" TagPrefix="CustomWebControls" %>

<!DOCTYPE html>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            if (typeof jQuery.ui != 'undefined') {
                $("#AddEditAccount_btnSave").button();
                $("#tabs").tabs();
                $("#tabs").tabs("option", "selected", 0);
            }
        });
        function updateData() {
            var profile = {};
            var roles = '';
            var groups = '';
            var canSaveRoles = false;
            var canSaveGroups = false;
            roles = $.map($('#AddEditAccount_ctlRoles_DstList option'), function (e) { return $(e).val(); });
            groups = $.map($('#AddEditAccount_ctlGroups_DstList option'), function (e) { return $(e).val(); });

            accountRoles = {};
            accountRoles.Roles = roles;

            accountGroups = {};
            accountGroups.Groups = groups;
            canSaveRoles = $("#<%=hdnCanSaveRoles.ClientID %>").val();
            canSaveGroups = $("#<%=hdnCanSaveGroups.ClientID %>").val();
            profile = {};
            if (document.getElementById("<%=txtAccount.ClientID%>") != null) {
                profile.Account = $("#<%=txtAccount.ClientID %>").val();
            } else {
                profile.Account = $("#<%=txtEmail.ClientID %>").val();
            }
            profile.AccountGroups = accountGroups;
            profile.AccountRoles = accountRoles;
            profile.CanSaveGroups = canSaveGroups;
            profile.CanSaveRoles = canSaveRoles;
            profile.EMail = $("#<%=txtEmail.ClientID %>").val();
            profile.EnableNotifications = $("#<%=chkEnableNotifications.ClientID %>").is(":checked");
            profile.Status = parseInt($("#<%=dropStatus.ClientID %> option:selected").val());
            profile.FirstName = $("#<%=txtFirstName.ClientID %>").val();
            profile.Id = parseInt($("#<%=txtAccount_seq_id.ClientID %>").val());
            profile.IsSystemAdmin = false;
            if (document.getElementById("<%=chkSysAdmin.ClientID %>") != null) {
                if (document.getElementById("<%=chkSysAdmin.ClientID %>").checked) {
                    profile.IsSystemAdmin = true;
                }
            }
            profile.LastName = $("#<%=txtLastName.ClientID %>").val();
            profile.Location = $("#<%=txtLocation.ClientID %>").val();
            profile.MiddleName = $("#<%=txtMiddleName.ClientID %>").val();
            profile.PreferredName = $("#<%=txtPreferredName.ClientID %>").val();
            profile.TimeZone = parseInt($("#<%=dropTimezone.ClientID %> option:selected").val());
            return profile;
        }

        function saveAddEditAccount($dialogWindow) {
            if (Page_ClientValidate()) {
                var theData = updateData();
                // profile is defined in AddEditAccounts.aspx
                GW.Common.debug(theData);
                var options = GW.Model.DefaultWebMethodOptions();
                options.async = false;
                options.data = JSON.stringify(theData);
                options.contentType = 'application/json; charset=utf-8';
                options.dataType = 'json';
                var mAction = GW.Common.getParameterByName('Action');
                if (mAction == 'SearchAccounts') {
                    options.url = GW.Common.getBaseURL() + "/gw/api/Accounts/Save?Action=SearchAccounts";
                } else {
                    options.url = GW.Common.getBaseURL() + "/gw/api/Accounts/Save?Action=Register";
                }
                GW.Common.JQueryHelper.callWeb(options, saveAddEditAccountSucess, saveAddEditAccountError);
                if (!($dialogWindow === undefined) && typeof jQuery.ui != 'undefined') {
                    $dialogWindow.dialog("close");
                }
            }
        }

        function saveAddEditAccountSucess(xhr) {
            switch (xhr) {
                case "true":
                    GW.Navigation.NavigationController.Refresh();
                    GW.Search.GetSearchResults();
                    break;
                case "Your account has been created":
                    location.reload();
                    break;
                default:
                    alert(xhr);
                    break;
            }
        }

        function saveAddEditAccountError(xhr, status, error) {

        }
    </script>
    <form id="frmAddEditAccount" runat="server">
	    <input type="hidden" id="hdnCanSaveRoles" runat="server" />
	    <input type="hidden" id="hdnCanSaveGroups" runat="server" />
	    <input type="hidden" id="hdnCanSaveStatus" runat="server" />
	    <div>
		    <div class="container" id="tabs">
			    <ul class="nav nav-tabs" role="tablist">
				    <li role="presentation"><a data-toggle="tab" href="#tabsGeneral">General</a></li>
				    <li role="presentation" id="rolesTab" runat="server"><a data-toggle="tab" href="#AddEditAccount_tabsRoles">Roles</a></li>
				    <li role="presentation" id="groupsTab" runat="server"><a data-toggle="tab" href="#AddEditAccount_tabsGroups">Groups</a></li>
				    <li role="presentation" id="derivedRolesTab" runat="server"><a data-toggle="tab" href="#AddEditAccount_tabsDerivedRoles">Derived Roles</a></li>
			    </ul>
                <div class="tab-content">
				    <div class="tab-pane fade in active" id="tabsGeneral">
					    <p>
						    <table border="0" cellpadding="3" cellspacing="0">
							    <tr>
								    <td>
									    <input id="txtAccount_seq_id" style="display: ''" runat="server" />
									    <table border="0" cellpadding="2" width="100%">
										    <tr>
											    <td align="center" colspan="4">
												    <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
											    </td>
										    </tr>
										    <tr id="trAccount" runat="server">
											    <td valign="top">
												    <span class="Form_LabelText">Account: </span>
											    </td>
											    <td valign="top">
												    <table border="0" cellpadding="0" cellspacing="0">
													    <tr>
														    <td>
															    <asp:TextBox ID="txtAccount" CssClass="Form_Field rounded" MaxLength="128" runat="server" />
															    <asp:Image ID="imgWarningAccount" ImageUrl="~/Public/GrowthWare/Images/help.gif" AlternateText="Help Image" Visible="false" runat="server" />
															    <asp:Literal Visible="False" ID="litAccountWarning" runat="server"></asp:Literal>
														    </td>
														    <td>
															    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="failureNotification" Display="Dynamic" runat="server" ErrorMessage="(required)" ControlToValidate="txtAccount" />
														    </td>
													    </tr>
												    </table>
											    </td>
											    <td id="tdStatus" valign="top" runat="server">
												    <span class="Form_LabelText">Status: </span>
											    </td>
											    <td valign="top">
												    <asp:DropDownList ID="dropStatus" CssClass="Form_Field rounded" runat="server">
													    <asp:ListItem Value="1">Active</asp:ListItem>
													    <asp:ListItem Value="4">Change Password</asp:ListItem>
													    <asp:ListItem Value="3">Disabled</asp:ListItem>
												    </asp:DropDownList>
											    </td>
										    </tr>
										    <tr id="trSysAdmin" visible="false" runat="server">
											    <td valign="top">
												    <span class="Form_LabelText">System Administrator: </span>
											    </td>
											    <td valign="top">
												    <asp:CheckBox ID="chkSysAdmin" runat="server" />
											    </td>
											    <td valign="top">
												    <span class="Form_LabelText">Failed Logon Attempts: </span>
											    </td>
											    <td valign="top">
												    <asp:Literal ID="litFailedAttempts" runat="server" />
												    <asp:TextBox ID="txtFailedAttempts" CssClass="Form_Field rounded" Visible="False" runat="server" />
											    </td>
										    </tr>
										    <tr>
											    <td valign="top">
												    <span class="Form_LabelText">First Name: </span>
											    </td>
											    <td valign="top">
												    <asp:TextBox ID="txtFirstName" MaxLength="15" CssClass="Form_Field rounded" runat="Server" />
												    <asp:RequiredFieldValidator ControlToValidate="txtFirstName" Display="Dynamic" Text="(required)" ErrorMessage="You must enter your first name" CssClass="failureNotification" runat="Server" ID="Requiredfieldvalidator5"/>
											    </td>
											    <td valign="top">
												    <span class="Form_LabelText">Last Name: </span>
											    </td>
											    <td valign="top">
												    <asp:TextBox ID="txtLastName" MaxLength="15" CssClass="Form_Field rounded" runat="Server" />
												    <asp:RequiredFieldValidator ControlToValidate="txtLastName" Display="Dynamic" Text="(required)" ErrorMessage="You must enter a last name" CssClass="failureNotification" runat="Server" ID="Requiredfieldvalidator6"/>
											    </td>
										    </tr>
										    <tr>
											    <td valign="top">
												    <span class="Form_LabelText">Middle Name: </span>
											    </td>
											    <td valign="top">
												    <asp:TextBox ID="txtMiddleName" MaxLength="15" CssClass="Form_Field rounded" runat="Server" />
											    </td>
											    <td valign="top">
												    <span class="Form_LabelText">Preferred Name: </span>
											    </td>
											    <td valign="top">
												    <asp:TextBox ID="txtPreferredName" MaxLength="50" CssClass="Form_Field rounded" runat="Server" />
												    <asp:RequiredFieldValidator ControlToValidate="txtPreferredName" Display="Dynamic" Text="(required)" ErrorMessage="You must enter a prefered name" CssClass="failureNotification" runat="Server" ID="Requiredfieldvalidator4" />
											    </td>
										    </tr>
										    <tr>
											    <td valign="top">
												    <span class="Form_LabelText">Email: </span>
											    </td>
											    <td valign="top">
												    <asp:TextBox ID="txtEmail" MaxLength="128" Columns="25" CssClass="Form_Field rounded" runat="Server" />
												    <asp:RequiredFieldValidator ControlToValidate="txtEmail" Display="Dynamic" Text="(required)" ErrorMessage="You must enter an email address" CssClass="failureNotification" runat="Server" ID="Requiredfieldvalidator2" />
												    <asp:RegularExpressionValidator ControlToValidate="txtEmail" Text="(invalid email)" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" runat="Server" ID="Regularexpressionvalidator1" />
											    </td>
											    <td valign="top">
												    <span class="Form_LabelText">Timezone: </span>
											    </td>
											    <td valign="top">
												    <asp:DropDownList ID="dropTimezone" CssClass="rounded" runat="server">
													    <asp:ListItem Text="Hawaii (GMT -10)" Value="-10" />
													    <asp:ListItem Text="Alaska (GMT -9)" Value="-9" />
													    <asp:ListItem Text="Pacific Time (GMT -8)" Value="-8" />
													    <asp:ListItem Text="Mountain Time (GMT -7)" Value="-7" />
													    <asp:ListItem Text="Central Time (GMT -6)" Value="-6" />
													    <asp:ListItem Text="Eastern Time (GMT -5)" Value="-5" />
													    <asp:ListItem Text="Atlantic Time (GMT -4)" Value="-4" />
													    <asp:ListItem Text="Brasilia Time (GMT -3)" Value="-3" />
													    <asp:ListItem Text="Greenwich Mean Time (GMT +0)" Value="0" />
													    <asp:ListItem Text="Central Europe Time (GMT +1)" Value="1" />
													    <asp:ListItem Text="Eastern Europe Time (GMT +2)" Value="2" />
													    <asp:ListItem Text="Middle Eastern Time (GMT +3)" Value="3" />
													    <asp:ListItem Text="Abu Dhabi Time (GMT +4)" Value="4" />
													    <asp:ListItem Text="Indian Time (GMT +5)" Value="5" />
													    <asp:ListItem Text="Eastern China Time (GMT +8)" Value="8" />
													    <asp:ListItem Text="Japan Time (GMT +9)" Value="9" />
													    <asp:ListItem Text="Australian Time (GMT +10)" Value="10" />
													    <asp:ListItem Text="Pacific Rim Time (GMT +11)" Value="11" />
													    <asp:ListItem Text="New Zealand Time (GMT +12)" Value="12" />
												    </asp:DropDownList>
											    </td>
										    </tr>
										    <tr>
											    <td colspan="4">
												    <span class="Form_SubSectionText"><b>Optional Information </b></span>
											    </td>
										    </tr>
										    <tr>
											    <td valign="top">
												    <span class="Form_LabelText">Location: </span>
											    </td>
											    <td valign="top">
												    <asp:TextBox ID="txtLocation" MaxLength="128" CssClass="Form_Field rounded" runat="Server" />
											    </td>
										    </tr>
										    <tr>
											    <td colspan="4">
												    <span class="Form_SubSectionText"><b>Email Options </b></span>
											    </td>
										    </tr>
										    <tr>
											    <td valign="top">
												    <span class="Form_LabelText">Email Notifications: </span>
											    </td>
											    <td colspan="3" valign="top">
												    <asp:CheckBox ID="chkEnableNotifications" CssClass="Form_Field rounded" Checked="true" runat="Server" Text="Receive Email Notifications" />
												    <br />
												    <span class="Form_Field">(This option controls Email Notification. If this option is enabled, you will see Email Notification.) </span>
											    </td>
										    </tr>
									    </table>
								    </td>
							    </tr>
						    </table>
					    </p>
				    </div>
				    <div class="tab-pane fade" id="tabsRoles" runat="server">
                        <p>
                            <table class="pickListTable">
                                <tr class="pickListTableHeader">
                                    <td>
                                        Roles
                                    </td>
                                </tr>
                                <tr>
                                    <td class="pickListTableHelp">
                                        Assign roles
                                    </td>
                                </tr>
                                <tr>
                                    <td class="pickListTableContents">
                                        <CustomWebControls:ListPicker CssClass="listPicker" Size="200" Rows="6" ID="ctlRoles" AllItemsText="All Roles" SelectedItemsText="Selected Roles" runat="Server" />
                                    </td>
                                </tr>
                            </table>
                        </p>
				    </div>
				    <div class="tab-pane fade" id="tabsGroups" runat="server">
                        <p>
                            <table class="pickListTable">
                                <tr class="pickListTableHeader">
                                    <td>
                                        Groups
                                    </td>
                                </tr>
                                <tr>
                                    <td class="pickListTableHelp">
                                        Assign groups
                                    </td>
                                </tr>
                                <tr>
                                    <td class="pickListTableContents">
                                        <CustomWebControls:ListPicker CssClass="listPicker" Size="200" Rows="6" ID="ctlGroups" AllItemsText="All Groups" SelectedItemsText="Selected Groups" runat="Server" />
                                    </td>
                                </tr>
                            </table>
                        </p>
				    </div>
				    <div class="tab-pane fade" id="tabsDerivedRoles" runat="server">
                        <p>
					        <table cellspacing="0" cellpadding="3" border="0">
						        <tr>
							        <td align="left" style="width: 480px">
								        <table class="pickListTable">
									        <!-- Roles -->
									        <tr class="pickListTableHeader">
										        <td>
											        Derived Roles
										        </td>
									        </tr>
									        <tr>
										        <td class="pickListTableHelp">
											        &nbsp;<img alt="Help" onclick="GW.Common.showHelpMSG(document.getElementById('derivedHelpRoles').innerHTML,'Help Derived Roles')" src='<%=ResolveUrl("~/Public/GrowthWare/Images/help.gif")%>' title=" &nbsp;Roles that may View&nbsp;" />
										        </td>
									        </tr>
									        <tr>
										        <td class="pickListTableContents">
											        <asp:ListBox ID="lstBoxRoles" style="width: 462px;" rows="5" runat="server"></asp:ListBox>
										        </td>
									        </tr>
								        </table>
								        <!-- View Roles -->
								        <div id="derivedHelpRoles" style="display: none">
									        Derived roles are calculated by combining roles that are directly assigned through Roles or indirectly assigned through Groups.<br />
								        </div>
							        </td>
						        </tr>
					        </table>
                        </p>				
				    </div>
			    <input type="button" id="btnSave" value="Save" onclick="javascript: saveAddEditAccount();" runat="server" style="display: inline-block;" />
                </div>
		    </div>
	    </div>

    </form>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddEditFunction.aspx.vb" Inherits="GrowthWare.WebApplication.AddEditFunction" %>
<%@ Register Src="~/UserControls/RolesControl.ascx" TagName="RolesControl" TagPrefix="ucRoles" %>
<%@ Register Src="~/UserControls/GroupControl.ascx" TagName="GroupControl" TagPrefix="ucGroups" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
	<script type="text/x-jquery-tmpl" id="functionOrderTemplate">
			<tr><td>${Name}</td><td>${Action}</td></tr>
	</script>
	<script type="text/javascript" language="javascript">
		// DOM level accessable variables ...
		$(document).ready(function () {
		    getFunctionMenuOrder();
		    if (typeof jQuery.ui != 'undefined') {
		        $("#tabs").tabs();
		        $("#tabs").tabs("option", "selected", 0);

		        $('#tabs')
                    .tabs()
                    .find('.ui-tabs-nav')
                        .css('white-space', 'nowrap')
                        .css('overflow', 'hidden')
                    .find('li')
                        .css('display', 'inline-block')
                        .css('float', 'none')
                        .css('vertical-align', 'bottom');
		    }
    	});

		function move(direction) {
			var functionSeqId = parseInt($("#<%=dropFunctions.ClientID %> option:selected").val());
		    var options = GW.Model.DefaultWebMethodOptions();
			options.async = true;
			options.url = GW.Common.getBaseURL() + "/gw/api/Functions/MoveMenu?Action=Search_Functions&functionSeqId=" + functionSeqId + "&direction=" + direction
			GW.Common.JQueryHelper.callWeb(options, getFunctionMenuOrder, moveError);
		}

	    function moveError(xhr, status, error) {
	        var mErrorException = JSON.parse(xhr.responseText);
	        var mErrorMessage = 'Error getting content';
	        mErrorMessage += '\nStatus: ' + status;
	        mErrorMessage += '\nError: ' + error;
	        mErrorMessage += '\nMessage: ' + mErrorException.ExceptionMessage;
	        alert(mErrorMessage);
	        GW.Search.GetSearchResults();
	    }

		function getFunctionMenuOrder() {
			var functionSeqID = parseInt($("#<%=divFunctionSeqId.ClientID %>").html());
		    var options = GW.Model.DefaultWebMethodOptions();
		    var profile = {};
		    profile.functionSeqId = functionSeqID;
		    options.async = true;
		    options.type = 'GET';
			options.contentType = 'application/json; charset=utf-8';
			options.dataType = 'json';
			options.url = GW.Common.getBaseURL() + "/gw/api/Functions/GetFunctionOrder?Action=Search_Accounts&functionSeqId=" + functionSeqID;
			GW.Common.JQueryHelper.callWeb(options, getFunctionMenuOrderSucess);
		}

		function getFunctionMenuOrderSucess(xhr) {
			$("#functionOrderTable > tbody").empty()
			$("#functionOrderTemplate").tmpl(xhr).appendTo("#functionOrderTable > tbody");
			$("#functionOrderTable").css('display', '');
		}

		// update the DOM level accessable variables to be used by SearchFuctions.aspx
		function updateData() {
			var profile = {};
			var directoryInfo = {};
			var viewRoles = '';
			var addRoles = '';
			var editRoles = '';
			var deleteRoles = '';

			viewRoles = $.map($('#RolesControl_ctlViewRoles_DstList option'), function (e) { return $(e).val(); });
			addRoles = $.map($('#RolesControl_ctlAddRoles_DstList option'), function (e) { return $(e).val(); });
			editRoles = $.map($('#RolesControl_ctlEditRoles_DstList option'), function (e) { return $(e).val(); });
			deleteRoles = $.map($('#RolesControl_ctlDeleteRoles_DstList option'), function (e) { return $(e).val(); });

			viewGroups = $.map($('#GroupsControl_ctlViewGroups_DstList option'), function (e) { return $(e).val(); });
			addGroups = $.map($('#GroupsControl_ctlAddGroups_DstList option'), function (e) { return $(e).val(); });
			editGroups = $.map($('#GroupsControl_ctlEditGroups_DstList option'), function (e) { return $(e).val(); });
			deleteGroups = $.map($('#GroupsControl_ctlDeleteGroups_DstList option'), function (e) { return $(e).val(); });

			functionRolesGroups = {};
			functionRolesGroups.ViewRoles = viewRoles;
			functionRolesGroups.AddRoles = addRoles;
			functionRolesGroups.EditRoles = editRoles;
			functionRolesGroups.DeleteRoles = deleteRoles;

			functionRolesGroups.ViewGroups = viewGroups;
			functionRolesGroups.AddGroups = addGroups;
			functionRolesGroups.EditGroups = editGroups;
			functionRolesGroups.DeleteGroups = deleteGroups;

			profile = {};
			profile.Id = parseInt($("#<%=divFunctionSeqId.ClientID %>").html());
			// action is not being picked up ...
			if (profile.Id == -1) {
				profile.Action = $("#<%=txtAction.ClientID %>").val();
			} else {
				profile.Action = $("#<%=divAction.ClientID %>").html();
			}
			profile.Description = $("#<%=txtDescription.ClientID %>").val();
			profile.EnableNotifications = $("#<%=chkEnableNotifications.ClientID %>").is(":checked");
			profile.EnableViewState = $("#<%=chkEnableViewState.ClientID %>").is(':checked');
			profile.FunctionTypeSeqID = parseInt($("#<%=dropFunctionType.ClientID %> option:selected").val());
			profile.IsNav = $("#<%=chkIsNav.ClientID %>").is(':checked');
			profile.LinkBehavior = parseInt($("#<%=dropLinkBehavior.ClientID %> option:selected").val());
			profile.MetaKeyWords = $("#<%=txtKeyWords.ClientID %>").val();
			profile.Name = $("#<%=txtName.ClientID %>").val();
			profile.NavigationTypeSeqId = parseInt($("#<%=dropNavType.ClientID %> option:selected").val());
			profile.Notes = $("#<%=txtNotes.ClientID %>").val();
			profile.NoUI = $("#<%=chkNoUI.ClientID %>").is(':checked');
			profile.ParentID = parseInt($("#<%=dropNavParent.ClientID %> option:selected").val());
			profile.RedirectOnTimeout = $("#<%=chkRedirectOnTimeout.ClientID %>").is(':checked');
			profile.Source = $("#<%=txtSource.ClientID %>").val();
		    profile.Controller = $("#<%=txtController.ClientID%>").val();
		    directoryInfo = {};
		    directoryInfo.Directory = $("#<%=txtDirectory.ClientID %>").val();
		    directoryInfo.Impersonate = $("#<%=chkImpersonation.ClientID %>").is(":checked");
		    directoryInfo.ImpersonateAccount = $("#<%=txtAccount.ClientID %>").val();
		    var iPassword = $("#<%=txtPassword.ClientID %>").val();
		    if (iPassword === undefined || iPassword.length == 0) iPassword = $("#<%=txtHidPwd.ClientID %>").val();
		    if (iPassword === undefined) iPassword = '';
		    directoryInfo.ImpersonatePassword = iPassword;
		    profile.DirectoryData = directoryInfo;
		    profile.RolesAndGroups = functionRolesGroups;
			//var theData = { uiProfile: profile, functionRolesGroups: functionRolesGroups, directoryData: directoryInfo };
		    return profile;
		}

	    function saveAddEditFunciton($dialogWindow) {
	        if (Page_ClientValidate()) {
	            var theData = updateData();
	            GW.Common.debug(theData);
	            var options = GW.Model.DefaultWebMethodOptions();
	            options.height = 585;
	            options.width = 1050;
	            options.async = false;
	            options.data = theData;
	            options.contentType = 'application/json; charset=utf-8';
	            options.dataType = 'json';
	            options.url = GW.Common.getBaseURL() + "/gw/api/Functions/Save?Action=Search_Functions";
	            GW.Common.JQueryHelper.callWeb(options, saveAddEditFuncitonSucess);
	            profile = {};
	            $dialogWindow.dialog("destroy");
	            $dialogWindow.remove();
	        }
		}

	    function saveAddEditFuncitonSucess(xhr, status, error) {
		    GW.Navigation.NavigationController.Refresh();
			GW.Search.GetSearchResults();
	    }
	</script>
</head>
<body>
    <div id="helpPopup" style="display: none;" />
    <form id="frmAddEditFunction" runat="server">
    <div>
        <div class="container" id="tabs">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation"><a data-toggle="tab"  href="#tabs-General">General</a></li>
                <li role="presentation"><a data-toggle="tab"  href="#tabs-MenuOrder">Menu Order</a></li>
                <li role="presentation"><a data-toggle="tab"  href="#tabs-Roles">Roles</a></li>
                <li role="presentation"><a data-toggle="tab"  href="#tabs-Groups">Groups</a></li>
                <li role="presentation"><a data-toggle="tab"  href="#tabs-DirectoryInformation">Directory Information</a></li>
                <li role="presentation"><a data-toggle="tab"  href="#tabs-DerivedRoles">Derived Roles</a></li>
            </ul>
            <div class="tab-content">
                <div class="tab-pane fade in active" id="tabs-General">
                    <p>
                        <table border="0" style="width: 100%;" cellpadding="2">
                            <tr id="trID" runat="server">
                                <td align="right">
                                    <span class="Form_LabelText">ID:</span>
                                </td>
                                <td style="width: 0px;">
                                    &nbsp;
                                </td>
                                <td align="left">
                                    <div id="divFunctionSeqId" runat="server">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <span class="Form_LabelText">Name:</span>
                                </td>
                                <td style="width: 0px;" valign="top">
                                    &nbsp;
                                </td>
                                <td align="left">
                                    <input type="text" id="txtName" class="rounded" style="width: 322px;" runat="server" />
                                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator5" runat="Server" CssClass="failureNotification" ErrorMessage="Required" Text="(required)" Display="Dynamic" ControlToValidate="txtName">(required)</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <span class="Form_LabelText">Description: </span>
                                </td>
                                <td style="width: 0px;" valign="top">
                                    &nbsp;
                                </td>
                                <td align="left">
                                    <!-- todo add max length -->
                                    <input type="text" class="rounded" id="txtDescription" runat="server" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="failureNotification" ErrorMessage="Required" Text="(required)" Display="Dynamic" ControlToValidate="txtDescription">(required)</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <span class="Form_LabelText">Notes: </span>
                                </td>
                                <td style="width: 0px;" valign="top">
                                    &nbsp;
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtNotes" MaxLength="255" onKeyPress="return GW.Common.Validation.textboxMultilineMaxNumber(this,255)" runat="Server" CssClass="rounded" Height="40px" TextMode="MultiLine" Width="500px" Columns="100" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <span class="Form_LabelText">Key Words: </span>
                                </td>
                                <td style="width: 0px;" valign="top">
                                    &nbsp;
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtKeyWords" MaxLength="255" onKeyPress="return GW.Common.Validation.textboxMultilineMaxNumber(this,512)" runat="Server" CssClass="rounded" Height="40px" TextMode="MultiLine" Width="500px" Columns="100" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <span class="Form_LabelText">Action:</span>
                                </td>
                                <td style="width: 0px;" valign="top">
                                    <img onclick="GW.Common.showHelpMSG(document.getElementById('helpActions').innerHTML,'Action Help')" style="border: 0px;" src="<%=ResolveUrl("~/Content/GrowthWare/Images/help.gif") %>" alt="&nbsp;Help about actions&nbsp;" />
                                </td>
                                <td align="left">
                                    <div id="divAction" runat="server">
                                    </div>
                                    <asp:TextBox ID="txtAction" MaxLength="255" runat="server" CssClass="rounded" Height="20px" Width="500px" Columns="100" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <span class="Form_LabelText">Type: </span>
                                </td>
                                <td style="width: 0px;" valign="top">
                                    &nbsp;
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="dropFunctionType" runat="server" CssClass="rounded">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="trSource" runat="server">
                                <td align="right" valign="top">
                                    <span class="Form_LabelText">Source: </span>
                                </td>
                                <td style="width: 0px;" valign="top">
                                    <img onclick="GW.Common.showHelpMSG(document.getElementById('helpSource').innerHTML,'Source Help')" style="border: 0px;" src="<%=ResolveUrl("~/Content/GrowthWare/Images/help.gif") %>" alt="&nbsp;Help about source&nbsp;" />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSource" MaxLength="512" runat="Server" CssClass="rounded" Width="500px" Columns="100" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="failureNotification" ErrorMessage="Required (Relitive location of the module)" Text="(required)" Display="Dynamic" ControlToValidate="txtSource">(required)</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr id="trController" runat="server">
                                <td align="right" valign="top">
                                    <span class="Form_LabelText">Controller: </span>
                                </td>
                                <td style="width: 0px;" valign="top">
                                    <img onclick="GW.Common.showHelpMSG(document.getElementById('helpController').innerHTML,'Controller Help')" style="border: 0px;" src="<%=ResolveUrl("~/Content/GrowthWare/Images/help.gif") %>" alt="&nbsp;Help about controller&nbsp;" />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtController" MaxLength="512" runat="Server" CssClass="rounded" Width="500px" Columns="100" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table style="width: 100%">
                                        <asp:CheckBox ID="chkEnableViewState" Text="Enable ViewState" runat="server" />
                                        <asp:CheckBox ID="chkEnableNotifications" Text="Enable Notifications" runat="server" />
                                        <asp:CheckBox ID="chkRedirectOnTimeout" Text="Redirect on Session Timeout" runat="server" />
                                        <asp:CheckBox ID="chkNoUI" runat="server" Text="No UI" />
                                        <asp:CheckBox ID="chkIsNav" Text="Is Nav" runat="server" />
                                    </table>
                                </td>
                            </tr>
                            <tr id="trParent" runat="server">
                                <td colspan="3">
                                    <table border="0" style="width: 100%;">
                                        <span class="Form_LabelText">Nav type: </span>
                                        <asp:DropDownList ID="dropNavType" runat="server" CssClass="rounded">
                                        </asp:DropDownList>
                                        &nbsp; <span class="Form_LabelText">Parent: </span>
                                        <asp:DropDownList ID="dropNavParent" runat="server" CssClass="rounded">
                                            <asp:ListItem Text="None" Value="0" />
                                        </asp:DropDownList>
                                        &nbsp; <span class="Form_LabelText">Link Behavior: </span>
                                        <asp:DropDownList ID="dropLinkBehavior" runat="server" CssClass="rounded">
                                            <asp:ListItem Text="None" Value="0" />
                                        </asp:DropDownList>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div id="helpActions" style="display: none">
                            Note: Action should not have any special charactors.<br />
                            Once an action has been entered it can not be changed other than by direct access to the data store.<br />
                        </div>
                        <div id="helpSource" style="display: none">
                            Note: If specifing a page use the forward slash Ex:&nbsp; /pages/common/mypage.aspx.<br />
                        </div>
                        <div id="helpController" style="display: none">
                            Note: Used for AngularJs implementations.<br />
                        </div>
                    </p>
                </div>
                <div class="tab-pane fade in active" id="tabs-MenuOrder">
				    <table border="0" cellpadding="3" cellspacing="0">
					    <tr>
						    <td>
							    <table border="0" style="display: none;" cellspacing="2" cellpadding="2" id="functionOrderTable">
                                    <thead>
								        <tr>
									        <th>
										        Name
									        </th>
									        <th>
										        Action
									        </th>
								        </tr>
                                    </thead>
                                    <tbody></tbody>
							    </table>
						    </td>
						    <td valign="top">
							    <asp:DropDownList ID="dropFunctions" CssClass="rounded" runat="server" />
							    &nbsp;
							    <input type="button" id="btnMoveUp" value="Up" onclick="javascript: move('up');" />
							    <input type="button" id="btnMoveDown" value="Down" onclick="javascript: move('down');" />
						    </td>
					    </tr>
				    </table>
                </div>
                <div class="tab-pane fade in active" id="tabs-Roles">
                    <p>
                        <ucRoles:RolesControl ID="RolesControl" runat="server" />
                    </p>
                </div>
                <div class="tab-pane fade in active" id="tabs-Groups">
                    <p>
                        <ucGroups:GroupControl ID="GroupsControl" runat="server" />
                    </p>
                </div>
                <div class="tab-pane fade in active" id="tabs-DirectoryInformation">
                    <p>
                        <table>
                            <tr>
                                <td align="right">
                                    <label>
                                        Directory:&nbsp;</label>
                                    <asp:TextBox ID="txtDirectoryID" CssClass="rounded" Visible="false" runat="server"></asp:TextBox>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDirectory" MaxLength="255" CssClass="rounded" runat="server" Width="563px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <label>
                                        Requries Impersonation:&nbsp;</label>
                                </td>
                                <td align="left">
                                    <asp:CheckBox ID="chkImpersonation" runat="server"></asp:CheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <label>
                                        Account:&nbsp;</label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAccount" MaxLength="50" CssClass="rounded" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <label>
                                        Password:&nbsp;</label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtPassword" MaxLength="50" TextMode="Password" CssClass="rounded" runat="server"></asp:TextBox>&nbsp;!Leave blank to keep the same password!
                                    <asp:TextBox ID="txtHidPwd" Visible="False" CssClass="rounded" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </p>
                </div>
                <div class="tab-pane fade in active" id="tabs-DerivedRoles">
                    <p>
                        <table cellspacing="0" cellpadding="3" border="0">
                            <tr>
                                <td align="left" style="width: 480px">
                                    <table class="pickListTable">
                                        <!-- View Roles -->
                                        <tr class="pickListTableHeader">
                                            <td>
                                                View Roles
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="pickListTableHelp">
                                                Determines who can view pages.&nbsp;<img alt="Help" onclick="GW.Common.showHelpMSG(document.getElementById('derivedHelpViewRoles').innerHTML,'Help Derived View Roles')" src='<%=ResolveUrl("~/Content/GrowthWare/Images/help.gif")%>' title=" &nbsp;Roles that may View&nbsp;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="pickListTableContents">
                                                <asp:ListBox ID="lstBoxViewRoles" Style="width: 462px;" Rows="5" runat="server"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- View Roles -->
                                    <div id="derivedHelpViewRoles" style="display: none">
                                        Derived roles are calculated by combining roles that are directly assigned through<br />
                                        Roles tab or indirectly assigned throught the Groups tab.<br />
                                        View roles are used to determine who can view a function.
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
                                                Determines who can add information on&nbsp;pages.&nbsp;<img alt="Help" onclick="GW.Common.showHelpMSG(document.getElementById('derivedHelpAddRoles').innerHTML,'Help Derived Add Roles')" src='<%=ResolveUrl("~/Content/GrowthWare/Images/help.gif")%>' title="&nbsp;Roles that may Add&nbsp;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="pickListTableContents">
                                                <asp:ListBox ID="lstBoxAddRoles" Style="width: 462px;" Rows="5" runat="server"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- Add Roles -->
                                    <div id="derivedHelpAddRoles" style="display: none">
                                        Derived roles are calculated by combining roles that are directly assigned through<br />
                                        Roles tab or indirectly assigned throught the Groups tab.<br />
                                        Add roles are used to determine who can Add.
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
                                        <tr>
                                            <td class="formSectionHelp">
                                                Determines who can edit or save&nbsp;information on a page.&nbsp;<img alt="Help" onclick="GW.Common.showHelpMSG(document.getElementById('derivedHelpEditRoles').innerHTML,'Help Derived Edit Roles')" src='<%=ResolveUrl("~/Content/GrowthWare/Images/help.gif")%>' title="&nbsp;Roles that may Edit&nbsp;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="pickListTableContents">
                                                <asp:ListBox ID="lstBoxEditRoles" Style="width: 462px;" Rows="5" runat="server"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- Edit Roles -->
                                    <div id="derivedHelpEditRoles" style="display: none">
                                        Derived roles are calculated by combining roles that are directly assigned through<br />
                                        Roles tab or indirectly assigned throught the Groups tab.<br />
                                        Edit roles are used to determine who can Edit.
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
                                                Determines who can delete information on a page.&nbsp;<img alt="Help" onclick="GW.Common.showHelpMSG(document.getElementById('derivedHelpDeleteRoles').innerHTML,'Help Derived Delete Roles')" src='<%=ResolveUrl("~/Content/GrowthWare/Images/help.gif")%>' title="Roles that may Delete" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="pickListTableContents">
                                                <asp:ListBox ID="lstBoxDeleteRoles" Style="width: 462px;" Rows="5" runat="server"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- Delete Roles -->
                                    <div id="derivedHelpDeleteRoles" style="display: none">
                                        Derived roles are calculated by combining roles that are directly assigned through<br />
                                        Roles tab or indirectly assigned throught the Groups tab.<br />
                                        Delete roles are used to determine who can Delete.
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </p>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

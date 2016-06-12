<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="AccountsGeneral.ascx.vb" Inherits="BaseApplication.AccountsGeneral" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:TextBox ID="txtAccount_seq_id" Visible="False" Runat="server" />
<table cellpadding="2" width="100%">
	<tr>
		<td align=center colspan="2">
			<B>General&nbsp; </B>
		</td>
	</tr>
	<tr>
		<td align="center" colspan="2">
			<asp:Label id=lblError Runat="server" ForeColor="Red"></asp:Label>
		</td>
	</tr>
	<tr>
		<td>
			<span class="Form_LabelText">Account: </span>
		</td>
		<td>
			<asp:Literal visible="False" id="litAccount" runat="server" />
			<asp:TextBox ID="txtAccount" MaxLength="25" Runat="server" />
			<asp:Literal visible="False" ID="litAccountWarning" Runat="server"></asp:Literal>
			<asp:RequiredFieldValidator id="RequiredFieldValidator1" CssClass="Form_Message" Display="Dynamic" runat="server" ErrorMessage="(required)" ControlToValidate="txtAccount"></asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
		<td>
			<span class="Form_LabelText">Status: </span>
		</td>
		<td>
			<asp:DropDownList ID="dropStatus" Runat="server">
				<asp:ListItem Value="0">Active</asp:ListItem>
				<asp:ListItem Value="1">Change Password</asp:ListItem>
				<asp:ListItem Value="2">Disabled</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td>
			<span class="Form_LabelText">Failed Logon Attempts: </span>
		</td>
		<td>
			<asp:Literal ID="litFailedAttempts" Runat="server" />
			<asp:TextBox ID="txtFailedAttempts" Visible="False" Runat="server" />
		</td>
	</tr>
	<tr>
		<td>
			<span class="Form_LabelText">First Name: </span>
		</td>
		<td>
			<asp:TextBox id="txtFirstName" MaxLength="15" CssClass="Form_Field" Runat="Server" />
			<asp:RequiredFieldValidator ControlToValidate="txtFirstName" Display="Dynamic" Text="(required)" ErrorMessage="You must enter your first name"
				CssClass="Form_Message" Runat="Server" ID="Requiredfieldvalidator5" NAME="Requiredfieldvalidator5" />
		</td>
	</tr>
	<tr>
		<td>
			<span class="Form_LabelText">Last Name: </span>
		</td>
		<td>
			<asp:TextBox id="txtLastName" MaxLength="15" CssClass="Form_Field" Runat="Server" />
			<asp:RequiredFieldValidator ControlToValidate="txtLastName" Display="Dynamic" Text="(required)" ErrorMessage="You must enter a last name"
				CssClass="Form_Message" Runat="Server" ID="Requiredfieldvalidator6" NAME="Requiredfieldvalidator6" />
		</td>
	</tr>
	<tr>
		<td>
			<span class="Form_LabelText">Middle Name: </span>
		</td>
		<td>
			<asp:TextBox id="txtMiddleName" MaxLength="15" CssClass="Form_Field" Runat="Server" />
		</td>
	</tr>
	<tr>
		<td>
			<span class="Form_LabelText">Prefered Name: </span>
		</td>
		<td>
			<asp:TextBox id="txtPreferedName" MaxLength="50" CssClass="Form_Field" Runat="Server" />
			<asp:RequiredFieldValidator ControlToValidate="txtPreferedName" Display="Dynamic" Text="(required)" ErrorMessage="You must enter a prefered name" CssClass="Form_Message" Runat="Server" ID="Requiredfieldvalidator4"/>
		</td>
	</tr>
	<tr>
		<td>
			<span class="Form_LabelText">Email: </span>
		</td>
		<td>
			<asp:TextBox id="txtEmail" MaxLength="50" Columns="40" CssClass="Form_Field" Runat="Server" />
			<asp:RequiredFieldValidator ControlToValidate="txtEmail" Display="Dynamic" Text="(required)" ErrorMessage="You must enter an email address" CssClass="Form_Message" Runat="Server" ID="Requiredfieldvalidator2"/>
			<asp:RegularExpressionValidator ControlToValidate="txtEmail" Text="(invalid email)" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
				Runat="Server" ID="Regularexpressionvalidator1"/>
		</td>
	</tr>
	<tr>
		<td>
			<span class="Form_LabelText">Timezone: </span>
		</td>
		<td>
			<asp:dropdownlist id="dropTimezone" CssClass="Form_List" runat="server">
				<asp:listitem Text="Hawaii (GMT -10)" Value="-10" />
				<asp:listitem Text="Alaska (GMT -9)" Value="-9" />
				<asp:listitem Text="Pacific Time (GMT -8)" Value="-8" />
				<asp:listitem Text="Mountain Time (GMT -7)" Value="-7" />
				<asp:listitem Text="Central Time (GMT -6)" Value="-6" />
				<asp:listitem Text="Eastern Time (GMT -5)" Value="-5" />
				<asp:listitem Text="Atlantic Time (GMT -4)" Value="-4" />
				<asp:listitem Text="Brasilia Time (GMT -3)" Value="-3" />
				<asp:listitem Text="Greenwich Mean Time (GMT +0)" Value="0" />
				<asp:listitem Text="Central Europe Time (GMT +1)" Value="1" />
				<asp:listitem Text="Eastern Europe Time (GMT +2)" Value="2" />
				<asp:listitem Text="Middle Eastern Time (GMT +3)" Value="3" />
				<asp:listitem Text="Abu Dhabi Time (GMT +4)" Value="4" />
				<asp:listitem Text="Indian Time (GMT +5)" Value="5" />
				<asp:listitem Text="Eastern China Time (GMT +8)" Value="8" />
				<asp:listitem Text="Japan Time (GMT +9)" Value="9" />
				<asp:listitem Text="Australian Time (GMT +10)" Value="10" />
				<asp:listitem Text="Pacific Rim Time (GMT +11)" Value="11" />
				<asp:listitem Text="New Zealand Time (GMT +12)" Value="12" />
			</asp:dropdownlist>
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<span class="Form_SubTitle"><b>Optional Information </b></span>
		</td>
	</tr>
	<tr>
		<td>
			<span class="Form_LabelText">Location: </span>
		</td>
		<td>
			<asp:TextBox id="txtLocation" MaxLength="50" CssClass="Form_Field" Runat="Server" />
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<span class="Form_SubTitle"><b>Email Options </b></span>
		</td>
	</tr>
	<tr>
		<td valign="top">
			<span class="Form_LabelText">Email Notifications: </span>
		</td>
		<td>
			<asp:Checkbox id="chkEnableNotifications" CssClass="Form_Field" Checked="true" Runat="Server"
				Text="Receive Email Notifications" />
			<br>
			<span class="Form_Field">
				(This option controls Email Notification. If this option is enabled, you will see Email Notification.)
			</span>
		</td>
	</tr>
	<tr>
		<td colspan="2" align="center">
			<asp:Button id="btnSave" runat="server" Text="Save" Visible="False"></asp:Button>
		</td>
	</tr>
</table>

<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ModulesGeneral.ascx.vb" Inherits="BaseApplication.ModulesGeneral" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table cellPadding=2 width="100%">
	<tr>
		<td class=Form_SubTitle align=center colSpan=2><B>General&nbsp;</B>
		</td>
	</tr>
	<TR>
		<TD>MODULE_SEQ_ID</TD>
		<TD>
			<asp:literal id=litModuleSeqId runat="server"></asp:literal>
		</TD>
	</TR>
	<tr>
		<td>
			<span class="Form_LabelText">Name:</span>
		</td>
		<td>
			<asp:textbox id="txtName" MaxLength="255" Runat="Server" CssClass="Form_Field"/>
			<asp:requiredfieldvalidator id=Requiredfieldvalidator5 Runat="Server" CssClass="Form_Message" ErrorMessage="Required" Text="(required)" Display="Dynamic" ControlToValidate="txtName">(required)</asp:requiredfieldvalidator>
			&nbsp;Note: Name should be unique but does not have to.
		</td>
	</tr>
	<tr>
		<td>
			<span class=Form_LabelText>Description: </span>
		</td>
		<td>
			<asp:textbox id="txtDescription" MaxLength="255" Runat="Server" CssClass="Form_Field"/>
			<asp:requiredfieldvalidator id=RequiredFieldValidator1 Runat="server" CssClass="Form_Message" ErrorMessage="Required" Text="(required)" Display="Dynamic" ControlToValidate="txtDescription">(required)</asp:requiredfieldvalidator>
		</td>
	</tr>
	<tr>
		<td>
			<span class="Form_LabelText">Source: </span>
		</td>
		<td>
			<asp:Literal ID="litSource" Runat="server"></asp:Literal>
			<asp:textbox id="txtSource" MaxLength="512" Runat="Server" CssClass="Form_Field"/>
			<asp:requiredfieldvalidator id=RequiredFieldValidator3 Runat="server" CssClass="Form_Message" ErrorMessage="Required (Relitive location of the module)" Text="(required)" Display="Dynamic" ControlToValidate="txtSource">(required)</asp:requiredfieldvalidator>
			<asp:Literal ID="litSourceNote" Runat="server" Text ="&nbsp;If specifing a page use the forward slash Ex:&nbsp; /pages/common/mypage.aspx"></asp:Literal>
		</td>
	</tr>
	<tr>
		<td>
			<span class="Form_LabelText">EnableViewState: </span>
		</td>
		<td>
			<asp:checkbox id="chkEnableViewState" runat="server"/>
		</td>
	</tr>

	<tr>
		<td>
			<span class="Form_LabelText">Is Nav: </span>
		</td>
		<td>
			<asp:checkbox id=chkIsNav runat="server"/>
		</td>
	</tr>
	<tr>
		<td>
			<span class="Form_LabelText">Nav type: </span>
		</td>
		<td>
			<asp:dropdownlist id=dropNavType runat="server" CssClass="Form_List">
			</asp:dropdownlist>
		</td>
	</tr>
	<tr>
		<td>
			<span class="Form_LabelText">Parent: </span>
		</td>
		<td>
			<asp:dropdownlist id=dropNavParent runat="server" CssClass="Form_List">
				<asp:ListItem text="None" Value="0"/>
			</asp:dropdownlist>
		</td>
	</tr>
	<tr>
		<td>
			<span class=Form_LabelText>Action:&nbsp; </span>
		</td>
		<td>
			<asp:Literal ID="litAction" Runat="server"></asp:Literal>
			<asp:textbox id="txtAction" MaxLength="255" Runat="server" CssClass="Form_Field"/>
			<asp:Literal ID="litActionNote" Runat="server" Text="&nbsp;Note: Action should not have any special charactors and must be unique."></asp:Literal>
		</td>
	</tr>
</table>

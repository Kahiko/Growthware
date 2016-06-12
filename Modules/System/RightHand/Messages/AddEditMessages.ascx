<%@ Control Language="vb" AutoEventWireup="false" Codebehind="AddEditMessages.ascx.vb" Inherits="BaseApplication.AddEditMessages" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<h3>Messages</h3>
<div class="pageDescription" style="WIDTH:500px">
	In this page, you can edit messages, which can be used as error or general 
	information text in response to an action by a user. For example, 'Invalid 
	Password' or 'Please allow 24 hours for your submission to appear.'
</div>
<label ID="ClientMSG" Runat="server" style="FONT-WEIGHT: bold; COLOR: red"></label>
<table border="0" cellpadding="0" cellspacing="3" width="100%">
	<tr>
		<td>
			<asp:DropDownList id="dropMessageNames" AutoPostBack="true" Runat="Server" />
		</td>
	</tr>
	<tr>
		<td>
			<asp:Label id="lblMessageDescription" Runat="Server" />
		</td>
	</tr>
	<tr>
		<td>
			<table border="0" cellpadding="0" cellspacing="3">
				<tr>
					<td>
						Message Title
					</td>
				</tr>
				<tr>
					<td>
						<asp:TextBox id="txtMessageTitle" Columns="50" Runat="Server" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td>
			Message Body
			<br>
			<asp:TextBox id="txtMessageBody" TextMode="MultiLine" Width="100%" Columns="50" Rows="20" Runat="Server" />
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:Button id="btnEdit" Text="Save Message" Runat="Server" />
		</td>
	</tr>
</table>

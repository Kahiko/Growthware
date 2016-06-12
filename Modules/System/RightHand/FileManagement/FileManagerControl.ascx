<%@ Control Language="vb" AutoEventWireup="false" Codebehind="FileManagerControl.ascx.vb" Inherits="BaseApplication.FileManagerControl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<SCRIPT>
function selectAll(){
	var btn = document.getElementById('_ctl0_RightHandModulesLoader__ctl0_cmdSelect');
	var checked = false;
	if(btn.value == "Select All"){
		btn.value = "De-Select All";
		checked = true;
	}else{
		btn.value = "Select All";
		checked = false;
	}
	try{
		var checkBoxes = document.getElementsByTagName('input');
		for (var i=0; i<checkBoxes.length; i++){
			var checkBox = checkBoxes[i];
			if(checkBox.type=='checkbox'){
				var id = checkBox.id
				if(id.indexOf('DeleteCheckBox') > 0){
					checkBox.checked=checked;
				}
			}
		}
	}catch(e){
		//alert(e.message);
	}
}

function btnDelete_Click() {
	answer = window.confirm("Are you sure want to delete this?");
	if(answer) {
		return true;
	}
	else {
		return false;
	}
	
}
</SCRIPT>
<font style="COLOR: red">
	<asp:literal id="litErrorMSG" Runat="server" EnableViewState="true" Visible="False"></asp:literal>
</font>
<table id="Table1" cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr style="FONT-SIZE: 8pt; FONT-FAMILY: verdana" align="left">
		<td vAlign="middle" noWrap align="left" colSpan="5">
			&nbsp;Current Directory:&nbsp;
			<asp:label id="currentDirectory" Runat="server"></asp:label>
			&nbsp;&nbsp;
			<b>
				<asp:label id="lblClientMSG" Runat="server" Visible="False"></asp:label>
			</b>
		</td>
	</tr>
	<tr id="firstRow" style="FONT-SIZE: 8pt; FONT-FAMILY: verdana" runat="server">
		<td width="1">&nbsp;</td>
		<td vAlign="middle" noWrap>
			<ASP:IMAGEBUTTON id="btnDelete" ALTERNATETEXT="Delete" RUNAT="server" ImageUrl="~/UI/Default/images/delete.gif"></ASP:IMAGEBUTTON><br>
			<input id="cmdSelect" type="button" value="Select All" onclick="selectAll();" name="cmdSelect" runat="server">
		</td>
		<td vAlign="middle" noWrap>
			&nbsp;<ASP:IMAGEBUTTON id="btnGoUp" ALTERNATETEXT="Up One Level" IMAGEURL="~/UI/Default/images/FolderUp.gif" RUNAT="server"></ASP:IMAGEBUTTON>
		</td>
		<td vAlign="middle" noWrap align="left">
			<asp:literal id="litUploadFile" Runat="server" Text="&nbsp;&nbsp;Upload File&nbsp;&nbsp;"></asp:literal>
			<input id="txtFileToUpload" type="file" name="txtFileToUpload" runat="server"> &nbsp;&nbsp;
			<input id="btnUpLoad" type="button" value="Upload" name="btnUpLoad" runat="server" onserverclick="btnUpLoad_ServerClick">
		</td>
		<td vAlign="middle" noWrap align="right" width="100%">
			<asp:literal id="CreateNewDirectory" Runat="server" Text="Create New Directory&nbsp;&nbsp;"></asp:literal>
			<ASP:TEXTBOX id="txtNewDirectory" RUNAT="server" ENABLEVIEWSTATE="True"></ASP:TEXTBOX>
			<ASP:IMAGEBUTTON id="btnNewDirectory" ALTERNATETEXT="New Folder" IMAGEURL="~/UI/Default/images/btnNewFolder.gif" RUNAT="server"></ASP:IMAGEBUTTON>
		</td>
	</tr>
	<tr style="FONT-SIZE: 8pt; FONT-FAMILY: verdana">
		<td colSpan="7">
			<asp:datagrid id="DGFileSystem" runat="server" OnUpdateCommand="DGFileSystem_UpdateCommand" OnCancelCommand="DGFileSystem_CancelCommand"
				OnEditCommand="DGFileSystem_EditCommand" Width="100%" AutoGenerateColumns="False" GridLines="Vertical"
				CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#999999"
				Font-Size="8pt" Font-Name="verdana" AllowPaging="True" Font-Names="verdana" EnableViewState="true">
				<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
				<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
				<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
				<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
				<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
				<Columns>
					<asp:TemplateColumn>
						<ItemTemplate>
							<input type="checkbox" id="DeleteCheckBox" runat="server" NAME="DeleteCheckBox" />
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" CancelText="Cancel" EditText="Rename"></asp:EditCommandColumn>
					<asp:TemplateColumn SortExpression="Name" HeaderText="Name">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemTemplate>
							&nbsp;
							<ASP:PLACEHOLDER ID="plhImgEdit" RUNAT="server" />
							<ASP:IMAGE ID="imgType" RUNAT="server" BORDERWIDTH="0" BORDERSTYLE="None" />
							<ASP:LINKBUTTON ID="lnkName" CSSCLASS ="FileManager" RUNAT="server" TEXT='<%# DataBinder.Eval(Container.DataItem,"Name") %>' COMMANDNAME="ItemClicked" CAUSESVALIDATION="false"/>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox runat="server" id="txtFileName" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' />
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Type" ReadOnly="True" HeaderText="Type">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Size" ReadOnly="True" HeaderText="Size">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Modified" ReadOnly="True" HeaderText="Modified">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					</asp:BoundColumn>
				</Columns>
				<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
			</asp:datagrid>
		</td>
	</tr>
</table>
<asp:label id="literalPath" Runat="server" Visible="False"></asp:label>

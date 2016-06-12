<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.AddEditDropDownDetails" Codebehind="AddEditDropDownDetails.ascx.vb" %>



<table cellSpacing=0 cellPadding=0 width="95%" border=0>
  <tr>
    <td align=left>Select a drop down<br 
      ><asp:dropdownlist id=dropDropDowns Runat="server" AutoPostBack="True">
			</asp:dropdownlist></td></tr>
  <tr>
    <td vAlign=top>
      <table cellSpacing=0 cellPadding=0 width="100%" border=0 
      >
        <tr>
          <td vAlign=top width="100%"><asp:datagrid id=dgResults Runat="Server" AutoGenerateColumns="false" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="5" GridLines="Horizontal">
							<HeaderStyle Font-Bold="True" BackColor="#DCDDDE"></HeaderStyle>
							<AlternatingItemStyle BackColor="#eeeeee"></AlternatingItemStyle>
							<columns>
								<asp:EditCommandColumn ButtonType="LinkButton" HeaderText="Edit" EditText="Edit" UpdateText="Update" CancelText="Cancel" />
								<asp:BoundColumn DataField="DROP_BOX_DET_CODE" HeaderText="Code" />
								<asp:BoundColumn DataField="DROP_BOX_DET_VALUE" HeaderText="Value" />
								<asp:TemplateColumn HeaderText="Status" SortExpression="Status">
									<ItemTemplate>
										<asp:Label ID="lblStatus" runat="server" Text="setbycode"/>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:DropDownList runat="server" id="edit_Status" SelectedIndex='<%# GetStatusIndex(Container.DataItem("DROP_BOX_DET_STATUS")) %>'>
											<asp:ListItem Value="0">Active</asp:ListItem>
											<asp:ListItem Value="3">Inactive</asp:ListItem>
										</asp:DropDownList>
									</EditItemTemplate>
								</asp:TemplateColumn>
							</columns>
						</asp:datagrid></td>
          <td>
            <table style="FONT: 8pt verdana">
              <tr>
                <td style="FONT: 10pt verdana" colSpan=2 
                  >Add a New Drop down value:</td></tr>
              <tr>
                <td noWrap>Code: </td>
                <td><input id=txtCode type=text 
                  name=txtCode runat="server"><br 
                  ></td></tr>
              <tr>
                <td noWrap>Value: </td>
                <td><input id=txtValue type=text 
                  name=txtVale runat="server"><br 
                  ></td></tr>
              <tr>
                <td></td>
                <td style="PADDING-TOP: 15px"><asp:button id=btnAddDropDownValue runat="server" Text="Add drop down"></asp:button></td></tr>
              <tr>
                <td style="PADDING-TOP: 15px" align=center colSpan=2 
                ><span id=Message 
                  runat="server"></span></td></tr></table></td></tr></table></td></tr></table>

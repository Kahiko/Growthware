<%@ Register TagPrefix="CustomWebControls" Namespace="Common.CustomWebControls" Assembly="Common" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ReportNavigator.ascx.vb" Inherits="BaseApplication.ReportNavigator" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table cellSpacing=0 cellPadding=0 width="100%" border=0>
	<tr>
		<td>
			<asp:button id=btnRefresh Runat="server" Text="Refresh"></asp:button>
		</td>
	</tr>
	<tr>
		<td>
			&nbsp;&nbsp;<CUSTOMWEBCONTROLS:ALPHAPICKER id="AlphaPicker" Runat="Server"></CUSTOMWEBCONTROLS:ALPHAPICKER>
		</td>
	</tr>
	<tr>
		<td>
			<table>
				<tr>
					<td>
						<table>
							<tr>
								<td>Records Per Page:</td>
							</tr>
							<tr>
								<td>
									<asp:textbox id="txtRecordsPerPage" Runat="server" AutoPostBack="True"></asp:textbox>
								</td>
							</tr>
						</table>
					</td>
					<td>
						<table>
							<tr>
								<td>Sort By:</td>
							</tr>
							<tr>
								<td>
									<asp:dropdownlist id="dropSortBy" Runat="Server" AutoPostBack="True">
										<asp:ListItem Text="Title" Value="0"></asp:ListItem>
										<asp:ListItem Text="ID" Value="1"></asp:ListItem>
									</asp:dropdownlist>
								</td>
							</tr>
						</table>
					</td>
					<td>
						<table>
							<tr>
								<td>Order By:</td>
							</tr>
							<tr>
								<td>
									<asp:dropdownlist id="dropOrderBy" Runat="Server" AutoPostBack="True">
										<asp:ListItem Text="Ascending" value="0" Selected="True"></asp:ListItem>
										<asp:ListItem Text="Descending" value="1"></asp:ListItem>
									</asp:dropdownlist>
								</td>
							</tr>
						</table>
					</td>
					<td>
						<table>
							<tr>
								<td>By Report Name:</td>
							</tr>
							<tr>
								<td>
									<asp:TextBox ID="txtReportName" Runat="server"></asp:TextBox>
								</td>
							</tr>
						</table>
					</td>
					<td>
						<table>
							<tr>
								<td>By Report ID:</td>
							</tr>
							<tr>
								<td>
									<asp:TextBox ID="txtReportID" Runat="server"></asp:TextBox>
								</td>
							</tr>
						</table>
					</td>
					<td valign="top">
						<asp:Button ID="btnSearch" Text="Search" Runat="server"></asp:Button>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td>
			&nbsp;<ASP:IMAGEBUTTON id="btnGoUp" ALTERNATETEXT="Up One Level" IMAGEURL="~/UI/Default/images/FolderUp.gif" RUNAT="server"></ASP:IMAGEBUTTON>		
		</td>
	</tr>
	<tr>
		<td>
			<asp:datagrid id=dgReportResults 
					runat="server" 
					AutoGenerateColumns="false" 
					EnableViewState="true" 
					Font-Names="verdana" 
					AllowPaging="True" 
					Allowsorting="True"
					Font-Name="verdana" 
					Font-Size="8pt" 
					BorderColor="#999999" 
					BorderStyle="None" 
					BorderWidth="1px" 
					BackColor="White" 
					CellPadding="3" 
					GridLines="Vertical" 
					Width="100%">
				<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
				<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
				<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
				<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#000084"></HeaderStyle>
				<FooterStyle ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
				<Columns>
					<asp:TemplateColumn HeaderText="Name">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
						<ItemTemplate>
							<ASP:IMAGE ID="imgType" RUNAT="server" BORDERWIDTH="0" BORDERSTYLE="None" />
							<asp:LinkButton id="lnkReport" TEXT='<%# DataBinder.Eval(Container.DataItem,"Title") %>' CommandName="ItemClicked" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"ID") %>' runat="server">LinkButton</asp:LinkButton>
							<asp:HyperLink ID="hyperReport" Text="set by code" Runat="server"></asp:HyperLink>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="Description" ReadOnly="True" HeaderText="Description">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Type" ReadOnly="True" HeaderText="Type">
						<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
					</asp:BoundColumn>
				</Columns>
				<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
			</asp:datagrid>
		</td>
	</tr>
	<tr>
		<td>
			<asp:PlaceHolder ID="plcReportsList" Runat="server"></asp:PlaceHolder>
		</td>
	</tr>
</table>

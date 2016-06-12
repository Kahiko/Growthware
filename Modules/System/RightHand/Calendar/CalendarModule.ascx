<%@ Control Language="vb" AutoEventWireup="false" Codebehind="CalendarModule.ascx.vb" Inherits="BaseApplication.CalendarModule" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table height="100%" cellSpacing="0" cellPadding="0" width="100%" border="0">
	<tr>
		<td>
			<font color="red">
				<asp:Literal ID="litError" Runat="server" Visible="False"></asp:Literal>
			</font>
		</td>
	</tr>
	<tr>
		<td align="center" valign="top">
			<table width="95%">
				<tr>
					<td>
						<asp:calendar id="CalendarControl" runat="server" Height="250px" Font-Size="9pt" Font-Names="Verdana" DayHeaderStyle-Wrap="false"
							Width="100%" DayHeaderStyle-HorizontalAlign="NotSet" DayStyle-HorizontalAlign="Left" BorderStyle="Solid" NextPrevFormat="ShortMonth" BackColor="White" ForeColor="Black" CellSpacing="1" BorderColor="Black">
							<TodayDayStyle ForeColor="White" BackColor="#999999">
							</TodayDayStyle>

							<DayStyle BackColor="#CCCCCC">
							</DayStyle>

							<NextPrevStyle Font-Size="8pt" Font-Bold="True" ForeColor="White">
							</NextPrevStyle>

							<DayHeaderStyle Font-Size="8pt" Font-Bold="True" Height="8pt" ForeColor="#333333">
							</DayHeaderStyle>

							<SelectedDayStyle ForeColor="White" BackColor="#333399">
							</SelectedDayStyle>

							<TitleStyle Font-Size="12pt" Font-Bold="True" Height="12pt" ForeColor="White" BackColor="#333399">
							</TitleStyle>

							<OtherMonthDayStyle ForeColor="#999999">
							</OtherMonthDayStyle>
						</asp:calendar>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td>
			<table width="100%">
				<tr>
					<td valign="top">
						<ul>Enter:
							<li>Select the date 
              
							<li>Enter Comment 
              
							<li>Click Save</li>
						</ul>
					</td>
					<td valign="top">
						<ul>Delete:
							<li>Select the date 
              
							<li>Copy the desired text 
              
							<li>Paste the text into the "Comments:" box 
              
							<li>Click Delete</li>
						</ul>
					</td>
				</tr>
				<tr>
					<td colspan="2">Comments:&nbsp;<asp:textbox id="txtComments" runat="server" Width="80%"></asp:textbox></td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td>
			<table width="100%">
				<tr>
					<td align="left">
						<asp:button id="BtnSave" runat="server" BackColor="SteelBlue" Width="82px" ForeColor="White" Font-Names="Arial" Text="Save" BorderStyle="None" Font-Bold="True" Visible="False"></asp:button>
					</td>
					<td align="right">
						<asp:button id="BtnDelete" runat="server" BackColor="SteelBlue" Width="80px" ForeColor="White" Font-Names="Arial" Text="Delete" BorderStyle="None" Font-Bold="True" Visible="False"></asp:button>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DatePicker.aspx.vb" Inherits="ApplicationBase.DatePicker" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
  <HEAD>
		<title>DatePicker</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<style type="text/css">
			BODY { PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 4px; PADDING-TOP: 0px }
			BODY { FONT-SIZE: 9pt; FONT-FAMILY: Verdana, Geneva, Sans-Serif }
			TABLE { FONT-SIZE: 9pt; FONT-FAMILY: Verdana, Geneva, Sans-Serif }
			TR { FONT-SIZE: 9pt; FONT-FAMILY: Verdana, Geneva, Sans-Serif }
			TD { FONT-SIZE: 9pt; FONT-FAMILY: Verdana, Geneva, Sans-Serif }
		</style>
		<script>
			//document.forms(0).
		</script>
	</HEAD>
	<body onblur="/*this.window.focus();*/" ms_positioning="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<asp:calendar id="Calendar1" runat="server" bordercolor="White" BorderWidth="1px" NextPrevFormat="FullMonth" BackColor="White" Width="304px" ForeColor="Black" Height="224px" Font-Size="9pt" Font-Names="Verdana">
				<TodayDayStyle BackColor="#CCCCCC">
				</TodayDayStyle>

				<NextPrevStyle Font-Size="8pt" Font-Bold="True" ForeColor="#333333" VerticalAlign="Bottom">
				</NextPrevStyle>

				<DayHeaderStyle Font-Size="8pt" Font-Bold="True">
				</DayHeaderStyle>

				<SelectedDayStyle ForeColor="White" BackColor="#333399">
				</SelectedDayStyle>

				<TitleStyle Font-Size="12pt" Font-Bold="True" BorderWidth="4px" ForeColor="#333399" BorderColor="Black" BackColor="White">
				</TitleStyle>

				<OtherMonthDayStyle ForeColor="#999999">
				</OtherMonthDayStyle>
			</asp:calendar>
		</form>
	</body>
</HTML>
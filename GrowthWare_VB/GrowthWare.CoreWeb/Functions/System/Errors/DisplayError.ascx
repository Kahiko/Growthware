<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DisplayError.ascx.vb" Inherits="GrowthWare.CoreWeb.DisplayError" %>
<script language="javascript" type="text/javascript">
    function ToggleDetails() {
        var lnk = document.getElementById("ToggleLink");
        var pnl = document.getElementById("pnlStack");
        if (lnk.innerText == "Hide") {
            lnk.innerText = "Show";
            pnl.style.visibility = "hidden";
        } else {
            lnk.innerText = "Hide";
            pnl.style.visibility = "visible";
        }
    }
</script>

<table id="Table3" cellspacing="2" cellpadding="0" width="75%" border="0">
	<tr>
		<td align="right" colspan="2">
			<%--<GWFCustomeWebControls:NotifyCheckBox ID="NotifyCheckBox" Text="Notify when contents change" runat="server" />&nbsp;&nbsp;--%>
		</td>
	</tr>
	<tr>
		<td style="height: 40px; width: 40px">
			<img id="error" src="/Public/Images/icon_error.gif" alt="Error icon" />
		</td>
		<td align="left" class="clsLegend">
			<font style="white-space: nowrap;" color="red" size="2">An application error was encountered.</font>
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<hr />
		</td>
	</tr>
</table>
<fieldset class="ErrorFieldset" style="width: 85%">
	<legend class="HTMLLegend">Unexpected Error</legend>
	<table id="Table2" cellspacing="10" cellpadding="0" width="1px" border="0">
		<tr>
			<td class="Form_LabelText">
				<b>Security Entity Name </b>
			</td>
			<td class="Form_LabelText">
				<asp:Label ID="lblBUName" runat="server"></asp:Label>
			</td>
			<td class="Form_LabelText">
				<b>Computer</b>
			</td>
			<td class="Form_LabelText">
				<asp:Label ID="lblComputer" runat="server"></asp:Label>
			</td>
			<td class="Form_LabelText">
				<b>Account</b>
			</td>
			<td class="Form_LabelText">
				<asp:Label ID="lblAccount" runat="server"></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="Form_LabelText">
				<b>Operating System</b>
			</td>
			<td class="Form_LabelText">
				<asp:Label ID="lblOpSystem" runat="server"></asp:Label>
			</td>
			<td class="Form_LabelText">
				<b>Browser Version </b>
			</td>
			<td class="Form_LabelText">
				<asp:Label ID="lblBrowser" runat="server"></asp:Label>
			</td>
			<td class="Form_LabelText" width="7%">
				<b>Time </b>
			</td>
			<td class="Form_LabelText">
				<asp:Label ID="lbltoday" class="Form_LabelText" runat="server"></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="Form_LabelText">
				<b>Error Action &nbsp;&nbsp;</b>
			</td>
			<td class="Form_LabelText" colspan="5">
				<asp:Label ID="lblErrSource" runat="server"></asp:Label>
			</td>
		</tr>
		<tr>
			<td valign="top" class="Form_LabelText">
				<b>Error Description &nbsp;&nbsp;</b>
			</td>
			<td class="Form_LabelText" colspan="5">
				<font color="red">
					<asp:Label ID="lblErrMsg" runat="server"></asp:Label>
				</font>
			</td>
		</tr>
		<tr>
			<td align="left" colspan="6" class="Form_LabelText">
				<p>
					<b>
						This error has been logged in &nbsp;<asp:Label CssClass="Form_LabelText" ID="lblErrorlog" runat="server"></asp:Label><br/><br/>
						Please Contact System Administrator or notify the help desk if the error persists.
					</b>
				</p>
			</td>
		</tr>
		<tr>
			<td>
				&nbsp;
			</td>
			<td>
				<a id="PrintLink" href="javascript:window.print();">Print this error</a>
			</td>
			<td>
				&nbsp;
			</td>
			<td>
				&nbsp;
			</td>
			<td>
				&nbsp;
			</td>
			<td>
				&nbsp;
			</td>
		</tr>
	</table>
	<table id="Table1" cellspacing="10" cellpadding="0" width="75%" border="0">
		<tr>
			<td colspan="2">
				<hr/>
			</td>
		</tr>
		<tr>
			<td valign="top" class="Form_LabelText" width="13%">
				<b>Other Error Details &nbsp;</b>
			</td>
			<td class="Form_LabelText" align="left">
				<a id="ToggleLink" href="javascript:ToggleDetails();">Show</a>
				<div id="pnlStack" style="visibility: hidden; white-space:normal;">
					&nbsp;
					<fieldset>
						<asp:Label ID="lblErrMsg1" Style="white-space: normal" runat="server"></asp:Label>
					</fieldset>
				</div>
			</td>
		</tr>
	</table>
</fieldset>

<%@ Control Language="vb" AutoEventWireup="false" Inherits="ApplicationBase.StatesYears" Codebehind="StatesYears.ascx.vb" %>



<script>
        function init(){
            optionTest = true;
            lgth = document.forms[0].Year.options.length - 1;
            populateYear();
        }

		function setConnectionString(){
			var selectedYear = document.getElementById('Year');
			var txtConnectionString = document.getElementById('ConnectionString');
			var chkIsActive = document.getElementById('isActive');
			var list = yearArray[selectedYear.options.selectedIndex];
			txtConnectionString.value=list[2];
			if(list[3]){
				chkIsActive.checked = true;
			}
		}
		
        function populateYear(){
			try{
				var box = document.forms[0].Year;
				var currentYear;
				var txtConnectionString = document.getElementById('ConnectionString');
				box.options.length = 0;
				for ( i = 0; i < yearArray.length; i ++){
					var list = yearArray[i];
					var txtCurrentYear = document.getElementById('RightHandModulesLoader__ctl0_StatesYears_txtCurrentYear')
					box.options[i] = new Option(list[i], list[i]);
					if(box.options[i].value == currentYear){
						box.options[i].selected = true;
						txtConnectionString.value=list[2]
					}else{
						box.options[i].selected = false;
					}
				}
				setConnectionString();
			}catch(e){
				// chances are that there is no year array
				//alert('Error in populateYear() ' + e + '\n' + e.message)
			}
        }
</script>
<center>
<table cellPadding="2" width="100%">
	<tr>
		<td class="Form_SubTitle" align="center" colSpan="2">
			<B>Year&nbsp;</B>
		</td>
	</tr>
	<TR>
		<td>
			<table>
				<tr>
					<TD>Year</TD>
					<TD><SELECT id="Year" onchange="setConnectionString();" name="Year"></SELECT></TD>
					<td><input id="ConnectionString" style="WIDTH: 256px" type="text"> </td>
					<td>Active: </td>
					<td>
						<input id="isActive" type="checkbox">
					</td>
				</tr>
			</table>
		</td>
	</TR>
</table>
<hr>
<asp:Panel id="pnlAddYear" Runat="Server">
<TABLE class=Form cellSpacing=3 cellPadding=0 border=0>
  <TR>
    <TD align=center colSpan=2>Add Year</TD>
  <TR>
    <TD>Year:</TD>
    <TD><asp:TextBox id=txtNewYear Runat="Server"></asp:TextBox></TD></TR>
  <TR>
    <TD>Connection String:</TD>
    <TD><asp:TextBox id=txtNewConnectionString Runat="Server"></asp:TextBox></TD></TR>
  <TR>
    <TD>Active: </TD>
    <TD><INPUT id=newIsActive type=checkbox> </TD></TR>
  <TR>
    <TD align=center colSpan=2><asp:Button id=btnAdd Runat="Server" Text="Add Year"></asp:Button></TD></TR></TABLE>
</asp:Panel>
</center>
<div style="DISPLAY: none">
	<asp:textbox id="txtCurrentYear" runat="server" />
</div>

<script>
	init();
</script>


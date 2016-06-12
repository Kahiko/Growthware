<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EncryptDecrypt.aspx.vb" Inherits="GrowthWare.WebApplication.EncryptDecrypt" %>
<form id="frmEncryptDecrypt" runat="server">
    <div>
		<table border="0" cellpadding="0" cellspacing="3">
			<tr>
				<td valign="top" align="right" style="height: 40px">
					<span class="Form_LabelText">Text:</span>
				</td>
				<td valign="top" align="left" style="width: 600px; height: 40px">
					<asp:TextBox ID="txtValue" TextMode="multiLine" runat="server" Height="70px" Width="600px"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td valign="top" align="right">
					<span class="Form_LabelText">Processed Text:</span>
				</td>
				<td valign="top" style="width: 600px">
					<asp:TextBox ID="txtProcessed" TextMode="multiLine" runat="server" Height="70px" Width="600px"></asp:TextBox>
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;
				</td>
				<td>
					<table border="0" cellpadding="0" cellspacing="3">
						<tr>
							<td align="left" style="width: 500px">
								<input type="button" id="cmdEncrypt" value="Encrypt" onclick="javascript:encrypt(true);" />
								<input type="button" id="cmdDecrypt" value="Decrypt" onclick="javascript:encrypt(false);" />
							</td>
							<td align="right">
								<a id="hyperCopyText" class="highlighttext" href="javascript:highlightAll('txtEncrypted')" runat="server">Highlight</a>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>    
    </div>
</form>
<script language="Javascript" type="text/javascript">
<!--

    function highlightAll(theField) {
        var copytoclip = 1;
        var tempval = $('#' + theField);
        tempval.focus()
        tempval.select()
        if (document.all && copytoclip == 1) {
            therange = tempval.createTextRange()
            therange.execCommand("Copy")
            window.status = "Contents highlighted and copied to clipboard!"
            setTimeout("window.status=''", 1800)
        }
    }

    function encrypt(encrypt) {
        $txtProcessed = $("#<%=txtProcessed.ClientID %>");
		    $txtProcessed.val('');
		    var textValue = new Object();
		    txtValue.textValue = $("#<%=txtValue.ClientID %>").val();
			var options = GW.Model.DefaultWebMethodOptions();
			options.async = true;
			if (encrypt) {
			    options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Encrypt/EncryptDecrypt.aspx/Encrypt?Action=" + GW.Navigation.currentAction;
			} else {
			    options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Encrypt/EncryptDecrypt.aspx/Decrypt?Action=" + GW.Navigation.currentAction;
			}
			options.data = txtValue;
			options.contentType = 'application/json; charset=utf-8';
			options.dataType = 'json';
			options.timeout = 3000;
			GW.Common.JQueryHelper.callWeb(options, encryptSucess, encryptError);
        }

        function encryptSucess(xhr) {
            $txtProcessed = $("#<%=txtProcessed.ClientID %>");
		    $txtProcessed.val(xhr.d);
		}

		function encryptError(xhr, status, error) {
		    $txtProcessed = $("#<%=txtProcessed.ClientID %>");
		    $txtProcessed.val('error getting content\n' + error);
		}
		//-->
</script>
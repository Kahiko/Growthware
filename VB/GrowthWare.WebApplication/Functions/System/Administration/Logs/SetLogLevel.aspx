<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SetLogLevel.aspx.vb" Inherits="GrowthWare.WebApplication.SetLogLevel" %>

<!DOCTYPE html>

<form id="frmSetLogLevel" runat="server">
    <p>
        <div>
            <select id="dropLogLevel" Class="rounded" runat="server">
                <option value="0">Debug</option>
                <option value="1">Information</option>
                <option value="2">Warning</option>
                <option value="3">Error</option>
                <option value="4">Fatal</option>
            </select>
            <br />
            <br />
            <input type="button" id="btnSubmit" onclick="javascript:setLogLevel();" value="Set Logging level" />
        </div>
    </p>
</form>
<script type="text/ecmascript" language="javascript">
    $(document).ready(function () {
        $('#btnSubmit').button();
    });

    function setLogLevel() {
        var logLevel = parseInt($("#<%=dropLogLevel.ClientID %> option:selected").val());
        GW.Common.debug('logLevel: ' + logLevel);
        var options = GW.Model.DefaultWebMethodOptions();
        options.async = true;
        options.data = { "logLevel": logLevel };
        options.contentType = 'application/json; charset=utf-8';
        options.dataType = 'json';
        options.url = GW.Common.getBaseURL() + "/Functions/System/Administration/Logs/SetLogLevel.aspx/InvokeSetLogLevel"
        GW.Common.JQueryHelper.callWeb(options);
    }
</script>
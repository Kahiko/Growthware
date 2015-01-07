<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddEditAccount.aspx.vb" UnobtrusiveValidationMode="None" Inherits="GrowthWare.WebApplication.AddEditAccount" %>
<%@ Register Src="~/UserControls/AddEditAccount.ascx" TagPrefix="uc" TagName="AddEditAccount" %>
<!DOCTYPE html>
<html>
    <head>
        <title>Add or Edit Account</title>

    </head>
    <body>
        <uc:AddEditAccount runat="server" id="AddEditAccount" />
    </body>
</html>

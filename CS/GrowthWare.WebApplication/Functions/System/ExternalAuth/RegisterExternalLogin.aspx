<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterExternalLogin.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.ExternalAuth.RegisterExternalLogin" %>
<%@ Register Src="~/UserControls/AddEditAccount.ascx" TagPrefix="uc" TagName="AddEditAccount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="frmRegisterExternalLogin" runat="server">
        <uc:AddEditAccount runat="server" id="AddEditAccount" />
    </form>
</body>
</html>

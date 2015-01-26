<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEditAccount.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.Accounts.AddEditAccount" %>
<%@ Register Src="~/UserControls/AddEditAccount.ascx" TagPrefix="uc" TagName="AddEditAccount" %>

<!DOCTYPE html>
<html>
    <head>
        <title>Add or Edit Account</title>
    </head>
    <body>
	    <form id="frmAddEditAccount" runat="server">
            <uc:AddEditAccount runat="server" id="AddEditAccountUserControl" />
	    </form>
    </body>
</html>
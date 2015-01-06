<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RegisterExternalLogin.aspx.vb" Inherits="GrowthWare.WebApplication.RegisterExternalLogin1" %>
<%@ Register Src="~/UserControls/AddEditAccount.ascx" TagPrefix="uc" TagName="AddEditAccount" %>
<!DOCTYPE html>
<html>

    <body>
        <h3>Register with your <%: ProviderName %> account</h3>

        <asp:PlaceHolder runat="server">
            <uc:AddEditAccount runat="server" id="AddEditAccount" />
        </asp:PlaceHolder>
    </body>
</html>
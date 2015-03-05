<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="OpenAuthProviders.ascx.vb" Inherits="GrowthWare.WebApplication.OpenAuthProviders" %>
<script type="text/javascript">
    function useProvider(provider) {
        window.location = "/Functions/System/ExternalAuth/OpenAuthProviderLogon.aspx?provider=" + provider;
    }

</script>
<div id="socialLoginList">
    <h4 id="thirdPartyAuthentication" runat="server" visible="false">Use another service to log in.</h4>
    <hr />
    <asp:ListView runat="server" ID="providerDetails" ItemType="System.String" SelectMethod="GetProviderNames" ViewStateMode="Disabled" Visible="false">
        <ItemTemplate>
            <p>
                <button type="button" class="btn btn-default" name="provider" onclick="javascript:useProvider('<%#: Item %>')" value="<%#: Item %>" title="Log in using your <%#: Item %> account.">
                    <%#: Item %>
                </button>
            </p>
        </ItemTemplate>
    </asp:ListView>
</div>
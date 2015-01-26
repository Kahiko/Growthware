<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEditSecurityEntities.aspx.cs" Inherits="GrowthWare.WebApplication.Functions.System.Administration.SecurityEntities.AddEditSecurityEntities" %>
<html>
    <head>
        <title>Add or Edit Security Entities</title>
            <script type="text/javascript" language="javascript">
                $(document).ready(function () {
                    $("#<%=dropDAL.ClientID %>").change(function (e) {
                        drapDALChange($(this).val())
                    });
                });

                function updateData() {
                    var profile = {};
                    profile.Id = parseInt($("#<%=txtSeqID.ClientID %>").val());
                    profile.Name = $("#<%=txtSecurityEntity.ClientID %>").val();
                    profile.Description = $("#<%=txtDescription.ClientID %>").val();
                    profile.Url = $("#<%=txtURL.ClientID %>").val();
                    profile.DAL = $("#<%=dropDAL.ClientID %> option:selected").val();
                    profile.DALAssemblyName = $("#<%=txtAssembly_Name.ClientID %>").val();
                    profile.DALNamespace = $("#<%=txtName_Space.ClientID %>").val();
                    profile.ConnectionString = $("#<%=txtConnectionstring.ClientID %>").val();
                    profile.ParentSeqId = parseInt($("#<%=dropParent.ClientID %> option:selected").val());
                    profile.Skin = $("#<%=dropSkin.ClientID %> option:selected").val();
                    profile.Style = $("#<%=dropStyles.ClientID %> option:selected").val();
                    profile.EncryptionType = parseInt($("#<%=dropEncryptionType.ClientID %> option:selected").val());
                    profile.StatusSeqId = parseInt($("#<%=dropStatus.ClientID %> option:selected").val());
                    return profile;
                }

                function saveAddEdit($dialogWindow) {
                    if (Page_ClientValidate()) {
                        var theData = updateData();
                        GW.Common.debug(theData);
                        var options = GW.Model.DefaultWebMethodOptions();
                        options.async = false;
                        options.data = theData;
                        options.contentType = 'application/json; charset=utf-8';
                        options.dataType = 'json';
                        options.url = GW.Common.getBaseURL() + "/gw/api/SecurityEntities/Save?Action=SearchSecurityEntities"
                        GW.Common.JQueryHelper.callWeb(options, saveAddEditSucess, saveAddEditError);
                        if (!($dialogWindow === undefined)) {
                            $dialogWindow.dialog("close");
                        }
                    }
                }

                function saveAddEditSucess(xhr) {
                    if (xhr == true) {
                        GW.Search.GetSearchResults();
                        location.reload(true);
                    } else {
                        alert('Error saving!!!');
                    }
                }

                function saveAddEditError(xhr, status, error) {

                }

                function drapDALChange(selectedDAL) {
                    $("#<%=txtConnectionstring.ClientID %>").val("");
	                switch (selectedDAL) {
	                    case "SQLServer":
	                        $("#<%=txtAssembly_Name.ClientID %>").val("GrowthWare.Framework.BusinessData");
	                        $("#<%=txtName_Space.ClientID %>").val("GrowthWare.Framework.BusinessData.DataAccessLayer.SQLServer.V2008");
	                        break;
                        case "Oracle":
                            $("#<%=txtAssembly_Name.ClientID %>").val("GrowthWare.Framework.BusinessData");
	                        $("#<%=txtName_Space.ClientID %>").val("GrowthWare.Framework.BusinessData.DataAccessLayer.Oracle.11g");
	                        break;
                        case "MySql":
                            $("#<%=txtAssembly_Name.ClientID %>").val("GrowthWare.Framework.BusinessData");
                            $("#<%=txtName_Space.ClientID %>").val("GrowthWare.Framework.BusinessData.DataAccessLayer.MySql.V5621");
	                        break;
                        default:
                            $("#<%=txtAssembly_Name.ClientID %>").val("GrowthWare.Framework.BusinessData");
                            $("#<%=txtName_Space.ClientID %>").val("GrowthWare.Framework.DataAccessLayer.SQLServer.V2008");
                    }
                }
            </script>
            <style type="text/css">
                .formLayout
                {
                    background-color: #f3f3f3;
                    border: solid 1px #a1a1a1;
                    padding: 5px;
                }
    
                .formLayout label, .formLayout input
                {
                    display: block;
                    width: 224px;
                    float: left;
                    margin-bottom: 10px;
                }
 
                .formLayout label
                {
                    text-align: right;
                    padding-right: 20px;
                }
 
                br
                {
                    clear: left;
                }
            </style>
    </head>
    <body>
        <form id="frmAddEditSecurityEntities" runat="server">
	        <asp:TextBox ID="txtSeqID" Style="display: none;" runat="server"></asp:textbox>
			<b><asp:literal id="litClientMsg" runat="server"></asp:literal></b>
            <div class="formLayout">
		        <label for="txtSecurityEntity" class="formLabelText">
			        <asp:literal id="litSecurityEntityTranslation" runat="server"></asp:literal>
			        :&nbsp;
		        </label>
			    <asp:literal id="litSecurityEntity" runat="server"></asp:literal>
			    <asp:TextBox ID="txtSecurityEntity" CssClass="rounded" onkeypress="return GW.Common.Validation.textboxMultilineMaxNumber(this,256,event)" Style="display: none;" runat="server" TextMode="MultiLine" Width="500px"></asp:TextBox>
			    <asp:requiredfieldvalidator controltovalidate="txtSecurityEntity" display="Dynamic" text="(required)" errormessage="Required" CssClass="failureNotification" runat="Server" id="Requiredfieldvalidator2">(required)</asp:requiredfieldvalidator>
                <br />
                <label for="txtDescription" class="formLabelText">Description: </label>
				<asp:TextBox ID="txtDescription" MaxLength="512" onkeypress="return GW.Common.Validation.textboxMultilineMaxNumber(this,512,event)" CssClass="rounded" runat="Server" TextMode="MultiLine" Width="500px" />
				<asp:requiredfieldvalidator controltovalidate="txtDescription" display="Dynamic" text="(required)" errormessage="Required" CssClass="failureNotification" runat="Server" id="Requiredfieldvalidator1">(required)</asp:requiredfieldvalidator>
                <br />
				<label for="txtURL" class="formLabelText">URL: </label>
				<asp:textbox id="txtURL" maxlength="128" CssClass="rounded" runat="Server" width="500px" />
                <br />
				<label for="dropDAL" class="formLabelText">Data Access Layer :</label>
				<asp:dropdownlist id="dropDAL" CssClass="rounded" runat="server">
					<asp:ListItem Value="SQLServer" Text="SQL Server" />
                    <asp:ListItem Value="MySql" Text="MySql" />
					<asp:ListItem Value="Oracle" Text="Oracle" />
				</asp:dropdownlist>
                <br />
				<label for="txtAssembly_Name" class="formLabelText">Data access layer DLL Name :</label>
				<asp:textbox id="txtAssembly_Name" width="500px" runat="server" maxlength="50" />
                <br />
                <label for="txtName_Space" class="formLabelText">Data access layer Namespace :</label>
			    <asp:textbox id="txtName_Space" width="500px" runat="server" maxlength="256" />
                <br />
				<label for="txtConnectionstring" class="formLabelText">Connection String :</label>
				<asp:TextBox ID="txtConnectionstring" TextMode="multiLine" MaxLength="512" onkeypress="return GW.Common.Validation.textboxMultilineMaxNumber(this,512)" runat="server" Rows="3" Wrap="true" Width="500px" />
				&nbsp;
				<asp:hyperlink id="hyperConnectionString" NavigateUrl="http://www.connectionstrings.com/" Target="_blank" runat="server">Build</asp:hyperlink>
                <br />
			    <label for="dropParent" class="formLabelText">Parent: </label>
			    <asp:dropdownlist id="dropParent" CssClass="rounded" runat="server" />
                <br />
				<label for="dropSkin" class="formLabelText">Skin: </label>
				<asp:dropdownlist id="dropSkin" CssClass="rounded" runat="server" />
                <br />
				<label for="dropStyles" class="formLabelText">Style: </label>
				<asp:dropdownlist id="dropStyles" CssClass="rounded" runat="server" />
                <br />
				<label for="dropEncryptionType" class="formLabelText">Encrypt Type:</label>
				<asp:dropdownlist id="dropEncryptionType" CssClass="rounded" runat="server">
					<asp:ListItem Value="1" Text="Triple DES" />
					<asp:ListItem Value="2" Text="DES" />
					<asp:ListItem Value="3" Text="None" />
				</asp:dropdownlist>
                <br />
				<label for="dropStatus" class="formLabelText">Status: </label>
				<asp:dropdownlist id="dropStatus" CssClass="rounded" runat="server">
					<asp:ListItem Value="1">Active</asp:ListItem>
					<asp:ListItem Value="2">Inactive</asp:ListItem>
				</asp:dropdownlist>
            </div>
        </form>
    </body>
</html>
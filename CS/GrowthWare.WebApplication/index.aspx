<%@ Page Title="" Language="C#" MasterPageFile="~/Public/Skins/Default/Default.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="GrowthWare.WebApplication.index" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%: Scripts.Render("~/bundles/jquery") %>
    <%: Scripts.Render("~/bundles/jqueryUI") %>
    <%: Scripts.Render("~/bundles/GrowthWare")%>

    <%: Styles.Render("~/Content/GrowthWare")%>
    <%: Styles.Render("~/Content/jQueryUIRedmond")%>

    <script type="text/javascript" language="javascript">
        function afterLoadFunctions() {
            // Create the refresh objects for all of the necessary UI elements
            var mRefreshObject = new GW.Navigation.RefreshObject();
            mRefreshObject.ContentAreaID = 'HorizontalHierarchicalDiv';
            mRefreshObject.Refresh = function refresh() {
                GW.Navigation.NavigationController.LoadPage("HorizontalHierarchicalMenu", "HorizontalHierarchicalDiv");
            }
            GW.Navigation.NavigationController.RegisterRefreshObject(mRefreshObject);

            //Put the vertical menu into the refresh objects
            mRefreshObject = new GW.Navigation.RefreshObject();
            mRefreshObject.ContentAreaID = 'VMenuDiv';
            mRefreshObject.Refresh = function refreshFunction() {
                GW.Navigation.NavigationController.LoadPage('VerticalMenu', 'VMenuDiv');
            }
            GW.Navigation.NavigationController.RegisterRefreshObject(mRefreshObject);

            //Put the horizontal menu into the refresh objects
            mRefreshObject = new GW.Navigation.RefreshObject();
            mRefreshObject.ContentAreaID = 'HorizontalDiv';
            mRefreshObject.Refresh = function refreshFunction() {
                GW.Navigation.NavigationController.LoadPage('HorizontalMenu', 'HorizontalDiv');
            }
            GW.Navigation.NavigationController.RegisterRefreshObject(mRefreshObject);

            //Put the GetPreferences into the refresh objects
            mRefreshObject = new GW.Navigation.RefreshObject();
            mRefreshObject.ContentAreaID = 'NotUsed';
            mRefreshObject.Refresh = function refreshFunction() {
                var options = GW.Model.DefaultWebMethodOptions();
                options.url = GW.Common.getBaseURL() + "/api/Accounts/GetPreferences?Action=GetPreferences";
                options.async = true;
                options.contentType = 'application/json; charset=utf-8';
                options.dataType = 'json';
                options.type = "GET";
                GW.Common.JQueryHelper.callWeb(options, getGetPreferencesSuccess);

                function getGetPreferencesSuccess(xhr) {
                    var mClientChoices = GW.Model.ClientChoices();
                    var mClientChoices = $.extend({}, mClientChoices, xhr);
                    if (mClientChoices.Environment != 'Prod') {
                        $('#spEnvironment').html('Environment: ' + mClientChoices.Environment);
                    } else {
                        $('#spEnvironment').css({ display: 'none' });
                    }
                    if (mClientChoices.AccountName != 'Anonymous') {
                        $('#spAccountName').html('Account: ' + mClientChoices.AccountName);
                    } else {
                        $('#spAccountName').css({ display: 'none' });
                    }
                    $('#spVersion').html('Version: ' + mClientChoices.Version);
                    $('#spSecurityEntity').html('Application: ' + mClientChoices.SecurityEntityName);
                }

            }
            GW.Navigation.NavigationController.RegisterRefreshObject(mRefreshObject);
        }

        $(document).ready(function () {
            var $loader = $('#loading'), timer = 0;
            $(document)
            .ajaxStart(function () {
                timer && clearTimeout(timer);
                timer = setTimeout(function () {
                    //$loader.show();
                    $loader.css('visibility', 'visible');
                },
				100);
            })
			.ajaxStop(function () {
			    clearTimeout(timer);
			    //$loader.hide();
			    $loader.css('visibility', 'hidden');
			});
            GW.Navigation.NavigationController.LoadFunctions(afterLoadFunctions);
            var currentHash = window.location.hash.substring(1);
            currentHash = currentHash.replace("?Action=", "");
            if (currentHash != '') {
                GW.Navigation.NavigationController.LoadPage(currentHash, "MainContentDiv");

            } else {
                GW.Navigation.NavigationController.LoadPage("GenericHome", "MainContentDiv");
            }
        });


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="MainContentDiv">
	</div>
</asp:Content>
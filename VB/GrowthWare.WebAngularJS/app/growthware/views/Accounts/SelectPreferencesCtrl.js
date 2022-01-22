(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'SelectPreferencesController';
    var mDependencyInjection = ['AccountService', 'MessageService', '$rootScope'];
    var mRetCtrl = function (acctSvc, msgSvc, $rootScope) {
        var thisCtrlr = this;
        // There is no set color schemes stored any where so it's completely up to the front end
        // to define them.
        var m_ColorSchemes = [
	          {color_Scheme: 'Blue'	    ,head_Color: '#C7C7C7'	,header_ForeColor: 'Black'	,row_BackColor: '#b6cbeb'	,alternatingRow_BackColor: '#6699cc'	,sub_Head_Color: '#b6cbeb'	,back_Color: '#ffffff'	,left_Color: '#eeeeee'}
	        , {color_Scheme: 'Green'	,head_Color: '#808577'	,header_ForeColor: 'White'	,row_BackColor: '#879966'	,alternatingRow_BackColor: '#c5e095'	,sub_Head_Color: '#879966'	,back_Color: '#ffffff'	,left_Color: '#eeeeee'}
	        , {color_Scheme: 'Yellow'	,head_Color: '#CF9C00'	,header_ForeColor: 'Black'	,row_BackColor: '#f8bc03'	,alternatingRow_BackColor: '#f8e094'	,sub_Head_Color: '#f8bc03'	,back_Color: '#ffffff'	,left_Color: '#f8e094'}
	        , {color_Scheme: 'Purple'	,head_Color: '#C7C7C7'	,header_ForeColor: 'Black'	,row_BackColor: '#be9cc5'	,alternatingRow_BackColor: '#91619b'	,sub_Head_Color: '#be9cc5'	,back_Color: '#ffffff'	,left_Color: '#eeeeee'}
	        , {color_Scheme: 'Red'	    ,head_Color: '#BA706A'	,header_ForeColor: 'White'	,row_BackColor: '#DE8587'	,alternatingRow_BackColor: '#A72A49'	,sub_Head_Color: '#df867f'	,back_Color: '#ffffff'	,left_Color: '#eeeeee'}
        ];
        thisCtrlr.colorSchemes = m_ColorSchemes;
        thisCtrlr.disableSave = true;
        thisCtrlr.dropFavoriteData = [];
        thisCtrlr.selectedFavorite = {};
        thisCtrlr.selectedColorScheme = {};

        function initCtrl() {
            // Request #1
            acctSvc.getPreferences(true).then(function (response) {
                // Response Handler #1
                // msgSvc.brodcastClientMsg(response);
                thisCtrlr.clientChoices = response;
                thisCtrlr.selectedColorScheme = thisCtrlr.clientChoices.ColorScheme;
                // Request #2
                return acctSvc.getSelectableActions(thisCtrlr.clientChoices.Account);
            }).then(function (response) {
                // Response Handler #2
                thisCtrlr.dropFavoriteData = response;
                for (var i = 0; i < thisCtrlr.dropFavoriteData.length; i++) {
                    var item = thisCtrlr.dropFavoriteData[i];
                    if (item.URL == thisCtrlr.clientChoices.Action && item.Title != 'Favorite') {
                        thisCtrlr.selectedFavorite = thisCtrlr.dropFavoriteData[i].URL;
                        break;
                    }
                }
                thisCtrlr.disableSave = false;
            });
        }

        thisCtrlr.save = function () {
            msgSvc.brodcastClientMsg('Saving');
            var mSelectedColorScheme = m_ColorSchemes.filter(function (item) {
                return item.color_Scheme === thisCtrlr.selectedColorScheme;
            })[0];
            thisCtrlr.clientChoices.AlternatingRowBackColor = mSelectedColorScheme.alternatingRow_BackColor;
            thisCtrlr.clientChoices.BackColor = mSelectedColorScheme.back_Color;
            thisCtrlr.clientChoices.ColorScheme = mSelectedColorScheme.color_Scheme;
            thisCtrlr.clientChoices.HeadColor = mSelectedColorScheme.head_Color;
            thisCtrlr.clientChoices.HeaderForeColor = mSelectedColorScheme.header_ForeColor;
            thisCtrlr.clientChoices.LeftColor = mSelectedColorScheme.left_Color;
            thisCtrlr.clientChoices.RowBackColor = mSelectedColorScheme.row_BackColor;
            thisCtrlr.clientChoices.SubheadColor = mSelectedColorScheme.sub_Head_Color;

            thisCtrlr.clientChoices.Action = thisCtrlr.selectedFavorite;

            acctSvc.saveClientChoices(thisCtrlr.clientChoices).then(
                /*** success ***/
                function (response) {
                    $rootScope.$broadcast('accountChanged', []);
                    msgSvc.brodcastClientMsg('Save Completed');
                    return acctSvc.updateAccountSession(false);
                },
                /*** error ***/
                function (response) {
                    GW.Common.debug(response);
                    msgSvc.brodcastClientMsg('Error saving the choices!');
                }
            ).then(function (response) {
                if (response == true) {
                    msgSvc.brodcastClientMsg('Save Completed and session information has been updated');
                } else {
                    msgSvc.brodcastClientMsg('Save Completed but there was an issue updating your session information.<br>You can logoff/logon or use the "Update" link.');
                }
            });
        }

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();
(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'SelectPreferencesController';
    var mDependencyInjection = ['AccountService', 'MessageService', '$scope', '$rootScope'];
    var mRetCtrl = function (acctSvc, msgSvc, $scope, $rootScope) {
        var thisCtrlr = this;
        var m_ViewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().
        // There is no set color schemes stored any where so it's completely up to the front end
        // to define them.
        var m_ColorSchemes = [
	          {color_Scheme: 'Blue'	    ,head_Color: '#C7C7C7'	,header_ForeColor: 'Black'	,row_BackColor: '#b6cbeb'	,alternatingRow_BackColor: '#6699cc'	,sub_Head_Color: '#b6cbeb'	,back_Color: '#ffffff'	,left_Color: '#eeeeee'}
	        , {color_Scheme: 'Green'	,head_Color: '#808577'	,header_ForeColor: 'White'	,row_BackColor: '#879966'	,alternatingRow_BackColor: '#c5e095'	,sub_Head_Color: '#879966'	,back_Color: '#ffffff'	,left_Color: '#eeeeee'}
	        , {color_Scheme: 'Yellow'	,head_Color: '#CF9C00'	,header_ForeColor: 'Black'	,row_BackColor: '#f8bc03'	,alternatingRow_BackColor: '#f8e094'	,sub_Head_Color: '#f8bc03'	,back_Color: '#ffffff'	,left_Color: '#f8e094'}
	        , {color_Scheme: 'Purple'	,head_Color: '#C7C7C7'	,header_ForeColor: 'Black'	,row_BackColor: '#be9cc5'	,alternatingRow_BackColor: '#91619b'	,sub_Head_Color: '#be9cc5'	,back_Color: '#ffffff'	,left_Color: '#eeeeee'}
	        , {color_Scheme: 'Red'	    ,head_Color: '#BA706A'	,header_ForeColor: 'White'	,row_BackColor: '#DE8587'	,alternatingRow_BackColor: '#A72A49'	,sub_Head_Color: '#df867f'	,back_Color: '#ffffff'	,left_Color: '#eeeeee'}
        ];
        m_ViewModel.colorSchemes = m_ColorSchemes;
        m_ViewModel.disableSave = true;
        m_ViewModel.dropFavoriteData = [];
        m_ViewModel.selectedFavorite = {};
        m_ViewModel.selectedColorScheme = {};

        function initCtrl() {
            // Request #1
            acctSvc.getPreferences(true).then(function (response) {
                // Response Handler #1
                // msgSvc.brodcastClientMsg(response);
                m_ViewModel.clientChoices = response;
                m_ViewModel.selectedColorScheme = m_ViewModel.clientChoices.ColorScheme;
                // Request #2
                return acctSvc.getSelectableActions(m_ViewModel.clientChoices.Account);
            }).then(function (response) {
                // Response Handler #2
                m_ViewModel.dropFavoriteData = response;
                for (var i = 0; i < m_ViewModel.dropFavoriteData.length; i++) {
                    var item = m_ViewModel.dropFavoriteData[i];
                    if (item.URL == m_ViewModel.clientChoices.Action && item.Title != 'Favorite') {
                        m_ViewModel.selectedFavorite = m_ViewModel.dropFavoriteData[i].URL;
                        break;
                    }
                }
                m_ViewModel.disableSave = false;
                // This is the last in the chain - Place all of the data elements on to scope at once
                $scope.vm = m_ViewModel; // Objects to be used by HTML
            });
        }

        // Functions that are avalible to the HTML
        $scope.onRecordsChanged = function (element) {
            console.log(element);
            //if (common.checkSpecialKeys()) {
            //    return;
            //}
        }

        $scope.save = function () {
            msgSvc.brodcastClientMsg('Saving');
            var mSelectedColorScheme = m_ColorSchemes.filter(function (item) {
                return item.color_Scheme === m_ViewModel.selectedColorScheme;
            })[0];
            m_ViewModel.clientChoices.AlternatingRowBackColor = mSelectedColorScheme.alternatingRow_BackColor;
            m_ViewModel.clientChoices.BackColor = mSelectedColorScheme.back_Color;
            m_ViewModel.clientChoices.ColorScheme = mSelectedColorScheme.color_Scheme;
            m_ViewModel.clientChoices.HeadColor = mSelectedColorScheme.head_Color;
            m_ViewModel.clientChoices.HeaderForeColor = mSelectedColorScheme.header_ForeColor;
            m_ViewModel.clientChoices.LeftColor = mSelectedColorScheme.left_Color;
            m_ViewModel.clientChoices.RowBackColor = mSelectedColorScheme.row_BackColor;
            m_ViewModel.clientChoices.SubheadColor = mSelectedColorScheme.sub_Head_Color;

            m_ViewModel.clientChoices.Action = m_ViewModel.selectedFavorite;

            acctSvc.saveClientChoices(m_ViewModel.clientChoices).then(
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
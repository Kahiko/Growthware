(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'LogonController';
    var mDependencyInjection = ['AccountService', 'MessageService', '$location'];
    var mRetCtrl = function (acctSvc, msgSvc, $location) {
        // init
        var thisCtrlr = this;

        thisCtrlr.showHidePassword = function () {
            if (thisCtrlr.passwordType == 'password') {
                thisCtrlr.passwordType = 'text';
            } else {
                thisCtrlr.passwordType = 'password';
            }
        }

        // Set the default value of inputType
        thisCtrlr.passwordType = 'password';

        thisCtrlr.logonInfo = { Account: 'Developer', Password: 'none' }

        thisCtrlr.logon = function () {
            //GW.Common.debug($scope.logonInfo);
            acctSvc.logon(thisCtrlr.logonInfo, logonCallBack);
        };

        function logonCallBack(response) {
            //GW.Common.debug(response);
            var currentAction = window.location.hash.substring(1);
            currentAction = currentAction.replace("?Action=", "");
            var $mClientMessage = $("#clientMessage.ClientID");
            var $mIncorrectLogon = $("#incorrectLogon.ClientID");
            var $mBtnRequestChange = $('#btnRequestChange');
            if (response == "true") {
                acctSvc.getPreferences()
                    .then(function (clientChoices) {
                        $location.path(clientChoices.Action);
                        msgSvc.brodcastClientMsg("Successfully logged on.");
                    }, function (result) {
                        msgSvc.brodcastClientMsg("Successfully logged on but can not automatically navigate to your 'Favorite' page.");
                        console.log("Failed to get getPreferences, result is " + result);
                    });
            } else {
                if (response == "Request") {
                    $mBtnRequestChange.css({ display: 'inline' });
                    $mBtnRequestChange.css('visibility', 'visible');
                } else {
                    var mRetHTML = response;
                    $mClientMessage.html(mRetHTML.toString()).fadeIn(3000);
                    $mIncorrectLogon.fadeIn(3000);
                }
            }
        }

        return thisCtrlr;
    };

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();

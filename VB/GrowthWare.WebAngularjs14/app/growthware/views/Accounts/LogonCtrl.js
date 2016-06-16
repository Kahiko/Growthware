(function () {
    'use strict';

    angular.module('growthwareApp').controller('LogonController', ['AccountService', '$controller', '$scope', '$location', function (acctSvc, $controller, $scope, $location) {
        // init
        var thisCtrlr = this;

        $scope.showHidePassword = function () {
            if ($scope.passwordType == 'password') {
                $scope.passwordType = 'text';
            } else {
                $scope.passwordType = 'password';
            }
        }

        // Set the default value of inputType
        $scope.passwordType = 'password';

        $scope.logonInfo = { Account: 'Developer', Password: 'none' }

        $scope.logon = function () {
            //GW.Common.debug($scope.logonInfo);
            acctSvc.logon($scope.logonInfo, logonCallBack);
        };

        function logonCallBack(response) {
            //GW.Common.debug(response);
            var currentAction = window.location.hash.substring(1);
            currentAction = currentAction.replace("?Action=", "");
            var $mClientMessage = $("#clientMessage.ClientID");
            var $mIncorrectLogon = $("#incorrectLogon.ClientID");
            var $mBtnRequestChange = $('#btnRequestChange');
            var $mLogonPage = $('#LogonPage');
            if (response == "true") {
                //var newHash = '';
                //if (currentAction.length == 0) {
                //    newHash = "?Action=Favorite";
                //} else {
                //    if (currentAction.toLowerCase() != "logon") {
                //        newHash = "?Action=" + currentAction;
                //    } else {
                //        newHash = "?Action=Favorite";
                //    }
                //}

                //if (navigator.userAgent.indexOf('Chrome/') != -1) {
                //    top.history.replaceState("", "", "#" + newHash);
                //    location.reload();
                //    return;
                //};
                //window.location.hash = newHash;

                //location.reload();


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
    }]);

})();

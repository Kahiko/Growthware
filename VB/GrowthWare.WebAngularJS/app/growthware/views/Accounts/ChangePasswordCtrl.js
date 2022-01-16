(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'ChangePasswordController';
    var mDependencyInjection = ['AccountService', 'MessageService', '$controller', '$scope'];

    var mRetCtrl = function (acctSvc, messageSvc, $controller, $scope) {
        var thisCtrlr = this;
        var viewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().

        viewModel.IsPageBusy = true;

        function initCtrl() {
            var passwordProfile = {};
            passwordProfile.OldPassword = "";
            passwordProfile.NewPassword = "";

            viewModel.passwordProfile = passwordProfile;
            viewModel.confirmPassword = "";

            messageSvc.getClientMessage().then(function (clientMSG) {
                // work with the response
                if (clientMSG != null) {
                    viewModel.clientMessage = clientMSG.Message;
                }
                // make the next call in the chain
                return acctSvc.getCurrentAccount();
            }).then(function (accountProfile) {
                // work with the response
                viewModel.accountProfile = accountProfile;
                if (viewModel.accountProfile.Status == 4) {
                    viewModel.passwordProfile.OldPassword = 'forced change';
                    angular.element('#NewPassword').focus();
                } else {
                    angular.element('#OldPassword').focus();
                }
            });
            viewModel.IsPageBusy = false;
        }

        $scope.vm = viewModel;

        $scope.changePassword = function () {
            viewModel.IsPageBusy = true;
            if (viewModel.confirmPassword == viewModel.passwordProfile.NewPassword) {
                if (viewModel.passwordProfile.OldPassword == 'forced change') {
                    viewModel.passwordProfile.OldPassword = viewModel.confirmPassword;
                }
                console.debug(viewModel.passwordProfile);
                acctSvc.changePassword(viewModel.passwordProfile).then(
                    /*** success ***/
                    function (result) {
                        viewModel.clientMessage = result;
                    },
                    /*** error ***/
                    function (result) {
                        viewModel.clientMessage = "Failed to change  password";
                        console.log(result);
                    }
                );
            } else {
                viewModel.clientMessage = "New and Confirm passwords must match!";
            }
            viewModel.IsPageBusy = false;
        }

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();
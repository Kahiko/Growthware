(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'SecurityEntityController';
    var mDependencyInjection = ['AccountService', 'SecurityEntityService'];
    var securityEntityCtrl = function (acctSvc, seSvc) {
        var thisCtrl = this;

        initCtrl();

        function initCtrl() {
            thisCtrl.selectedSE = {};
            thisCtrl.validSecurityEntities = [];
            getValid();
        }

        function getValid() {
            console.log('Start getValid');
            acctSvc.getPreferences().then(
                /* success */
                function (clientChoices) {
                    thisCtrl.clientChoices = clientChoices;
                    thisCtrl.selectedSE = { "SE_SEQ_ID": clientChoices.SecurityEntityID, "Name": clientChoices.SecurityEntityName };
                    seSvc.getValidSecurityEntities().then(
                        /*** Success ***/
                        function (response) {
                            thisCtrl.validSecurityEntities = response;
                        },
                        /*** Error ***/
                        function (result) {
                            console.log("Failed to get getValidSecurityEntities, result is " + result);
                        });
                },
                /* error */
                function (result) {
                    console.log("Failed to get getPreferences, result is " + result);
                });
            console.log('End getValid');
        };

        thisCtrl.save = function () {
            //alert(thisCtrl.selectedSE.SE_SEQ_ID);
            seSvc.selectSecurityEntity(thisCtrl.selectedSE.SE_SEQ_ID).then(
                /*** Success ***/
                function (result) {
                    getValid();
                    GW.Navigation.NavigationController.Reload();
                },
                /*** error ***/
                function (result) {
                    console.log("Failed to selectSecurityEntity, result is " + result);
                }
            );
        };

        return thisCtrl;
    }; 

    securityEntityCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, securityEntityCtrl);
})();
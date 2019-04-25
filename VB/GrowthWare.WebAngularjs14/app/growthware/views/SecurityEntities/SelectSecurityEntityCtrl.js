(function () {
    'use strict';
    var securityEntityCtrl = function (acctSvc, seSvc, $scope) {
        var thisCtrl = this;

        initCtrl();

        function initCtrl() {
            $scope.selectedSE = {};
            $scope.validSecurityEntities = [];
            getValid();
        }

        function getValid() {
            console.log('Start getValid');
            acctSvc.getPreferences().then(
                /* success */
                function (clientChoices) {
                    $scope.clientChoices = clientChoices;
                    $scope.selectedSE = { "SE_SEQ_ID": clientChoices.SecurityEntityID, "Name": clientChoices.SecurityEntityName };
                    seSvc.getValidSecurityEntities().then(
                        /*** Success ***/
                        function (response) {
                            $scope.validSecurityEntities = response;
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

        $scope.save = function () {
            //alert($scope.selectedSE.SE_SEQ_ID);
            seSvc.selectSecurityEntity($scope.selectedSE.SE_SEQ_ID).then(
                /*** Success ***/
                function (result) {
                    getValid();
                },
                /*** error ***/
                function (result) {
                    console.log("Failed to selectSecurityEntity, result is " + result);
                }
            );
        };

        return thisCtrl;
    }; 

    securityEntityCtrl.$inject = ['AccountService', 'SecurityEntityService', '$scope'];

    angular.module('growthwareApp').controller('SecurityEntityController', securityEntityCtrl);
})();
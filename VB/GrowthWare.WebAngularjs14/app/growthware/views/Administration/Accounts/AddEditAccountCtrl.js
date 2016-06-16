(function () {
    'use strict';

    var addEditAccountCtrl = function (acctSvc, $scope) {
        // init
        var thisCrtl = this;

        initCtrl();

        function initCtrl() {
            alert('hi');
        };

        thisCrtl.myFun = function () {

        };

        return thisCrtl;
    };

    addEditAccountCtrl.$inject = ['AccountService', '$scope'];

    angular.module('growthwareApp').controller('AddEditAccountController', addEditAccountCtrl);

})();
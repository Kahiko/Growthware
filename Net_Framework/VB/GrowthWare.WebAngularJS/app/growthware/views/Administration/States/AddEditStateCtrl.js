(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'AddEditStateController';
    var mDependencyInjection = ['StatesService', 'MessageService', '$uibModalInstance', '$scope', '$route', 'modalData'];

    var mRetCtrl = function (statesSvc, msgSvc, $uibModalInstance, $scope, $route, modalData) {
        var thisCtrlr = this;
        var m_Route = $route.current.$$route.originalPath;
        var m_Action = m_Route.substr(1, m_Route.length - 1);
        var m_ViewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().
        m_ViewModel.profile = {};
        m_ViewModel.selectedStatus_SeqID = -1;
        m_ViewModel.avalibleStatuses = [
            { value: 1, text: "Active" },
            { value: 2, text: "Inactive" }
        ]

        // msgSvc.brodcastClientMsg(response.Message); // Example

        function initCtrl() {
            statesSvc.getProfile(modalData.data.editId).then(function (response) {
                // work with the response
                m_ViewModel.profile = response;
                m_ViewModel.selectedStatus_SeqID = JSON.stringify(m_ViewModel.profile.Status_SeqID);
                // make the next call in the chain
                // return yourNamedSvc.secondMethodWithPromise();
            }).then(function (response) {
                // work with the response

                // Objects to be used by HTML
                $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
            });
        }

        // Function called by other functions such as initCtrl
        function populateFromPage() {
            m_ViewModel.profile.Status_SeqID = m_ViewModel.selectedStatus_SeqID * 1;
        }

        // Functions that are avalible to the HTML

        $scope.cancelEdit = function () {
            $uibModalInstance.dismiss('AddEditStateController cancel'); // use for popup edit
        };

        $scope.save = function () {
            populateFromPage();
            statesSvc.save(m_ViewModel.profile, m_Action).then(
                /*** success ***/
                function (result) {
                    console.log(result);
                    if (result == true) {
                        msgSvc.brodcastClientMsg('State information has been saved');
                        $scope.cancelEdit();
                        $route.reload();
                    } else {
                        m_ViewModel.clientMessage = 'State information was not saved!';
                    }
                },
                /*** error ***/
                function (result) {
                    m_ViewModel.clientMessage = 'State information was not saved!';
                    console.log(result);
                }
            );
        };

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();
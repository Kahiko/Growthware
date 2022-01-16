(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'LineCountController';
    var mDependencyInjection = ['FileService', '$scope', '$route'];

    var mRetCtrl = function (fileSvc, $scope, $route) {
        var thisCtrlr = this;
        var m_Route = $route.current.$$route.originalPath;
        var m_Action = m_Route.substr(1, m_Route.length - 1);
        var m_ViewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().
        // set up default values
        m_ViewModel.countInfo = {
            theDirectory: '',
            excludePattern: '',
            includeFiles: '*.vb, *.aspx, *.ascx, *.asax, *.config, *.js'
        };
        m_ViewModel.lineCount = '';

        function initCtrl() {
            var mExcludePattern = [];
            mExcludePattern.push('ASSEMBLYINFO')
            mExcludePattern.push('.DESIGNER')
            mExcludePattern.push('JQUERY-')
            mExcludePattern.push('JQUERY.VALIDATE')
            mExcludePattern.push('MODERNIZR-')
            mExcludePattern.push('jquery.tmpl.')
            mExcludePattern.push('jquery.unobtrusive')
            mExcludePattern.push('angular')
            m_ViewModel.countInfo.excludePattern = mExcludePattern.join(", ");
            // Objects to be used by HTML
            $scope.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
        }

        // Functions that are avalible to the HTML
        $scope.getCount = function () {
            // do nothing atm
            fileSvc.getLineCount(m_ViewModel.countInfo, m_Action).then(function (response) {
                m_ViewModel.lineCount = response;
            });
        }

        // Uncomment the next line if you have not set this when calling services in initCtrl
        // $scope.vm = viewModel;

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();
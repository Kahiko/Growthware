(function () {
    'use strict';
    var mApplication = 'growthwareApp';
    var mControllerName = 'EncryptDecryptController';
    var mDependencyInjection = ['ConfigurationService', 'MessageService', '$timeout'];

    var mRetCtrl = function (configSvc, msgSvc, $timeout) {
        var thisCtrlr = this;
        var m_ViewModel = {}; // Initialize the object, before adding data to it.  { } is declarative shorthand for new Object().
        m_ViewModel.textValue = '';
        m_ViewModel.processedText = '';

        // msgSvc.brodcastClientMsg('the controller is not finished');

        function initCtrl() {
            thisCtrlr.vm = m_ViewModel; // Place all of the data elements on to scope at once after it is complete
        }

        // Functions that are avalible to the HTML
        thisCtrlr.doProcessTextValue = function (encrypt) {
            configSvc.encrypt(thisCtrlr.textValue, encrypt).then(function (response) {
                thisCtrlr.processedText = response;
            }).catch(function () {
                msgSvc.brodcastClientMsg('An error occured attempting to encrypt/decrypt the text!');
            });
        }

        thisCtrlr.doHighlightAll = function (theField) {
            var mTempval = $('#' + theField);
            mTempval.focus()
            mTempval.select()
            try {
                if (document.all) {
                    therange = mTempval.createTextRange();
                    therange.execCommand("Copy");
                    msgSvc.brodcastClientMsg("Contents highlighted and copied to clipboard!");
                    $timeout(function () {
                        msgSvc.brodcastClientMsg('');
                    }, 3000);
                }
            } catch (e) {
                // do nothing
            }
        }

        initCtrl();

        return thisCtrlr;
    }

    mRetCtrl.$inject = mDependencyInjection;

    angular.module(mApplication).controller(mControllerName, mRetCtrl);

})();
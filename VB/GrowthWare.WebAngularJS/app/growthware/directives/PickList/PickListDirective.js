(function () {
    'use strict';

    var app = angular.module('growthwareApp');
    angular.module('growthwareApp').directive('gwPicklist', ['$http', function ($http) {

        return {
            restrict: 'E',
            scope: {
                availableItems: '=',
                selectedItems: '=',
                allItemsText: '@',
                header: '@',
                id: '@',
                picklistTableHelp: '@',
                rows: '@',
                selectedItemsText: '@',
                size: '@',
                sortOnChange: '@'
            },
            link: function ($scope, $element, $attrs) {
                $scope.text = $attrs.text;
                $scope.allItemsText = $attrs.allItemsText;
                $scope.header = $attrs.header;
                $scope.id = $attrs.id;
                $scope.picklistTableHelp = $attrs.picklistTableHelp;
                $scope.rows = $attrs.rows;
                $scope.selectedItemsText = $attrs.selectedItemsText;
                $scope.size = $attrs.size;
                $scope.sortOnChange = $attrs.sortOnChange;
                $scope.sortOnIndex = $attrs.sortOnIndex || 0;

                /************************* Reorder option elements of an HTML select */
                $scope.moveUp = function (listBox) {
                    var objListBox = document.getElementById($scope.id + listBox);
                    var selectOptions = objListBox.getElementsByTagName('option');
                    var i = 0;
                    for (i = 1; i < selectOptions.length; i++) {
                        var opt = selectOptions[i];
                        if (opt.selected) {
                            objListBox.removeChild(opt);
                            objListBox.insertBefore(opt, selectOptions[i - 1]);
                        }
                    }
                    var newSelectedItems = [];
                    for (i = 0; i < objListBox.length; i++) {
                        newSelectedItems.push(objListBox.options[i].text);
                    }
                    $scope.selectedItems = newSelectedItems;
                };

                $scope.moveDown = function (listBox) {
                    var objListBox = document.getElementById($scope.id + listBox);
                    var selectOptions = objListBox.getElementsByTagName('option');
                    for (var i = selectOptions.length - 2; i >= 0; i--) {
                        var opt = selectOptions[i];
                        if (opt.selected) {
                            var nextOpt = selectOptions[i + 1];
                            opt = objListBox.removeChild(opt);
                            nextOpt = objListBox.replaceChild(opt, nextOpt);
                            objListBox.insertBefore(nextOpt, opt);
                        }
                    }
                    var newSelectedItems = [];
                    for (i = 0; i < objListBox.length; i++) {
                        newSelectedItems.push(objListBox.options[i].text);
                    }
                    $scope.selectedItems = newSelectedItems;
                }
                /************************* End Reorder option elements of an HTML select */

                $scope.showHelpMSG = function () {
                    var message = $scope.picklistTableHelp || "";
                    if (message.length > 0) {
                        alert(message);
                    }
                };

                $scope.switchAll = function (source, destination) {
                    /*
                    switchAll was created to move all of the options from one list box to another.
                    Parameters:
                    objFromBox - The from list box as an object
                    objToBox - The to list box as an object
                    */
                    selectAllInListBox(source);
                    $scope.switchList(source, destination);
                };

                $scope.switchList = function (source, destination) {
                    /*
                    SwitchList was created to move data from one list box to another
                    keeping the sort order based on the text of the options.
                    Parameters:
                    objFromBox - The from listbox as an object
                    objToBox - The to listbox as an object
                    */
                    var objFromBox = document.getElementById($scope.id + source);
                    var objToBox = document.getElementById($scope.id + destination);
                    if ($scope.sortOnChange == undefined) $scope.sortOnChange = true;
                    var arrFbox = new Array();
                    var arrTbox = new Array();
                    var arrLookup = new Array();
                    var i;
                    var selectedCount = 0;
                    for (i = 0; i < objFromBox.length; i++) {
                        if (objFromBox.options[i].selected) selectedCount++;
                    }
                    if (selectedCount > 0) {
                        for (i = 0; i < objToBox.options.length; i++) {
                            arrLookup[objToBox.options[i].text] = objToBox.options[i].value;
                            arrTbox[i] = objToBox.options[i].text;
                        }
                        var fLength = 0;
                        var tLength = arrTbox.length;
                        for (i = 0; i < objFromBox.options.length; i++) {
                            arrLookup[objFromBox.options[i].text] = objFromBox.options[i].value;
                            if (objFromBox.options[i].selected && objFromBox.options[i].value != "") {
                                arrTbox[tLength] = objFromBox.options[i].text;
                                tLength++;
                            } else {
                                arrFbox[fLength] = objFromBox.options[i].text;
                                fLength++;
                            }
                        }
                        if ($scope.sortOnChange.toLowerCase() == "true") {
                            arrFbox.sort(GW.Common.naturalSort($scope.sortOnIndex));
                            arrTbox.sort(GW.Common.naturalSort($scope.sortOnIndex));
                        }
                        objFromBox.length = 0;
                        objToBox.length = 0;
                        for (i = 0; i < arrFbox.length; i++) {
                            var no = new Option();
                            no.value = arrLookup[arrFbox[i]];
                            no.text = arrFbox[i];
                            objFromBox[i] = no;
                        }
                        for (i = 0; i < arrTbox.length; i++) {
                            var no = new Option();
                            no.value = arrLookup[arrTbox[i]];
                            no.text = arrTbox[i];
                            objToBox[i] = no;
                        }
                        var objDstList = document.getElementById($scope.id + '_DstList');
                        var newSelectedItems = [];
                        for (i = 0; i < objDstList.length; i++) {
                            newSelectedItems.push(objDstList.options[i].text);
                        }
                        for (i = objDstList.options.length - 1 ; i >= 0 ; i--) {
                            objDstList.remove(i);
                        };
                        $scope.selectedItems = newSelectedItems;

                    }
                };

                function selectAllInListBox(listBox) {
                    /*
                    selectAllInListBox was created to select all of the options in a listbox.
                    Need for when you want to move all of the data from one list box to the other.
                    Parameters:
                    objListBox - The list box as an object to be selected
                    */
                    var objListBox = document.getElementById($scope.id + listBox);
                    var lengthOfListBox = objListBox.length;
                    for (i = 0; i < lengthOfListBox; i++) {
                        try {
                            objListBox.options[i].selected = true;
                        } catch (e) {
                            //alert(e); // just here for testing wouldn't suggest you have it in your code
                            return;
                        }
                    }
                };

 
                angular.element(document).ready(function () {
                    // All avalible items will include the selected ones as well
                    // so wee need to remove the selected items from the all itmes.
                    setTimeout(function () {
                        var objSrcList = document.getElementById($scope.id + '_SrcList');
                        var objDstList = document.getElementById($scope.id + '_DstList');
                        for (var i = objDstList.length -1; i >= 0; i--) {
                            for (var x = objSrcList.length -1; x >= 0; x--) {
                                if (objDstList.options[i].text == objSrcList.options[x].text) {
                                    objSrcList.remove(x);
                                    break;
                                }
                            }
                        }
                    }, 500);
                });
            },
            templateUrl: GW.Common.getBaseURL() + '/app/growthware/directives/PickList/PickListTemplate.html'
        };
    }]);
})();
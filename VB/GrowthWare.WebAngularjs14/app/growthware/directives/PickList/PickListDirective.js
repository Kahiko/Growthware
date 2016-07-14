(function () {
    'use strict';

    var app = angular.module('growthwareApp');
    angular.module('growthwareApp').directive('gwPicklist', ['$http', function ($http) {

        return {
            restrict: 'E',
            scope: {
                avalibleItems: '=',
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

                /************************* Reorder option elements of an HTML select */
                $scope.moveUp = function (listBox) {
                    var objListBox = document.getElementById($scope.id + listBox);
                    var selectOptions = objListBox.getElementsByTagName('option');
                    for (var i = 1; i < selectOptions.length; i++) {
                        var opt = selectOptions[i];
                        if (opt.selected) {
                            objListBox.removeChild(opt);
                            objListBox.insertBefore(opt, selectOptions[i - 1]);
                        }
                    }
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
                            arrFbox.sort(naturalSort);
                            arrTbox.sort(naturalSort);
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

                /********** Natural Sorting *****************/
                /*
                * Natural Sort algorithm for Javascript - Version 0.6 - Released under MIT license
                * Author: Jim Palmer (based on chunking idea from Dave Koelle)
                * Contributors: Mike Grier (mgrier.com), Clint Priest, Kyle Adams, guillermo
                */
                function naturalSort(a, b) {
                    var re = /(^-?[0-9]+(\.?[0-9]*)[df]?e?[0-9]?$|^0x[0-9a-f]+$|[0-9]+)/gi,
                        sre = /(^[ ]*|[ ]*$)/g,
                        dre = /(^([\w ]+,?[\w ]+)?[\w ]+,?[\w ]+\d+:\d+(:\d+)?[\w ]?|^\d{1,4}[\/\-]\d{1,4}[\/\-]\d{1,4}|^\w+, \w+ \d+, \d{4})/,
                        hre = /^0x[0-9a-f]+$/i,
                        ore = /^0/,
                    // convert all to strings and trim()
                        x = a.toString().replace(sre, '') || '',
                        y = b.toString().replace(sre, '') || '',
                    // chunk/tokenize
                        xN = x.replace(re, '\0$1\0').replace(/\0$/, '').replace(/^\0/, '').split('\0'),
                        yN = y.replace(re, '\0$1\0').replace(/\0$/, '').replace(/^\0/, '').split('\0'),
                    // numeric, hex or date detection
                        xD = parseInt(x.match(hre)) || (xN.length != 1 && x.match(dre) && Date.parse(x)),
                        yD = parseInt(y.match(hre)) || xD && y.match(dre) && Date.parse(y) || null;
                    // first try and sort Hex codes or Dates
                    if (yD)
                        if (xD < yD) return -1;
                        else if (xD > yD) return 1;
                    // natural sorting through split numeric strings and default strings
                    for (var cLoc = 0, numS = Math.max(xN.length, yN.length) ; cLoc < numS; cLoc++) {
                        // find floats not starting with '0', string or 0 if not defined (Clint Priest)
                        var oFxNcL = !(xN[cLoc] || '').match(ore) && parseFloat(xN[cLoc]) || xN[cLoc] || 0;
                        var oFyNcL = !(yN[cLoc] || '').match(ore) && parseFloat(yN[cLoc]) || yN[cLoc] || 0;
                        // handle numeric vs string comparison - number < string - (Kyle Adams)
                        if (isNaN(oFxNcL) !== isNaN(oFyNcL)) return (isNaN(oFxNcL)) ? 1 : -1;
                            // rely on string comparison if different types - i.e. '02' < 2 != '02' < '2'
                        else if (typeof oFxNcL !== typeof oFyNcL) {
                            oFxNcL += '';
                            oFyNcL += '';
                        }
                        if (oFxNcL < oFyNcL) return -1;
                        if (oFxNcL > oFyNcL) return 1;
                    }
                    return 0;
                }
                /********** End Natural Sorting *****************/

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
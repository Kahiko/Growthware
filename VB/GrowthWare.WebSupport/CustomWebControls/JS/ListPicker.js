/************************* Moving selectetions from one box to the next ***/
function switchList(objFromBox, objToBox, sortOnChange) {
    /*
    SwitchList was created to move data from one list box to another
    keeping the sort order based on the text of the options.
    Parameters:
    objFromBox - The from listbox as an object
    objToBox - The to listbox as an object
    */
    if (sortOnChange == undefined) sortOnChange = true;
    var arrFbox = new Array();
    var arrTbox = new Array();
    var arrLookup = new Array();
    var i;
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
    if (sortOnChange.toLowerCase() == "true") {
        arrFbox.sort();
        arrTbox.sort();
    }
    objFromBox.length = 0;
    objToBox.length = 0;
    var c;
    for (c = 0; c < arrFbox.length; c++) {
        var no = new Option();
        no.value = arrLookup[arrFbox[c]];
        no.text = arrFbox[c];
        objFromBox[c] = no;
    }
    for (c = 0; c < arrTbox.length; c++) {
        var no = new Option();
        no.value = arrLookup[arrTbox[c]];
        no.text = arrTbox[c];
        objToBox[c] = no;
    }
    var listID = objFromBox.id.substr(0, objFromBox.id.length - 7);
    var objSrcList = document.getElementById(listID + 'SrcList');
    var objDstList = document.getElementById(listID + 'DstList');
    var selectedState = '';
    var selectedStateField = document.getElementById(listID + 'SelectedState');
    var allStateField = document.getElementById(listID + 'AllState');
    var allState = '';

    for (i = 0; i < objSrcList.length; i++) {
        allState += objSrcList.options[i].text + ',';
    }
    allStateField.value = allState.substr(0, allState.length - 1);

    for (i = 0; i < objDstList.length; i++) {
        selectedState += objDstList.options[i].text + ',';
    }
    selectedStateField.value = selectedState.substr(0, selectedState.length - 1);


}

function selectAllInListBox(objListBox, sortOnChange) {
    /*
    selectAllInListBox was created to select all of the options in a listbox.
    Need for when you want to move all of the data from one list box to the other.
    Parameters:
    objListBox - The list box as an object to be selected
    */
    var lengthOfListBox = objListBox.length;
    var iCounter = 0;
    for (iCounter = 0; iCounter < lengthOfListBox; iCounter++) {
        try {
            objListBox.options[iCounter].selected = true;
        } catch (e) {
            //alert(e); // just here for testing wouldn't suggest you have it in your code
            return;
        }
    }
}

function switchAll(objFromBox, objToBox, sortOnChange) {
    /*
    switchAll was created to move all of the options from one list box to another.
    Parameters:
    objFromBox - The from list box as an object
    objToBox - The to list box as an object
    */
    selectAllInListBox(objFromBox, sortOnChange);
    switchList(objFromBox, objToBox, sortOnChange);
}
/************************* End Moving selectetions from one box to the next */

/************************* Reorder option elements of an HTML select */
function moveUp(objListBox) {
    var selectOptions = objListBox.getElementsByTagName('option');
    for (var i = 1; i < selectOptions.length; i++) {
        var opt = selectOptions[i];
        if (opt.selected) {
            objListBox.removeChild(opt);
            objListBox.insertBefore(opt, selectOptions[i - 1]);
        }
    }
}

function moveDown(objListBox) {
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

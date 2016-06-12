// Text is stored in the textarea for the Html Designer
function htb_OnHtmlChanged(htmlDesignerElement, hiddenElement) {
    hiddenElement.value = htmlDesignerElement.html;
}

// Associate the Html Designers with the corresponding hidden input
function htb_InitializeElements() {
    for (i=0;i<htmlDesignerList.length; i++) {
        var htmlDesignerElementID = htmlDesignerList[i] + "HtmlDesigner";
        var hiddenElementID = htmlDesignerList[i];

        document.all[htmlDesignerElementID].html = document.all[hiddenElementID].value;
    }
}
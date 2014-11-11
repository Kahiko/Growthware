// Namespaces
if (typeof GW == "undefined" || !GW) {
	window.GW = {
		name: 'Growthware Core Web',
		version: '1.0.0.0'
	};
}

if (typeof GW.Model == "undefined" || !GW.Model) {
	GW.Model = {
		name: 'Growthware Core Web Model objects javascript',
		version: '1.0.0.0'
	};
}

GW.Model = {
	DefaultWebMethodOptions: function () {
		var defaultOptions = {
			type: 'POST',
			async: true,
			cache: false,
			timeout: 8000,
			url: '',
			data: {},
			dataType: 'html',
			contentType: 'application/html; charset=utf-8',
			abortable: false
		};
		return defaultOptions;
	},

	DefaultDialogOptions: function () {
		var defaultOptions = {
			autoOpen: true,
			buttons: {},
			closeOnEscape: true,
			closeText: 'close',
			dialogClass: '',
			disabled: false,
			draggable: true,
			height: 'auto',
			hide: null,
			maxHeight: false,
			maxWidth: false,
			minHeight: 150,
			minWidth: 150,
			modal: false,
			position: { my: "center", at: "center", of: window },
			resizable: true,
			show: null,
			stack: true,
			title: '',
			width: 300,
			zindex: 1000
		};
		return defaultOptions;
	},

	ClientChoices: function () {
		var defaultOptions = {
			AccountName: '',
			Action: '',
			BackColor: '',
			ColorScheme: '',
			HeadColor: '',
			LeftColor: '',
			RecordsPerPage: '',
			SecurityEntityID: '',
			SecurityEntityName: '',
			SubheadColor: '',
			Version: '',
			Environment: ''
		};
		return defaultOptions;
	},

	WebConfig: function () {
		var mRetVal = {
			AlwaysLeftNav: '',
			AppDisplayedName: '',
			AppendToFile: '',
			BasePage: '',
			Central_Management: '',
			ConversionPattern: '',
			DBStatus: '',
			DefaultAction: '',
			DefaultAuthenticatedAction: '',
			Encryption_Type: '',
			EnvironmentDisplayed: '',
			WorkingEnvironment: '',
			Environments: '',
			ExpectedUpBy: '',
			LDAP_Domain: '',
			LDAP_Server: '',
			NewEnvironment: '',
			SecurityEntityTranslation: '',
			ServerSideViewState: '',
			ServerSideViewStatePages: '',
			UnderConstruction: '',

			App_Name: '',
			Assembly_Name: '',
			Authentication_Type: '',
			Auto_Create: '',
			Auto_Create_ClientChoicesAccount: '',
			Auto_Create_SecurityEntity: '',
			Auto_Create_Roles: '',


			Connectionstring: '',
			DAL: '',
			Default_Security_Entity_ID: '',
			Enable_Cache: '',
			Enable_Encryption: '',
			Failed_Attempts: '',
			Force_HTTPS: '',
			Log_Path: '',
			Log_Priority: '',
			Log_Retention: '',
			Name_Space: '',
			Registering_Roles: '',
			Registration_Post_Action: '',
			Skin_Type: '',
			SMTP_Account: '',
			SMTP_From: '',
			SMTP_Password: '',
			SMTP_Server: '',
			Synchronize_Password: ''
		};
		return mRetVal;
	},

	BoostrapModal: function () {
	    var mModal = 
'<!-- Modal -->' +
'<div class="modal modal-wide fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
'    <div class="modal-dialog">' +
'        <div class="modal-content">' +
'            <div class="modal-header">' +
'                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
'                 <h4 class="modal-title" id="myModalTitle">Modal title</h4>' +
'            </div>' +
'            <div class="modal-body"></div>' +
'            <div class="modal-footer">' +
'                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>' +
'                <button type="button" id="mModalBtnSave" class="btn btn-primary">Save changes</button>' +
'            </div>' +
'        </div>' +
'        <!-- /.modal-content -->' +
'    </div>' +
'    <!-- /.modal-dialog -->' +
'</div>' +
'<!-- /.modal -->'
	    return mModal;
	}
}
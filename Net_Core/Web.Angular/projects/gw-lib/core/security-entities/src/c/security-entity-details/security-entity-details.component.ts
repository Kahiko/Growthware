
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
// Library
import { BaseDetailComponent, IBaseDetailComponent } from '@growthware/core/base/components';
import { ConfigurationService } from '@growthware/core/configuration';
import { DataService } from '@growthware/common/services';
import { GWCommon } from '@growthware/common/services';
import { LogLevel, LoggingService } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
import { IKeyValuePair, INameValuePair } from '@growthware/common/interfaces';
import { SecurityService } from '@growthware/core/security';
// Feature
import { SecurityEntityService } from '../../security-entity.service';
import { ISecurityEntityProfile, SecurityEntityProfile } from '../../security-entity-profile.model';
import { IRegistrationInformation, registrationInformation } from '../../registration-information.model';
import { IValidSecurityEntities } from '../../valid-security-entities.model';

@Component({
	selector: 'gw-core-security-entity-details',
	standalone: true,
	imports: [
		FormsModule,
		ReactiveFormsModule,
		MatButtonModule,
		MatIconModule,
		MatInputModule,
		MatSelectModule,
		MatTabsModule,
		MatTooltipModule
	],
	templateUrl: './security-entity-details.component.html',
	styleUrls: ['./security-entity-details.component.scss']
})
export class SecurityEntityDetailsComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {

	private _Profile: ISecurityEntityProfile = new SecurityEntityProfile();
	private _RegistrationInformation: IRegistrationInformation = new registrationInformation();

	canEnterName: boolean = false;
	securityEntityTranslation: string = 'Security Entity';
	securityEntityName: string = '';

	selectedDal: string = '';
	selectedEncryptionType: number = -1;
	selectedSecurityEntity: number = 1;
	selectedSkin:string = '';
	selectedParent: number = -1;
	selectedStatusSeqId: number = 0;
	selectedStyle:string = '';

	validDataAccessLayers: INameValuePair[] = [
		{ name: 'SQLServer', value: 'SQL Server' },
		{ name: 'MySql', value: 'MySql' },
		{ name: 'Oracle', value: 'Oracle' },
	];

	validEncryptionTypes: IKeyValuePair[] = [
		{ key: 0, value: 'None' },
		{ key: 3, value: 'Aes' },
		{ key: 2, value: 'Des' },
		{ key: 1, value: 'TripleDes' },
	];

	validSkins: INameValuePair[] = [
		{ name: 'Arc', value: 'Arc' },
		{ name: 'Blue Arrow', value: 'Blue Arrow' },
		{ name: 'Dashboard', value: 'Dashboard' },
		{ name: 'Default', value: 'Default' },
		{ name: 'DevOps', value: 'DevOps' },
		{ name: 'Professional', value: 'Professional' },
	];

	validParents: IKeyValuePair[] = [];

	validSecurityEntities: IValidSecurityEntities[] = [];

	validStatuses: IKeyValuePair[] = [
		{key: 1, value: 'Active'},
		{key: 2, value: 'Inactive'}
	];

	constructor(
		private _FormBuilder: FormBuilder,
		private _ConfigurationSvc: ConfigurationService,
		dataSvc: DataService,
		private _GWCommon: GWCommon,
		loggingSvc: LoggingService,
		modalSvc: ModalService,
		profileSvc: SecurityEntityService,
		securitySvc: SecurityService
	) {
		super();
		this._DataSvc = dataSvc;
		this._LoggingSvc = loggingSvc;
		this._ModalSvc = modalSvc;
		this._ProfileSvc = profileSvc;
		this._SecuritySvc = securitySvc;
	}

	ngOnInit(): void {
		// console.log('SecurityEntityDetailsComponent.ngOnInit.modalReason', this._ProfileSvc.modalReason);
		// console.log('SecurityEntityDetailsComponent.ngOnInit.selectedRow', this._ProfileSvc.selectedRow);
		// console.log('SecurityEntityDetailsComponent.ngOnInit._Profile', this._Profile);
		this._Subscription.add(
			this._ConfigurationSvc.securityEntityTranslation$.subscribe((val: string) => { 
				this.securityEntityTranslation = val;
			})
		);
		let mEditId = -1;
		if(this._ProfileSvc.modalReason.toLocaleLowerCase() != 'newprofile') {
			mEditId = this._ProfileSvc.selectedRow.SecurityEntitySeqId;
		} else {
			this.canEnterName = true;
		}
		// console.log('SecurityEntityDetailsComponent.ngOnInit.mEditId', mEditId);
		this._SecuritySvc.getSecurityInfo('search_security_entities').then((securityInfo) => {  // #1 Request/Handler getSecurityInfo
			// console.log('SecurityEntityDetailsComponent.ngOnInit.securityInfo', securityInfo);
			this._SecurityInfo = securityInfo;
			return this._ProfileSvc.getValidParents(mEditId);                                   // #2 Request getValidParents
		}).catch((error) => {                                                              		// #1 Error Handler getSecurityInfo
			this._LoggingSvc.toast('Error getting security info:\r\n' + error, this.securityEntityTranslation + ' Details:', LogLevel.Error);
		}).then((parrents: IKeyValuePair[]) => {                                                // #2 Request getValidParents Handler
			// console.log('SecurityEntityDetailsComponent.ngOnInit.parrents', parrents);
			this.validParents = parrents;
			return this._ProfileSvc.getSecurityEntity(mEditId);                                 // #3 Request getSecurityEntity
		}).catch((error) => {                                                              		// #2 Error Handler
			this._LoggingSvc.toast('Error getting Security Entity:\r\n' + error, this.securityEntityTranslation + ' Details:', LogLevel.Error);
		}).then((profile: ISecurityEntityProfile) => {                                          // #3 Request getProfile Handler
			// console.log('SecurityEntityDetailsComponent.ngOnInit.profile', profile);
			this._Profile = profile;
			return this._ProfileSvc.getValidSecurityEntities();                      			// #4 Request getValidSecurityEntities
		}).catch((error) => {                                                              		// #3 Error Handler
			this._LoggingSvc.toast('Error getting valid Security Entities:\r\n' + error, this.securityEntityTranslation + ' Details:', LogLevel.Error);
		}).then((response: IValidSecurityEntities[]) => {										// #4 Request getValidSecurityEntities Handler
			this.validSecurityEntities = response;
			return this._ProfileSvc.getRegistrationInformation(mEditId);                      	// #5 Request GetRegistrationInformation
		}).catch((error) => {																	// #4 Error Handler
			this._LoggingSvc.errorHandler(error, 'SecurityEntityDetailsComponent', 'ngOnInit');
		}).then((response: IRegistrationInformation) => {										// #5 Request GetRegistrationInformation Handler
			if(!this._GWCommon.isNullOrUndefined(response)) {
				this._RegistrationInformation = response;
			}
			this._RegistrationInformation.id = this._Profile.id;
			this.populateForm();
			console.log('SecurityEntityDetailsComponent.ngOnInit this._RegistrationInformation', this._RegistrationInformation);
		}).catch((error) => {																	// #5 Error Handler
			this._LoggingSvc.errorHandler(error, 'SecurityEntityDetailsComponent', 'ngOnInit');
		});
		this.createForm();
	}

	override delete(): void {
		throw new Error('Method not implemented.');
	}

	override createForm(): void {
		const ConnectionStringValidation: Validators[] = [Validators.required];
		if(this._Profile.id != -1) {
			ConnectionStringValidation.splice(0, 1);
		}
		this.frmProfile = this._FormBuilder.group({
			// SecurityEntity or Details
			name: [this._Profile.name, [Validators.required]],
			description: [this._Profile.description, [Validators.required]],
			url: [this._Profile.url],
			dataAccessLayerAssemblyName: [this._Profile.dataAccessLayerAssemblyName, [Validators.required]],
			dataAccessLayerNamespace: [this._Profile.dataAccessLayerNamespace, [Validators.required]],
			connectionString: [this._Profile.connectionString, ConnectionStringValidation],
			// RegistrationInformation:
			accountChoices: [this._RegistrationInformation.accountChoices],
			addAccount: [this._RegistrationInformation.addAccount],
			groups: [this._RegistrationInformation.groups],
			roles: [this._RegistrationInformation.roles],
		});
	}

	getErrorMessage(fieldName: string) {
		switch (fieldName) {
		case 'connectionString':
			if (this.controls['connectionString'].hasError('required')) {
				return 'Required';
			}
			break;
		case 'description':
			if (this.controls['description'].hasError('required')) {
				return 'Required';
			}
			break;
		case 'dataAccessLayerAssemblyName':
			if (this.controls['dataAccessLayerAssemblyName'].hasError('required')) {
				return 'Required';
			}
			break;
		case 'dataAccessLayerNamespace':
			if (this.controls['dataAccessLayerNamespace'].hasError('required')) {
				return 'Required';
			}
			break;
		case 'name':
			if (this.controls['name'].hasError('required')) {
				return 'Required';
			}
			break;
		default:
			break;
		}
		return undefined;
	}

	populateForm(): void {
		this.securityEntityName = this._Profile.name;
		this.selectedDal = this._Profile.dataAccessLayer;
		this.selectedEncryptionType = this._Profile.encryptionType;
		this.selectedParent = this._Profile.parentSeqId;
		this.selectedSkin = this._Profile.skin;
		this.selectedStatusSeqId = this._Profile.statusSeqId;
		this.selectedStyle = this._Profile.style;
		this.createForm();
	}

	override populateProfile(): void {
		this._Profile.connectionString = this.controls['connectionString'].getRawValue();
		if(!this._Profile.connectionString) {
			this._Profile.connectionString = '';
		}
		this._Profile.description = this.controls['description'].getRawValue();
		this._Profile.dataAccessLayer = this.selectedDal;
		this._Profile.dataAccessLayerAssemblyName = this.controls['dataAccessLayerAssemblyName'].getRawValue();
		this._Profile.dataAccessLayerNamespace = this.controls['dataAccessLayerNamespace'].getRawValue();
		this._Profile.encryptionType = this.selectedEncryptionType;
		// this._Profile.id // No need to set
		this._Profile.name = this.controls['name'].getRawValue();
		this._Profile.parentSeqId = this.selectedParent;
		this._Profile.skin = this.selectedSkin;
		this._Profile.statusSeqId = this.selectedStatusSeqId;
		// this._Profile.style = this.selectedStyle; // legacy havent figured out how to do this yet
		this._Profile.url = this.controls['url'].getRawValue();
		// console.log('SecurityEntityDetailsComponent.populateProfile._Profile', this._Profile);
		// RegistrationInformation:
		this._RegistrationInformation.accountChoices = this.controls['accountChoices'].getRawValue();
		this._RegistrationInformation.addAccount = this.controls['addAccount'].getRawValue();
		this._RegistrationInformation.id = this._Profile.id;
		this._RegistrationInformation.groups = this.controls['groups'].getRawValue();
		this._RegistrationInformation.securityEntitySeqIdOwner = this.selectedSecurityEntity;
		this._RegistrationInformation.roles = this.controls['roles'].getRawValue();
	}

	override save(): void {
		this._ProfileSvc.save(this._Profile, this._RegistrationInformation).then((response: boolean) => {
			if(response) {
				this._LoggingSvc.toast(this.securityEntityTranslation + ' has been saved', this.securityEntityTranslation + ' Details:', LogLevel.Success);
				this.onClose();
			}
		}).catch((error: string) => {
			this._LoggingSvc.toast('Error saving ' + this.securityEntityTranslation + ':\r\n' + error, this.securityEntityTranslation + ' Details:', LogLevel.Error);
		});
	}

}

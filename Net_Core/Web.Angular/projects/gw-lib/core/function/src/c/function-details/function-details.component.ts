import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import {
	CdkDrag,
	CdkDragDrop,
	CdkDragPlaceholder,
	CdkDropList,
	moveItemInArray,
} from '@angular/cdk/drag-drop';
// Library
import { AccountService } from '@growthware/core/account';
import { BaseDetailComponent, IBaseDetailComponent } from '@growthware/core/base/components';
import { IClientChoices } from '@growthware/core/clientchoices'
import { GroupService } from '@growthware/core/group';
import { LoggingService, LogLevel } from '@growthware/core/logging';
import { ModalService, IModalOptions, ModalOptions } from '@growthware/core/modal';
import { IKeyValuePair, KeyValuePair } from '@growthware/common/interfaces';
import { RoleService } from '@growthware/core/role';
import { PickListComponent } from '@growthware/core/pick-list';
import { ListComponent } from '@growthware/core/pick-list';
import { SecurityService } from '@growthware/core/security';
import { SnakeListComponent } from '@growthware/core/snake-list';
// Feature
import { FunctionService } from '../../function.service';
import { IFunctionProfile, FunctionProfile } from '../../function-profile.model';
import { IFunctionMenuOrder } from '../../function-menu-order.model';

@Component({
	selector: 'gw-core-function-details',
	standalone: true,
	imports: [
		FormsModule,
		ReactiveFormsModule,
		ListComponent,
		PickListComponent,
		SnakeListComponent,
		MatButtonModule,
		MatCheckboxModule,
		MatFormFieldModule,
		MatGridListModule,
		MatIconModule,
		MatInputModule,
		MatSelectModule,
		MatTabsModule,
		CdkDropList,
		CdkDrag,
		CdkDragPlaceholder
	],
	templateUrl: './function-details.component.html',
	styleUrls: ['./function-details.component.scss']
})
export class FunctionDetailsComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {

	@ViewChild('helpAction') private _HelpAction!: TemplateRef<unknown>;
	@ViewChild('helpControl') private _HelpControl!: TemplateRef<unknown>;
	@ViewChild('helpSource') private _HelpSource!: TemplateRef<unknown>;
	@ViewChild('helpPassword') private _HelpPassword!: TemplateRef<unknown>;

	private _HelpOptions: IModalOptions = new ModalOptions('help', 'Help', '', 1);
	private _Profile: IFunctionProfile = new FunctionProfile();

	avalibleParents = [{ key: -1, value: 'None' }];

	clientChoices: IClientChoices = this._AccountSvc.clientChoices();

	derivedRolesId: string = 'derivedRoles';

	functionMenuOrders: IFunctionMenuOrder[] = [];

	groupsPickListNameView: string = 'viewGroups';
	groupsPickListNameAdd: string = 'addGroups';
	groupsPickListNameEdit: string = 'editGroups';
	groupsPickListNameDelete: string = 'deleteGroups';

	public availableAddGroups: Array<string> = [];
	public availableDeleteGroups: Array<string> = [];
	public availableEditGroups: Array<string> = [];
	public availableViewGroups: Array<string> = [];

	public selectedAddGroups: Array<string> = [];
	public selectedDeleteGroups: Array<string> = [];
	public selectedEditGroups: Array<string> = [];
	public selectedViewGroups: Array<string> = [];

	rolesPickListNameView: string = 'viewRoles';
	rolesPickListNameAdd: string = 'addRoles';
	rolesPickListNameEdit: string = 'editRoles';
	rolesPickListNameDelete: string = 'deleteRoles';

	public availableAddRoles: Array<string> = [];
	public availableDeleteRoles: Array<string> = [];
	public availableEditRoles: Array<string> = [];
	public availableViewRoles: Array<string> = [];

	public selectedAddRoles: Array<string> = [];
	public selectedDeleteRoles: Array<string> = [];
	public selectedEditRoles: Array<string> = [];
	public selectedViewRoles: Array<string> = [];

	derivedRolesViewName: string = 'derivedRolesView';
	derivedRolesAddName: string = 'derivedRolesAdd';
	derivedRolesEditName: string = 'derivedRolesEdit';
	derivedRolesDeleteName: string = 'derivedRolesDelete';

	public derivedAddGroups: Array<string> = [];
	public derivedDeleteGroups: Array<string> = [];
	public derivedEditGroups: Array<string> = [];
	public derivedViewGroups: Array<string> = [];

	pickListTableContentsBackground = this.clientChoices.evenRow;
	pickListTableContentsFont = this.clientChoices.evenFont;
	pickListTableHeaderBackground = this.clientChoices.oddRow;

	selectedFunctionType: number = 1;
	selectedNavigationType: number = 1;
	selectedParentId: number = -1;
	selectedLinkBehavior: number = 1;

	showRoles: boolean = false;
	showGroups: boolean = false;

	validFunctionTypes: IKeyValuePair[] = [new KeyValuePair()];

	validLinkBehaviors: IKeyValuePair[] = [new KeyValuePair()];

	validNavigationTypes: IKeyValuePair[] = [new KeyValuePair()];

	constructor(
		private _AccountSvc: AccountService,
		private _FormBuilder: FormBuilder,
		private _GroupSvc: GroupService,
		private _RoleSvc: RoleService,
		loggingSvc: LoggingService,
		modalSvc: ModalService,
		profileSvc: FunctionService,
		securitySvc: SecurityService
	) {
		super();
		this._LoggingSvc = loggingSvc;
		this._ModalSvc = modalSvc;
		this._ProfileSvc = profileSvc;
		this._SecuritySvc = securitySvc;
	}

	ngOnInit(): void {
		// console.log('modalReason', this._ProfileSvc.modalReason);
		// console.log('selectedRow', this._ProfileSvc.selectedRow);
		let mEditId = -1;
		if (this._ProfileSvc.modalReason.toLocaleLowerCase() != 'newprofile') {
			mEditId = this._ProfileSvc.selectedRow.FunctionSeqId;
		}
		this._SecuritySvc.getSecurityInfo('FunctionSecurity').then((securityInfo) => {  // Request/Handler #1
			// console.log('securityInfo', securityInfo);
			this._SecurityInfo = securityInfo;
			return this._ProfileSvc.getFunction(mEditId);                               // Request #2 getFunction(mEditId)
		}).catch((error) => {                                                           // Request #1 Error Handler
			this._LoggingSvc.toast('Error getting function security:\r\n' + error, 'Function Details:', LogLevel.Error);
		}).then((profile) => {                                                          // Request #2 Handler
			// console.log('FunctionDetailsComponent.ngOnInit.profile', profile);
			if (profile) {
				this._Profile = profile;
				this.functionMenuOrders = this._Profile.functionMenuOrders;
			}
			return this._ProfileSvc.getFuncitonTypes();                                 // Request #3 getFuncitonTypes()
		}).catch((error) => {                                                           // Request #2 Error Handler
			this._LoggingSvc.toast('Error getting function:\r\n' + error, 'Function Details:', LogLevel.Error);
		}).then((functionTypes: IKeyValuePair[]) => {                                   // Request #3 Handler
			// console.log('FunctionDetailsComponent.ngOnInit.functionTypes', functionTypes);
			this.validFunctionTypes = functionTypes;
			return this._ProfileSvc.getNavigationTypes();                               // Request #4 getNavigationTypes()
		}).catch((error) => {                                                           // Request #3 Error Handler
			this._LoggingSvc.toast('Error getting function types:\r\n' + error, 'Function Details:', LogLevel.Error);
		}).then((navigationTypes: IKeyValuePair[]) => {                                 // Request #4 Handler
			// console.log('FunctionDetailsComponent.ngOnInit.navigationTypes', navigationTypes);
			this.validNavigationTypes = navigationTypes;
			return this._ProfileSvc.getLinkBehaviors();                                 // Request #5 getLinkBehaviors() Request
		}).catch((error) => {                                                           // Request #4 Error Handler
			this._LoggingSvc.toast('Error getting navigation types:\r\n' + error, 'Function Details:', LogLevel.Error);
		}).then((linkBehaviors: IKeyValuePair[]) => {                                   // Request #5 Handler
			// console.log('FunctionDetailsComponent.ngOnInit.linkBehaviors', linkBehaviors);
			this.validLinkBehaviors = linkBehaviors;
			return this._ProfileSvc.getAvalibleParents();                               // Request #6 getAvalibleParents()
		}).catch((error) => {                                                           // Request #5 Error Handler
			this._LoggingSvc.toast('Error getting link behaviors:\r\n' + error, 'Function Details:', LogLevel.Error);
		}).then((avalibleParents) => {                                                    // Request #6 Handler
			// console.log('FunctionDetailsComponent.ngOnInit.avalibleParents', avalibleParents);
			this.avalibleParents = avalibleParents;
			return this._RoleSvc.getRoles();                                            // Request #7 getRoles()
		}).catch((error) => {                                                           // Request #6 Error Handler
			this._LoggingSvc.toast('Error getting avalible parents:\r\n' + error, 'Function Details:', LogLevel.Error);
		}).then((roles: string[] | void) => {                                           // Request #7 Handler
			// console.log('FunctionDetailsComponent.ngOnInit.roles', roles);
			if (roles) {
				this.availableAddRoles = roles;
				this.availableDeleteRoles = roles;
				this.availableEditRoles = roles;
				this.availableViewRoles = roles;
			}
			return this._GroupSvc.getGroups();                                          // Request #8 getGroups()
		}).catch((error) => {                                                           // Request #7 Error Handler
			this._LoggingSvc.toast('Error getting avalible roles:\r\n' + error, 'Function Details:', LogLevel.Error);
		}).then((groups: string[] | void) => {                                          // Request #8 Handler
			// console.log('FunctionDetailsComponent.ngOnInit.groups', groups);
			if (groups) {
				this.availableAddGroups = groups;
				this.availableDeleteGroups = groups;
				this.availableEditGroups = groups;
				this.availableViewGroups = groups;
			}
			this.applySecurity();
			this.showGroups = this._Profile.canSaveGroups;
			this.showRoles = this._Profile.canSaveRoles;
			this.populateForm();
		}).catch((error) => {                                                           // Request #8 Error Handler
			this._LoggingSvc.toast('Error getting avalible groups:\r\n' + error, 'Function Details:', LogLevel.Error);
		});
		this._Profile = new FunctionProfile();
		this.createForm();
	}

	createForm(): void {
		this.frmProfile = this._FormBuilder.group({
			action: [this._Profile.action, [Validators.required]],
			description: [this._Profile.description],
			directory: [this._Profile.directoryData.directory],
			enableViewState: [this._Profile.enableViewState],
			enableNotifications: [this._Profile.enableNotifications],
			id: [{ value: this._Profile.id, disabled: true }],
			impersonate: [this._Profile.directoryData.impersonate],
			impersonationAccount: [this._Profile.directoryData.impersonateAccount],
			impersonatePassword: [this._Profile.directoryData.impersonatePassword],
			isNavigable: [this._Profile.isNavigable],
			linkBehavior: [this._Profile.linkBehavior],
			// functionTypeSeqId: [this._Profile.functionTypeSeqId],
			groups: [this._Profile.groups],
			name: [this._Profile.name, [Validators.required]],
			metaKeywords: [this._Profile.metaKeywords],
			navigationTypeSeqId: [this._Profile.navigationTypeSeqId],
			notes: [this._Profile.notes],
			noUI: [this._Profile.noUI],
			parentId: [this._Profile.parentId],
			redirectOnTimeout: [this._Profile.redirectOnTimeout],
			roles: [this._Profile.roles],
			source: [this._Profile.source],
			controller: [this._Profile.controller],
		});
	}

	delete(): void {
		this._ProfileSvc.deleteFunction(this._Profile.id).then((response: boolean) => {
			if (response) {
				this._LoggingSvc.toast('Function has been deleted', 'Function Details:', LogLevel.Success);
				this.onClose();
			} else {
				this._LoggingSvc.toast('Function could not be deleted!', 'Function Details:', LogLevel.Error);
			}
		}).catch(() => {
			this._LoggingSvc.toast('Function could not be deleted!', 'Function Details:', LogLevel.Error);
		});
	}

	drop(event: CdkDragDrop<string[]>) {
		moveItemInArray(this.functionMenuOrders, event.previousIndex, event.currentIndex);
	}

	getErrorMessage(fieldName: string) {
		switch (fieldName) {
			case 'name':
				if (this.controls['name'].hasError('required')) {
					return 'Required';
				}
				break;
			case 'action':
				if (this.controls['action'].hasError('required')) {
					return 'Required';
				}
				break;
			default:
				break;
		}
		return undefined;
	}

	onHelp(controleName: string): void {
		switch (controleName) {
			case 'Action':
				this._HelpOptions.contentPayLoad = this._HelpAction;
				break;
			case 'Source':
				this._HelpOptions.contentPayLoad = this._HelpSource;
				break;
			case 'Control':
				this._HelpOptions.contentPayLoad = this._HelpControl;
				break;
			case 'Passord':
				this._HelpOptions.contentPayLoad = this._HelpPassword;
				break;
			default:
				break;
		}
		this._ModalSvc.open(this._HelpOptions);
	}

	private populateForm(): void {
		this.createForm();

		this.derivedAddGroups = this._Profile.derivedAddRoles;
		this.derivedDeleteGroups = this._Profile.derivedDeleteRoles;
		this.derivedEditGroups = this._Profile.derivedEditRoles;
		this.derivedViewGroups = this._Profile.derivedViewRoles;

		this.selectedAddRoles = this._Profile.assignedAddRoles;
		this.selectedDeleteRoles = this._Profile.assignedDeleteRoles;
		this.selectedEditRoles = this._Profile.assignedEditRoles;
		this.selectedViewRoles = this._Profile.assignedViewRoles;

		this.selectedAddGroups = this._Profile.addGroups;
		this.selectedDeleteGroups = this._Profile.deleteGroups;
		this.selectedEditGroups = this._Profile.editGroups;
		this.selectedViewGroups = this._Profile.viewGroups;

		// const mGroups = this._Profile.groups!;
		this.selectedFunctionType = this._Profile.functionTypeSeqId;
		this.selectedNavigationType = this._Profile.navigationTypeSeqId;
		this.selectedParentId = this._Profile.parentId;
		this.selectedLinkBehavior = this._Profile.linkBehavior;
	}

	populateProfile(): void {
		this._Profile.action = this.controls['action'].getRawValue();
		this._Profile.controller = this.controls['controller'].getRawValue();
		this._Profile.description = this.controls['description'].getRawValue();
		this._Profile.directoryData.directory = this.controls['directory'].getRawValue(); // I don't think this is correct need to verify
		this._Profile.enableViewState = this.controls['enableViewState'].getRawValue();
		this._Profile.enableNotifications = this.controls['enableNotifications'].getRawValue();

		this._Profile.addGroups = this.selectedAddGroups;
		this._Profile.deleteGroups = this.selectedDeleteGroups;
		this._Profile.editGroups = this.selectedEditGroups;
		this._Profile.viewGroups = this.selectedViewGroups;

		this._Profile.assignedAddRoles = this.selectedAddRoles;
		this._Profile.assignedDeleteRoles = this.selectedDeleteRoles;
		this._Profile.assignedEditRoles = this.selectedEditRoles;
		this._Profile.assignedViewRoles = this.selectedViewRoles;
		// this._Profile.id = this.controls['id'].getRawValue();
		this._Profile.directoryData.impersonate = this.controls['impersonate'].getRawValue();
		this._Profile.directoryData.impersonateAccount = this.controls['impersonationAccount'].getRawValue();
		this._Profile.directoryData.impersonatePassword = this.controls['impersonatePassword'].getRawValue();
		this._Profile.isNavigable = this.controls['isNavigable'].getRawValue();
		this._Profile.functionTypeSeqId = this.selectedFunctionType;
		this._Profile.linkBehavior = this.selectedLinkBehavior;
		this._Profile.metaKeywords = this.controls['metaKeywords'].getRawValue();
		this._Profile.name = this.controls['name'].getRawValue();
		this._Profile.navigationTypeSeqId = this.selectedNavigationType;
		this._Profile.notes = this.controls['notes'].getRawValue();
		this._Profile.noUI = this.controls['noUI'].getRawValue();
		this._Profile.parentId = this.selectedParentId;
		this._Profile.redirectOnTimeout = this.controls['redirectOnTimeout'].getRawValue();
		this._Profile.source = this.controls['source'].getRawValue();
		// console.log('FunctionDetailsComponent.populateProfile', this._Profile);
	}

	save(): void {
		// this._LoggingSvc.toast('FunctionDetailsComponent.save', 'Function Details:', LogLevel.Error);
		this._ProfileSvc.save(this._Profile).then((respnse: boolean) => {
			if (respnse) {
				this._LoggingSvc.toast('Function has been saved', 'Function Details:', LogLevel.Success);
				this.onClose();
			} else {
				this._LoggingSvc.toast('Function could not be saved!', 'Function Details:', LogLevel.Error);
			}
		}).catch(() => {
			this._LoggingSvc.toast('Function could not be saved!', 'Function Details:', LogLevel.Error);
		});
	}
}

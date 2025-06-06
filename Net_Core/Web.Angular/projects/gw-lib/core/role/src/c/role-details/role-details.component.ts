import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { ClientChoicesService, IClientChoices } from '@growthware/core/clientchoices';
import { LoggingService, LogLevel } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
import { PickListComponent } from '@growthware/core/pick-list';
import { ISecurityInfo, SecurityInfo, SecurityService } from '@growthware/core/security';
import { SearchService } from '@growthware/core/search';
// Feature
import { RoleService } from '../../role.service';
import { IRoleProfile, RoleProfile } from '../../role-profile.model';

@Component({
	selector: 'gw-core-role-details',
	standalone: true,
	imports: [
		FormsModule,
		PickListComponent,
		ReactiveFormsModule,
		MatButtonModule,
		MatCheckboxModule,
		MatIconModule,
		MatInputModule,
		MatTabsModule
	],
	templateUrl: './role-details.component.html',
	styleUrls: ['./role-details.component.scss']
})
export class RoleDetailsComponent implements OnInit {

	private _Role: IRoleProfile = new RoleProfile();
	private _SecurityInfo: ISecurityInfo = new SecurityInfo();

	availableRoles: Array<string> = [];
	canDelete: boolean = false;
	canSave: boolean = false;
	frmRole!: FormGroup;
	membersPickListName: string = 'membersList';
	securityInfo!: ISecurityInfo;
	selectedRoles: Array<string> = [];

	pickListTableContentsBackground = '#6699cc';
	pickListTableContentsFont = 'White';
	pickListTableHeaderBackground = '#b6cbeb';

	constructor(
		private _ClientChoicesSvc: ClientChoicesService,
		private _FormBuilder: FormBuilder,
		private _LoggingSvc: LoggingService,
		private _ModalSvc: ModalService,
		private _RoleSvc: RoleService,
		private _SearchSvc: SearchService,
		private _SecuritySvc: SecurityService,
	) {
		this.frmRole = this._FormBuilder.group({});
	}

	ngOnInit(): void {
		const mClientChoices: IClientChoices = this._ClientChoicesSvc.getClientChoices();
		this.pickListTableContentsBackground = mClientChoices.evenRow;
		this.pickListTableContentsFont = mClientChoices.evenFont;
		this.pickListTableHeaderBackground = mClientChoices.oddRow;

		this._SecuritySvc.getSecurityInfo('Search_Roles').then((securityInfo: ISecurityInfo) => { // Request #1
			// Response Hendler #1
			// console.log('RoleDetailsComponent.ngOnInit.getSecurityInfo.response', response);
			this._SecurityInfo = securityInfo;
			let mRoleSeqId: number = -1;
			if (this._RoleSvc.modalReason === 'EditProfile') {
				mRoleSeqId = this._RoleSvc.selectedRow.RoleSeqId;
				this.canSave = securityInfo.mayEdit;
			} else {
				this.canSave = securityInfo.mayAdd;
			}
			// TODO: Looks like this._RoleSvc.selectedRow is not always being set correctly
			return this._RoleSvc.getRoleForEdit(mRoleSeqId); // Request #2
		}).catch((error) => { // Request #1 error
			this._LoggingSvc.toast('Error getting security info for \'EditRole\' :\r\n' + error, 'Role Details:', LogLevel.Error);
		}).then((profile) => {
			if (profile) {
				this._Role = profile;
				this.availableRoles = this._Role.accountsNotInRole;
				this.selectedRoles = this._Role.accountsInRole;
				if (profile.id === -1 || profile.isSystemOnly || profile.isSystem) {
					this.canDelete = false;
					if (profile.isSystemOnly) {
						this.canSave = false;
					}
				} else {
					this.canDelete = this._SecurityInfo.mayDelete;
				}
			}
			this.populateForm();
		}).catch((error) => {
			this._LoggingSvc.errorHandler(error, 'className', 'method');
			this.populateForm();
		});
		this.populateForm();
	}

	get controls() {
		return this.frmRole.controls;
	}

	closeModal(): void {
		this._ModalSvc.close(this._RoleSvc.addEditModalId);
	}

	getErrorMessage(fieldName: string) {
		switch (fieldName) {
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

	onCancel(): void {
		this.closeModal();
	}

	onDelete(): void {
		this._RoleSvc.delete(this._Role.id).then(() => {
			this.updateSearch();
			this.closeModal();
		}).catch(() => {
			this._LoggingSvc.toast('The role could not be deleted', 'Delete Role', LogLevel.Error);
		});
	}

	onSubmit(): void {
		this.populateProfile();
		this._RoleSvc.save(this._Role).then(() => {
			this.updateSearch();
			this._LoggingSvc.toast('The role has been saved', 'Save Role', LogLevel.Success);
		}).catch(() => {
			this._LoggingSvc.toast('The role could not be saved', 'Save Role', LogLevel.Error);
		});
		this.closeModal();
	}

	private populateForm(): void {
		let mDisabledName = false;
		if (this._Role.isSystem || this._Role.isSystemOnly) {
			mDisabledName = true;
		}
		this.frmRole = this._FormBuilder.group({
			name: [{ value: this._Role.name, disabled: mDisabledName }, Validators.required],
			description: [this._Role.description],
			isSystem: [{ value: this._Role.isSystem, disabled: this._Role.isSystem }],
			isSystemOnly: [{ value: this._Role.isSystemOnly, disabled: this._Role.isSystemOnly || this._Role.isSystem }],
		});
	}

	private populateProfile(): void {
		this._Role.name = this.controls['name'].getRawValue();
		this._Role.description = this.controls['description'].getRawValue();
		this._Role.isSystem = this.controls['isSystem'].getRawValue();
		this._Role.isSystemOnly = this.controls['isSystemOnly'].getRawValue();
		this._Role.accountsInRole = this.selectedRoles;
	}

	updateSearch(): void {
		const mSearchCriteria = this._SearchSvc.getSearchCriteria('Roles'); // from SearchAccountsComponent line 25
		if (mSearchCriteria != null) {
			this._SearchSvc.setSearchCriteria('Roles', mSearchCriteria);
		}
	}
}

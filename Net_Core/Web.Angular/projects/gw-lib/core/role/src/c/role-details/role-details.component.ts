import { Component, OnDestroy, OnInit } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
// Library
// import { GWCommon } from '@growthware/common/services';
import { DataService } from '@growthware/common/services';
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
export class RoleDetailsComponent implements OnDestroy, OnInit {

	private _Role: IRoleProfile = new RoleProfile();
	private _SecurityInfo: ISecurityInfo = new SecurityInfo();
	private _Subscription: Subscription = new Subscription();

	canDelete: boolean = false;
	canSave: boolean = false;
	frmRole!: FormGroup;
	membersPickListName: string = 'membersList';
	securityInfo!: ISecurityInfo;

	constructor(
		private _FormBuilder: FormBuilder,
		// private _GWCommon: GWCommon,
		private _DataSvc: DataService,
		private _LoggingSvc: LoggingService,
		private _ModalSvc: ModalService,
		private _RoleSvc: RoleService,
		private _SearchSvc: SearchService,
		private _SecuritySvc: SecurityService,
	) {
		this.frmRole = this._FormBuilder.group({});
	}

	ngOnInit(): void {
		this._Subscription.add(this._DataSvc.dataChanged$.subscribe((data) => {
			// console.log('GroupDetailsComponent.ngOnInit',data.name.toLowerCase()); // used to determine the data name 
			switch (data.name.toLowerCase()) {
				case 'membersList':
					// set the paload to whatever you are using to track the "selected" items
					this._Role.accountsInRole = data.value;
					break;
				default:
					break;
			}
		}));
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
				setTimeout(() => { this._DataSvc.notifyDataChanged(this.membersPickListName + '_AvailableItems', this._Role.accountsNotInRole); }, 500);
				setTimeout(() => { this._DataSvc.notifyDataChanged(this.membersPickListName + '_SelectedItems', this._Role.accountsInRole); }, 500);
				if (profile.id === -1 || profile.isSystemOnly) {
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

	ngOnDestroy(): void {
		this._Subscription.unsubscribe();
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
		this.frmRole = this._FormBuilder.group({
			name: [this._Role.name, Validators.required],
			description: [this._Role.description],
			isSystem: [{ value: this._Role.isSystem, disabled: this._Role.isSystemOnly }],
			isSystemOnly: [{ value: this._Role.isSystemOnly, disabled: this._Role.isSystemOnly }],
		});
	}

	private populateProfile(): void {
		this._Role.name = this.controls['name'].getRawValue();
		this._Role.description = this.controls['description'].getRawValue();
		this._Role.isSystem = this.controls['isSystem'].getRawValue();
		this._Role.isSystemOnly = this.controls['isSystemOnly'].getRawValue();
	}

	updateSearch(): void {
		const mSearchCriteria = this._SearchSvc.getSearchCriteria('Roles'); // from SearchAccountsComponent line 25
		if (mSearchCriteria != null) {
			this._SearchSvc.setSearchCriteria('Roles', mSearchCriteria);
		}
	}
}

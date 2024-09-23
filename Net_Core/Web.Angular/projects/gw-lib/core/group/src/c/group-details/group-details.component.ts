import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatLineModule } from '@angular/material/core';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { LoggingService, LogLevel } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
import { PickListComponent } from '@growthware/core/pick-list';
import { SearchService } from '@growthware/core/search';
import { ISecurityInfo, SecurityService } from '@growthware/core/security';
// Feature
import { GroupService } from '../../group.service';
import { IGroupProfile, GroupProfile } from '../../group-profile.model';

@Component({
	selector: 'gw-core-group-details',
	standalone: true,
	imports: [
		FormsModule,
		PickListComponent,
		ReactiveFormsModule,
		MatButtonModule,
		MatFormFieldModule,
		MatLineModule,
		MatIconModule,
		MatInputModule,
		MatTabsModule
	],
	templateUrl: './group-details.component.html',
	styleUrls: ['./group-details.component.scss']
})
export class GroupDetailsComponent implements OnInit {
	private _GroupProfile: IGroupProfile = new GroupProfile();

	canDelete: boolean = false;
	frmGroup!: FormGroup;
	height: number = 350;
	securityInfo!: ISecurityInfo;
	showDerived: boolean = false;
	rolesPickListName: string = 'roles';

	availableRoles: Array<string> = [];
	selectedRoles: Array<string> = [];

	constructor(
		private _FormBuilder: FormBuilder,
		private _GroupSvc: GroupService,
		private _LoggingSvc: LoggingService,
		private _ModalSvc: ModalService,
		private _SearchSvc: SearchService,
		private _SecuritySvc: SecurityService
	) { }

	ngOnInit(): void {
		let mIdToGet = -1;
		if (this._GroupSvc.modalReason.toLowerCase() != 'newprofile') {
			// console.log('selectedRow', this._GroupSvc.selectedRow);
			mIdToGet = this._GroupSvc.selectedRow.GroupSeqId;
		}
		this._GroupSvc.getGroupForEdit(mIdToGet).then((response: IGroupProfile) => {
			this._GroupProfile = response;
			this.availableRoles = this._GroupProfile.rolesNotInGroup;
			this.selectedRoles = this._GroupProfile.rolesInGroup;
			this.populateForm();
			return this._SecuritySvc.getSecurityInfo('Manage_Groups');
		}).then((response: ISecurityInfo) => {
			this.securityInfo = response;
			this.applySecurity();
		});
		this.populateForm();
	}

	applySecurity(): void {
		this.canDelete = this.securityInfo.mayDelete;
		if (this._GroupSvc.modalReason.toLowerCase() == 'newprofile') {
			this.canDelete = false;
		}
	}

	get controls() {
		return this.frmGroup.controls;
	}

	closeModal(): void {
		this._ModalSvc.close(this._GroupSvc.addEditModalId);
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
		// console.log('GroupDetailsComponent.onDelete');
		this._GroupSvc.delete(this._GroupProfile.id).then((response) => {
			this.updateSearch();
			this._LoggingSvc.toast('The group has been deleted', 'Delete Group', LogLevel.Success);
			this.closeModal();
		}).catch((error) => {
			this._LoggingSvc.errorHandler(error, 'GroupService', 'delete');
			this._LoggingSvc.toast('The group could not be deleted', 'Delete Group', LogLevel.Error);
		});
	}

	onSubmit(form: FormGroup): void {
		this.populateProfile();
		this._GroupSvc.saveGroup(this._GroupProfile).then(() => {
			this.updateSearch();
			this._LoggingSvc.toast('The group has been saved', 'Save Group', LogLevel.Success);
			this.closeModal();
		}).catch(() => {
			this._LoggingSvc.toast('The group could not be saved', 'Save Group', LogLevel.Error);
		});
	}

	private populateForm(): void {
		this.frmGroup = this._FormBuilder.group({
			name: [this._GroupProfile.name, Validators.required],
			description: [this._GroupProfile.description],
		});
	}

	private populateProfile(): void {
		this._GroupProfile.name = this.controls['name'].getRawValue();
		this._GroupProfile.description = this.controls['description'].getRawValue();
		this._GroupProfile.rolesInGroup = this.selectedRoles;
	}

	updateSearch(): void {
		const mSearchCriteria = this._SearchSvc.getSearchCriteria('Groups'); // from SearchAccountsComponent line 25
		if (mSearchCriteria != null) {
			this._SearchSvc.setSearchCriteria('Groups', mSearchCriteria);
		}
	}

}

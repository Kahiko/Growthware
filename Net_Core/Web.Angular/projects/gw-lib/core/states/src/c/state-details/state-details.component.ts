import { Component, OnInit } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { BaseDetailComponent, IBaseDetailComponent } from '@growthware/core/base/components';
import { IKeyValuePair } from '@growthware/common/interfaces';
import { LogLevel, LoggingService } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
import { SecurityService } from '@growthware/core/security';
// Feature
import { IStateProfile, StateProfile } from '../../state-profile.model';
import { StatesService } from '../../states.service';

@Component({
	selector: 'gw-core-state-details',
	standalone: true,
	imports: [
		FormsModule,
		ReactiveFormsModule,
		MatButtonModule,
		MatCheckboxModule,
		MatIconModule,
		MatInputModule,
		MatSelectModule,
		MatTabsModule
	],
	templateUrl: './state-details.component.html',
	styleUrls: ['./state-details.component.scss']
})
export class StateDetailsComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {

	private _Profile: IStateProfile = new StateProfile();

	state: string = 'state';

	selectedStatus: number = -1;
	validStatuses: IKeyValuePair[] = [
		{ key: 1, value: 'Active' },
		{ key: 2, value: 'Inactive' }
	];

	constructor(
		private _FormBuilder: FormBuilder,
		loggingSvc: LoggingService,
		modalSvc: ModalService,
		profileSvc: StatesService,
		securitySvc: SecurityService,
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
		this.selectedStatus = 1;
		if (this._ProfileSvc.selectedRow.Status.trim() !== 'Active') {
			this._Profile.statusId = 2;
			this.selectedStatus = 2;
		}
		this._SecuritySvc.getSecurityInfo('EditState').then((securityInfo) => {  // #1 getSecurityInfo Request/Handler
			// console.log('StateDetailsComponent.ngOnInit.securityInfo', securityInfo);
			this._SecurityInfo = securityInfo;
			return this._ProfileSvc.getState(this._ProfileSvc.selectedRow.State);      // #2 getState Request
		}).catch((error) => {                                               // #1 getSecurityInfo Error Handler
			this._LoggingSvc.toast('Error getting security info:\r\n' + error, 'State Details:', LogLevel.Error);
		}).then((response) => {                                                  // #2 getState Handler
			this._Profile = response;
			this.applySecurity();
			this.populateForm();
		}).catch((error) => {                                                // #2 getStateError Handler
			this._LoggingSvc.toast('Error getting State:\r\n' + error, 'State Details:', LogLevel.Error);
		});
		this.createForm();
	}

	delete() {
		// only here to satisfy the base component we will not be allowing the deletion of a state
	}

	createForm() {
		// console.log('StateDetailsComponent.createForm._Profile', this._Profile);
		this.frmProfile = this._FormBuilder.group({
			state: [this._Profile.state],
			description: [this._Profile.description],
			status: [this._Profile.statusId]
		});
	}

	populateForm() {
		this.createForm();
		this.state = this._Profile.state;
	}

	populateProfile() {
		this._Profile.description = this.frmProfile.get('description')?.value;
		this._Profile.state = this.state;
		this._Profile.statusId = this.selectedStatus;
	}

	save() {
		this._ProfileSvc.saveState(this._Profile).then((response: boolean) => {
			if (response) {
				this._LoggingSvc.toast('State has been saved', 'State Details:', LogLevel.Success);
				this.onClose();
			}
		}).catch((error: string) => {
			this._LoggingSvc.toast('Error saving State:\r\n' + error, 'State Details:', LogLevel.Error);
		});
	}
}

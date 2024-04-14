import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
// Library
import { BaseDetailComponent, IBaseDetailComponent } from '@growthware/core/base/components';
import { LoggingService } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
import { ISecurityInfo, SecurityService } from '@growthware/core/security';
// import { SecurityService, ISecurityInfo } from '@growthware/core/security';
// Feature
import { NameValuePairService } from '../../name-value-pairs.service';
import { INvpChildProfile, NvpChildProfile } from '../../name-value-pair-child-profile.model';

@Component({
	selector: 'gw-core-name-value-pair-child-detail',
	standalone: true,
	imports: [
		// Angular
		CommonModule,
		FormsModule,
		ReactiveFormsModule,
		// Angular Material
		MatButtonModule,
		MatFormFieldModule,
		MatGridListModule,
		MatSelectModule,
		MatTabsModule,
	],
	templateUrl: './name-value-pair-child-detail.component.html',
	styleUrls: ['./name-value-pair-child-detail.component.scss']
})
export class NameValuePairChildDetailComponent extends BaseDetailComponent implements IBaseDetailComponent, OnInit {

	private _Action: string = '';
	private _Profile: INvpChildProfile = new NvpChildProfile();

	isReadonly: boolean = true;

	constructor(
		profileSvc: NameValuePairService,
		loggingSvc: LoggingService,
		modalSvc: ModalService,
		securitySvc: SecurityService,
		private _FormBuilder: FormBuilder,
		private _Router: Router,
	) {
		super();
		this._Action = this._Router.url.split('?')[0].replace('/', '').replace('\\', '');
		this._ModalSvc = modalSvc;
		this._ProfileSvc = profileSvc;
		this._SecuritySvc = securitySvc;
	}
	ngOnInit(): void {
		this.createForm();
		this._SecuritySvc.getSecurityInfo('search_name_value_pairs').then((securityInfo: ISecurityInfo) => {  // Request #1
			if (securityInfo != null) {                                                                       // Response Handler #1
				this._SecurityInfo = securityInfo;
			}
			return this._ProfileSvc.getChildProfile();                                                        // Request #2
		}).catch().then((response: INvpChildProfile) => {
			if (response) {                                                                                   // Response Handler #2
				this._Profile = response;
				// console.log('NameValuePairChildDetailComponent.ngOnInit this._Profile', this._Profile);
				this.canSave = this._SecurityInfo.mayEdit;
				if (this._ProfileSvc.modalReason === 'Add') {
					this.isReadonly = false;
				}
				this._Profile = response;
				this.frmProfile.patchValue(this._Profile);
				//   this.selectedStatus = this._Profile.status;
			}
		});
	}

	override delete(): void {
		throw new Error('Method not implemented.');
	}
	override createForm(): void {
		this.frmProfile = this._FormBuilder.group({

		});
	}
	override populateProfile(): void {
		throw new Error('Method not implemented.');
	}
	override save(): void {
		throw new Error('Method not implemented.');
	}

}

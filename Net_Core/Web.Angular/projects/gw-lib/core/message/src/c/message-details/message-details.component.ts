import { Component, OnInit } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
// Angular Material
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
// Library
import { LoggingService, LogLevel } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
import { SecurityService, ISecurityInfo } from '@growthware/core/security';
// Feature
import { MessageService } from '../../message.service';
import { IMessageProfile, MessageProfile } from '../../message-profile.model';

@Component({
	selector: 'gw-core-message-details',
	standalone: true,
	imports: [
		FormsModule,
		ReactiveFormsModule,
		MatButtonModule,
		MatIconModule
	],
	templateUrl: './message-details.component.html',
	styleUrls: ['./message-details.component.scss']
})
export class MessageDetailsComponent implements OnInit {

	private _Profile: IMessageProfile = new MessageProfile();
	private _SecurityInfo!: ISecurityInfo;

	canSave: boolean = false;
	frmMessage!: FormGroup;

	constructor(
    private _FormBuilder: FormBuilder,
    private _LoggingSvc: LoggingService,
    private _MessageSvc: MessageService,
    private _ModalSvc: ModalService,
    private _SecuritySvc: SecurityService,
	) { }

	ngOnInit(): void {
		this.populateForm(); // need to make sure the form controls are set ¯\_(ツ)_/¯
		let mIdToGet = -1;
		// console.log('selectedRow', this._MessageSvc.selectedRow);
		if(this._MessageSvc.modalReason.toLowerCase() != 'newprofile') {
			mIdToGet = this._MessageSvc.selectedRow.MessageSeqId;
		}
		// console.log('mIdToGet', mIdToGet);
		this._SecuritySvc.getSecurityInfo('Search_Messages').then((securityInfo: ISecurityInfo) => { // #1 Request/Handler
			this._SecurityInfo = securityInfo;
			return this._MessageSvc.getProfile(mIdToGet);                   // #2 Request
		}).catch((error) => {                                        // Request #1 Error Handler
			this._LoggingSvc.toast('Error getting the security :\r\n' + error, 'Message Details:', LogLevel.Error);
		}).then((response) => {                                           // Request 2 Handler
			if(response) {
				this._Profile = response;
			}
			this.populateForm();
			this.applySecurity();
		}).catch((error) => {                                              // Request #2 Error Handler
			this._LoggingSvc.toast('Error getting the message :\r\n' + error, 'Message Details:', LogLevel.Error);
		});
	}

	private applySecurity(): void {
		if(this._SecurityInfo) {
			if(this._SecurityInfo.mayEdit) {
				this.canSave = true;
			}
		}
	}

	get controls() {
		return this.frmMessage.controls;
	}

	closeModal(): void {
		this._ModalSvc.close(this._MessageSvc.addEditModalId);
	}

	/**
 * Submits the form and saves the message.
 *
 * @param {FormGroup} form - The form to be submitted.
 * @return {void} This function does not return a value.
 */
	onSubmit(form: FormGroup): void {
		if(form.valid) {
			this.populateProfile();
			// console.log('Profile', this._Profile);
			this._MessageSvc.save(this._Profile).then(() => {
				this._LoggingSvc.toast('Message has been saved', 'Save Message', LogLevel.Success);
				this.closeModal();
			}).catch(() => {
				this._LoggingSvc.toast('Message could not be saved', 'Save Message', LogLevel.Error);
			});
		}
	}

	onCancel(): void {
		this.closeModal();
	}

	private populateForm(): void {
		if(this._Profile.id > -1) {
			this.frmMessage = this._FormBuilder.group({
				avalibleTags: [{value: this._Profile.avalibleTags, disabled: true }],
				description: [this._Profile.description],
				formatAsHtml:[this._Profile.formatAsHtml],
				messageBody: [this._Profile.body, Validators.required],
				name: [{value : this._Profile.name, disabled: false }],
				title: [this._Profile.title, Validators.required],
			});
		} else {
			this.frmMessage = this._FormBuilder.group({
				avalibleTags: [{value: this._Profile.avalibleTags, disabled: true }],
				description: [this._Profile.description],
				formatAsHtml:[this._Profile.formatAsHtml],
				messageBody: [this._Profile.body, Validators.required],
				name: [{value : this._Profile.name, disabled: this._Profile.id > -1}, Validators.required],
				title: [this._Profile.title, Validators.required],
			});
		}
	}

	private populateProfile(): void {
		this._Profile.body = this.controls['messageBody'].getRawValue();
		this._Profile.description = this.controls['description'].getRawValue();
		this._Profile.formatAsHtml = this.controls['formatAsHtml'].getRawValue();
		this._Profile.name = this.controls['name'].getRawValue();
		this._Profile.title = this.controls['title'].getRawValue();
	}
}
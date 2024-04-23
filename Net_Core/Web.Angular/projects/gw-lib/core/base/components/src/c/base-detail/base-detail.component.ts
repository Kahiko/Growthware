import { Component, OnDestroy } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';
// Library
import { DataService } from '@growthware/common/services';
import { LoggingService } from '@growthware/core/logging';
import { ModalService } from '@growthware/core/modal';
import { SecurityService, ISecurityInfo, SecurityInfo } from '@growthware/core/security';

export interface IBaseDetailComponent extends BaseDetailComponent {
    delete(): void;
    createForm(): void;
    populateProfile(): void;
    save(): void;
}

@Component({
	selector: 'gw-core-base-search',
	template: '',
	styles: [
	]
})
export abstract class BaseDetailComponent implements IBaseDetailComponent, OnDestroy {
	protected _Subscription: Subscription = new Subscription();

	frmProfile!: FormGroup;

	protected canDelete: boolean = false;
	protected canSave: boolean = false;
	protected _SecurityInfo: ISecurityInfo = new SecurityInfo();

	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	protected _ProfileSvc: any = {};
	protected _DataSvc!: DataService;
	protected _LoggingSvc!: LoggingService;
	protected _ModalSvc!: ModalService;
	protected _SecuritySvc!: SecurityService;

	ngOnDestroy(): void {
		this._Subscription.unsubscribe();
		this._ProfileSvc.modalReason = '';
		this._ProfileSvc.selectedRow = '';
	}

	protected applySecurity() {
		this.canSave = this._SecurityInfo.mayEdit || this._SecurityInfo.mayAdd;
		this.canDelete = this._SecurityInfo.mayDelete;
		if(this._ProfileSvc.modalReason.toLowerCase() == 'newprofile') {
			this.canDelete = false;
		}
	}

	protected onCancel(): void {
		this.onClose();
	}

	protected onClose(): void {
		this._ModalSvc.close(this._ProfileSvc.addEditModalId);
		// this._LoggingSvc.toast('BaseDetailComponent.onClose', 'Function Details:', LogLevel.Error);
	}

	protected onDelete(): void {
		this.delete();
	}

	protected onSubmit(): void {
		// this._LoggingSvc.toast('BaseDetailComponent.onSubmit', 'Function Details:', LogLevel.Error);
		this.populateProfile();
		this.save();
	}

	get controls() {
		return this.frmProfile.controls;
	}

	public findInvalidControls(theForm: FormGroup) {
		const mRetVal = [];
		const mFormControls = theForm.controls;
		for (const mControlName in mFormControls) {
			if (mFormControls[mControlName].invalid) {
				mRetVal.push(mControlName);
			}
		}
		return mRetVal;
	}

    abstract delete(): void;
    abstract createForm(): void;
    abstract populateProfile(): void;
    abstract save(): void;

}